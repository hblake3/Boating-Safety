using UnityEngine;

public class UIAudioManager : MonoBehaviour
{
    public static UIAudioManager Instance;

    public AudioSource audioSource;

    [Header("UI")]
    public AudioClip hoverSound;
    public AudioClip clickSound;
    public AudioClip popSound;
    public AudioClip errorSound;
    public AudioClip correctSound;

    [Header("Top-Down")]
    public AudioClip collisionSound;
    public AudioClip decrementSound;
    public AudioClip fanfareSound;
    public AudioClip encourageSound;

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

    public void PlayBoatHit()
    {
        audioSource.PlayOneShot(collisionSound);
    }

    public void PlayDecrement()
    {
        audioSource.PlayOneShot(decrementSound);
    }

    public void PlayFanfare()
    {
        audioSource.PlayOneShot(fanfareSound);
    }

    public void PlayEncouragement()
    {
        audioSource.PlayOneShot(encourageSound);
    }
}