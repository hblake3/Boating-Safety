using TMPro;
using UnityEngine;
using UnityEngine.TextCore.Text;
using UnityEngine.UI;
using static GameManager;

public class LifeJacketSelectionUI : MonoBehaviour
{
    public TextMeshProUGUI nameText;
    public TextMeshProUGUI detailsText;
    public FeedbackPopupUI feedbackPopup;
    public SelectionDialogueController dialogueController;
    public GameObject readyButton;
    public Image characterDisplayImage;

    [SerializeField] private Button[] jacketButtons;

    void Start()
    {
        // Hide ready button until a jacket is selected
        readyButton.SetActive(false);
    }

    public void SelectJacket(LifeJacketData jacket)
    {
        // store globally
        Instance.selectedJacket = jacket;


        // Update UI with jacket details
        nameText.text = jacket.jacketName;
        detailsText.text = jacket.description;
        dialogueController.OnJacketSelected(Instance.selectedCharacter);
        
        // Show ready button
        readyButton.SetActive(true);

    }

    public void ConfirmSelection()
    {
        var required = Instance.requiredJacket;
        var jacket = Instance.selectedJacket;
        //Debug.Log($"Required jacket: {required}, Selected jacket: {jacket.jacketType}");
        if (jacket.jacketType == required)
        {
            // Update character display image
            characterDisplayImage.sprite = jacket.fullbodysprite;
            dialogueController.HideSpeechBubble();

            // Prevent selecting another jacket until they click "Next"
            foreach (Button btn in jacketButtons)
            {
                btn.interactable = false;
            }


            feedbackPopup.ShowCorrect(
                "Correct!",
                $"This life jacket fits <i>{Instance.selectedCharacter.characterName}</i> " +
                $"because their age is <u>{Instance.selectedCharacter.age}</u>,\nand their weight is <u>{Instance.selectedCharacter.weight}</u>."
            );
        }
        else
        {
            dialogueController.HideSpeechBubble();

            // Prevent selecting another jacket until they click "Try Again"
            foreach (Button btn in jacketButtons)
            {
                btn.interactable = false;
            }

            feedbackPopup.ShowIncorrect(
                "Not Quite",
                $"This jacket is not a good fit.\nTry to remember <i>{Instance.selectedCharacter.characterName}</i>'s <u>size</u> and <u>weight</u>."
            );
        }
    }


    public void Next()
    {
        feedbackPopup.Close();

        dialogueController.OnNextClicked1(Instance.selectedCharacter);

        // Hide the life jacket selection panel
        gameObject.SetActive(false);
    }

    public void TryAgain()
    {
        // Hide feedback panel
        feedbackPopup.Close();

        // Re-enable jacket buttons
        foreach (Button btn in jacketButtons)
        {
            btn.interactable = true;
        }

        // Clear selected jacket
        Instance.selectedJacket = null;

        // Clear jacket details
        nameText.text = "Select a life jacket";
        detailsText.text = "";

        // Hide ready button until a jacket is selected again
        readyButton.SetActive(false);

        // Optional: reset dialogue
        dialogueController.OnReadyClicked(Instance.selectedCharacter);
    }
}
