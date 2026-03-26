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

    // [ DELEGATES ]
    public delegate void OnScoreChanged(int newScore);
    public static OnScoreChanged onScoreChanged;

    public delegate void OnStarsChanged(int newStarCount);
    public static OnStarsChanged onStarsChanged;

    private void OnEnable()
    {
        GameController.onBoatingStarted += HandleIncrementScoreOverTime;
        GameController.onBoatingStopped += HandleStopIncrementingScore;
        BoatCollision.onBoatHit += DecrementScore;
    }

    private void OnDisable()
    {
        GameController.onBoatingStarted -= HandleIncrementScoreOverTime;
        GameController.onBoatingStopped -= HandleStopIncrementingScore;
        BoatCollision.onBoatHit -= DecrementScore;
    }

    private void Start()
    {
        score = 0;
        currentStars = 0;
        scoreDecrementAmount = 50;

        BroadcastScore();
        BroadcastStars();
    }

    private void HandleIncrementScoreOverTime()
    {
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
            score += scoreIncrementAmount;
            BroadcastScore();
            UpdateStars();
        }
    }

    private void DecrementScore()
    {
        if (score >= scoreDecrementAmount)
            score -= scoreDecrementAmount;
        else
            score = 0;

        BroadcastScore();
        UpdateStars();
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
}