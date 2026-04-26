using UnityEngine;

public class BoatPassSideTrigger : MonoBehaviour
{
    [SerializeField] private bool isCorrectSide = false;
    [SerializeField] private string playerBoatTag = "Boat";

    private Boat1PassOutcome parentOutcome;

    private void Awake()
    {
        parentOutcome = GetComponentInParent<Boat1PassOutcome>();
    }

    private void OnTriggerEnter(Collider other)
    {
        if (!other.CompareTag(playerBoatTag))
            return;

        if (parentOutcome == null)
            return;

        if (isCorrectSide)
            parentOutcome.ResolveCorrectSide();
        else
            parentOutcome.ResolveWrongSide();
    }
}