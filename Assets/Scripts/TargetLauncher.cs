using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using BNG;

public class TargetLauncher : BaseBehaviour
{
    [Header("Wave Settings")]
    public int targetsPerWave = 8;
    public float timeBetweenWaves = 6f;
    public float timeAlive = 6f;
    public AudioClip spawnSound;

    [Header("Launch Settings")]
    public float launchForce = 20f;
    public Vector3 launchDelta = new Vector3();
    
    [Header("Scoreboard Settings")]
    public Scoreboard currentScore;
    public Scoreboard highScore;

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
                VRUtils.Instance.PlaySpatialClipAt(spawnSound, launchPosition.position, 1.0f);
                for (int i = 0; i < targetsPerWave; i++)
                {
                    StartCoroutine(SpawnTarget());
                    yield return null;
                }
            }
            yield return this.time.WaitForSeconds(timeBetweenWaves);
            highScore.UpdateHighScore(currentScore.GetScore());
            currentScore.ResetScore();
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
            target.onDestroyed.AddListener(currentScore.IncrementScore);
            target.gameObject.SetActive(true);
            target.LaunchInDirection(launchPosition.up, launchForce, launchDelta);

            yield return target.RespawnIn(timeAlive);

            target.onDestroyed.RemoveListener(currentScore.IncrementScore);
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
