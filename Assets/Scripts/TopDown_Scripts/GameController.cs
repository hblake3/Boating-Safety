using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class GameController : MonoBehaviour
{
    [Header("References")]
    [SerializeField] private SpeechBubbleController speech;
    [SerializeField] private QuizController quiz;
    [SerializeField] private LogDodgeControllerNew logDodge;
    [SerializeField] private NoWakeSegmentController noWakeSegment;
    [SerializeField] private BoatsSegmentController boatsSegment;
    [SerializeField] private BoatController playerBoatController;
    [SerializeField] private Transform playerBoatTransform;
    [SerializeField] private GameObject gamePlayUIElements;
    [SerializeField] private GameObject endGameDisplay;
    [SerializeField] private CanvasGroup blackPanelForFadeFX;
    [SerializeField] private ScoreController scoreController;
    [SerializeField] private TextMeshProUGUI endGameScoreText;
    [SerializeField] private float endGameScoreCountDuration = 2.5f;

    // end-game variables & fields
    [SerializeField] private float boatExitSpeed = 12f;
    [SerializeField] private float boatExitTargetY = 14f;
    [SerializeField] private float blackFadeDuration = 1.25f;
    [SerializeField] private Image badge;
    [SerializeField] private Sprite captainsBadge;
    [SerializeField] private Sprite firstMateBadge;
    [SerializeField] private Sprite guppyBadge;
    [SerializeField] private GameObject sunburstFX;
    [SerializeField] private float badgePopScale = 1.15f;
    [SerializeField] private float badgePopDuration = 0.35f;
    [SerializeField] private float badgeColorFadeDuration = 0.25f;
    [SerializeField] private TextMeshProUGUI portraitSpeechBubbleText;
    private bool endSequenceStarted = false;
    private Coroutine endSequenceRoutine;
    [SerializeField] private float badgeSettleScale = 1.15f;
    [SerializeField] private float noBadgePopScale = 1.1f;
    [SerializeField] private float noBadgeSettleScale = 0.92f;
    [SerializeField] private float badgeSettleDuration = 0.2f;
    [SerializeField] private GameObject mainMenuButton;
    [SerializeField] private float mainMenuButtonDelay = 3f;

    private bool introDialogueDone = false;
    private bool navigationQuizDone = false;
    private bool logDodgeDone = false;
    private bool fogDialoguePart1Done = false;
    private bool fogDialoguePart2Done = false;
    private bool fogQuizDone = false;
    private bool fogGameplayDone = false;
    private bool finalReturnDialogueDone = false;
    private bool noWakeDialogueDone = false;
    private bool noWakeQuizDone = false;
    private bool noWakeGameplayDone = false;
    private bool otherBoatersDialogueDone = false;
    private bool otherBoatersQuizDone = false;
    private bool otherBoatersGameplayDone = false;

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
        NoWakeGameplay,
        OtherBoatersSpeech,
        OtherBoatersQuiz,
        OtherBoatersGameplay
    }


    private void OnEnable()
    {
        SpeechBubbleController.onDialogueFinished += HandleSpeech;
        QuizController.onQuizPassed += HandleQuizPassed;
        LogDodgeControllerNew.onLogDodgeComplete += HandleLogDodgeComplete;
        LogDodgeControllerNew.onLogDodgeFogComplete += HandleFogGameplayComplete;
        FogController.onFogCleared += HandleFogCleared;
        NoWakeSegmentController.onNoWakeSegmentComplete += HandleNoWakeGameplayComplete;
        BoatsSegmentController.onBoatsSegmentComplete += HandleOtherBoatersGameplayComplete;
    }

    private void OnDisable()
    {
        SpeechBubbleController.onDialogueFinished -= HandleSpeech;
        QuizController.onQuizPassed -= HandleQuizPassed;
        LogDodgeControllerNew.onLogDodgeComplete -= HandleLogDodgeComplete;
        LogDodgeControllerNew.onLogDodgeFogComplete -= HandleFogGameplayComplete;
        FogController.onFogCleared -= HandleFogCleared;
        NoWakeSegmentController.onNoWakeSegmentComplete -= HandleNoWakeGameplayComplete;
        BoatsSegmentController.onBoatsSegmentComplete -= HandleOtherBoatersGameplayComplete;
    }

    private void Start()
    {
        // use the selected debug stage for testing a specific stage
        if (useDebugStart)
        {
            BeginAtStage(debugStartStage);
            return;
        }

        // initializers for end-game scenario
        if (blackPanelForFadeFX != null)
        {
            blackPanelForFadeFX.gameObject.SetActive(false);
            blackPanelForFadeFX.alpha = 0f;
        }
        if (endGameDisplay != null)
            endGameDisplay.SetActive(false);
        if (endGameScoreText != null)
            endGameScoreText.text = "0";
        if (sunburstFX != null)
            sunburstFX.SetActive(false);
        if (mainMenuButton != null)
            mainMenuButton.SetActive(false);

        ShowIntroDialogue();
    }

    private void BeginAtStage(DebugStartStage stage)
    {
        // clear old progress flags before jumping into a debug stage
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

            case DebugStartStage.OtherBoatersSpeech:
                introDialogueDone = true;
                navigationQuizDone = true;
                logDodgeDone = true;
                fogDialoguePart1Done = true;
                fogDialoguePart2Done = true;
                fogQuizDone = true;
                fogGameplayDone = true;
                noWakeDialogueDone = true;
                noWakeQuizDone = true;
                noWakeGameplayDone = true;
                ShowPostNoWakeDialogue();
                break;

            case DebugStartStage.OtherBoatersQuiz:
                introDialogueDone = true;
                navigationQuizDone = true;
                logDodgeDone = true;
                fogDialoguePart1Done = true;
                fogDialoguePart2Done = true;
                fogQuizDone = true;
                fogGameplayDone = true;
                noWakeDialogueDone = true;
                noWakeQuizDone = true;
                noWakeGameplayDone = true;
                otherBoatersDialogueDone = true;
                ShowOtherBoatersQuiz();
                break;

            case DebugStartStage.OtherBoatersGameplay:
                introDialogueDone = true;
                navigationQuizDone = true;
                logDodgeDone = true;
                fogDialoguePart1Done = true;
                fogDialoguePart2Done = true;
                fogQuizDone = true;
                fogGameplayDone = true;
                noWakeDialogueDone = true;
                noWakeQuizDone = true;
                noWakeGameplayDone = true;
                otherBoatersDialogueDone = true;
                otherBoatersQuizDone = true;
                StartOtherBoatersGameplay();
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
        otherBoatersDialogueDone = false;
        otherBoatersQuizDone = false;
        otherBoatersGameplayDone = false;

        // reset active gameplay pieces before starting from a debug point
        onBoatingStopped?.Invoke();
        onClearFog?.Invoke();
    }

    private void HandleSpeech()
    {
        // move from the intro speech into the first quiz
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

        if (noWakeGameplayDone && !otherBoatersDialogueDone)
        {
            otherBoatersDialogueDone = true;
            ShowOtherBoatersQuiz();
            return;
        }

        // after the final dialogue move into the end game flow
        if (otherBoatersGameplayDone && !finalReturnDialogueDone)
        {
            finalReturnDialogueDone = true;
            StartEndGameSequence();
            return;
        }
    }

    private void HandleQuizPassed()
    {
        // each quiz pass starts the next gameplay segment
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

        if (otherBoatersDialogueDone && !otherBoatersQuizDone)
        {
            otherBoatersQuizDone = true;
            StartOtherBoatersGameplay();
            return;
        }
    }

    private void HandleOtherBoatersGameplayComplete()
    {
        onBoatingStopped?.Invoke();

        otherBoatersGameplayDone = true;
        ShowFinalReturnDialogue();
    }

    private void ShowFinalReturnDialogue()
    {
        speech.StartDialogue(new List<string>
    {
        "Good job navigating on the lake!",
        "Let's head back to the docks and see how you did!"
    });
    }

    private void StartEndGameSequence()
    {
        if (endSequenceStarted)
            return;

        endSequenceStarted = true;

        if (endSequenceRoutine != null)
            StopCoroutine(endSequenceRoutine);

        endSequenceRoutine = StartCoroutine(RunEndGameSequence());
    }

    private IEnumerator RunEndGameSequence()
    {
        if (playerBoatController != null)
            playerBoatController.LockControls();

        while (playerBoatTransform != null && playerBoatTransform.position.y < boatExitTargetY)
        {
            playerBoatTransform.position += Vector3.up * boatExitSpeed * Time.deltaTime;
            yield return null;
        }

        if (blackPanelForFadeFX != null)
        {
            blackPanelForFadeFX.gameObject.SetActive(true);
            blackPanelForFadeFX.alpha = 0f;
        }

        if (gamePlayUIElements != null)
            gamePlayUIElements.SetActive(false);

        // Fade fully to black
        if (blackPanelForFadeFX != null)
        {
            float elapsed = 0f;

            while (elapsed < blackFadeDuration)
            {
                elapsed += Time.deltaTime;
                blackPanelForFadeFX.alpha = Mathf.Clamp01(elapsed / blackFadeDuration);
                yield return null;
            }

            blackPanelForFadeFX.alpha = 1f;
        }

        // switch to end game UI while the screen is fully black
        if (endGameDisplay != null)
            endGameDisplay.SetActive(true);
        // reset the score count up text field
        if (endGameScoreText != null)
            endGameScoreText.text = "0";

        // Then fade back out to reveal the end screen
        if (blackPanelForFadeFX != null)
        {
            float elapsed = 0f;

            while (elapsed < blackFadeDuration)
            {
                elapsed += Time.deltaTime;
                blackPanelForFadeFX.alpha = 1f - Mathf.Clamp01(elapsed / blackFadeDuration);
                yield return null;
            }

            blackPanelForFadeFX.alpha = 0f;
        }

        yield return StartCoroutine(CountUpEndGameScore());
        yield return StartCoroutine(RevealEndBadge());

        yield return new WaitForSeconds(mainMenuButtonDelay);
        if (mainMenuButton != null)
            mainMenuButton.SetActive(true);
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

        // fog controller fades the screen back out
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
        // first safety check before the player starts driving
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

    private void ShowOtherBoatersQuiz()
    {
        QuizQuestionData question = new QuizQuestionData
        {
            question = "If another boat is coming straight toward you, which way should you steer?",
            answers = new string[3]
            {
            "Steer to the right.",
            "Steer to the left.",
            "Close your eyes and hope for the best."
            },
            correctAnswerIndex = 0,
            correctFeedback = "Correct! Both boaters should steer to the right.",
            incorrectFeedback = "Not quite. Try again!"
        };

        quiz.ShowQuiz(question);
    }

    private void StartNoWakeGameplay()
    {
        onBoatingStarted?.Invoke();
        noWakeSegment.StartSegment();
    }

    private void StartOtherBoatersGameplay()
    {
        onBoatingStarted?.Invoke();
        boatsSegment.StartSegment();
    }

    private void ShowPostNoWakeDialogue()
    {
        speech.StartDialogue(new List<string>
    {
        "Nice work through the No Wake zone!",
        "Now let's practice what to do when you meet other boaters.",
        "When boats are coming toward each other head-on, each boater should steer to the right.",
        "Let's make sure you know the rule before we continue."
    });
    }

    // count up the score in the end-game screen
    private IEnumerator CountUpEndGameScore()
    {
        if (scoreController == null || endGameScoreText == null)
            yield break;

        int finalScore = scoreController.score;
        float elapsed = 0f;

        while (elapsed < endGameScoreCountDuration)
        {
            elapsed += Time.deltaTime;

            float t = Mathf.Clamp01(elapsed / endGameScoreCountDuration);
            int displayedScore = Mathf.RoundToInt(Mathf.Lerp(0f, finalScore, t));

            endGameScoreText.text = displayedScore.ToString("N0");
            yield return null;
        }

        endGameScoreText.text = finalScore.ToString("N0");
    }

    private IEnumerator RevealEndBadge()
    {
        if (scoreController == null || badge == null)
            yield break;

        int finalScore = scoreController.score;
        string badgeMessage = "";
        bool earnedBadge = true;

        // determine the text and sprite based on user's score
        if (finalScore > 3000)
        {
            badge.sprite = captainsBadge;
            badgeMessage = "Wow! You earned the Captain's Badge! Perfect!";
        }
        else if (finalScore > 2000)
        {
            badge.sprite = firstMateBadge;
            badgeMessage = "You got the First Mate Badge! Fantastic!";
        }
        else if (finalScore > 1000)
        {
            badge.sprite = guppyBadge;
            badgeMessage = "You earned the Guppy Badge! Well done!";
        }
        else
        {
            earnedBadge = false;
            badgeMessage = "You didn't get a badge this time! Try again!";
        }

        RectTransform badgeRect = badge.rectTransform;
        badgeRect.localScale = Vector3.one;
        badge.color = Color.black;

        // if the user didn't earn a badge, scale out the badge image
        if (!earnedBadge)
        {
            LeanTween.cancel(badge.gameObject);
            LeanTween.cancel(badgeRect);

            badge.gameObject.SetActive(true);
            badgeRect.localScale = Vector3.one;

            LeanTween.scale(badgeRect, Vector3.one * noBadgePopScale, badgePopDuration)
                .setEaseOutBack()
                .setOnComplete(() =>
                {
                    LeanTween.scale(badgeRect, Vector3.zero, badgeSettleDuration)
                        .setEaseInBack()
                        .setOnComplete(() =>
                        {
                            badge.gameObject.SetActive(false);
                        });
                });

            yield return new WaitForSeconds(badgePopDuration + badgeSettleDuration);

            if (portraitSpeechBubbleText != null)
                portraitSpeechBubbleText.text = badgeMessage;

            yield break;
        }

        // cancel any remaining tweens
        LeanTween.cancel(badge.gameObject);
        LeanTween.cancel(badgeRect);


        // otherwise, scale the badge up for the reward effect
        LeanTween.scale(badgeRect, Vector3.one * badgePopScale, badgePopDuration)
            .setEaseOutBack()
            .setOnComplete(() =>
            {
                LeanTween.scale(badgeRect, Vector3.one * badgeSettleScale, badgeSettleDuration)
                    .setEaseInOutSine();
            });

        bool fadeComplete = false;

        // set its color to white and rotate the sunburst
        LeanTween.value(badge.gameObject, 0f, 1f, badgeColorFadeDuration)
            .setOnUpdate((float value) =>
            {
                badge.color = Color.Lerp(Color.black, Color.white, value);
            })
            .setOnComplete(() =>
            {
                badge.color = Color.white;
                fadeComplete = true;

                if (sunburstFX != null)
                {
                    sunburstFX.SetActive(true);
                    sunburstFX.transform.localEulerAngles = Vector3.zero;

                    LeanTween.cancel(sunburstFX);

                    LeanTween.rotateAroundLocal(sunburstFX, Vector3.forward, -360f, 12f)
                        .setEaseLinear()
                        .setRepeat(-1);
                }
            });

        yield return new WaitUntil(() => fadeComplete);

        if (portraitSpeechBubbleText != null)
            portraitSpeechBubbleText.text = badgeMessage;
    }
}