using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BulletTrailMaker : MonoBehaviour
{
    public float decaySpeed = 2.0f;
    public ObjectPool bulletTrailPool;

    List<LineRenderer> activeTrails = new List<LineRenderer>();

    public void CreateBulletTrail(Vector3 origin, Vector3 end)
    {
        LineRenderer trail = GetTrail();
        if (!trail) return;
        trail.transform.position = origin;

        var trailPositions = new Vector3[2] { Vector3.zero, end};
        trail.SetPositions(trailPositions);
        StartCoroutine(DecayTrail(trail));
    }

    IEnumerator DecayTrail(LineRenderer trail)
    {
        activeTrails.Add(trail);

        Color trailColor = trail.material.color;
        float alpha = 1.0f;
        
        while (alpha > 0)
        {
            //TODO: I don't know if this actually works.
            trail.material.SetColor("_MainColor", new Color(trailColor.r, trailColor.b, trailColor.g, alpha));
            alpha -= Time.deltaTime * decaySpeed;
            yield return new WaitForEndOfFrame();
        }

        activeTrails.Remove(trail);
        bulletTrailPool.AddObject(trail.transform);

    }

    LineRenderer GetTrail()
    {
        Transform trailTransform = bulletTrailPool.GetNextObject();
        LineRenderer trail;
        if (trailTransform)
        {
            trail = trailTransform.GetComponent<LineRenderer>();
        }
        else
        {
            trail = activeTrails[0];
            activeTrails.Remove(trail);
        }
        trail.gameObject.SetActive(true);
        return trail;
    }
}
