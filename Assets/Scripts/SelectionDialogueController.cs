using UnityEngine;
using static GameManager;

public class SelectionDialogueController : MonoBehaviour
{
    public enum SelectionState
    {
        CharacterSelection,
        ReadyPrompt,
        LifeJacketSelection,
        ConfirmJacket
    }

    public SpeechBubbleUI speechBubble;
    public RectTransform characterRect;

    private SelectionState currentState;

    public void OnCharacterSelected(CharacterData character)
    {
        currentState = SelectionState.ReadyPrompt;

        speechBubble.Show(
            $"Hi! I'm {character.characterName}.\nClick the <color=#5c9e43>green</color> button if you're ready to go boating!",
            character.speechBubbleAnchoredPosition
        );
    }

    public void OnReadyClicked(CharacterData character)
    {
        currentState = SelectionState.LifeJacketSelection;

        speechBubble.Show(
            "First, I need a life jacket!\nWhich one is the right one for me?",
            character.speechBubbleAnchoredPosition
        );
    }

    public void OnJacketSelected(CharacterData character)
    {
        currentState = SelectionState.ConfirmJacket;

        speechBubble.Show(
            "Are you sure?\nClick the check if you are!",
            character.speechBubbleAnchoredPosition
        );
    }

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
}
