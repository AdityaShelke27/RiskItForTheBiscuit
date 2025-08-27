using UnityEngine;

public abstract class Character : MonoBehaviour
{
    [SerializeField] protected float p_MaxHealth;
    [SerializeField] protected float p_Speed;
    protected float p_Health;
    protected Rigidbody2D p_Rigidbody;
    
    protected void Move(Vector2 dir)
    {
        p_Rigidbody.linearVelocity = dir * p_Speed;
    }
    public abstract void TakeDamage(float damage);
    protected virtual void Die()
    {
        Destroy(gameObject);
    }
}
