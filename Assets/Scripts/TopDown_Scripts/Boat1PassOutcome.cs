using UnityEngine;

public class Boat1PassOutcome : MonoBehaviour
{
    [SerializeField] private ScoreController scoreController;
    [SerializeField] private int correctSideRewardAmount = 300;
    [SerializeField] private int wrongSidePenaltyAmount = 300;

    // prevents both triggers from resolving the same encounter.
    private bool resolved = false;

    public void ResetRule()
    {
        resolved = false;
    }

    // for when the player navigates the correct side of the other boat
    public void ResolveCorrectSide()
    {
        if (resolved)
            return;

        resolved = true;
        scoreController.ApplyCustomReward(correctSideRewardAmount);
    }

    // for when the player navigates the wrong side of the other boat
    public void ResolveWrongSide()
    {
        if (resolved)
            return;

        resolved = true;
        scoreController.ApplyCustomPenalty(wrongSidePenaltyAmount);
    }
}