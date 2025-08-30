using UnityEngine;

public abstract class Character : MonoBehaviour
{
    [SerializeField] protected float p_MaxHealth;
    [SerializeField] protected float p_Speed;
    [SerializeField] protected float p_Health;
    [SerializeField] protected Animator p_Animator;
    protected AudioSource p_AudioSource;
    protected BoxCollider2D p_Collider;
    protected Rigidbody2D p_Rigidbody;
    protected bool p_IsAlive = true;
    protected virtual void Awake()
    {
        p_Rigidbody = GetComponent<Rigidbody2D>();
        p_Collider = GetComponent<BoxCollider2D>();
        p_AudioSource = GetComponent<AudioSource>();
    }
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
    public abstract void Heal(float amount);
    public bool GetIsAlive()
    {
        return p_IsAlive;
    }
    public Vector3 GetColliderCenter()
    {
        return transform.position + (Vector3)p_Collider.offset;
    }
    public float GetMaxHealth()
    {
        return p_MaxHealth;
    }
}
