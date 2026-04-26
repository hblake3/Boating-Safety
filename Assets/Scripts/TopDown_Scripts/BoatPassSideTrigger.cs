using UnityEngine;

public class BoatPassSideTrigger : MonoBehaviour
{
    [SerializeField] private bool isCorrectSide = false;
    [SerializeField] private string playerBoatTag = "Boat";

    private Boat1PassOutcome parentOutcome;

    private void Awake()
    {
        // grab the parent outcome controller once at startup
        parentOutcome = GetComponentInParent<Boat1PassOutcome>();
    }

    private void OnTriggerEnter(Collider other)
    {
        // only react to the player boat
        if (!other.CompareTag(playerBoatTag))
            return;

        // safety check in case the trigger is missing its parent controller
        if (parentOutcome == null)
            return;

        // Resolve the pass based on which side this trigger represents
        if (isCorrectSide)
            parentOutcome.ResolveCorrectSide();
        else
            parentOutcome.ResolveWrongSide();
    }
}