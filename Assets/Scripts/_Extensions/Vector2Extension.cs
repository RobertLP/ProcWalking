using UnityEngine;

public static class Vector2Extension
{
	public static Vector2 RandomV( this Vector2 v, float min, float max )
	{
		for (int i = 0; i < 2; i++)
			v [i] = Random.Range (min, max);
		return v;
	}

	public static Vector2 RandomV( this Vector2 v, int min, int max )
	{
		for (int i = 0; i < 2; i++)
			v [i] = Random.Range (min, max);
		return v;
	}


	public static Vector2 FloorQuick(this Vector2 v)
	{
		for (int i = 0; i < 2; i++)
			v [i] = (int)v [i];
		return v;
	}

	public static Vector2 Clamp(this Vector2 v, Vector2 min, Vector2 max)
	{
		for (int i = 0; i < 2; i++)
			v [i] = Mathf.Clamp(v[i], min[i], max[i]);
		return v;
	}

	public static Vector2 ClampAngles(this Vector2 v, Vector2 min, Vector2 max)
	{
		for (int i = 0; i < 2; i++)
			v [i] = Mathfc.ClampAngle(v[i], min[i], max[i]);
		return v;
	}
}