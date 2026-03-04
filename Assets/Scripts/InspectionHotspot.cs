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
        manager = Object.FindFirstObjectByType<SafetyInspectionManager>();

        image.sprite = defaultSprite;
    }

    public void OnHotspotClicked()
    {
        if (isCorrect)
        {
            manager.CorrectSelected(this);
            image.sprite = correctSprite;
        }
        else
        {
            manager.WrongSelected(this);

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


