using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class SpeechBubbleController : MonoBehaviour
{
    [Header("UI References")]
    [SerializeField] private GameObject rootPanel;     // the whole bubble/panel
    [SerializeField] private TextMeshProUGUI bodyText; // where the line is shown

    private readonly List<string> lines = new();
    private int index = -1;
    private bool playing;

    // Delegate broadcast
    public delegate void OnDialogueFinished();
    public static OnDialogueFinished onDialogueFinished;

    public void StartDialogue(List<string> newLines)
    {
        lines.Clear();
        if (newLines != null) lines.AddRange(newLines);

        index = -1;
        playing = true;

        rootPanel.SetActive(true);
        Next(); // immediately show first line
    }

    // Advances the text to the next or ends the dialogue
    public void Next()
    {
        if (!playing) return;

        index++;

        if (index >= lines.Count)
        {
            EndDialogue();
            return;
        }

        bodyText.text = lines[index];
    }

    private void EndDialogue()
    {
        playing = false;
        rootPanel.SetActive(false);

        onDialogueFinished?.Invoke();
    }
}
