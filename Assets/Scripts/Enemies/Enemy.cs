using System;
using UnityEngine;

public abstract class Enemy : Character
{
    [SerializeField] protected float p_AttackRange;
    [SerializeField] protected float p_ChaseRange;
    [SerializeField] protected float p_Damage;
    [SerializeField] protected float p_AttackFrequency;
    protected EnemyState p_State;
    protected delegate void StateUpdate(float deltaTime);
    protected StateUpdate m_StateUpdate;

    protected virtual void Chase()
    {
        p_State = EnemyState.Chase;
        m_StateUpdate = ChaseUpdate;
    }
    protected virtual void Attack()
    {
        p_State = EnemyState.Attack;
        m_StateUpdate = AttackUpdate;
    }
    public abstract void OnSpawned(Player player);
    protected override void Die()
    {
        base.Die();
    }

    protected virtual void ChaseUpdate(float deltaTime)
    {
        
    }
    protected virtual void AttackUpdate(float deltaTime)
    {

    }
}
