using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using BNG;
using Chronos;

[RequireComponent(typeof(Timeline))]
public class TimeDamagable : Damageable
{
    Timeline timeline;

    // Start is called before the first frame update
    protected override void Start()
    {
        base.Start();
        timeline = GetComponent<Timeline>();    
    }

    public override void DestroyThis()
    {
        StartCoroutine(Destroy(0.05f));
    }

    IEnumerator Destroy(float delay)
    {
        yield return this.timeline.WaitForSeconds(delay);
        base.DestroyThis();
    }

    protected override IEnumerator RespawnRoutine(float seconds)
    {
        yield return timeline.WaitForSeconds(seconds);
        yield return base.RespawnRoutine(0);
    }

}
