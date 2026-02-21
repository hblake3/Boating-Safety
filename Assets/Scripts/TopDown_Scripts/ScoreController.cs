using System.Collections;
using UnityEngine;
using static BoatController;

public class ScoreController : MonoBehaviour
{
    public int score {  get; private set; }
    private int scoreIncrementAmount = 5;
    public int scoreDecrementAmount { get; private set; }
    public bool scoreIsIncrementing { get; private set; }

    // [ DELEGATES ]
    public delegate void OnScoreChanged(int newScore);
    public static OnScoreChanged onScoreChanged;

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
        scoreDecrementAmount = 50;
        BroadcastScore();
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
        }
    }

    private void DecrementScore()
    {
        if (score >= scoreDecrementAmount)
            score -= scoreDecrementAmount;
        else
            score = 0;
        BroadcastScore();
    }

    private void BroadcastScore()
    {
        onScoreChanged?.Invoke(score);
    }


}
