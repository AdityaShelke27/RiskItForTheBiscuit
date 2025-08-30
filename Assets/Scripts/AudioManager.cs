using UnityEngine;

public class AudioManager : MonoBehaviour
{
    public static AudioManager Instance;

    [SerializeField] AudioSource m_ShootEffect;
    [SerializeField] AudioSource m_ActivateMachineEffect;
    [SerializeField] AudioSource m_RecordingEffect;
    [SerializeField] AudioClip m_DeathEffect;
    [SerializeField] AudioClip m_HurtEffect;
    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        }
    }
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {

    }

    public void PlayShoot()
    {
        m_ShootEffect.pitch = Random.Range(0.9f, 1.1f);
        m_ShootEffect.Play();
    }
    public void PlayDeath(AudioSource source)
    {
        source.resource = m_DeathEffect;
        source.pitch = Random.Range(0.9f, 1.1f);
        source.Play();
    }
    public void PlayHurt(AudioSource source)
    {
        source.resource = m_HurtEffect;
        source.pitch = Random.Range(0.9f, 1.1f);
        source.Play();
    }
    public void PlayActivateMachine()
    {
        m_ActivateMachineEffect.pitch = Random.Range(0.9f, 1.1f);
        m_ActivateMachineEffect.Play();
    }
    public void PlayRecording()
    {
        m_RecordingEffect.Play();
    }
    public void StopRecording()
    {
        m_RecordingEffect.Stop();
    }
}
