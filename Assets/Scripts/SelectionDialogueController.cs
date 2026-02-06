using UnityEngine;
using static GameManager;

public class SelectionDialogueController : MonoBehaviour
{
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
}
