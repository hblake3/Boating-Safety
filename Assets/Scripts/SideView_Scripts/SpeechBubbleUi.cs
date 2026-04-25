using UnityEngine;
using TMPro;
using UnityEngine.UI;
using System.Collections;
using UnityEngine.SceneManagement;

public class SpeechBubbleUI : MonoBehaviour
{
    public RectTransform bubbleRect;
    public TextMeshProUGUI bubbleText;
    public FeedbackPopupUI feedbackPopup;
    public Button nextButton;
    public Button nextButton2;
    public Button nextButton3;
    public SceneFlowController sceneFlowController;
    public GearSelectionManager gearSelectionManager;
    void Start()
    {
        Hide();
    }

    // Shows the speech bubble with specified text at a position offset from the character's position
    public void Show(string text, Vector2 anchoredPosition)
    {
        bubbleText.text = text;
        bubbleRect.anchoredPosition = anchoredPosition;
        gameObject.SetActive(true);

        UIAudioManager.Instance.PlayPop();
    }

    public void Hide()
    {
        nextButton.interactable = false;
        nextButton.gameObject.SetActive(false);
        nextButton2.interactable = false;
        nextButton2.gameObject.SetActive(false);
        nextButton3.interactable = false;
        nextButton3.gameObject.SetActive(false);
        gameObject.SetActive(false);
    }

    public void ShowNextButton()
    {
        nextButton.gameObject.SetActive(true);
        nextButton.interactable = true;
    }

    public void StartInspection()
    {
        Hide();

        // Trigger the zoom and transition in the SceneFlowController
        sceneFlowController.InspectionTransition();
    }

    public void ShowNextButton2()
    {
        nextButton2.gameObject.SetActive(true);
        nextButton2.interactable = true;
    }

    public void StartSafetyGear()
    {
        Hide();

        // Start the gear selection process
        gearSelectionManager.StartGearSelection();
    }

    public void ShowNextButton3()
    {
        nextButton3.gameObject.SetActive(true);
        nextButton3.interactable = true;
    }

    public void BeginTopDown()
    {
        // Transition to the top-down scene with a fade effect
        if (SceneTransitionManager.Instance != null)
        {
            SceneTransitionManager.Instance.TransitionToScene("TopDown");
            Hide();
        }
        else // for debugging in the editor, just load the scene without transition
        {
            SceneManager.LoadScene("TopDown");
             Hide();
        }

    }

}
