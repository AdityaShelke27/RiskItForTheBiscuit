using System;
using UnityEngine;
using UnityEngine.UI;

public abstract class Enemy : Character
{
    [SerializeField] protected float p_AttackRange;
    [SerializeField] protected float p_ChaseRange;
    [SerializeField] protected float p_Damage;
    [SerializeField] protected float p_AttackFrequency;
    [SerializeField] protected Slider p_HealthSlider;
    [SerializeField] protected Animator p_Animator;
    protected Transform p_Body;
    protected Character p_Target;
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
    public abstract void OnSpawned();
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
    protected virtual void FindNewTarget()
    {
        GameObject[] targets = GameObject.FindGameObjectsWithTag("Player");

        GameObject minDistanceTarget = targets[0];
        float minDistance = float.MaxValue;
        foreach (GameObject target in targets)
        {
            float dist = Vector2.Distance(transform.position, target.transform.position);
            if (dist < minDistance)
            {
                minDistance = dist;
                minDistanceTarget = target;
            }
        }

        p_Target = minDistanceTarget.GetComponent<Character>();
    }
}
