
using UnityEngine;

public class BulletImpactAudioController : MonoBehaviour
{
    private AudioSource m_audioSource;
    public AudioClip[] ImpactWizzles;
    private AudioClip m_clip;

    // Start is called before the first frame update
    void OnEnable()
    {
        m_audioSource = GetComponent<AudioSource>();
        m_clip = ImpactWizzles[Random.Range(0, ImpactWizzles.Length)];
        m_audioSource.PlayOneShot(m_clip);
    }
}
