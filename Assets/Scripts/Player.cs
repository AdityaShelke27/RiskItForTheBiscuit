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

    InputSystem_Actions m_InputActions;
    public delegate void InteractDelegate();
    public InteractDelegate interact;

    bool m_IsRecording = false;
    List<RecordFrame> m_RecordedEcho = new();
    [SerializeField] float m_FirstEchoRecordTime;

    private void Awake()
    {
        p_Rigidbody = GetComponent<Rigidbody2D>();
        m_InputActions = new InputSystem_Actions();
        m_InputActions.Player.Enable();

        m_InputActions.Player.Shoot.performed += Shoot;
        m_InputActions.Player.Record.performed += Record;
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
        PointArmToMouse();
        if (m_IsRecording)
        {
            RecordFrame frame = new(
                new Vector2(transform.position.x, transform.position.y), 
                m_GunPivot.localRotation.z, 
                m_InputActions.Player.Shoot.triggered
                );
            
            m_RecordedEcho.Add(frame);
        }
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

        m_IsRecording = true;
        m_RecordedEcho.Clear();
        StartCoroutine(SetRecordingFalse());
    }
    IEnumerator SetRecordingFalse()
    {
        yield return new WaitForSeconds(m_FirstEchoRecordTime);

        m_IsRecording = false;
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
