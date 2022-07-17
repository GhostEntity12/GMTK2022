using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public static class ExtensionMethods
{
	public static float GetLength(this LineRenderer r)
	{
		Vector3[] points = new Vector3[r.positionCount];
		r.GetPositions(points);

		float sum = 0;
		for (int i = 1; i < points.Length; i++)
		{
			sum += (points[i - 1] - points[i]).magnitude;
		}
		return sum;
	}

	public static void AddUnique<T>(this List<T> l, List<T> n)
	{
		foreach (var item in n)
		{
			if (!l.Contains(item))
			{
				l.Append(item);
			}
		}
	}
	public static int Increment(this int i, int max) => (i + 1 + max) % max;

	public static int Decrement(this int i, int max) => (i - 1 + max) % max;
}
