using System.Collections.Generic;
using UnityEngine;

public class ObjectScrolling : MonoBehaviour
{
    [Header("Scroll Tuning")]
    [SerializeField] private float speedMultiplier = 1f;
    [SerializeField] private float respawnMinY = 9f;
    [SerializeField] private float respawnMaxY = 11f;
    [SerializeField] private float despawnY = -10f;

    [Header("Respawn Separation")]
    [SerializeField] private float minRespawnYSpacing = 2f;
    [SerializeField] private float sameLaneXThreshold = 0.5f;
    [SerializeField] private int maxRespawnAttempts = 10;

    private static readonly List<ObjectScrolling> scrollers = new List<ObjectScrolling>();

    private float baseSpeed;

    private void OnEnable()
    {
        BoatController.onSpeedChanged += UpdateSpeed;
        UpdateSpeed(BoatController.CurrentSpeed);

        if (!scrollers.Contains(this))
            scrollers.Add(this);
    }

    private void OnDisable()
    {
        BoatController.onSpeedChanged -= UpdateSpeed;
        scrollers.Remove(this);
    }

    private void Update()
    {
        float moveSpeed = baseSpeed * speedMultiplier;
        transform.position += Vector3.down * moveSpeed * Time.deltaTime;

        if (transform.position.y <= despawnY)
        {
            Respawn();
        }
    }

    private void UpdateSpeed(float newSpeed)
    {
        baseSpeed = newSpeed;
    }

    private void Respawn()
    {
        float newY = FindValidRespawnY();

        transform.position = new Vector3(
            transform.position.x,
            newY,
            transform.position.z
        );
    }

    private float FindValidRespawnY()
    {
        for (int i = 0; i < maxRespawnAttempts; i++)
        {
            float candidateY = Random.Range(respawnMinY, respawnMaxY);

            if (IsRespawnPositionClear(candidateY))
                return candidateY;
        }

        return respawnMaxY + minRespawnYSpacing;
    }

    private bool IsRespawnPositionClear(float candidateY)
    {
        foreach (ObjectScrolling other in scrollers)
        {
            if (other == this)
                continue;

            if (Mathf.Abs(other.transform.position.x - transform.position.x) > sameLaneXThreshold)
                continue;

            if (Mathf.Abs(other.transform.position.y - candidateY) < minRespawnYSpacing)
                return false;
        }

        return true;
    }
}