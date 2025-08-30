using System;
using UnityEngine;
using UnityEngine.UI;
using Random = UnityEngine.Random;

public abstract class Enemy : Character
{
    [SerializeField] protected float p_AttackRange;
    [SerializeField] protected float p_ChaseRange;
    [SerializeField] protected float p_Damage;
    [SerializeField] protected float p_AttackFrequency;
    [SerializeField] protected Slider p_HealthSlider;
    [SerializeField] protected GameObject p_Collectible;
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

        if(Random.Range(0, 5) == 0) Instantiate(p_Collectible, transform.position, Quaternion.identity);
        Invoke(nameof(DestroyEnemy), 2);
    }
    void DestroyEnemy()
    {
        Destroy(gameObject);
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
