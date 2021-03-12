using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using BNG;
using Chronos;

[RequireComponent(typeof(Timeline))]
public class TimeDamagable : Damageable
{
    Timeline timeline;

    private void Awake()
    {
        timeline = GetComponent<Timeline>();
    }

    public override void DestroyThis()
    {
        StartCoroutine(Destroy(0.05f));
    }

    public IEnumerator RespawnIn(float seconds)
    {
        yield return RespawnRoutine(seconds);
    }

    public void LaunchInDirection(Vector3 direction, float force, Vector3 delta)
    {
        Vector3 offset = GetRandomOffset(delta);
        this.timeline.rigidbody.AddForce((direction + offset) * force, ForceMode.Impulse) ;
    }

    Vector3 GetRandomOffset(Vector3 delta)
    {
        return new Vector3(
            Random.Range(-delta.x, delta.x),
            Random.Range(1, delta.y),
            Random.Range(-delta.z, delta.z)
            ); 
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
