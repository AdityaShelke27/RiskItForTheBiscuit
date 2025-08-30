using System.Collections.Generic;
using System.Collections;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UI;

public class Player : Character
{
    [SerializeField] Transform m_GunPivot;
    [SerializeField] Transform m_FirePoint;
    [SerializeField] Camera m_Camera;
    [SerializeField] GameObject m_BulletPrefab;
    [SerializeField] GameObject m_EchoPrefab;
    [SerializeField] Slider m_HealthSlider;

    InputSystem_Actions m_InputActions;
    public delegate void InteractDelegate();
    public InteractDelegate interact;

    bool m_IsRecording = false;
    int m_EchosDeployed = 0;
    [SerializeField] Transform m_PlayerBody;
    [SerializeField] int m_MaxDeployableEchos;
    List<RecordFrame> m_RecordedEcho = new();
    [SerializeField] float m_FirstEchoRecordTime;
    bool isRight = true;

    protected override void Awake()
    {
        base.Awake();

        m_InputActions = new InputSystem_Actions();
        m_InputActions.Player.Enable();

        m_InputActions.Player.Shoot.performed += Shoot;
        m_InputActions.Player.Record.performed += Record;
        m_InputActions.Player.Interact.performed += Interact;
    }
    void Start()
    {
        p_Health = p_MaxHealth;
        m_HealthSlider.maxValue = p_MaxHealth;
        m_HealthSlider.value = p_Health;
    }

    void Update()
    {
        Vector2 val = m_InputActions.Player.Move.ReadValue<Vector2>();
        p_Animator.SetBool("IsMoving", val != Vector2.zero);
        Move(val);
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
        if(ExtractionArea.GetIsInteractable())
        {
            ExtractionArea.ActivateMachine();
        }
        /*if(m_IsEchoReady)
        {
            DeployEcho();
            m_EchosDeployed++;
            m_IsEchoReady = false;
        }*/
    }
    void DeployEcho()
    {
        GameObject echo = Instantiate(m_EchoPrefab, Vector3.zero, Quaternion.identity);
        echo.GetComponent<Echo>().OnSpawned(m_RecordedEcho, p_Health / 2, p_Speed);
        TakeDamage(p_Health / 2);
    }
    void PointArmToMouse()
    {
        Vector3 mousePos = m_Camera.ScreenToWorldPoint(m_InputActions.Player.MousePos.ReadValue<Vector2>());
        Vector3 dir = mousePos - transform.position;

        float angle = Mathf.Atan2(dir.y, dir.x) * Mathf.Rad2Deg;
        m_GunPivot.localRotation = Quaternion.AngleAxis(angle, Vector3.forward);
        m_GunPivot.localScale = new Vector3(m_GunPivot.localScale.x, Mathf.Cos(angle * Mathf.Deg2Rad) < 0 ? -1 : 1, m_GunPivot.localScale.z);
    }
    void Shoot(InputAction.CallbackContext context)
    {
        GameObject bullet = Instantiate(m_BulletPrefab, m_FirePoint.position, m_FirePoint.rotation);
        bullet.GetComponent<Bullet>().FireBullet(gameObject.tag);

        AudioManager.Instance.PlayShoot();
    }
    void Record(InputAction.CallbackContext context)
    {
        if (!ExtractionArea.GetIsMachineActive()) return;
        if (m_IsRecording) return;

        ExtractionArea.DeactivateMachine();
        if(m_EchosDeployed >= m_MaxDeployableEchos)
        {
            Debug.Log("Cannot Deploy more echos");
            return;
        }

        Debug.Log("Recording Started");
        AudioManager.Instance.PlayRecording();
        m_IsRecording = true;
        m_RecordedEcho.Clear();
        StartCoroutine(SetRecordingFalse());
    }
    IEnumerator SetRecordingFalse()
    {
        yield return new WaitForSeconds(m_FirstEchoRecordTime);
        Debug.Log("Recording Complete");
        m_IsRecording = false;
        AudioManager.Instance.StopRecording();

        DeployEcho();
        m_EchosDeployed++;
    }
    public override void TakeDamage(float damage)
    {
        p_Health -= damage;
        m_HealthSlider.value = p_Health;
        if (p_Health <= 0)
        {
            Die();
        }
    }

    public override void Heal(float amount)
    {
        p_Health += amount;
        if(p_Health > p_MaxHealth)
        {
            p_Health = p_MaxHealth;
        }
        m_HealthSlider.value = p_Health;
    }
}
