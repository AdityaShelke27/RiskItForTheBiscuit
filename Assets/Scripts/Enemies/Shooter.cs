using System.Collections;
using UnityEngine;

public class Shooter : Enemy
{
    [SerializeField] GameObject m_BulletPrefab;
    [SerializeField] Transform m_FirePoint;
    [SerializeField] Transform m_GunPivot;
    GameObject m_Player;
    StateUpdate m_StateUpdate;
    private void Awake()
    {
        p_Rigidbody = GetComponent<Rigidbody2D>();
    }
    void Start()
    {
        m_Player = GameObject.Find("Player");

        p_Health = p_MaxHealth;
    }
    void Update()
    {
        if (Vector2.Distance(transform.position, m_Player.transform.position) > p_MinPlayerDistance)
        {
            Chase();
        }
        else
        {
            Attack();
        }

        m_StateUpdate(Time.deltaTime);
    }

    protected override void Chase()
    {
        base.Chase();

        Move((m_Player.transform.position - transform.position).normalized);
    }

    protected override void Attack()
    {
        base.Attack();

        Move(Vector2.zero);

        StartCoroutine(AttackFrequency());
    }
    IEnumerator AttackFrequency()
    {
        while (p_State == EnemyState.Attack)
        {
            Shoot();

            yield return new WaitForSeconds(p_AttackFrequency);
        }
    }
    void Shoot()
    {
	Vector3 dir = m_Player.transform.position - transform.position;

        float angle = Mathf.Atan2(dir.y, dir.x) * Mathf.Rad2Deg;
        m_GunPivot.localRotation = Quaternion.AngleAxis(angle, Vector3.forward);

        GameObject bullet = Instantiate(m_BulletPrefab, m_FirePoint.position, m_FirePoint.rotation);
        bullet.GetComponent<Bullet>().FireBullet(gameObject.tag);
    }
    protected override void Die()
    {
        p_State = EnemyState.Die;
        base.Die();
    }
}
