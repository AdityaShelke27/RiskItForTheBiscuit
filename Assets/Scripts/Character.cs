using UnityEngine;

public abstract class Character : MonoBehaviour
{
    [SerializeField] protected float p_MaxHealth;
    [SerializeField] protected float p_Speed;
    [SerializeField] protected float p_Health;
    protected BoxCollider2D p_Collider;
    protected Rigidbody2D p_Rigidbody;
    protected bool p_IsAlive = true;
    
    protected void Move(Vector2 dir)
    {
        p_Rigidbody.linearVelocity = dir * p_Speed;
    }
    public abstract void TakeDamage(float damage);
    protected virtual void Die()
    {
        p_IsAlive = false;
        //Destroy(gameObject);
    }
}
