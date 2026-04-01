using UnityEngine;

public class Scrolling_Log : MonoBehaviour
{
    private bool active = false;
    private float moveSpeed;

    private void OnEnable()
    {
        // Subscribe to onSpeedChanged from BoatController (awaits for command that speed has been changed)
        BoatController.onSpeedChanged += UpdateLogSpeed;

        // Immediately sync to current speed
        UpdateLogSpeed(BoatController.CurrentSpeed);
    }

    private void OnDisable()
    {
        // Unubscribe to onSpeedChanged from BoatController
        BoatController.onSpeedChanged -= UpdateLogSpeed;
    }
    private void Update()
    {
        if (!active) return;

        transform.position += Vector3.down * moveSpeed * Time.deltaTime;
    }

    // Called by LogDodgeController
    public void Spawn(Vector3 startPos)
    {
        transform.position = startPos;
        active = true;
        gameObject.SetActive(true);
    }

    // Called by LogDodgeController
    public void Despawn()
    {
        active = false;
        gameObject.SetActive(false);
    }

    public bool IsBelowY(float yThreshold)
    {
        return transform.position.y <= yThreshold;
    }

    private void UpdateLogSpeed(float newSpeed)
    {
        moveSpeed = newSpeed;
    }
}
