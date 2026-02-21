using System.Collections.Generic;
using UnityEngine;

public class GameController : MonoBehaviour
{
    [Header("References")]
    [SerializeField] private ScoreController scoreController;
    [SerializeField] private SpeechBubbleController speech;
    [SerializeField] private LogDodgeControllerNew logDodge;

    // [ DELEGATES ]
    public delegate void OnBoatingStarted();
    public static OnBoatingStarted onBoatingStarted;

    public delegate void OnBoatingStopped();
    public static OnBoatingStopped onBoatingStopped;

    private void OnEnable()
    {
        SpeechBubbleController.onDialogueFinished += StartLogDodge;
        LogDodgeControllerNew.onLogDodgeComplete += ShowPostLogDialogue;
    }

    private void OnDisable()
    {
        SpeechBubbleController.onDialogueFinished -= StartLogDodge;
        LogDodgeControllerNew.onLogDodgeComplete -= ShowPostLogDialogue;
    }

    private void Start()
    {
        ShowIntroDialogue();
    }

    private void ShowIntroDialogue()
    {
        speech.StartDialogue(new List<string>
        {
            "It's time to hit the open waters and earn your captain's badge!",
            "But first, you'll have to show how much you know about boating safety.",
            "We'll cover topics such as obstacle avoidance, weather conditions, ...",
            "... proper speed control, marker recognition, and ...",
            "... how to safely pass other boaters. How exciting!",
            "What's that? You don't know how to navigate the boat?",
            "Not a worry! Let's go over your controls.",
            "The left and right arrow buttons will steer the boat.",
            "And the up and down arrow buttons will control your speed.",
            "Remember to always drive at a safe speed!",
            "That's it! You have everything you need to get started!",
            "Uh oh! I see some logs in the water up ahead.",
            "This is the perfect way to show off your boating safety skills.",
            "Let's see how well you can avoid the obstacles ahead!"
        });
    }

    private void StartLogDodge()
    {
        // start the log segment and begin incrementing the score
        logDodge.StartSegment();

        // broadcast the boating has started
        BroadcastBoatingStarted();
    }

    private void ShowPostLogDialogue()
    {
        BroadcastBoatingStopped();

        speech.StartDialogue(new List<string>
    {
        "Great ",
        "Fog will make hazards harder to see.",
        "Reduce speed and proceed carefully."
    });
    }

    private void BroadcastBoatingStarted()
    {
        onBoatingStarted?.Invoke();
    }

    private void BroadcastBoatingStopped()
    {
        onBoatingStopped?.Invoke();
    }

}
