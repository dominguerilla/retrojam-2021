using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Chronos;
using BNG;

public class TimeGun : BaseBehaviour
{
    [SerializeField]
    float slowmoDuration = 6f;

    [SerializeField]
    TargetTime targetTimeController;

    // TODO: SnapZone calls OnDetach on the first frame of an auto-equipped TimeGun. Have it ignore the first OnDetach().
    // Maybe better to override SnapZone?
    bool _isInitialized = false;

    public void OnDetach()
    {
        if (!_isInitialized)
        {
            _isInitialized = true;
            return;
        }
        StartSlowMotion(this.slowmoDuration);
    }

    void StartSlowMotion(float duration)
    {
        targetTimeController.SetSlowMotionForDuration(this, duration);
    }
}
