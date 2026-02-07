using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LogDodgeController : MonoBehaviour
{
    [Header("Log Pool")]
    [SerializeField] Transform logPoolParent;

    [Header("Log Spawn Settings")]
    [SerializeField] float spawnY = 10f;
    [SerializeField] float despawnY = -10f;
    [SerializeField] float logSpeed = 5f;
    private const float LEFT = -5f;
    private const float MIDLEFT = -2.5f;
    private const float CENTER = 0f;
    private const float MIDRIGHT = 2.5f;
    private const float RIGHT = 5f;

    private List<Scrolling_Log> logPool = new List<Scrolling_Log>();
    private bool active = false;


    // [ WAVE FIELDS ]
    [SerializeField] float timeBetweenWaves = 3.5f; // time between each wave
    private Coroutine activeWaveRoutine;
    private int poolIndex = 0; // which log in the pool we're using


    private void Start()
    {
        StartSegment();
    }

    void Awake()
    {
        // add logs to the logPool list
        foreach (Transform child in logPoolParent)
        {
            Scrolling_Log log = child.GetComponent<Scrolling_Log>();
            if (log != null)
            {
                log.Despawn();
                logPool.Add(log);
            }
        }
    }

    void Update()
    {
        if (!active) return;

        foreach (var log in logPool)
        {
            if (log.gameObject.activeSelf && log.IsBelowY(despawnY))
            {
                log.Despawn();
            }
        }
    }

    public void StartSegment()
    {
        active = true;

        if (activeWaveRoutine != null)
            StopCoroutine(activeWaveRoutine);

        activeWaveRoutine = StartCoroutine(RunLogDodgeSequence());
    }


    public void StopSegment()
    {
        active = false;

        foreach (var log in logPool)
        {
            log.Despawn();
        }
    }

    private Scrolling_Log GetNextLogFromPool()
    {
        if (logPool.Count == 0)
            return null;

        Scrolling_Log log = logPool[poolIndex];
        poolIndex = (poolIndex + 1) % logPool.Count;
        return log;
    }

    private bool AllLogsDespawned()
    {
        foreach (var log in logPool)
        {
            if (log.gameObject.activeSelf)
                return false;
        }
        return true;
    }

    private LogWaveDefinition CreateWave1()
    {
        // Wave 1
        LogWaveDefinition wave = new LogWaveDefinition();

        for (int repeat = 0; repeat < 2; repeat++)
        {
            // Left to Right
            wave.spawns.Add(new LogSpawnInstruction { laneX = LEFT, delayAfter = 0.85f });
            wave.spawns.Add(new LogSpawnInstruction { laneX = MIDLEFT, delayAfter = 0.85f });
            wave.spawns.Add(new LogSpawnInstruction { laneX = CENTER, delayAfter = 0.85f });
            wave.spawns.Add(new LogSpawnInstruction { laneX = MIDRIGHT, delayAfter = 0.85f });
            wave.spawns.Add(new LogSpawnInstruction { laneX = RIGHT, delayAfter = 0.85f });
            wave.spawns.Add(new LogSpawnInstruction { laneX = MIDRIGHT, delayAfter = 0.85f });
            wave.spawns.Add(new LogSpawnInstruction { laneX = CENTER, delayAfter = 0.85f });
            wave.spawns.Add(new LogSpawnInstruction { laneX = MIDLEFT, delayAfter = 0.85f });
        }

        return wave;
    }

    private LogWaveDefinition CreateWave2()
    {
        // Wave 1
        LogWaveDefinition wave = new LogWaveDefinition();

        for (int repeat = 0; repeat < 3; repeat++)
        {
            // Left to Right
            wave.spawns.Add(new LogSpawnInstruction { laneX = LEFT, delayAfter = 0.35f });
            wave.spawns.Add(new LogSpawnInstruction { laneX = CENTER, delayAfter = 0.35f });

            // Right to Left
            wave.spawns.Add(new LogSpawnInstruction { laneX = RIGHT, delayAfter = 0.35f });

            // Mids
            wave.spawns.Add(new LogSpawnInstruction { laneX = MIDLEFT, delayAfter = 0.35f });
            wave.spawns.Add(new LogSpawnInstruction { laneX = MIDRIGHT, delayAfter = 0.35f });
        }

        return wave;
    }
    private LogWaveDefinition CreateWave3()
    {
        // Wave 2
        LogWaveDefinition wave = new LogWaveDefinition();

        for (int repeat = 0; repeat < 3; repeat++)
        {
            // Left to Right
            wave.spawns.Add(new LogSpawnInstruction { laneX = LEFT, delayAfter = 0.30f });
            wave.spawns.Add(new LogSpawnInstruction { laneX = MIDLEFT, delayAfter = 0.30f });
            wave.spawns.Add(new LogSpawnInstruction { laneX = CENTER, delayAfter = 0.30f });
            wave.spawns.Add(new LogSpawnInstruction { laneX = MIDRIGHT, delayAfter = 1.00f });

            // Right to Left
            wave.spawns.Add(new LogSpawnInstruction { laneX = RIGHT, delayAfter = 0.30f });
            wave.spawns.Add(new LogSpawnInstruction { laneX = MIDRIGHT, delayAfter = 0.30f });
            wave.spawns.Add(new LogSpawnInstruction { laneX = CENTER, delayAfter = 0.30f });
            wave.spawns.Add(new LogSpawnInstruction { laneX = MIDLEFT, delayAfter = 1.00f });
        }

        return wave;
    }
    private LogWaveDefinition CreateWave4()
    {
        Debug.Log("WAVE 3 Begins!");

        // Wave 3
        LogWaveDefinition wave = new LogWaveDefinition();

        float[] lanes = {LEFT, MIDLEFT, CENTER, MIDRIGHT, RIGHT};

        for (int repeat = 0; repeat < 6; repeat++)
        {
            int safeLane = Random.Range(1, 5);
            foreach (float lane in lanes)
            {
                if (lane != lanes[safeLane])
                {
                    wave.spawns.Add(new LogSpawnInstruction { laneX = lane, delayAfter = 0f });
                }
            }

            // Pause between bursts
            wave.spawns.Add(new LogSpawnInstruction
            {
                laneX = -20, // again, dummy lane for pause
                delayAfter = 2.0f
            });
        }

        return wave;
    }


    private IEnumerator RunWave(LogWaveDefinition wave)
    {
        poolIndex = 0;

        foreach (var spawn in wave.spawns)
        {
            Scrolling_Log log = GetNextLogFromPool();
            if (log != null)
            {
                Vector3 spawnPos = new Vector3(spawn.laneX, spawnY, 0f);
                log.Spawn(spawnPos, logSpeed);
            }

            yield return new WaitForSeconds(spawn.delayAfter);
        }

        // Wait until all logs have left the screen
        yield return new WaitUntil(AllLogsDespawned);

        // Cooldown between waves
        yield return new WaitForSeconds(timeBetweenWaves);
    }

    private IEnumerator RunLogDodgeSequence()
    {
        yield return StartCoroutine(RunWave(CreateWave1()));
        yield return StartCoroutine(RunWave(CreateWave2()));
        yield return StartCoroutine(RunWave(CreateWave3()));
        yield return StartCoroutine(RunWave(CreateWave4()));

        StopSegment();
    }

}


[System.Serializable]
public class LogSpawnInstruction
{
    public float laneX;        // X position (lane)
    public float delayAfter;   // Time to wait after spawning this log
}

[System.Serializable]
public class LogWaveDefinition
{
    public List<LogSpawnInstruction> spawns = new List<LogSpawnInstruction>();
}
