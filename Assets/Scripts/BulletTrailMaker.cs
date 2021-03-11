using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BulletTrailMaker : MonoBehaviour
{
    public float decaySpeed = 2.0f;
    public List<LineRenderer> bulletTrailPool = new List<LineRenderer>();

    List<LineRenderer> activeTrails = new List<LineRenderer>();

    // Start is called before the first frame update
    void Start()
    {
        
    }

    public void CreateBulletTrail(Vector3 origin, Vector3 end)
    {
        LineRenderer trail = GetTrail();
        trail.gameObject.SetActive(true);
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
            trail.material.SetColor("_MainColor", new Color(trailColor.r, trailColor.b, trailColor.g, alpha));
            alpha -= Time.deltaTime * decaySpeed;
            yield return new WaitForEndOfFrame();
        }

        activeTrails.Remove(trail);
        trail.gameObject.SetActive(false);
        bulletTrailPool.Add(trail);

    }

    LineRenderer GetTrail()
    {
        LineRenderer trail;
        if (bulletTrailPool.Count > 0)
        {
            trail = bulletTrailPool[0];
            bulletTrailPool.RemoveAt(0);
            return trail;
        }
        if (activeTrails.Count <= 0)
        {
            throw new System.InvalidOperationException("No bullet trails in reserve or active pool!");
        }
        trail = activeTrails[0];
        activeTrails.RemoveAt(0);
        return trail;
        
    }
}
