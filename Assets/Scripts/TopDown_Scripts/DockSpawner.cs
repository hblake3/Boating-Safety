using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DockSpawner : MonoBehaviour
{
    public delegate void OnDockSequenceComplete();
    public static OnDockSequenceComplete onDockSequenceComplete;

    [SerializeField] private float spawnStartY = 14f;

    private List<Scrolling_Dock> dockPool = new List<Scrolling_Dock>();
    private List<Scrolling_Dock> shuffledDockOrder = new List<Scrolling_Dock>();

    private Coroutine spawnRoutine;
    private int docksSpawnedThisRun = 0;
    private int docksTargetThisRun = 0;
    private int activeDockCount = 0;
    private int nextDockIndex = 0;
    private float minDelayBetweenDocks = 1.5f;
    private float maxDelayBetweenDocks = 3f;
    private bool sequenceActive = false;

    private void Awake()
    {
        dockPool.Clear();

        // Build the dock pool from all child dock objects
        foreach (Transform child in transform)
        {
            Scrolling_Dock dock = child.GetComponent<Scrolling_Dock>();

            if (dock != null)
            {
                dock.ForceReset();
                dockPool.Add(dock);
            }
        }
    }

    private void OnEnable()
    {
        Scrolling_Dock.onDockDespawned += HandleDockDespawned;
    }

    private void OnDisable()
    {
        Scrolling_Dock.onDockDespawned -= HandleDockDespawned;
    }

    public void BeginSpawning(int docksToSpawn, float minDelay, float maxDelay)
    {
        // start clean each time
        ResetAllDocks();

        if (dockPool.Count == 0)
        {
            onDockSequenceComplete?.Invoke();
            return;
        }

        docksTargetThisRun = Mathf.Max(0, docksToSpawn);
        minDelayBetweenDocks = minDelay;
        maxDelayBetweenDocks = Mathf.Max(minDelay, maxDelay);

        docksSpawnedThisRun = 0;
        activeDockCount = 0;
        nextDockIndex = 0;
        sequenceActive = true;

        // shuffle at the start of the segment
        shuffledDockOrder = new List<Scrolling_Dock>(dockPool);
        Shuffle(shuffledDockOrder);

        if (spawnRoutine != null)
            StopCoroutine(spawnRoutine);

        spawnRoutine = StartCoroutine(SpawnRoutine());
    }

    public void ResetAllDocks()
    {
        sequenceActive = false;

        // stop any active spawning before clearing the run
        if (spawnRoutine != null)
        {
            StopCoroutine(spawnRoutine);
            spawnRoutine = null;
        }

        docksSpawnedThisRun = 0;
        docksTargetThisRun = 0;
        activeDockCount = 0;
        nextDockIndex = 0;

        // reset every dock back to its inactive state
        foreach (Scrolling_Dock dock in dockPool)
        {
            if (dock != null)
                dock.ForceReset();
        }

        shuffledDockOrder.Clear();
    }

    private IEnumerator SpawnRoutine()
    {
        while (sequenceActive && docksSpawnedThisRun < docksTargetThisRun)
        {
            Scrolling_Dock dockToSpawn = shuffledDockOrder[nextDockIndex];

            // wait until this dock is available again
            while (sequenceActive && dockToSpawn.IsActive)
                yield return null;

            if (!sequenceActive)
                yield break;

            dockToSpawn.Spawn(spawnStartY);
            docksSpawnedThisRun++;
            activeDockCount++;

            nextDockIndex++;
            if (nextDockIndex >= shuffledDockOrder.Count)
                nextDockIndex = 0;

            if (docksSpawnedThisRun < docksTargetThisRun)
                yield return new WaitForSeconds(Random.Range(minDelayBetweenDocks, maxDelayBetweenDocks));
        }

        spawnRoutine = null;
    }

    private void HandleDockDespawned(Scrolling_Dock dock)
    {
        if (!sequenceActive)
            return;

        activeDockCount = Mathf.Max(0, activeDockCount - 1);

        // finish after every dock has spawned and cleared
        if (docksSpawnedThisRun >= docksTargetThisRun && activeDockCount == 0)
        {
            sequenceActive = false;
            onDockSequenceComplete?.Invoke();
        }
    }

    private void Shuffle(List<Scrolling_Dock> docks)
    {
        // shuffle the dock order
        for (int i = 0; i < docks.Count; i++)
        {
            int randomIndex = Random.Range(i, docks.Count);
            (docks[i], docks[randomIndex]) = (docks[randomIndex], docks[i]);
        }
    }
}