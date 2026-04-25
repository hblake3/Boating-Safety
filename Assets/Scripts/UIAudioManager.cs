using UnityEngine;

public class UIAudioManager : MonoBehaviour
{
    public static UIAudioManager Instance;

    public AudioSource audioSource;

    public AudioClip hoverSound;
    public AudioClip clickSound;
    public AudioClip popSound;
    public AudioClip errorSound;
    public AudioClip correctSound;

    void Awake()
    {
        Instance = this;

        if (audioSource == null)
            audioSource = GetComponent<AudioSource>();
    }
    public void PlayHover()
    {
        audioSource.PlayOneShot(hoverSound);
    }

    public void PlayClick()
    {
        audioSource.PlayOneShot(clickSound);
    }

    public void PlayPop()
    {
        audioSource.PlayOneShot(popSound);
    }

    public void PlayError()
    {
        audioSource.PlayOneShot(errorSound);
    }

    public void PlayCorrect()
    {
        audioSource.PlayOneShot(correctSound);
    }
}