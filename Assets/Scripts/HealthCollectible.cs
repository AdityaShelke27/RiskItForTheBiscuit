using UnityEngine;

public class HealthCollectible : MonoBehaviour
{
    [SerializeField] float m_HealPercent;
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.CompareTag("Player"))
        {
            Character character = collision.GetComponent<Character>();
            character.Heal(character.GetMaxHealth() * m_HealPercent);

            Destroy(gameObject);
        }
    }
}
