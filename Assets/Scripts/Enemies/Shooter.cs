using System.Collections;
using UnityEngine;

public class Shooter : Enemy
{
    [SerializeField] GameObject m_BulletPrefab;
    [SerializeField] Transform m_FirePoint;
    [SerializeField] Transform m_GunPivot;
    Player m_Player;
    Coroutine m_AttackCoroutine;
    private void Awake()
    {
        p_Rigidbody = GetComponent<Rigidbody2D>();
    }
    void Update()
    {
        float distance = Vector2.Distance(transform.position, m_Player.transform.position);
        if (p_State == EnemyState.Attack)
        {
            if (distance > p_ChaseRange)
            {
                SwitchState(EnemyState.Chase);
            }
        }
        else // If currently chasing or idle
        {
            if (distance <= p_AttackRange)
            {
                SwitchState(EnemyState.Attack);
            }
            else
            {
                SwitchState(EnemyState.Chase);
            }
        }

        m_StateUpdate?.Invoke(Time.deltaTime);
    }
    public override void OnSpawned(Player player)
    {
        m_Player = player;

        p_Health = p_MaxHealth;
        p_HealthSlider.maxValue = p_MaxHealth;
        p_HealthSlider.value = p_Health;
    }
    protected override void ChaseUpdate(float deltaTime)
    {
        base.ChaseUpdate(deltaTime);

        Move((m_Player.transform.position - transform.position).normalized);
    }
    protected override void Chase()
    {
        base.Chase();
    }
    public override void TakeDamage(float damage)
    {
        p_Health -= damage;
        p_HealthSlider.value = p_Health;

        if (p_Health <= 0)
        {
            Die();
        }
    }
    protected override void Attack()
    {
        base.Attack();
        Move(Vector2.zero);

        m_AttackCoroutine = StartCoroutine(AttackFrequency());
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

    void SwitchState(EnemyState state)
    {
        if(p_State == state) return;

        if(p_State == EnemyState.Attack)
        {
            StopCoroutine(m_AttackCoroutine);
        }

        switch(state)
        {
            case EnemyState.None:
                break;
            case EnemyState.Chase:
                Chase();
                break;
            case EnemyState.Attack:
                Attack();
                break;
            case EnemyState.Die:
                Die();
                break;
        }
    }
}
