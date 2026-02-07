using UnityEngine;

public class Scrolling_Log : MonoBehaviour
{
    private bool active = false;
    private float speed = 5f;

    // Called by LogDodgeController
    public void Spawn(Vector3 startPos, float moveSpeed)
    {
        transform.position = startPos;
        speed = moveSpeed;
        active = true;
        gameObject.SetActive(true);
    }

    // Called by LogDodgeController
    public void Despawn()
    {
        active = false;
        gameObject.SetActive(false);
    }

    private void Update()
    {
        if (!active) return;

        transform.position += Vector3.down * speed * Time.deltaTime;
    }

    public bool IsBelowY(float yThreshold)
    {
        return transform.position.y <= yThreshold;
    }
}
