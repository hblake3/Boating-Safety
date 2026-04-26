using System.Collections;
using UnityEngine;

public class BoatsSegmentController : MonoBehaviour
{
    public delegate void OnBoatsSegmentComplete();
    public static OnBoatsSegmentComplete onBoatsSegmentComplete;

    [SerializeField] private Transform playerBoatTransform;
    [SerializeField] private Scrolling_OtherBoat boat1;
    [SerializeField] private Boat1PassOutcome boat1PassOutcome;

    [SerializeField] private float startDelay = 2f;
    [SerializeField] private float delayBetweenRounds = 1.5f;
    [SerializeField] private float spawnY = 14f;

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
        // clear any previous run before starting fresh
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

        // stop the active timing sequence if one is running
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

        // segment may have been stopped during the delay
        if (!segmentActive)
            yield break;

        SpawnBoat1Round();
    }

    private void SpawnBoat1Round()
    {
        // Reset the pass rule before each new attempt
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

        // End the segment once all rounds have been completed
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

        // make sure the segment was not stopped between rounds
        if (!segmentActive)
            yield break;

        SpawnBoat1Round();
    }
}