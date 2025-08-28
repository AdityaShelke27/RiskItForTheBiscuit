using UnityEngine;

public class ExtractionArea : MonoBehaviour
{
    [SerializeField] Player m_Player;
    static bool m_IsMachineActive;
    static bool m_IsInteractable = false;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.gameObject == m_Player.gameObject)
        {
            m_IsInteractable = true;
        }
    }
    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.gameObject == m_Player.gameObject)
        {
            m_IsInteractable = false;
        }
    }

    public static bool GetIsMachineActive()
    {
        return m_IsMachineActive;
    }
    public static void DeactivateMachine()
    {
        m_IsMachineActive = false;
    }
    public static bool GetIsInteractable()
    {
        return m_IsInteractable;
    }
    public static void ActivateMachine()
    {
        m_IsMachineActive = true;
        m_IsInteractable = false;
        UIManager.Instance.MachineActiveFlash();
    }
}
