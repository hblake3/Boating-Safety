using UnityEngine;
using TMPro;


public class FeedbackPopupUI : MonoBehaviour
{
    public RectTransform popupPanel;
    public RectTransform bubbleRect;
    public TextMeshProUGUI titleText;
    public TextMeshProUGUI bodyText;
    public GameObject nextButton;
    public GameObject tryAgainButton;


    public void Start()
    {
        // Ensure the popup is hidden at the start
        gameObject.SetActive(false);
    }

    public void ShowCorrect(string title, string body)
    {
        gameObject.SetActive(true);
        titleText.text = title;
        bodyText.text = body;

        nextButton.SetActive(true);
        tryAgainButton.SetActive(false);

        // Play the correct answer sound effect
        UIAudioManager.Instance.PlayCorrect();
    }

    public void ShowIncorrect(string title, string body)
    {
        gameObject.SetActive(true);
        titleText.text = title;
        bodyText.text = body;

        nextButton.SetActive(false);
        tryAgainButton.SetActive(true);

        // Play the incorrect answer sound effect
        UIAudioManager.Instance.PlayError();

    }

    public void Close()
    {
        gameObject.SetActive(false);
    }
}
