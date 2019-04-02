using UnityEngine;

public static class Vector3Extension
{
	public static Vector3 Devide( this Vector3 v, Vector3 devideBy )
	{
		return new Vector3( v.x / devideBy.x, v.y / devideBy.y, v.z / devideBy.z );
	}

	public static bool Approximately( this Vector3 v, Vector3 other, float marginOfError = 0 )
	{
		if(marginOfError == 0)
		{
			for (int i = 0; i < 3; i++)
				if(Mathf.Approximately(v [i], other [i])) return false;

		}
		else
		{
			for (int i = 0; i < 3; i++)
				if(Mathf.Abs(v [i] - other [i]) > marginOfError) return false;
		}


//		if(marginOfError == 0)
//		{
//			if(Mathf.Approximately(v.x, other.x)) return false;
//			if(Mathf.Approximately(v.y, other.y)) return false;
//			if(Mathf.Approximately(v.z, other.z)) return false;
//		}
//		else
//		{
//			if(Mathf.Abs(v.x - other.x) > marginOfError) return false;
//			if(Mathf.Abs(v.y - other.y) > marginOfError) return false;
//			if(Mathf.Abs(v.z - other.z) > marginOfError) return false;
//		}
		return true;
	}

	public static Vector3 Abs( this Vector3 v)
	{
		return new Vector3( Mathf.Abs(v.x), Mathf.Abs(v.y), Mathf.Abs(v.z) );
	}

	public static Vector3 Normalize( this Vector3 v, Vector3 min, Vector3 max )
	{
		for (int i = 0; i < 3; i++)
			v [i] = Mathfc.NormalizeValue( v [i], min [i], max [i] );
//		v.x = Mathfc.NormalizeValue( v.x, min.x, max.x );
//		v.y = Mathfc.NormalizeValue( v.y, min.y, max.y );
//		v.z = Mathfc.NormalizeValue( v.z, min.z, max.z );
		return v;
	}
	public static Vector3 Normalize( this Vector3 v, float min, float max )
	{
		for (int i = 0; i < 3; i++)
			v [i] = Mathfc.NormalizeValue( v [i], min, max );
//		v.x = Mathfc.NormalizeValue( v.x, min, max );
//		v.y = Mathfc.NormalizeValue( v.y, min, max );
//		v.z = Mathfc.NormalizeValue( v.z, min, max );
		return v;
	}

	public static Vector3 RandomV( this Vector3 v, float min, float max )
	{
		for (int i = 0; i < 3; i++)
			v [i] = Random.Range (min, max);
		return v;
	}

	public static Vector3 FloorQuick(this Vector3 v)
	{
		for (int i = 0; i < 3; i++)
			v [i] = (int)v [i];
		return v;
	}

	public static Vector3 Clamp(this Vector3 v, Vector3 min, Vector3 max)
	{
		for (int i = 0; i < 3; i++)
			v [i] = Mathf.Clamp(v[i], min[i], max[i]);
		return v;
	}

	public static Vector3 ClampAngles(this Vector3 v, Vector3 min, Vector3 max)
	{
		for (int i = 0; i < 3; i++)
			v [i] = Mathfc.ClampAngle(v[i], min[i], max[i]);
		return v;
	}

	public static Vector3 Ceil(this Vector3 v)
	{
		for (int i = 0; i < 3; i++)
			v [i] = Mathf.CeilToInt(v[i]);
		return v;
	}
}
