using System.Collections.Generic;
using UnityEngine;

public class GameController : MonoBehaviour
{
    [Header("References")]
    [SerializeField] private SpeechBubbleController speech;
    [SerializeField] private QuizController quiz;
    [SerializeField] private LogDodgeControllerNew logDodge;
    [SerializeField] private NoWakeSegmentController noWakeSegment;

    private bool introDialogueDone = false;
    private bool navigationQuizDone = false;
    private bool logDodgeDone = false;
    private bool fogDialoguePart1Done = false;
    private bool fogDialoguePart2Done = false;
    private bool fogQuizDone = false;
    private bool fogGameplayDone = false;

    private bool noWakeDialogueDone = false;
    private bool noWakeQuizDone = false;
    private bool noWakeGameplayDone = false;

    public delegate void OnBoatingStarted();
    public static event OnBoatingStarted onBoatingStarted;

    public delegate void OnBoatingStopped();
    public static event OnBoatingStopped onBoatingStopped;

    public delegate void OnStartFog();
    public static event OnStartFog onStartFog;

    public delegate void OnClearFog();
    public static event OnClearFog onClearFog;

    // *** DEBUG STAGE SELECTION ***
    [SerializeField] private bool useDebugStart = false;
    [SerializeField] private DebugStartStage debugStartStage = DebugStartStage.NormalFlow;
    public enum DebugStartStage
    {
        NormalFlow,
        NavigationSpeech,
        NavigationQuiz,
        LogDodgeGameplay,
        FogSpeech,
        FogQuiz,
        FogGameplay,
        NoWakeSpeech,
        NoWakeQuiz,
        NoWakeGameplay
    }


    private void OnEnable()
    {
        SpeechBubbleController.onDialogueFinished += HandleSpeech;
        QuizController.onQuizPassed += HandleQuizPassed;
        LogDodgeControllerNew.onLogDodgeComplete += HandleLogDodgeComplete;
        LogDodgeControllerNew.onLogDodgeFogComplete += HandleFogGameplayComplete;
        FogController.onFogCleared += HandleFogCleared;
        NoWakeSegmentController.onNoWakeSegmentComplete += HandleNoWakeGameplayComplete;
    }

    private void OnDisable()
    {
        SpeechBubbleController.onDialogueFinished -= HandleSpeech;
        QuizController.onQuizPassed -= HandleQuizPassed;
        LogDodgeControllerNew.onLogDodgeComplete -= HandleLogDodgeComplete;
        LogDodgeControllerNew.onLogDodgeFogComplete -= HandleFogGameplayComplete;
        FogController.onFogCleared -= HandleFogCleared;
        NoWakeSegmentController.onNoWakeSegmentComplete -= HandleNoWakeGameplayComplete;
    }

    private void Start()
    {
        if (useDebugStart)
        {
            BeginAtStage(debugStartStage);
            return;
        }

        ShowIntroDialogue();
    }

    private void BeginAtStage(DebugStartStage stage)
    {
        ResetSegmentStateForDebug();

        switch (stage)
        {
            case DebugStartStage.NormalFlow:
            case DebugStartStage.NavigationSpeech:
                ShowIntroDialogue();
                break;

            case DebugStartStage.NavigationQuiz:
                introDialogueDone = true;
                ShowNavigationQuiz();
                break;

            case DebugStartStage.LogDodgeGameplay:
                introDialogueDone = true;
                navigationQuizDone = true;
                StartLogDodge();
                break;

            case DebugStartStage.FogSpeech:
                introDialogueDone = true;
                navigationQuizDone = true;
                logDodgeDone = true;
                ShowFogDialoguePart1();
                break;

            case DebugStartStage.FogQuiz:
                introDialogueDone = true;
                navigationQuizDone = true;
                logDodgeDone = true;
                fogDialoguePart1Done = true;
                ShowFogQuiz();
                break;

            case DebugStartStage.FogGameplay:
                introDialogueDone = true;
                navigationQuizDone = true;
                logDodgeDone = true;
                fogDialoguePart1Done = true;
                fogDialoguePart2Done = true;
                onStartFog?.Invoke();
                StartFogGameplay();
                break;

            case DebugStartStage.NoWakeSpeech:
                introDialogueDone = true;
                navigationQuizDone = true;
                logDodgeDone = true;
                fogDialoguePart1Done = true;
                fogDialoguePart2Done = true;
                fogQuizDone = true;
                fogGameplayDone = true;
                ShowNoWakeDialogue();
                break;

            case DebugStartStage.NoWakeQuiz:
                introDialogueDone = true;
                navigationQuizDone = true;
                logDodgeDone = true;
                fogDialoguePart1Done = true;
                fogDialoguePart2Done = true;
                fogQuizDone = true;
                fogGameplayDone = true;
                noWakeDialogueDone = true;
                ShowNoWakeQuiz();
                break;

            case DebugStartStage.NoWakeGameplay:
                introDialogueDone = true;
                navigationQuizDone = true;
                logDodgeDone = true;
                fogDialoguePart1Done = true;
                fogDialoguePart2Done = true;
                fogQuizDone = true;
                fogGameplayDone = true;
                noWakeDialogueDone = true;
                noWakeQuizDone = true;
                StartNoWakeGameplay();
                break;
        }
    }

    private void ResetSegmentStateForDebug()
    {
        introDialogueDone = false;
        navigationQuizDone = false;
        logDodgeDone = false;
        fogDialoguePart1Done = false;
        fogDialoguePart2Done = false;
        fogQuizDone = false;
        fogGameplayDone = false;
        noWakeDialogueDone = false;
        noWakeQuizDone = false;
        noWakeGameplayDone = false;

        onBoatingStopped?.Invoke();
        onClearFog?.Invoke(); // if you already added this event
    }

    private void HandleSpeech()
    {
        if (!introDialogueDone)
        {
            introDialogueDone = true;
            ShowNavigationQuiz();
            return;
        }

        if (logDodgeDone && !fogDialoguePart1Done)
        {
            fogDialoguePart1Done = true;
            onStartFog?.Invoke();
            ShowFogDialoguePart2();
            return;
        }

        if (fogDialoguePart1Done && !fogDialoguePart2Done)
        {
            fogDialoguePart2Done = true;
            ShowFogQuiz();
            return;
        }

        if (fogGameplayDone && !noWakeDialogueDone)
        {
            noWakeDialogueDone = true;
            ShowNoWakeQuiz();
            return;
        }
    }

    private void HandleQuizPassed()
    {
        if (introDialogueDone && !navigationQuizDone)
        {
            navigationQuizDone = true;
            StartLogDodge();
            return;
        }

        if (fogDialoguePart2Done && !fogQuizDone)
        {
            fogQuizDone = true;
            StartFogGameplay();
            return;
        }

        if (noWakeDialogueDone && !noWakeQuizDone)
        {
            noWakeQuizDone = true;
            StartNoWakeGameplay();
            return;
        }
    }

    private void HandleLogDodgeComplete()
    {
        onBoatingStopped?.Invoke();

        logDodgeDone = true;
        ShowFogDialoguePart1();
    }

    private void HandleFogCleared()
    {
        ShowNoWakeDialogue();
    }

    private void HandleFogGameplayComplete()
    {
        onBoatingStopped?.Invoke();

        fogGameplayDone = true;

        onClearFog?.Invoke();
    }

    private void HandleNoWakeGameplayComplete()
    {
        onBoatingStopped?.Invoke();

        noWakeGameplayDone = true;
        ShowPostNoWakeDialogue();
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

    private void ShowNoWakeDialogue()
    {
        speech.StartDialogue(new List<string>
        {
            "Nice work making it through the fog!",
            "Up ahead is a \"No Wake\" zone.",
            "What is a wake, you ask?",
            "A \"wake\" is the waves and ripples a boat makes as it moves through the water.",
            "Big wakes can rock other boats and make the water unsafe for everyone.",
            "In a No Wake zone, you must drive slowly to avoid creating large wakes!",
            "That way everyone can enjoy the lake safely together!"
        });
    }

    private void ShowNoWakeQuiz()
    {
        QuizQuestionData question = new QuizQuestionData
        {
            question = "What should you do when you enter a No Wake zone?",
            answers = new string[3]
            {
                "Go slowly so you don’t make big waves.",
                "Drive fast to get through the No Wake zone quickly.",
                "Go to sleep - it is a \"No Wake\" zone, afterall!"
            },
            correctAnswerIndex = 0,
            correctFeedback = "Correct! In a No Wake zone you should drive slowly.",
            incorrectFeedback = "Not quite. Try again!"
        };

        quiz.ShowQuiz(question);
    }

    private void StartNoWakeGameplay()
    {
        onBoatingStarted?.Invoke();
        noWakeSegment.StartSegment();
    }

    private void ShowPostNoWakeDialogue()
    {
        speech.StartDialogue(new List<string>
    {
        "Nice work through the No Wake zone!",
        "You slowed down and passed the docks safely.",
        "Next, you'll learn about another important boating marker.",
        "Let's keep going!"
    });
    }
}