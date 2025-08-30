using System.Collections;
using UnityEngine;

public class Shooter : Enemy
{
    [SerializeField] GameObject m_BulletPrefab;
    [SerializeField] Transform m_FirePoint;
    [SerializeField] Transform m_GunPivot;
    Vector3 m_OriginalBodyScale;
    Coroutine m_AttackCoroutine;
    bool isFlipped = false;
    void Update()
    {
        if (!p_IsAlive) return;

        if (p_Target == null || !p_Target.GetIsAlive())
        {
            FindNewTarget();
            if (p_Target == null) return;
        }

        float distance = Vector2.Distance(transform.position, p_Target.transform.position);
        isFlipped = transform.position.x < p_Target.transform.position.x ? true : false;
        p_Body.localScale = new(isFlipped ? -m_OriginalBodyScale.x : m_OriginalBodyScale.x, m_OriginalBodyScale.y, m_OriginalBodyScale.z);
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
        p_Body = p_Animator.transform;
        m_OriginalBodyScale = p_Body.localScale;
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
    public override void TakeDamage(float damage)
    {
        p_Health -= damage;
        p_HealthSlider.value = p_Health;
        p_Animator.SetTrigger("Damage");
        AudioManager.Instance.PlayHurt(p_AudioSource);
        if (p_Health <= 0)
        {
            SwitchState(EnemyState.Die);
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
            p_Animator.SetTrigger("Attack");
            yield return new WaitForSeconds(p_AttackFrequency);
        }
    }
    void Shoot()
    {
	    Vector3 dir = p_Target.GetColliderCenter() - transform.position;

        float angle = Mathf.Atan2(dir.y, dir.x) * Mathf.Rad2Deg;
        m_FirePoint.rotation = Quaternion.AngleAxis(angle/* * (isFlipped ? -1 : 1)*/, Vector3.forward);

        GameObject bullet = Instantiate(m_BulletPrefab, m_FirePoint.position, m_FirePoint.rotation);
        bullet.GetComponent<Bullet>().FireBullet(gameObject.tag);
    }
    private void OnDrawGizmos()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawSphere(m_FirePoint.position, 0.1f);
    }
    protected override void Die()
    {
        p_State = EnemyState.Die;
        p_Rigidbody.linearVelocity = Vector2.zero;
        p_Collider.enabled = false;
        AudioManager.Instance.PlayDeath(p_AudioSource);
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
                p_Animator.SetBool("IsMoving", true);
                Chase();
                break;
            case EnemyState.Attack:
                p_Animator.SetBool("IsMoving", false);
                Attack();
                break;
            case EnemyState.Die:
                p_Animator.SetBool("IsMoving", false);
                p_Animator.SetTrigger("Dead");
                Die();
                break;
        }
    }
    public override void Heal(float amount)
    {
        p_Health += amount;
        if (p_Health > p_MaxHealth)
        {
            p_Health = p_MaxHealth;
        }
        p_HealthSlider.value = p_Health;
    }
}
