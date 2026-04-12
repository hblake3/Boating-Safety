using UnityEngine.InputSystem;
using System.Collections;
using UnityEngine;

public class ScoreController : MonoBehaviour
{
    public int score { get; private set; }
    public int currentStars { get; private set; }

    private int scoreIncrementAmount = 5;
    public int scoreDecrementAmount { get; private set; }
    public bool scoreIsIncrementing { get; private set; }

    private int starLevel1Threshold = 1000;
    private int starLevel2Threshold = 2000;
    private int starLevel3Threshold = 3000;

    [SerializeField] private BoatController boatController;
    [SerializeField] private int noWakePenaltyAmount = 25;

    private bool noWakeRulesActive = false;
    private Coroutine noWakePenaltyRoutine;
    private float currentBoatSpeed;

    private bool noWakeViolationActive = false;

    // [ DELEGATES ]
    public delegate void OnScoreChanged(int newScore);
    public static OnScoreChanged onScoreChanged;

    public delegate void OnStarsChanged(int newStarCount);
    public static OnStarsChanged onStarsChanged;

    public delegate void OnScoreDecremented(int amount);
    public static OnScoreDecremented onScoreDecremented;

    public delegate void OnNoWakeViolationChanged(bool isViolating);
    public static OnNoWakeViolationChanged onNoWakeViolationChanged;

    private void OnEnable()
    {
        GameController.onBoatingStarted += HandleIncrementScoreOverTime;
        GameController.onBoatingStopped += HandleStopIncrementingScore;
        BoatCollision.onBoatHit += DecrementScoreFromCollision;
        BoatController.onSpeedChanged += CacheBoatSpeed;
    }

    private void OnDisable()
    {
        GameController.onBoatingStarted -= HandleIncrementScoreOverTime;
        GameController.onBoatingStopped -= HandleStopIncrementingScore;
        BoatCollision.onBoatHit -= DecrementScoreFromCollision;
        BoatController.onSpeedChanged -= CacheBoatSpeed;
    }

    private void Start()
    {
        score = 0;
        currentStars = 0;
        scoreDecrementAmount = 50;
        currentBoatSpeed = boatController.GetMoveSpeeds()[0];

        BroadcastScore();
        BroadcastStars();
    }

    private void HandleIncrementScoreOverTime()
    {
        if (scoreIsIncrementing) return;

        scoreIsIncrementing = true;
        StartCoroutine(IncrementScoreOverTime());
    }

    private void HandleStopIncrementingScore()
    {
        scoreIsIncrementing = false;
    }

    private IEnumerator IncrementScoreOverTime()
    {
        while (scoreIsIncrementing)
        {
            yield return new WaitForSeconds(0.25f);

            if (noWakeRulesActive && !AtLowestSpeed())
                continue;

            score += scoreIncrementAmount;
            BroadcastScore();
            UpdateStars();
        }
    }

    private void DecrementScoreFromCollision()
    {
        ApplyPenalty(scoreDecrementAmount);
    }

    private void CacheBoatSpeed(float newSpeed)
    {
        currentBoatSpeed = newSpeed;
        UpdateNoWakeViolationState();
    }


    private void ApplyPenalty(int amount)
    {
        if (amount <= 0)
            return;

        score -= amount;

        if (score < 0)
            score = 0;

        BroadcastScore();
        UpdateStars();
        onScoreDecremented?.Invoke(amount);
    }

    private void UpdateStars()
    {
        int newStarCount = CalculateStarsFromScore();

        if (newStarCount != currentStars)
        {
            currentStars = newStarCount;
            BroadcastStars();
        }
    }

    private int CalculateStarsFromScore()
    {
        if (score >= starLevel3Threshold)
            return 3;
        else if (score >= starLevel2Threshold)
            return 2;
        else if (score >= starLevel1Threshold)
            return 1;
        else
            return 0;
    }

    private void BroadcastScore()
    {
        onScoreChanged?.Invoke(score);
    }

    private void BroadcastStars()
    {
        onStarsChanged?.Invoke(currentStars);
    }

    // Below is added for manual testing of scoring system. It can be deleted in final production.
    private void Update()
    {
        if (Keyboard.current == null)
            return;

        if (Keyboard.current.numpad8Key.wasPressedThisFrame)
        {
            AdjustScore(100);
        }

        if (Keyboard.current.numpad2Key.wasPressedThisFrame)
        {
            AdjustScore(-100);
        }
    }

    private void AdjustScore(int amount)
    {
        score += amount;

        if (score < 0)
            score = 0;

        BroadcastScore();
        UpdateStars();
    }

    public void StartNoWakeRules()
    {
        noWakeRulesActive = true;

        if (noWakePenaltyRoutine != null)
            StopCoroutine(noWakePenaltyRoutine);

        UpdateNoWakeViolationState();
        noWakePenaltyRoutine = StartCoroutine(NoWakePenaltyLoop());
    }

    public void StopNoWakeRules()
    {
        noWakeRulesActive = false;

        if (noWakePenaltyRoutine != null)
        {
            StopCoroutine(noWakePenaltyRoutine);
            noWakePenaltyRoutine = null;
        }

        UpdateNoWakeViolationState();
    }

    private IEnumerator NoWakePenaltyLoop()
    {
        while (noWakeRulesActive)
        {
            yield return new WaitForSeconds(1f);

            if (!scoreIsIncrementing)
                continue;

            if (!AtLowestSpeed())
            {
                ApplyPenalty(noWakePenaltyAmount);
            }
        }
    }

    private bool AtLowestSpeed()
    {
        return Mathf.Approximately(currentBoatSpeed, boatController.GetMoveSpeeds()[0]);
    }

    private void UpdateNoWakeViolationState()
    {
        bool isViolating = noWakeRulesActive && !AtLowestSpeed();

        if (isViolating == noWakeViolationActive)
            return;

        noWakeViolationActive = isViolating;
        onNoWakeViolationChanged?.Invoke(noWakeViolationActive);
    }
}