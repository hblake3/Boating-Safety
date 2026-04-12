using UnityEngine;

public class NoWakeBuoyController : MonoBehaviour
{
    [SerializeField] private float despawnY = -10f;

    private float moveSpeed;
    private bool active;
    private Vector3 startPosition;

    private void Awake()
    {
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

        if (transform.position.y <= despawnY)
        {
            ResetBuoy();
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
        active = false;
        transform.position = startPosition;
        gameObject.SetActive(false);
    }

    private void UpdateBuoySpeed(float newSpeed)
    {
        moveSpeed = newSpeed;
    }
}