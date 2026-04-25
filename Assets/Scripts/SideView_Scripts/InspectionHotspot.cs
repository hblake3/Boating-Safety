using UnityEngine;
using UnityEngine.UI;

public class InspectionHotspot : MonoBehaviour
{
    [Header("Setup")]
    public bool isCorrect;
    public Sprite defaultSprite;
    public Sprite wrongSprite;
    public Sprite correctSprite;

    private Image image;
    private Button button;

    private SafetyInspectionManager manager;

    void Awake()
    {
        image = GetComponent<Image>();
        button = GetComponent<Button>();
        manager = Object.FindAnyObjectByType<SafetyInspectionManager>();

        image.sprite = defaultSprite;
    }

    public void OnHotspotClicked()
    {
        if (isCorrect)
        {
            manager.CorrectSelected(this);
            image.sprite = correctSprite;

            // play correct sound effect
            UIAudioManager.Instance.PlayCorrect();
        }
        else
        {
            manager.WrongSelected(this);

            // play error sound effect
            UIAudioManager.Instance.PlayError();
        }
    }
    public void DisableHotspot()
    {
        button.interactable = false;
    }

    public void ShowRedX()
    {
        image.sprite = wrongSprite;
    }

    public void EnableHotspot()
    {
        image.sprite = defaultSprite;
        button.interactable = true;
    }
}


