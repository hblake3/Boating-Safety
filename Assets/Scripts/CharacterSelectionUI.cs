using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class CharacterSelectionUI : MonoBehaviour
{
    [Header("UI References")]
    public Image characterDisplayImage;
    public TextMeshProUGUI nameText;
    public TextMeshProUGUI detailsText;

    [Header("Default Character")]
    public CharacterData defaultCharacter;

    void Start()
    {
        // Initialize with default character
        SelectCharacter(defaultCharacter);
    }

    public void SelectCharacter(CharacterData character)
    {

        if (GameManager.Instance == null)
        {
            Debug.LogError("GameManager not found in scene!");
            return;
        }

        // Store globally
        GameManager.Instance.selectedCharacter = character;
        GameManager.Instance.requiredJacket = character.requiredJacket;

        // Update visuals
        characterDisplayImage.sprite = character.fullBodySprite;
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
        //    $"Requires: {character.requiredJacket}";
    }
}
