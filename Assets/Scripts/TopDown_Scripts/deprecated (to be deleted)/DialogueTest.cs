using System.Collections.Generic;
using UnityEngine;

public class DialogueTest : MonoBehaviour
{
    [SerializeField] SpeechBubbleController speech;

    void Start()
    {
        speech.StartDialogue(new List<string>()
        {
            "You are entering an obstacle field.",
            "Avoid the logs.",
            "Stay in safe lanes."
        });
    }
}
