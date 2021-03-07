using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Chronos;
using BNG;

public class TimeGun : RaycastWeapon
{
    [SerializeField]
    float slowmoDuration = 6f;

    [SerializeField]
    TargetTime targetTimeController;

    public override void OnGrab (Grabber grabber)
    {
        base.OnGrab(grabber);

        Debug.Log($"TimeGun.OnGrab: Grabber {grabber.gameObject}");
        if (grabber.gameObject.name.StartsWith("Holster"))
        {
            Debug.Log("Slowmo Toggled");
            ToggleTargetSlowMotion(this.slowmoDuration);
        }
    }

    public void OnDoff()
    {
        
    }

    void ToggleTargetSlowMotion(float duration)
    {
        targetTimeController.TryToggleSlowMotion(this, duration);
    }
}
