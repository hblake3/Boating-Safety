using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class SafetyInspectionManager : MonoBehaviour
{
    [Header("UI Elements")]
    [SerializeField] private GameObject speechBubble;
    [SerializeField] private TextMeshProUGUI dialogueText;
    [SerializeField] private Button nextArrowButton;

    [Header("Buddy Steps")]
    [SerializeField] private GameObject buddyStrapStep;
    [SerializeField] private GameObject buddyEarStep;

    [Header("Barry Steps")]
    [SerializeField] private GameObject barryStrapStep;
    [SerializeField] private GameObject barryEarStep;

    public SceneFlowController sceneFlowController;

    // Determine the current step of the inspection (straps vs ear) to know what dialogue to show
    public enum InspectionStep
    {
        Straps,
        Ear
    }
    public InspectionStep currentStep;

    // Create list of responses for wrong answers for each step to add some variety to the feedback
    List<string> wrongStrapResponses = new List<string>()
    {
        "Uh oh, that doesn't look right. Try again.",
        "Not quite! Look for the straps that should be snug around my body.",
        "Hmm, that star isn't in the right spot. Remember, the straps should be tight and snug around my body!",
        "That's not it! Try looking a little closer at my body and finding the straps that should be snug around me.",
    };
    List<string> wrongEarResponses = new List<string>()
    {
        "That's not it! Try looking a little higher.",
        "Hmm, that doesn't look quite right. Try again!",
        "Not quite!",
        "Uh oh, that star isn't in the right spot. Remember, the life jacket should NOT be above my ears when I lift my arms up!"
    };


    void Start()
    {
        currentStep = InspectionStep.Straps;

        speechBubble.SetActive(false);
        // Ensure the speech bubble starts at the top of the screen and is rotated(flipped) to (0,0,0) for the straps question
        speechBubble.GetComponent<RectTransform>().anchoredPosition = new Vector2(0, 400);
        speechBubble.GetComponent<RectTransform>().localRotation = Quaternion.Euler(0, 0, 0);

        // Ensure the dialogue text is also rotated back to (0,0,0) in case it was flipped during the ear step
        dialogueText.GetComponent<RectTransform>().localRotation = Quaternion.Euler(0, 0, 0);

        // Start with all hotspots and next button disabled until the inspection is triggered
        buddyStrapStep.SetActive(false);
        buddyEarStep.SetActive(false);
        barryStrapStep.SetActive(false);
        barryEarStep.SetActive(false);

        nextArrowButton.gameObject.SetActive(false);

        // Hide the entire inspection UI until the inspection is triggered
        this.gameObject.SetActive(false);
    }

    public void WrongSelected(InspectionHotspot hotspot)
    {
        // Show a variety of wrong responses to keep the feedback feeling fresh
        if (currentStep == InspectionStep.Straps)
        {
            dialogueText.text = wrongStrapResponses[Random.Range(0, wrongStrapResponses.Count)];
        }
        else if (currentStep == InspectionStep.Ear)
        {
            dialogueText.text = wrongEarResponses[Random.Range(0, wrongEarResponses.Count)];
        }

        hotspot.ShowRedX();
        hotspot.DisableHotspot();
    }

    public void CorrectSelected(InspectionHotspot hotspot)
    {
        if (currentStep == InspectionStep.Straps)
        {
            dialogueText.text =
            "Great job! Always make sure all straps on the lifejacket are tight and snug!";
        }
        else if (currentStep == InspectionStep.Ear)
        {
            dialogueText.text =
            "That's right! If the life jacket is above the ears, it's too big!";
        }

        nextArrowButton.gameObject.SetActive(true);
        hotspot.DisableHotspot();
    }

    public void StartInspection()
    {
        // trigger the first inspection question (straps)
        this.gameObject.SetActive(true);
        speechBubble.SetActive(true);
        if (GameManager.Instance.selectedCharacter.characterName == "Buddy the Beaver")
        {
            buddyStrapStep.SetActive(true);
        }
        else
        {
            barryStrapStep.SetActive(true);
        }
        dialogueText.text =
                "Let's start by checking if my life jacket is properly secured. " +
                "Can you find the straps on my life jacket? (Click the <color=#92d3f5>star</color> that is in the right spot.)";
    }

    public void OnNextPressed()
    {
        if (currentStep == InspectionStep.Straps)
        {
            currentStep = InspectionStep.Ear;

            // trigger the next inspection question (ear)
            nextArrowButton.gameObject.SetActive(false);

            // Flip the speech bubble and move it down to the bottom of the screen for better visibility of the character's ear
            speechBubble.GetComponent<RectTransform>().localRotation = Quaternion.Euler(0, 0, 180);
            speechBubble.GetComponent<RectTransform>().anchoredPosition = new Vector2(0, -400);
            // flip the dialogue text
            dialogueText.GetComponent<RectTransform>().localRotation = Quaternion.Euler(0, 0, 180);
            // move next button to bottom of screen and rotate it to match the flipped speech bubble
            nextArrowButton.GetComponent<RectTransform>().anchoredPosition = new Vector2(-750, 60);
            nextArrowButton.GetComponent<RectTransform>().localRotation = Quaternion.Euler(0, 0, 180);

            // Swap out hotspots and update dialogue for the ear strap step
            if (GameManager.Instance.selectedCharacter.characterName == "Buddy the Beaver")
            {
                buddyStrapStep.SetActive(false);
                buddyEarStep.SetActive(true);
            }
            else
            {
                barryStrapStep.SetActive(false);
                barryEarStep.SetActive(true);
            }
            dialogueText.text =
                    "Now let’s check if the life jacket really fits me!\r\n" +
                    "When I lift my arms up, the life jacket should NOT move above my…? ";
        }
        else if (currentStep == InspectionStep.Ear)
        {
            // End the inspection and reset the UI

            speechBubble.SetActive(false);
            if (GameManager.Instance.selectedCharacter.characterName == "Buddy the Beaver")
            {
                buddyEarStep.SetActive(false);
            }
            else
            {
                barryEarStep.SetActive(false);
            }
            dialogueText.text = "";

            sceneFlowController.ExitInspection();

        }

    }
}