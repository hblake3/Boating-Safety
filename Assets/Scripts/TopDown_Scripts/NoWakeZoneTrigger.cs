using UnityEngine;

public class NoWakeZoneTrigger : MonoBehaviour
{
    public delegate void OnNoWakeZoneEntered();
    public static OnNoWakeZoneEntered onNoWakeZoneEntered;

    [SerializeField] private string boatTag = "Boat";

    private bool triggered = false;

    public void ResetTrigger()
    {
        triggered = false;
    }

    private void OnTriggerEnter(Collider other)
    {
        Debug.Log($"NO WAKE TRIGGER touched by: {other.name}");

        if (triggered)
            return;

        if (!other.CompareTag(boatTag))
        {
            Debug.Log("Touched object is not tagged as Boat");
            return;
        }

        triggered = true;
        Debug.Log("NO WAKE TRIGGER accepted boat");
        onNoWakeZoneEntered?.Invoke();
    }
}