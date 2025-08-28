using System.Collections;
using UnityEngine;

public class UIManager : MonoBehaviour
{
    public static UIManager Instance;
    [SerializeField] GameObject m_MachineActivePanel;
    private void Awake()
    {
        if(Instance == null)
        {
            Instance = this;
        }
        else
        {
            Destroy(Instance);
            Instance = this;
        }
    }
    void Start()
    {
        m_MachineActivePanel.SetActive(false);
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void MachineActiveFlash()
    {
        StartCoroutine(StartMachineActiveFlash());
    }

    IEnumerator StartMachineActiveFlash()
    {
        while(ExtractionArea.GetIsMachineActive())
        {
            m_MachineActivePanel.SetActive(true);

            yield return new WaitForSeconds(0.7f);

            m_MachineActivePanel.SetActive(false);

            yield return new WaitForSeconds(0.5f);
        }
    }
}
