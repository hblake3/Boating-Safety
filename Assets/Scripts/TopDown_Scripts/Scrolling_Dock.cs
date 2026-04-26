using UnityEngine;

public class Scrolling_Dock : MonoBehaviour
{
    public delegate void OnDockDespawned(Scrolling_Dock dock);
    public static OnDockDespawned onDockDespawned;

    [SerializeField] private float despawnY = -10f;

    private bool active = false;
    private float moveSpeed;
    private Vector3 startPosition;

    public bool IsActive => active;

    private void Awake()
    {
        startPosition = transform.position;
    }

    private void OnEnable()
    {
        BoatController.onSpeedChanged += UpdateDockSpeed;
        UpdateDockSpeed(BoatController.CurrentSpeed);
    }

    private void OnDisable()
    {
        BoatController.onSpeedChanged -= UpdateDockSpeed;
    }

    private void Update()
    {
        if (!active) return;

        transform.position += Vector3.down * moveSpeed * Time.deltaTime;

        if (transform.position.y <= despawnY)
        {
            Despawn(true);
        }
    }

    public void Spawn(float spawnY)
    {
        transform.position = new Vector3(startPosition.x, spawnY, startPosition.z);
        active = true;
        gameObject.SetActive(true);
    }

    public void Despawn(bool notify)
    {
        active = false;
        gameObject.SetActive(false);

        if (notify)
            onDockDespawned?.Invoke(this);
    }

    public void ForceReset()
    {
        active = false;
        gameObject.SetActive(false);
        transform.position = startPosition;
    }

    private void UpdateDockSpeed(float newSpeed)
    {
        moveSpeed = newSpeed;
        //Debug.Log($"{gameObject.name} speed = {newSpeed}");
    }
}