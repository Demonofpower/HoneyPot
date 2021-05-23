using System;
using UnityEngine;

// Token: 0x0200013F RID: 319
public class MathUtils
{
	// Token: 0x06000785 RID: 1925 RVA: 0x00037E48 File Offset: 0x00036048
	public static bool CompareNumbers(NumberComparisonType comparison, float a, float b)
	{
		switch (comparison)
		{
		case NumberComparisonType.EQUAL_TO:
			return a == b;
		case NumberComparisonType.LESS_THAN:
			return a < b;
		case NumberComparisonType.LESS_THEN_OR_EQUAL_TO:
			return a <= b;
		case NumberComparisonType.GREATER_THAN:
			return a > b;
		case NumberComparisonType.GREATER_THAN_OR_EQUAL_TO:
			return a >= b;
		default:
			return false;
		}
	}

	// Token: 0x06000786 RID: 1926 RVA: 0x000079E8 File Offset: 0x00005BE8
	public static float NormalizeDegreeAngle(float angle)
	{
		while (angle > 360f)
		{
			angle -= 360f;
		}
		while (angle < 0f)
		{
			angle += 360f;
		}
		return angle;
	}

	// Token: 0x06000787 RID: 1927 RVA: 0x00037E98 File Offset: 0x00036098
	public static bool IsPointWithinBounds(Vector3 point, Bounds bounds)
	{
		return point.x >= bounds.min.x && point.x <= bounds.max.x && point.y >= bounds.min.y && point.y <= bounds.max.y;
	}

	// Token: 0x06000788 RID: 1928 RVA: 0x00037F14 File Offset: 0x00036114
	public static Vector3 LerpCubicCurveBezierPoint(Vector3 p0, Vector3 p1, Vector3 p2, Vector3 p3, float t)
	{
		float num = 1f - t;
		float num2 = t * t;
		float num3 = num * num;
		float d = num3 * num;
		float d2 = num2 * t;
		Vector3 a = d * p0;
		a += 3f * num3 * t * p1;
		a += 3f * num * num2 * p2;
		return a + d2 * p3;
	}
}
