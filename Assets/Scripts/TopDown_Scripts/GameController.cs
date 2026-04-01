using System.Collections.Generic;
using UnityEngine;

public class GameController : MonoBehaviour
{
    [Header("References")]
    [SerializeField] private SpeechBubbleController speech;
    [SerializeField] private QuizController quiz;
    [SerializeField] private LogDodgeControllerNew logDodge;

    private bool introDialogueDone = false;
    private bool navigationQuizDone = false;
    private bool logDodgeDone = false;
    private bool fogDialoguePart1Done = false;
    private bool fogDialoguePart2Done = false;
    private bool fogQuizDone = false;
    private bool fogGameplayDone = false;

    public delegate void OnBoatingStarted();
    public static event OnBoatingStarted onBoatingStarted;

    public delegate void OnBoatingStopped();
    public static event OnBoatingStopped onBoatingStopped;

    public delegate void OnStartFog();
    public static event OnStartFog onStartFog;

    private void OnEnable()
    {
        SpeechBubbleController.onDialogueFinished += HandleSpeech;
        QuizController.onQuizPassed += HandleQuizPassed;
        LogDodgeControllerNew.onLogDodgeComplete += HandleLogDodgeComplete;
        LogDodgeControllerNew.onLogDodgeFogComplete += HandleFogGameplayComplete;
    }

    private void OnDisable()
    {
        SpeechBubbleController.onDialogueFinished -= HandleSpeech;
        QuizController.onQuizPassed -= HandleQuizPassed;
        LogDodgeControllerNew.onLogDodgeComplete -= HandleLogDodgeComplete;
        LogDodgeControllerNew.onLogDodgeFogComplete -= HandleFogGameplayComplete;
    }

    private void Start()
    {
        ShowIntroDialogue();
    }

    private void HandleSpeech()
    {
        // Intro dialogue finished -> show navigation quiz
        if (!introDialogueDone)
        {
            introDialogueDone = true;
            ShowNavigationQuiz();
            return;
        }

        // Fog dialogue part 1 finished -> turn on fog, then continue speech
        if (logDodgeDone && !fogDialoguePart1Done)
        {
            fogDialoguePart1Done = true;
            onStartFog?.Invoke();
            ShowFogDialoguePart2();
            return;
        }

        // Fog dialogue part 2 finished -> show fog quiz
        if (fogDialoguePart1Done && !fogDialoguePart2Done)
        {
            fogDialoguePart2Done = true;
            ShowFogQuiz();
            return;
        }
    }

    private void HandleQuizPassed()
    {
        // Navigation quiz passed -> start first gameplay segment
        if (introDialogueDone && !navigationQuizDone)
        {
            navigationQuizDone = true;
            StartLogDodge();
            return;
        }

        // Fog quiz passed -> start fog gameplay segment
        if (fogDialoguePart2Done && !fogQuizDone)
        {
            fogQuizDone = true;
            StartFogGameplay();
            return;
        }
    }

    private void HandleLogDodgeComplete()
    {
        onBoatingStopped?.Invoke();

        logDodgeDone = true;
        ShowFogDialoguePart1();
    }

    private void HandleFogGameplayComplete()
    {
        onBoatingStopped?.Invoke();

        fogGameplayDone = true;

        // Later: clear fog and move to buoy speech/quiz/gameplay
    }

    private void ShowIntroDialogue()
    {
        speech.StartDialogue(new List<string>
        {
            "It's time to hit the open waters and earn your Captain's Badge!",
            "To get your badge, we'll answer questions and also tackle some driving tests.",
            "What's that? You don't know how to drive the boat?",
            "Not a worry! Let's go over your controls!",
            "The left and right arrow buttons will steer the boat.",
            "And the up and down arrow buttons will control your speed.",
            "Remember to always drive at a safe speed!",
            "Alright, you have everything you need to get started!",
            "We'll begin with your first safety quiz.",
            "After that, we'll put your driving skills to the test by avoiding obstacles!",
            "Let's see what you know!"
        });
    }

    private void ShowNavigationQuiz()
    {
        QuizQuestionData question = new QuizQuestionData
        {
            question = "What can we do to make sure we are boating safely?",
            answers = new string[3]
            {
                "Drive as fast as possible.",
                "Drive at a safe speed.",
                "Ask the water nicely to stop making waves."
            },
            correctAnswerIndex = 1,
            correctFeedback = "Correct! Always drive at a safe speed!",
            incorrectFeedback = "Not quite. Try again!"
        };

        quiz.ShowQuiz(question);
    }

    private void StartLogDodge()
    {
        onBoatingStarted?.Invoke();
        logDodge.StartSegment();
    }

    private void ShowFogDialoguePart1()
    {
        speech.StartDialogue(new List<string>
        {
            "Great work navigating those obstacles!",
            "Uh oh... do you sense a change in the weather?"
        });
    }

    private void ShowFogDialoguePart2()
    {
        speech.StartDialogue(new List<string>
        {
            "Fog can make it much harder to react in time.",
            "Stay alert and continue boating safely."
        });
    }

    private void ShowFogQuiz()
    {
        QuizQuestionData question = new QuizQuestionData
        {
            question = "What should you do when visibility is poor?",
            answers = new string[3]
            {
                "Speed up to get through faster.",
                "Ask a seagull to fly ahead and be your guide.",
                "Slow down and stay alert."
            },
            correctAnswerIndex = 2,
            correctFeedback = "Correct! Slow down during poor visibility!",
            incorrectFeedback = "Not quite. Try again!"
        };

        quiz.ShowQuiz(question);
    }

    private void StartFogGameplay()
    {
        onBoatingStarted?.Invoke();
        logDodge.StartFogSegment();
    }
}