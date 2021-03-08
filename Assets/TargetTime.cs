using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Chronos;

/// <summary>
/// Control a clock with a single BaseBehaviour at a time.
/// </summary>
public class TargetTime : BaseBehaviour
{
    [SerializeField]
    string clockName;

    [SerializeField]
    float slowMotionTimeScale = 0.25f;

    Clock targetClock;
    MonoBehaviour controllingObject;

    bool _isSlowMotion = false;
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
    public bool TryToggleSlowMotion(MonoBehaviour timeObject)
    {
        if (controllingObject == null || controllingObject == timeObject)
        {
            _isSlowMotion = !_isSlowMotion;
            if (_isSlowMotion)
            {
                targetClock.localTimeScale = slowMotionTimeScale;
                controllingObject = timeObject;
                return true;
            }
            else
            {
                targetClock.localTimeScale = 1.0f;
                controllingObject = null;
                return false;
            }
        }
        return false;
    }

    public bool TryToggleSlowMotion(MonoBehaviour timeObject, float duration)
    {
        if (targetClock == null) return false;
        if (controllingObject == null || controllingObject == timeObject)
        {
            StartCoroutine(SlowMotion(timeObject, duration));
            return true;
        }
        return false;
    }

    IEnumerator SlowMotion(MonoBehaviour timeObject, float duration) {
        
        while (targetClock.localTimeScale > 0)
        {
            targetClock.localTimeScale = Mathf.Max(targetClock.localTimeScale - 0.03f, 0);
            yield return new WaitForEndOfFrame();
        }
        TryToggleSlowMotion(timeObject);
        yield return timeline.WaitForSeconds(duration);

        while (targetClock.localTimeScale < 1.0)
        {
            targetClock.localTimeScale = Mathf.Min(targetClock.localTimeScale + 0.03f, 1.0f);
            yield return new WaitForEndOfFrame();
        }
        TryToggleSlowMotion(timeObject);
        yield return null;
    }
}
