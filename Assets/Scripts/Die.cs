using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class Die : Poolable
{
    public int Side { get; private set; }

    static Vector3 CheckSide = Vector3.forward;

    // Update is called once per frame
    protected virtual void Update()
    {
        Side = SideToCamera();
        //r.material.color = colors[Side];
    }

    int SideToCamera()
    {
		float[] vectorDiffs = new[] {
            Mathf.Abs(1 - Vector3.Dot(transform.up, CheckSide)),
            Mathf.Abs(1 - Vector3.Dot(-transform.up, CheckSide)),
            Mathf.Abs(1 - Vector3.Dot(transform.right, CheckSide)),
            Mathf.Abs(1 - Vector3.Dot(-transform.right, CheckSide)),
            Mathf.Abs(1 - Vector3.Dot(transform.forward, CheckSide)),
            Mathf.Abs(1 - Vector3.Dot(-transform.forward, CheckSide)),
        };


		float minValue = float.MaxValue;
        int minIndex = -1;

		for (int i = 0; i < vectorDiffs.Length; i++)
        {
            if (vectorDiffs[i] <= minValue)
            {
                minValue = vectorDiffs[i];
                minIndex = i;
            }
        }

        return minIndex;
    }

    public void Remove() => ReturnToPool();
}
