using System;
using TMPro;
using UnityEditor.PackageManager;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.TextCore.Text;
using UnityEngine.UI;
using static GameManager;

public class GearSelectionManager : MonoBehaviour
{
    [Header("UI")]
    public TextMeshProUGUI selectedItemTitle;
    public TextMeshProUGUI selectedItemDetails;
    public TextMeshProUGUI feedbackText;
    public Button confirmButton;
    public GameObject gearPanel;
    public GameObject descriptionPanel;

    [Header("Feedback UI")]
    public GameObject ghostPanel;
    public GameObject ghostPanel2; // second ghost panel that blocks interaction of panels during tutorial
    public GameObject buddyPortrait;
    public GameObject barryPortrait;

    public GameObject speechBubble;
    public TextMeshProUGUI bubbleText;
    public Button continueButton;

    [Header("Other")]
    public SpeechBubbleUI speechBubbleUI;

    private GearItem currentSelection;

    // Tutorial state variables
    private int tutorialStep = 0;
    private bool tutorialActive = false;
    // Tracking correct selections for end-of-activity feedback
    private int correctSelections = 0;
    private int totalCorrect = 5;

    // Add this field to the class to define semiTransparentWhite once and use it everywhere.
    private static readonly Color semiTransparentWhite = new Color(1f, 1f, 1f, 0.8f);

    void Start()
    {
        // Hide this UI until the player starts the activity
        this.gameObject.SetActive(false);

    }

    void Update()
    {
        if (currentSelection != null)
        {
            if (EventSystem.current.currentSelectedGameObject != currentSelection.gameObject)
            {
                EventSystem.current.SetSelectedGameObject(currentSelection.gameObject);
            }
        }
    }

    public void StartGearSelection()
    {
        this.gameObject.SetActive(true);
        StartTutorial();
    }

    void StartTutorial()
    {
        tutorialActive = true;
        tutorialStep = 0;
        // Show ghost panel to dim the background and focus attention on the tutorial panels
        ghostPanel.SetActive(true);
        // Show second ghost panel to block interaction with the description panel during the tutorial
        ghostPanel2.SetActive(true);

        // Set color of panels to be semi-transparent white for tutorial
        gearPanel.GetComponent<Image>().color = semiTransparentWhite;
        descriptionPanel.GetComponent<Image>().color = semiTransparentWhite;

        speechBubble.SetActive(true);

        ShowCharacterIcon();

        continueButton.onClick.RemoveAllListeners();
        continueButton.onClick.AddListener(NextTutorialStep);

        ShowTutorialStep();
    }

    void ShowCharacterIcon()
    {
        string charName = GameManager.Instance.selectedCharacter.characterName;

        buddyPortrait.SetActive(charName == "Buddy the Beaver");
        barryPortrait.SetActive(charName == "Barry the Bear");
    }

    void ShowTutorialStep()
    {
        switch (tutorialStep)
        {

            case 0:
                bubbleText.text = "First, choose the safety gear that we need to bring on board!";
                // Return color to the gear panel to normal to highlight the interactable items for the tutorial
                gearPanel.GetComponent<Image>().color = Color.white;
                break;

            case 1:
                bubbleText.text = "Then, confirm your choice using the confirm button at the bottom.";
                // Change color of gear panel to be semi-transparent again to de-emphasize it for the tutorial
                gearPanel.GetComponent<Image>().color = semiTransparentWhite;
                // Return description panel to normal to highlight the item details for the tutorial
                descriptionPanel.GetComponent<Image>().color = Color.white;
                break;

            case 2:
                EndTutorial();
                break;
        }
    }

    void NextTutorialStep()
    {
        tutorialStep++;
        ShowTutorialStep();
    }

    void EndTutorial()
    {
        tutorialActive = false;

        ghostPanel.SetActive(false);
        ghostPanel2.SetActive(false);
        // Return panels to normal colors for the main activity
        gearPanel.GetComponent<Image>().color = Color.white;
        descriptionPanel.GetComponent<Image>().color = Color.white;

        speechBubble.SetActive(false);

        continueButton.onClick.RemoveAllListeners();
    }

    public void SelectItem(GearItem item)
    {
        if (speechBubble.activeSelf && !tutorialActive)
        {
            speechBubble.SetActive(false);
        }

        if (item == null) return;
        if (item.alreadyChosen) return;

        // Deselect previous selection visually (but don't clear it as "chosen")
        if (currentSelection != null && currentSelection != item)
        {
            currentSelection.SetSelected(false);
        }

        currentSelection = item;

        // Keep the new item visually selected until ConfirmSelection or another item is clicked
        currentSelection.SetSelected(true);

        // Update UI to show selected item and description
        selectedItemTitle.text = item.itemName;
        switch (item.itemName)
        {
            case "Life Jacket":
                selectedItemDetails.text = "An extra life jacket, just incase.";
                break;
            case "Fire Extinguisher":
                selectedItemDetails.text = "A handheld device used to put out small fires.";
                break;
            case "Air Horn":
                selectedItemDetails.text = "A device full of air that makes a loud blaring sound.";
                break;
            case "Navigation Lights":
                selectedItemDetails.text = "Device with color-coded lights (green, red, and white).";
                break;
            case "Throw Ring":
                selectedItemDetails.text = "A throwable ring-shaped flotation device.";
                break;
            case "Dumbbell":
                selectedItemDetails.text = "A weight used for working out.";
                break;
            case "Soccer Ball":
                selectedItemDetails.text = "A fun little ball for the beach activities.";
                break;
            case "Microwave":
                selectedItemDetails.text = "For heating up snacks on the boat.";
                break;
            case "TV":
                selectedItemDetails.text = "A 40-inch television for entertainment.";
                break;
            case "Mini Fridge":
                selectedItemDetails.text = "A small refridgerator to keep drinks cold.";
                break;
            default:
                selectedItemDetails.text = "";
                break;
        }

        confirmButton.interactable = true;
    }

    public void ConfirmSelection()
    {
        if (currentSelection == null) return;

        confirmButton.interactable = false;

        // Clear visual selection immediately so the button doesn't remain highlighted when disabled
        currentSelection.SetSelected(false);

        if (currentSelection.isCorrect)
        {
            correctSelections++;
            feedbackText.text = GetPositiveFeedback(currentSelection.itemName);
            MarkItem(currentSelection, true);

            //play correct sound effect
            UIAudioManager.Instance.PlayCorrect();
        }
        else
        {
            feedbackText.text = GetNegativeFeedback();
            MarkItem(currentSelection, false);

            //play incorrect sound effect
            UIAudioManager.Instance.PlayError();
        }

        // Mark this item as chosen so it can't be selected again
        currentSelection.alreadyChosen = true;
        currentSelection = null;
        selectedItemTitle.text = "Select an item.";
        selectedItemDetails.text = "";

        if (correctSelections >= totalCorrect)
        {
            // disable all the buttons and interactions since the activity is now complete
            foreach (GearItem item in gearPanel.GetComponentsInChildren<GearItem>())
            {
                Button btn = item.GetComponent<Button>();
                btn.interactable = false;
            }

            // Show the positive feedback for the final item first, then after
            // the player dismisses that bubble show the final congratulation.
            ShowFeedbackBubble(feedbackText.text, ShowFinalCongratulation);
            return;
        }

        ShowFeedbackBubble(feedbackText.text);
    }

    void ShowFeedbackBubble(string message, Action onHidden = null)
    {
        if (tutorialActive) return;

        speechBubble.SetActive(true);
        bubbleText.text = message;

        // Ensure the continue button is active/clickable and has only the intended listener.
        continueButton.onClick.RemoveAllListeners();
        continueButton.interactable = true;

        // Add a closure that hides the bubble and then invokes the provided callback.
        continueButton.onClick.AddListener(() =>
        {
            // hide the bubble first
            speechBubble.SetActive(false);

            // remove listeners immediately to avoid duplicate invocations on subsequent bubbles
            continueButton.onClick.RemoveAllListeners();

            // invoke the callback (if any)
            onHidden?.Invoke();
        });
    }

    void HideSpeechBubble()
    {
        // Generic helper: hide the bubble. Note: callbacks are handled by the listeners added in ShowFeedbackBubble
        speechBubble.SetActive(false);
    }

    // Called after the player dismisses the positive feedback for the final item.
    void ShowFinalCongratulation()
    {
        // Show final message and ensure we run the final acknowledgement handler when dismissed.
        ShowFeedbackBubble("Great job! You brought all the important safety gear!", OnAllCorrectAcknowledged);
    }

    void OnAllCorrectAcknowledged()
    {
        // Finish this final activity
        speechBubbleUI.Show("Now that we have all our safety gear, we're ready to set sail! Let's get going!", 
            Instance.selectedCharacter.speechBubbleAnchoredPosition);
        speechBubbleUI.ShowNextButton3();


        this.gameObject.SetActive(false);
    }

    void MarkItem(GearItem item, bool correct)
    {
        Button btn = item.GetComponent<Button>();
        btn.interactable = false;

        // Add overlay (check/X)
        Transform overlay = item.transform.Find("ResultIcon");
        overlay.gameObject.SetActive(true);

        Image iconImage = overlay.GetComponent<Image>();
        iconImage.color = correct ? Color.green : Color.red;

        // Ensure visuals are reset in case the item had a selection highlight
        item.ResetVisuals();
    }

    string GetPositiveFeedback(string itemName)
    {
        switch (itemName)
        {
            case "Life Jacket":
                return "Correct! Life jackets help keep everyone safe in the water.";
            case "Fire Extinguisher":
                return "Yes! Fires can happen on boats, so this is important.";
            case "Air Horn":
                return "Right! A whistle or horn helps signal and alert others.";
            case "Navigation Lights":
                return "Correct! Lights help other boats see you and tell which direction you are going.";
            case "Throw Ring":
                return "Exactly! A throw ring can help someone in the water.";
            default:
                return "Great choice!";
        }
    }

    string GetNegativeFeedback()
    {
        string[] responses =
        {
            "Oh no! That's not needed on the boat.",
            "Not quite. Try something else!",
            "That won't help keep us safe. Try again!"
        };

        return responses[UnityEngine.Random.Range(0, responses.Length)];
    }
}