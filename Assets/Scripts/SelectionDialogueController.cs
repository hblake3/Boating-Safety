using UnityEngine;
using static GameManager;

public class SelectionDialogueController : MonoBehaviour
{

    public SpeechBubbleUI speechBubble;
    public RectTransform characterRect;

    public void OnCharacterSelected(CharacterData character)
    {
        speechBubble.Show(
            $"Hi! I'm {character.characterName}.\nClick the <color=#5c9e43>green</color> button if you're ready to go boating!",
            character.speechBubbleAnchoredPosition
        );
    }

    public void OnReadyClicked(CharacterData character)
    {
        speechBubble.Show(
            "First, I need a life jacket!\nWhich one is the right one for me?",
            character.speechBubbleAnchoredPosition
        );
    }

    public void OnJacketSelected(CharacterData character)
    {
        speechBubble.Show(
            "Are you sure?\nClick the check if you are!",
            character.speechBubbleAnchoredPosition
        );
    }

    // Called after confirming the correct jacket; transition to the inspection process
    public void OnNextClicked1(CharacterData character)
    {
       speechBubble.Show(
            "Great job! Now lets make sure my life jacket is safe and secure!",
            character.speechBubbleAnchoredPosition
       );

        speechBubble.ShowNextButton();

    }

    public void HideSpeechBubble()
    {
        speechBubble.Hide();
    }

    // Called after clicking the next button on the speech bubble; transition to the safety gear process
    public void OnNextClicked2(CharacterData character)
    {
        speechBubble.Show(
            "Now, let's grab some gear to take on the boat with us!",
            character.speechBubbleAnchoredPosition
        );

        speechBubble.ShowNextButton2();

    }
}
