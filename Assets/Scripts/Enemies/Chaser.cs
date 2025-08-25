using System.Collections;
using UnityEngine;

public class Chaser : Enemy
{
    GameObject m_Player;
    Character m_PlayerCharacter;
    private void Awake()
    {
        p_Rigidbody = GetComponent<Rigidbody2D>();
    }
    void Start()
    {
        m_Player = GameObject.Find("Player");

        p_Health = p_MaxHealth;
    }

    // Update is called once per frame
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
    }

    void Chase()
    {
        if (p_State == EnemyState.Chase) return;

        p_State = EnemyState.Chase;
        Move((m_Player.transform.position - transform.position).normalized);
    }
    void Attack()
    {
        if (p_State == EnemyState.Attack) return;

        p_State = EnemyState.Attack;
        Move(Vector2.zero);

        StartCoroutine(AttackFrequency());
    }
    IEnumerator AttackFrequency()
    {
        while(p_State == EnemyState.Attack)
        {
            m_PlayerCharacter.TakeDamage(p_Damage);

            yield return new WaitForSeconds(p_AttackFrequency);
        }
    }

    protected override void Die()
    {
        p_State = EnemyState.Die;
        base.Die();
    }
}
