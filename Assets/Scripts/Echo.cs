using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Echo : Character
{
    [SerializeField] Transform m_GunPivot;
    [SerializeField] Transform m_FirePoint;
    [SerializeField] GameObject m_BulletPrefab;
    [SerializeField] Transform m_Body;
    [SerializeField] Slider m_HealthSlider;

    RecordFrame[] m_RecordSequence;
    bool m_IsActive;
    int m_FramePoint = 0;

    private void Awake()
    {
        p_Rigidbody = GetComponent<Rigidbody2D>();
    }

    // Update is called once per frame
    void Update()
    {
        if (!m_IsActive) return;

        if (m_FramePoint >= m_RecordSequence.Length) m_FramePoint = 0;

        ExecuteFrame(m_FramePoint++);
    }

    public override void TakeDamage(float damage)
    {
        p_Health -= damage;

        if (p_Health <= 0)
        {
            Die();
        }
    }

    public void OnSpawned(List<RecordFrame> recordedEcho, float maxHealth, float speed)
    {
        Debug.Log("Echo Deployed");
        m_RecordSequence = recordedEcho.ToArray();
        p_Speed = speed;
        p_MaxHealth = maxHealth;
        p_Health = p_MaxHealth;
        m_IsActive = true;
        m_FramePoint = 0;
        m_HealthSlider.maxValue = p_MaxHealth;
        m_HealthSlider.value = p_Health;
    }
    void ExecuteFrame(int frameIdx)
    {
        RecordFrame frame = m_RecordSequence[frameIdx];
        transform.position = frame.pos;
        m_GunPivot.localRotation = Quaternion.AngleAxis(frame.armAngle, Vector3.forward);
        m_Body.localScale = new Vector3(frame.isRight ? 1 : -1, m_Body.localScale.y, m_Body.localScale.z);
        if (frame.didShoot)
        {
            Shoot();
        }
    }
    void Shoot()
    {
        GameObject bullet = Instantiate(m_BulletPrefab, m_FirePoint.position, m_FirePoint.rotation);
        bullet.GetComponent<Bullet>().FireBullet(gameObject.tag);
    }
}
