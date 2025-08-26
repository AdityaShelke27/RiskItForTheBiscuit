using System.Collections;
using UnityEngine;

public class Chaser : Enemy
{
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
    protected override void Attack()
    {
        base.Attack();

        Move(Vector2.zero);
        m_AttackCoroutine = StartCoroutine(AttackFrequency());
    }
    IEnumerator AttackFrequency()
    {
        while(p_State == EnemyState.Attack)
        {
            m_Player.TakeDamage(p_Damage);

            yield return new WaitForSeconds(p_AttackFrequency);
        }
    }

    protected override void Die()
    {
        p_State = EnemyState.Die;
        base.Die();
    }
    void SwitchState(EnemyState state)
    {
        if (p_State == state) return;

        if (p_State == EnemyState.Attack)
        {
            StopCoroutine(m_AttackCoroutine);
        }

        switch (state)
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

    public override void TakeDamage(float damage)
    {
        p_Health -= damage;

        if (p_Health <= 0)
        {
            Die();
        }
    }

    
}
