using System.Collections;
using UnityEngine;

public class BoatsSegmentController : MonoBehaviour
{
    public delegate void OnBoatsSegmentComplete();
    public static OnBoatsSegmentComplete onBoatsSegmentComplete;

    [Header("References")]
    [SerializeField] private Transform playerBoatTransform;
    [SerializeField] private Scrolling_OtherBoat boat1;
    [SerializeField] private Boat1PassOutcome boat1PassOutcome;

    [Header("Timing")]
    [SerializeField] private float startDelay = 2f;
    [SerializeField] private float delayBetweenRounds = 1.5f;
    [SerializeField] private float spawnY = 14f;

    [Header("Rounds")]
    [SerializeField] private int roundsToRun = 3;

    private Coroutine routine;
    private bool segmentActive = false;
    private int roundsCompleted = 0;

    private void OnEnable()
    {
        Scrolling_OtherBoat.onOtherBoatDespawned += HandleOtherBoatDespawned;
    }

    private void OnDisable()
    {
        Scrolling_OtherBoat.onOtherBoatDespawned -= HandleOtherBoatDespawned;
    }

    public void StartSegment()
    {
        StopSegment();

        segmentActive = true;
        roundsCompleted = 0;

        boat1.ForceReset();
        boat1PassOutcome.ResetRule();

        routine = StartCoroutine(BeginSequence());
    }

    public void StopSegment()
    {
        segmentActive = false;
        roundsCompleted = 0;

        if (routine != null)
        {
            StopCoroutine(routine);
            routine = null;
        }

        boat1.ForceReset();
        boat1PassOutcome.ResetRule();
    }

    private IEnumerator BeginSequence()
    {
        yield return new WaitForSeconds(startDelay);

        if (!segmentActive)
            yield break;

        SpawnBoat1Round();
    }

    private void SpawnBoat1Round()
    {
        boat1PassOutcome.ResetRule();

        float spawnX = boat1.transform.position.x;
        boat1.Spawn(spawnY, spawnX);
    }

    private void HandleOtherBoatDespawned(Scrolling_OtherBoat despawnedBoat)
    {
        if (!segmentActive)
            return;

        if (despawnedBoat != boat1)
            return;

        roundsCompleted++;

        if (roundsCompleted >= roundsToRun)
        {
            segmentActive = false;
            onBoatsSegmentComplete?.Invoke();
            return;
        }

        StartCoroutine(SpawnNextRoundAfterDelay());
    }

    private IEnumerator SpawnNextRoundAfterDelay()
    {
        yield return new WaitForSeconds(delayBetweenRounds);

        if (!segmentActive)
            yield break;

        SpawnBoat1Round();
    }
}