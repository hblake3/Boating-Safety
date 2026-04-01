using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class CharacterSelectionUI : MonoBehaviour
{
    [Header("UI References")]
    public Image characterDisplayImage;
    public TextMeshProUGUI nameText;
    public TextMeshProUGUI detailsText;
    public GameObject readyButton;
    public GameObject CharDisplay;
    public GameObject CharSelectionPanel;
    public GameObject LifeJacketSelectionPanel;
    public GameObject TopBar;
    public GameObject speechBubbleUI;

    public SelectionDialogueController dialogueController;

    //[Header("Default Character")]
    //public CharacterData defaultCharacter;

    void Start()
    {
        speechBubbleUI.SetActive(false);
        CharSelectionPanel.SetActive(true);
        LifeJacketSelectionPanel.SetActive(false);
        readyButton.SetActive(false);
        characterDisplayImage.enabled = false;
        nameText.text = "Select a character";
        detailsText.text = "";
    }

    public void SelectCharacter(CharacterData character)
    {

        if (GameManager.Instance == null)
        {
            return;
        }

        // Store globally
        GameManager.Instance.selectedCharacter = character;
        GameManager.Instance.requiredJacket = character.requiredJacket;

        // Update visuals
        characterDisplayImage.sprite = character.fullBodySprite;
        CharDisplay.SetActive(true);
        characterDisplayImage.enabled = true;
        // Apply size
        RectTransform rt = characterDisplayImage.rectTransform;
        rt.sizeDelta = character.displaySize;
        // Apply position
        rt.anchoredPosition = character.displayPosition;


        // Update text
        nameText.text = character.characterName;
        detailsText.text =
            $"Age: {character.age}\n" +
            $"Weight: {character.weight} lbs\n";
        
        // Show ready button
        readyButton.SetActive(true);
        
        // Dialogue change event
        dialogueController.OnCharacterSelected(character);

    }

    public void OnReadyButtonPressed()
    {
        // Transition to Life Jacket Selection
        CharSelectionPanel.SetActive(false);
        LifeJacketSelectionPanel.SetActive(true);
        TopBar.SetActive(false);

        // Dialogue change event
        dialogueController.OnReadyClicked(GameManager.Instance.selectedCharacter);

    }
}
