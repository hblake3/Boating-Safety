using System.Collections;
using UnityEngine;

public class NoWakeSegmentController : MonoBehaviour
{
    public delegate void OnNoWakeSegmentComplete();
    public static OnNoWakeSegmentComplete onNoWakeSegmentComplete;

    [Header("References")]
    [SerializeField] private ScoreController scoreController;
    [SerializeField] private NoWakeBuoyController buoysStartController;
    [SerializeField] private NoWakeBuoyController buoysEndController;
    [SerializeField] private NoWakeZoneTrigger buoyTrigger;
    [SerializeField] private DockSpawner dockSpawner;

    [Header("Timing")]
    [SerializeField] private float buoyStartDelay = 2f;

    [Header("Dock Settings")]
    [SerializeField] private int docksToSpawn = 6;
    [SerializeField] private float minDelayBetweenDocks = 1.5f;
    [SerializeField] private float maxDelayBetweenDocks = 3f;

    private Coroutine routine;
    private bool segmentActive = false;
    private bool zoneStarted = false;
    private bool waitingForEndBuoys = false;

    private void OnEnable()
    {
        NoWakeZoneTrigger.onNoWakeZoneEntered += HandleNoWakeZoneEntered;
        DockSpawner.onDockSequenceComplete += HandleDockSequenceComplete;
        NoWakeBuoyController.onBuoyExitedScreen += HandleBuoyExitedScreen;
    }

    private void OnDisable()
    {
        NoWakeZoneTrigger.onNoWakeZoneEntered -= HandleNoWakeZoneEntered;
        DockSpawner.onDockSequenceComplete -= HandleDockSequenceComplete;
        NoWakeBuoyController.onBuoyExitedScreen -= HandleBuoyExitedScreen;
    }

    public void StartSegment()
    {
        StopSegment();

        segmentActive = true;
        zoneStarted = false;
        waitingForEndBuoys = false;

        buoyTrigger.ResetTrigger();
        buoysStartController.ResetBuoy();
        buoysEndController.ResetBuoy();
        dockSpawner.ResetAllDocks();
        scoreController.StopNoWakeRules();

        routine = StartCoroutine(BeginSequence());
    }

    public void StopSegment()
    {
        segmentActive = false;
        zoneStarted = false;
        waitingForEndBuoys = false;

        if (routine != null)
        {
            StopCoroutine(routine);
            routine = null;
        }

        buoysStartController.ResetBuoy();
        buoysEndController.ResetBuoy();
        dockSpawner.ResetAllDocks();
        scoreController.StopNoWakeRules();
    }

    private IEnumerator BeginSequence()
    {
        yield return new WaitForSeconds(buoyStartDelay);

        if (!segmentActive)
            yield break;

        buoysStartController.StartMoving();
    }

    private void HandleNoWakeZoneEntered()
    {
        if (!segmentActive || zoneStarted)
            return;

        zoneStarted = true;

        scoreController.StartNoWakeRules();
        dockSpawner.BeginSpawning(docksToSpawn, minDelayBetweenDocks, maxDelayBetweenDocks);
    }

    private void HandleDockSequenceComplete()
    {
        if (!segmentActive)
            return;

        scoreController.StopNoWakeRules();

        waitingForEndBuoys = true;
        buoysEndController.StartMoving();
    }

    private void HandleBuoyExitedScreen(NoWakeBuoyController buoyController)
    {
        if (!segmentActive)
            return;

        if (!waitingForEndBuoys)
            return;

        if (buoyController != buoysEndController)
            return;

        waitingForEndBuoys = false;
        segmentActive = false;

        onNoWakeSegmentComplete?.Invoke();
    }
}