using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Chronos;
using BNG;

/// <summary>
/// Control a clock with a single BaseBehaviour at a time.
/// </summary>
public class TargetTime : BaseBehaviour
{
    [SerializeField]
    string clockName;

    [SerializeField]
    float slowMotionTimeScale = 0.25f;

    [SerializeField]
    AudioClip timeFreezeStartSound;

    [SerializeField]
    AudioClip timeFreezeStopSound;

    Clock targetClock;
    MonoBehaviour controllingObject;

    bool _isTimeAltered = false;
    Timeline timeline;

    private void Start()
    {
        targetClock = Timekeeper.instance.Clock(clockName);
        timeline = this.time;
        if (targetClock == null)
        {
            Debug.LogError($"No clock named {clockName} found!");
            Destroy(this);
        }
    }

    /// <summary>
    /// Slows time and returns true if the target clock hasn't been locked. Returns false if some other BaseBehaviour is currently controlling the targetClock.
    /// </summary>
    /// <param name="timeObject"></param>
    /// <returns></returns>
    public bool SetTimeScale(MonoBehaviour timeObject, float timeScale)
    {
        if (controllingObject == null || controllingObject == timeObject)
        {
            targetClock.localTimeScale = timeScale;
            return true;
        }
        return false;
    }

    public bool StartTimeFreeze(MonoBehaviour timeObject, float duration)
    {
        if (targetClock == null || _isTimeAltered) return false;
        if (controllingObject == null || controllingObject == timeObject)
        {
            StartCoroutine(FreezeTime(timeObject, duration));
            return true;
        }
        return false;
    }

    IEnumerator FreezeTime(MonoBehaviour timeObject, float duration) {
        controllingObject = timeObject;

        VRUtils.Instance.PlaySpatialClipAt(timeFreezeStartSound, timeObject.transform.position, 1f);
        // gradually reduce time scale
        while (targetClock.localTimeScale > 0)
        {
            targetClock.localTimeScale = Mathf.Max(targetClock.localTimeScale - 0.03f, 0);
            yield return new WaitForEndOfFrame();
        }

        // freeze time
        SetTimeScale(timeObject, 0.0f);
        _isTimeAltered = true;
        yield return timeline.WaitForSeconds(duration);

        VRUtils.Instance.PlaySpatialClipAt(timeFreezeStopSound, timeObject.transform.position, 1f);
        // gradually increase time scale
        while (targetClock.localTimeScale < 1.0)
        {
            targetClock.localTimeScale = Mathf.Min(targetClock.localTimeScale + 0.01f, 1.0f);
            yield return new WaitForEndOfFrame();
        }
        SetTimeScale(timeObject, 1.0f);

        _isTimeAltered = false;
        controllingObject = null;
        yield return null;
    }
}
