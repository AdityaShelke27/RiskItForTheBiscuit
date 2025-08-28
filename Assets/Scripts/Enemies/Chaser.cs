using System.Collections;
using UnityEngine;

public class Chaser : Enemy
{
    Coroutine m_AttackCoroutine;
    private void Awake()
    {
        p_Rigidbody = GetComponent<Rigidbody2D>();
    }

    void Update()
    {
        if(p_Target == null)
        {
            FindNewTarget();
            if(p_Target == null) return;
        }

        float distance = Vector2.Distance(transform.position, p_Target.transform.position);
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
    public override void OnSpawned()
    {
        p_Health = p_MaxHealth;
        p_HealthSlider.maxValue = p_MaxHealth;
        p_HealthSlider.value = p_Health;
    }
    protected override void ChaseUpdate(float deltaTime)
    {
        base.ChaseUpdate(deltaTime);
        
        Move((p_Target.transform.position - transform.position).normalized);
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
            p_Target.TakeDamage(p_Damage);

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
        p_HealthSlider.value = p_Health;

        if (p_Health <= 0)
        {
            Die();
        }
    }
}
