using UnityEngine;

public class Scrolling_OtherBoat : MonoBehaviour
{
    public delegate void OnOtherBoatDespawned(Scrolling_OtherBoat boat);
    public static OnOtherBoatDespawned onOtherBoatDespawned;

    [SerializeField] private float despawnY = -20f;
    [SerializeField] private float matchPlayerXAtY = 6.5f;
    [SerializeField] private Transform playerBoatTransform;
    private float minX = -3.5f; // minimum x pos allowed
    private float maxX = 4.5f; // maximum x pos allowed

    private bool activeBoat = false;
    private bool hasMatchedPlayerX = false;
    private float moveSpeed;
    private Vector3 startPosition;

    public bool IsActive => activeBoat;

    private void Awake()
    {
        // save the starting position for clean resets
        startPosition = transform.position;
    }

    private void OnEnable()
    {
        BoatController.onSpeedChanged += UpdateBoatSpeed;
        UpdateBoatSpeed(BoatController.CurrentSpeed);
    }

    private void OnDisable()
    {
        BoatController.onSpeedChanged -= UpdateBoatSpeed;
    }

    private void Update()
    {
        if (!activeBoat) return;

        transform.position += Vector3.down * moveSpeed * Time.deltaTime;

        // line up with the player once the boat gets close enough to play area
        if (!hasMatchedPlayerX && playerBoatTransform != null && transform.position.y <= matchPlayerXAtY)
        {
            float clampedX = Mathf.Clamp(playerBoatTransform.position.x, minX, maxX);
            transform.position = new Vector3(clampedX, transform.position.y, transform.position.z);
            hasMatchedPlayerX = true;
        }

        // despawn after the boat leaves the play area
        if (transform.position.y <= despawnY)
        {
            Despawn(true);
        }
    }

    public void Spawn(float spawnY, float spawnX)
    {
        // Start each pass from the requested spawn point
        transform.position = new Vector3(spawnX, spawnY, startPosition.z);
        activeBoat = true;
        hasMatchedPlayerX = false;
        gameObject.SetActive(true);
    }

    public void Despawn(bool notify)
    {
        activeBoat = false;
        gameObject.SetActive(false);

        // Let the segment know this boat is done
        if (notify)
            onOtherBoatDespawned?.Invoke(this);
    }

    public void ForceReset()
    {
        // hide and return the boat to its original start position
        activeBoat = false;
        hasMatchedPlayerX = false;
        gameObject.SetActive(false);
        transform.position = startPosition;
    }

    private void UpdateBoatSpeed(float newSpeed)
    {
        moveSpeed = newSpeed;
    }
}