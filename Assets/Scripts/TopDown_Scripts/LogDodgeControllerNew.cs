using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LogDodgeControllerNew : MonoBehaviour
{
    [Header("Log Pool")]
    [SerializeField] Transform logPoolParent;

    [Header("Spawn Settings")]
    [SerializeField] float spawnStartY = 14f;    // top of wave formation
    [SerializeField] float verticalSpacing = 2.5f;
    [SerializeField] float despawnY = -10f;
    [SerializeField] float timeBetweenWaves = 1.75f;

    private const float LEFT = -5f;
    private const float MIDLEFT = -2.5f;
    private const float CENTER = 0f;
    private const float MIDRIGHT = 2.5f;
    private const float RIGHT = 5f;
    private const float DUMMY = -200f; // "dummy" log to create 
    private const float ROW = -999f; // marker meaning: next entries form a horizontal row


    private List<Scrolling_Log> pool = new List<Scrolling_Log>();
    private int poolIndex = 0;
    private bool active;
    private Coroutine routine;

    // [ DELEGATES ]
    public delegate void OnLogDodgeComplete();
    public static OnLogDodgeComplete onLogDodgeComplete;

    public delegate void OnLogDodgeFogComplete();
    public static OnLogDodgeFogComplete onLogDodgeFogComplete;


    private void Awake()
    {
        foreach (Transform child in logPoolParent)
        {
            var log = child.GetComponent<Scrolling_Log>();
            if (log != null)
            {
                log.Despawn();
                pool.Add(log);
            }
        }
    }

    public void StartSegment()
    {
        active = true;

        if (routine != null)
            StopCoroutine(routine);

        routine = StartCoroutine(RunSequence());
    }

    public void StartFogSegment()
    {
        active = true;

        if (routine != null)
            StopCoroutine(routine);

        routine = StartCoroutine(RunFogSequence());
    }

    public void StopSegment()
    {
        active = false;

        foreach (var log in pool)
            log.Despawn();
    }

    private IEnumerator RunSequence()
    {
        yield return RunWave(CreateWave0());
        yield return RunWave(CreateWave1());
        yield return RunWave(CreateWave2());
        yield return RunWave(CreateWave3());

        active = false;

        onLogDodgeComplete?.Invoke();
    }

    private IEnumerator RunFogSequence()
    {
        yield return RunWave(CreateWave2());
        yield return RunWave(CreateWave3());

        active = false;

        onLogDodgeFogComplete?.Invoke();
    }

    private IEnumerator RunWave(List<float> lanes)
    {
        float currentY = spawnStartY;
        bool rowMode = false;

        foreach (float token in lanes)
        {
            if (token == ROW)
            {
                rowMode = true;   // next logs share the same Y
                continue;
            }

            if (token == DUMMY)
            {
                currentY += verticalSpacing; // move to next row
                rowMode = false;
                continue;
            }

            SpawnLog(token, currentY);

            // If we're not in row mode, each log gets its own row
            if (!rowMode)
                currentY += verticalSpacing;
        }

        yield return new WaitUntil(AllLogsDespawned);
        yield return new WaitForSeconds(timeBetweenWaves);
    }

    private void SpawnLog(float laneX, float y)
    {
        var log = GetNext();
        if (log == null) return;

        log.Spawn(new Vector3(laneX, y, -1f));
    }


    private Scrolling_Log GetNext()
    {
        if (pool.Count == 0) return null;

        var log = pool[poolIndex];
        poolIndex = (poolIndex + 1) % pool.Count;
        return log;
    }

    private bool AllLogsDespawned()
    {
        foreach (var log in pool)
        {
            if (log.gameObject.activeSelf && log.transform.position.y > despawnY)
                return false;
        }
        return true;
    }

    private void RepeatPattern(List<float> wave, List<float> pattern, int times)
    {
        for (int i = 0; i < times; i++)
            wave.AddRange(pattern);
    }


    // ---------- Wave Definitions ----------


    // intro wave to learn lanes
    private List<float> CreateWave0()
    {
        List<float> wave = new List<float>();

        List<float> pattern = new List<float>
        {
            LEFT, DUMMY, DUMMY,
            MIDLEFT, DUMMY, DUMMY,
            CENTER, DUMMY, DUMMY,
            MIDRIGHT, DUMMY, DUMMY,
            RIGHT, DUMMY, DUMMY,
            MIDRIGHT, DUMMY, DUMMY,
            CENTER, DUMMY, DUMMY,
            MIDLEFT, DUMMY, DUMMY,
            LEFT, DUMMY, DUMMY
        };

        RepeatPattern(wave, pattern, 1);

        return wave;
    }

    // similar to intro but with no spacing between lanes
    private List<float> CreateWave1()
    {
        List<float> wave = new List<float>();

        List<float> pattern = new List<float>
    {
        LEFT, MIDLEFT, CENTER, MIDRIGHT, DUMMY, DUMMY, DUMMY,
        RIGHT, MIDRIGHT, CENTER, MIDLEFT, DUMMY, DUMMY, DUMMY
    };

        RepeatPattern(wave, pattern, 2);

        return wave;
    }

    // scattered wave
    private List<float> CreateWave2()
    {
        List<float> wave = new List<float>();

        List<float> pattern = new List<float>
    {
            LEFT, CENTER, RIGHT, MIDLEFT, MIDRIGHT, LEFT, CENTER, RIGHT, MIDLEFT, MIDRIGHT
    };

        RepeatPattern(wave, pattern, 2);

        return wave;
    }

    private List<float> CreateWave3()
    {
        List<float> result = new List<float>();

        float[] lanes = { LEFT, MIDLEFT, CENTER, MIDRIGHT, RIGHT };

        int lastGap = -1;

        for (int repeat = 0; repeat < 6; repeat++)
        {
            int gap = Random.Range(0, lanes.Length);
            while (gap == lastGap)
                gap = Random.Range(0, lanes.Length);

            lastGap = gap;

            result.Add(ROW); // start horizontal row

            for (int i = 0; i < lanes.Length; i++)
            {
                if (i == gap) continue;
                result.Add(lanes[i]);
            }

            result.Add(DUMMY);
            result.Add(DUMMY);
            result.Add(DUMMY);
            result.Add(DUMMY);
            result.Add(DUMMY);
            result.Add(DUMMY);
            result.Add(DUMMY);   // for row spacing

        }

        return result;
    }


}
