using UnityEngine;

public class Scrolling_OtherBoat : MonoBehaviour
{
    public delegate void OnOtherBoatDespawned(Scrolling_OtherBoat boat);
    public static OnOtherBoatDespawned onOtherBoatDespawned;

    [SerializeField] private float despawnY = -20f;
    [SerializeField] private float matchPlayerXAtY = 6.5f;
    [SerializeField] private Transform playerBoatTransform;

    private bool activeBoat = false;
    private bool hasMatchedPlayerX = false;
    private float moveSpeed;
    private Vector3 startPosition;

    public bool IsActive => activeBoat;

    private void Awake()
    {
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

        if (!hasMatchedPlayerX && playerBoatTransform != null && transform.position.y <= matchPlayerXAtY)
        {
            transform.position = new Vector3(playerBoatTransform.position.x, transform.position.y, transform.position.z);
            hasMatchedPlayerX = true;
        }

        if (transform.position.y <= despawnY)
        {
            Despawn(true);
        }
    }

    public void Spawn(float spawnY, float spawnX)
    {
        transform.position = new Vector3(spawnX, spawnY, startPosition.z);
        activeBoat = true;
        hasMatchedPlayerX = false;
        gameObject.SetActive(true);
    }

    public void Despawn(bool notify)
    {
        activeBoat = false;
        gameObject.SetActive(false);

        if (notify)
            onOtherBoatDespawned?.Invoke(this);
    }

    public void ForceReset()
    {
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