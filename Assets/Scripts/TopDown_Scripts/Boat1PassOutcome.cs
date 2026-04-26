using UnityEngine;

public class Boat1PassOutcome : MonoBehaviour
{
    [SerializeField] private ScoreController scoreController;
    [SerializeField] private int correctSideRewardAmount = 300;
    [SerializeField] private int wrongSidePenaltyAmount = 300;

    private bool resolved = false;

    public void ResetRule()
    {
        resolved = false;
    }

    public void ResolveCorrectSide()
    {
        if (resolved)
            return;

        resolved = true;
        scoreController.ApplyCustomReward(correctSideRewardAmount);
    }

    public void ResolveWrongSide()
    {
        if (resolved)
            return;

        resolved = true;
        scoreController.ApplyCustomPenalty(wrongSidePenaltyAmount);
    }
}