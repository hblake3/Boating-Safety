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

    }

    public void ShowIncorrect(string title, string body)
    {
        gameObject.SetActive(true);
        titleText.text = title;
        bodyText.text = body;

        nextButton.SetActive(false);
        tryAgainButton.SetActive(true);

    }

    public void Close()
    {
        gameObject.SetActive(false);
    }
}
