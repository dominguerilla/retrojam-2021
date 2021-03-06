using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Chronos;

public class Target : BaseBehaviour
{
    /// <summary>
    /// Starts moving the object on Scene start.
    /// </summary>
    [SerializeField]
    bool moveOnStart = false;

    /// <summary>
    /// How swiftly the target moves.
    /// </summary>
    [SerializeField]
    float moveSpeed = 15f;

    /// <summary>
    /// The maximum distance away from its starting location that this target travels in any given axis.
    /// </summary>
    [SerializeField]
    float maxDistanceDelta = 10f;

    /// <summary>
    /// Is multiplied by the maxDistanceDelta and added to the current position.
    /// </summary>
    [SerializeField]
    Vector3 movementDirectionScalar = Vector3.zero;

    Vector3 pivotPoint;
    Timeline timeline;

    private void Start()
    {
        SetPivotPoint(this.transform.position);
        this.timeline = this.time;
        if (moveOnStart)
        {
            StartMoving();
        }
    }

    void StartMoving()
    {
        StartCoroutine(LinearPingPong());
    }

    IEnumerator LinearPingPong()
    {
        int flip = 1;
        while (true)
        {
            if (time.timeScale > 0)
            {
                float currentDistanceFromPivot = (this.transform.position - this.pivotPoint).magnitude;
                if (currentDistanceFromPivot >= maxDistanceDelta)
                {
                    flip *= -1;
                }
                this.transform.position = Vector3.MoveTowards(this.transform.position, this.pivotPoint + (movementDirectionScalar * maxDistanceDelta * flip), moveSpeed * timeline.deltaTime);
            }
            yield return new WaitForEndOfFrame();
        }
    }

    void SetPivotPoint(Vector3 worldPosition)
    {
        this.pivotPoint = worldPosition;
    }
}
