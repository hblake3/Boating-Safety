using UnityEngine;

public class Boat1PassRule : MonoBehaviour
{
    [SerializeField] private ScoreController scoreController;
    [SerializeField] private Transform playerBoatTransform;
    [SerializeField] private int wrongSidePenaltyAmount = 100;
    [SerializeField] private string playerBoatTag = "Boat";

    private bool evaluated = false;

    public void ResetRule()
    {
        evaluated = false;
    }

    private void OnTriggerExit(Collider other)
    {
        if (evaluated)
            return;

        if (!other.CompareTag(playerBoatTag))
            return;

        evaluated = true;

        // Wrong for head-on:
        // player ended up to the LEFT of Boat1
        if (playerBoatTransform.position.x < transform.position.x)
        {
            scoreController.ApplyCustomPenalty(wrongSidePenaltyAmount);
        }
    }
}