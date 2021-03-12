using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TargetLauncher : BaseBehaviour
{
    [Header("Wave Settings")]
    public int targetsPerWave = 8;
    public float timeBetweenWaves = 6f;
    public float timeAlive = 6f;

    [Header("Launch Settings")]
    public float launchForce = 20f;
    public Vector3 launchDelta = new Vector3();

    [SerializeField]
    ObjectPool targetPool;

    [SerializeField]
    Transform launchPosition;

    [SerializeField]
    bool isSpawning = false;

    private void Start()
    {
        
        ToggleSpawn(targetsPerWave, timeBetweenWaves);
       
    }

    public void ToggleSpawn(int targetsPerWave, float timeBetweenWaves)
    {
        StartCoroutine(SpawnRoutine(targetsPerWave, timeBetweenWaves));
    }

    IEnumerator SpawnRoutine(int targetsPerWave, float timeBetweenWaves)
    {
        yield return this.time.WaitForSeconds(3f);
        while (true)
        {
            if (isSpawning)
            {
                for (int i = 0; i < targetsPerWave; i++)
                {
                    StartCoroutine(SpawnTarget());
                    yield return null;
                }
            }
            yield return this.time.WaitForSeconds(timeBetweenWaves);
        }
    }

    IEnumerator SpawnTarget()
    {
        Transform targetTransform = targetPool.GetNextObject();
        if (!targetTransform)
        {
            Debug.LogError("Ran out of targets in pool!");
            yield break;
        }

        TimeDamagable target = targetTransform.GetComponent<TimeDamagable>();
        if (target)
        {
            target.SetPositionRotation(launchPosition.position, Quaternion.identity);
            target.gameObject.SetActive(true);
            target.LaunchInDirection(launchPosition.up, launchForce, launchDelta);
            yield return target.RespawnIn(timeAlive);
            targetPool.AddObject(target.transform);
        }
        else
        {
            Debug.LogError($"No TimeDamageable in { targetTransform.name }!");
            yield break;
        }

        yield return null;
    }
}
