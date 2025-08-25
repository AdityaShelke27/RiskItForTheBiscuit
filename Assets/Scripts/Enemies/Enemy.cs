using System;
using UnityEngine;

public class Enemy : Character
{
    [SerializeField] protected float p_MinPlayerDistance;
    [SerializeField] protected float p_Damage;
    [SerializeField] protected float p_AttackFrequency;
    protected EnemyState p_State;
    protected delegate void StateUpdate(float deltaTime);
    StateUpdate m_StateUpdate;

    protected virtual void Chase()
    {
        if (p_State == EnemyState.Chase) return;

        p_State = EnemyState.Chase;
        m_StateUpdate = ChaseUpdate;
    }
    protected virtual void Attack()
    {
        if (p_State == EnemyState.Attack) return;

        p_State = EnemyState.Attack;
        m_StateUpdate = AttackUpdate;
    }
    protected virtual void ChaseUpdate(float deltaTime)
    {

    }
    protected virtual void AttackUpdate(float deltaTime)
    {

    }
}
