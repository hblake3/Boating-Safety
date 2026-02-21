using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class SpeechBubbleUI : MonoBehaviour
{
    public RectTransform bubbleRect;
    public TextMeshProUGUI bubbleText;
    public FeedbackPopupUI feedbackPopup;
    public Button nextButton;
    public SceneFlowController sceneFlowController;

    void Start()
    {
        Hide();
        // disable speech bubble's next button until needed
        nextButton.gameObject.SetActive(false);

    }

    // Shows the speech bubble with specified text at a position offset from the character's position
    public void Show(string text, Vector2 anchoredPosition)
    {
        bubbleText.text = text;
        bubbleRect.anchoredPosition = anchoredPosition;
        gameObject.SetActive(true);

    }

    public void Hide()
    {
        gameObject.SetActive(false);
    }

    public void ShowNextButton()
    {
        nextButton.gameObject.SetActive(true);
    }

    public void StartInspection()
    {
        Debug.Log("Starting inspection, zooming in...");
        Hide();

        // Trigger the zoom and transition in the SceneFlowController
        sceneFlowController.InspectionTransition();

    }


}
