using UnityEngine;

public class ObjectScrolling : MonoBehaviour
{
    public float speed;

    private void OnEnable()
    {
        // Subscribe to onSpeedChanged from BoatController (awaits for command that speed has been changed)
        BoatController.onSpeedChanged += UpdateSpeed;
    }

    private void OnDisable()
    {
        // Unubscribe to onSpeedChanged from BoatController
        BoatController.onSpeedChanged -= UpdateSpeed;
    }

    void Update()
    {
        transform.position += Vector3.down * speed * Time.deltaTime;

        if (transform.position.y <= -10f)
            transform.position = new Vector3(transform.position.x, Random.Range(9f,10f), transform.position.z);
    }

    private void UpdateSpeed(float newSpeed)
    {
        speed = newSpeed;
    }
}
