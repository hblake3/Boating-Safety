using UnityEngine;
using UnityEngine.UI;

public class MuteToggleButton : MonoBehaviour
{
    public Image buttonImage;

    [Header("Sprites")]
    public Sprite audioOn;
    public Sprite audioOnPressed;
    public Sprite audioOff;
    public Sprite audioOffPressed;

    private bool isMuted = false;

    void Start()
    {
        isMuted = PlayerPrefs.GetInt("Muted", 0) == 1;

        UIAudioManager.Instance.audioSource.mute = isMuted;

        UpdateVisual();
    }


    public void ToggleMute()
    {

        // Play click BEFORE muting (only if currently unmuted)
        if (!isMuted)
        {
            UIAudioManager.Instance.PlayClick();
        }

        // Toggle state
        isMuted = !isMuted;

        // Mute logic
        UIAudioManager.Instance.audioSource.mute = isMuted;

        // Save state
        PlayerPrefs.SetInt("Muted", isMuted ? 1 : 0);

        UpdateVisual();
    }

    void UpdateVisual()
    {
        var button = GetComponent<Button>();
        var spriteState = button.spriteState;

        if (isMuted)
        {
            buttonImage.sprite = audioOff;

            spriteState.pressedSprite = audioOffPressed;
        }
        else
        {
            buttonImage.sprite = audioOn;

            spriteState.pressedSprite = audioOnPressed;
        }

        button.spriteState = spriteState;
    }
}