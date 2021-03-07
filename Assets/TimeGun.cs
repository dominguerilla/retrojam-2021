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

    public void OnDetach()
    {
        Debug.Log("Slowmo Toggled");
        ToggleTargetSlowMotion(this.slowmoDuration);
    }

    void ToggleTargetSlowMotion(float duration)
    {
        targetTimeController.TryToggleSlowMotion(this, duration);
    }
}
