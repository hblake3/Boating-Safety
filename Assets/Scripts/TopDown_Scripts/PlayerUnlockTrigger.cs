using UnityEngine;

public class PlayerUnlockTrigger : MonoBehaviour
{
    [SerializeField] private BoatController playerBoatController;
    [SerializeField] private string playerBoatTag = "Boat";

    private bool triggered = false;

    public void ResetTrigger()
    {
        triggered = false;
    }

    private void OnTriggerEnter(Collider other)
    {
        if (triggered)
            return;

        if (!other.CompareTag(playerBoatTag))
            return;

        triggered = true;
        playerBoatController.UnlockControls();
    }
}