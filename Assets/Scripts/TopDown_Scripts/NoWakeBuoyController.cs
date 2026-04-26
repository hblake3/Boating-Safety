using UnityEngine;

public class NoWakeBuoyController : MonoBehaviour
{
    public delegate void OnBuoyExitedScreen(NoWakeBuoyController buoyController);
    public static OnBuoyExitedScreen onBuoyExitedScreen;

    [SerializeField] private float despawnY = -10f;

    private float moveSpeed;
    private bool active;
    private Vector3 startPosition;

    private void Awake()
    {
        // save the original spot so the buoy can restart from the same place
        startPosition = transform.position;
    }

    private void OnEnable()
    {
        BoatController.onSpeedChanged += UpdateBuoySpeed;
        UpdateBuoySpeed(BoatController.CurrentSpeed);
    }

    private void OnDisable()
    {
        BoatController.onSpeedChanged -= UpdateBuoySpeed;
    }

    private void Update()
    {
        if (!active) return;

        transform.position += Vector3.down * moveSpeed * Time.deltaTime;

        // If the buoy leaves the screen, let the segment know
        if (transform.position.y <= despawnY)
        {
            active = false;
            onBuoyExitedScreen?.Invoke(this);
            gameObject.SetActive(false);
        }
    }

    public void StartMoving()
    {
        transform.position = startPosition;
        gameObject.SetActive(true);
        active = true;
    }

    public void ResetBuoy()
    {
        // hide and stop the buoy
        active = false;
        transform.position = startPosition;
        gameObject.SetActive(false);
    }

    private void UpdateBuoySpeed(float newSpeed)
    {
        moveSpeed = newSpeed;
    }
}