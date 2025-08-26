using UnityEngine;

public class Bullet : MonoBehaviour
{
    [SerializeField] float m_Speed;
    [SerializeField] float m_AliveTime;
    [SerializeField] float m_Damage;
    private Rigidbody2D m_Rigidbody;
    string m_OwnerTag;
    private void Awake()
    {
        m_Rigidbody = GetComponent<Rigidbody2D>();
    }
    void DestroyBullet()
    {
        Destroy(gameObject);
    }
    private void OnCollisionEnter2D(Collision2D collision)
    {
        if(!collision.collider.CompareTag(m_OwnerTag) && collision.collider.TryGetComponent(out Character character))
        {
            character.TakeDamage(m_Damage);
        }
        DestroyBullet();
    }
    public void FireBullet(string OwnerTag)
    {
        m_OwnerTag = OwnerTag;
        m_Rigidbody.linearVelocity = transform.right * m_Speed;
        Invoke(nameof(DestroyBullet), m_AliveTime);
    }
}
