using UnityEngine;

public class CameraFollowPlayer : MonoBehaviour
{
    [SerializeField] GameObject m_Player;
    [SerializeField] float m_Offset;
    Vector3 m_LerpPos;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        m_LerpPos = m_Player.transform.position;
        m_LerpPos.z = m_Offset;
        transform.position = m_LerpPos;

    }
}
