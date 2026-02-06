using UnityEngine;
using TMPro;

public class SpeechBubbleUI : MonoBehaviour
{
    public RectTransform bubbleRect;
    public TextMeshProUGUI bubbleText;

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
}
