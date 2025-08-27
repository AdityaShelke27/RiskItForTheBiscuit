using System.Collections.Generic;
using System.Collections;
using UnityEngine;
using UnityEngine.InputSystem;

public class Player : Character
{
    [SerializeField] Transform m_GunPivot;
    [SerializeField] Transform m_FirePoint;
    [SerializeField] Camera m_Camera;
    [SerializeField] GameObject m_BulletPrefab;
    [SerializeField] GameObject m_EchoPrefab;

    InputSystem_Actions m_InputActions;
    public delegate void InteractDelegate();
    public InteractDelegate interact;

    bool m_IsRecording = false;
    bool m_IsEchoReady = false;
    int m_EchosDeployed = 0;
    [SerializeField] Transform m_PlayerBody;
    [SerializeField] int m_MaxDeployableEchos;
    List<RecordFrame> m_RecordedEcho = new();
    [SerializeField] float m_FirstEchoRecordTime;
    bool isRight = true;

    private void Awake()
    {
        p_Rigidbody = GetComponent<Rigidbody2D>();
        m_InputActions = new InputSystem_Actions();
        m_InputActions.Player.Enable();

        m_InputActions.Player.Shoot.performed += Shoot;
        m_InputActions.Player.Record.performed += Record;
        m_InputActions.Player.Interact.performed += Interact;
    }
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        p_Health = p_MaxHealth;
    }

    // Update is called once per frame
    void Update()
    {
        Move(m_InputActions.Player.Move.ReadValue<Vector2>());
        PlayerFlipCorrection();
        PointArmToMouse();
        if (m_IsRecording)
        {
            RecordFrame frame = new(
                new Vector2(transform.position.x, transform.position.y), 
                (short) m_GunPivot.localRotation.eulerAngles.z, 
                m_InputActions.Player.Shoot.triggered,
                isRight
                );
            
            m_RecordedEcho.Add(frame);
        }
    }
    void PlayerFlipCorrection()
    {
        if (p_Rigidbody.linearVelocityX > 0.1f)
        {
            isRight = true;
        }
        else if (p_Rigidbody.linearVelocityX < -0.1f)
        {
            isRight = false;
        }

        m_PlayerBody.localScale = new Vector3(isRight ? 1 : -1, m_PlayerBody.localScale.y, m_PlayerBody.localScale.z);
        //m_GunPivot.localScale = new Vector3(isRight ? 1 : -1, m_GunPivot.localScale.y, m_GunPivot.localScale.z);
    }
    void Interact(InputAction.CallbackContext context)
    {
        if(m_IsEchoReady)
        {
            DeployEcho();
            m_EchosDeployed++;
            m_IsEchoReady = false;
        }
    }
    void DeployEcho()
    {
        GameObject echo = Instantiate(m_EchoPrefab, Vector3.zero, Quaternion.identity);
        echo.GetComponent<Echo>().OnSpawned(m_RecordedEcho, p_MaxHealth, p_Speed);
    }
    void PointArmToMouse()
    {
        Vector3 mousePos = m_Camera.ScreenToWorldPoint(m_InputActions.Player.MousePos.ReadValue<Vector2>());
        Vector3 dir = mousePos - transform.position;

        float angle = Mathf.Atan2(dir.y, dir.x) * Mathf.Rad2Deg;
        m_GunPivot.localRotation = Quaternion.AngleAxis(angle, Vector3.forward);
    }
    void Shoot(InputAction.CallbackContext context)
    {
        GameObject bullet = Instantiate(m_BulletPrefab, m_FirePoint.position, m_FirePoint.rotation);
        bullet.GetComponent<Bullet>().FireBullet(gameObject.tag);
    }
    void Record(InputAction.CallbackContext context)
    {
        if (m_IsRecording) return;

        if(m_EchosDeployed >= m_MaxDeployableEchos)
        {
            Debug.Log("Cannot Deploy more echos");
            return;
        }

        Debug.Log("Recording Started");
        m_IsRecording = true;
        m_RecordedEcho.Clear();
        StartCoroutine(SetRecordingFalse());
    }
    IEnumerator SetRecordingFalse()
    {
        yield return new WaitForSeconds(m_FirstEchoRecordTime);
        Debug.Log("Recording Complete");
        m_IsRecording = false;
        m_IsEchoReady = true;
    }
    public override void TakeDamage(float damage)
    {
        p_Health -= damage;

        if (p_Health <= 0)
        {
            Die();
        }
    }
}
