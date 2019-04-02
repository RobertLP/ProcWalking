using System;
using UnityEngine;

public static class Mathfc 
{
	public static float NormalizeValue(float value, float min, float max)
	{
		return (value - min) / (max - min);
	}

	public static float NormalizeValueInverse(float value, float min, float max)
	{
		return value * (max - min);
	}
		

	public static void Randomize<T>(T[] items)
	{
		System.Random rand = new System.Random();
	
		// For each spot in the array, pick
		// a random item to swap into that spot.
		for (int i = 0; i < items.Length - 1; i++)
		{
			int j = rand.Next(i, items.Length);
			T temp = items[i];
			items[i] = items[j];
			items[j] = temp;
		}
	}

	public static float ClampAngle(float angle, float min, float max)
	{
		angle = Mathf.Repeat(angle, 360);
		min = Mathf.Repeat(min, 360);
		max = Mathf.Repeat(max, 360);
		bool inverse = false;
		var tmin = min;
		var tangle = angle;
		if(min > 180)
		{
			inverse = !inverse;
			tmin -= 180;
		}
		if(angle > 180)
		{
			inverse = !inverse;
			tangle -= 180;
		}
		var result = !inverse ? tangle > tmin : tangle < tmin;
		if(!result)
			angle = min;

		inverse = false;
		tangle = angle;
		var tmax = max;
		if(angle > 180)
		{
			inverse = !inverse;
			tangle -= 180;
		}
		if(max > 180)
		{
			inverse = !inverse;
			tmax -= 180;
		}

		result = !inverse ? tangle < tmax : tangle > tmax;
		if(!result)
			angle = max;
		return angle;
	}

	public static bool QuaternionsEqual(Quaternion q1, Quaternion q2)
	{
		return (q1.Equals(q2) || (q1 == q2) || (q1.eulerAngles == q2.eulerAngles) );
	}

	public enum QuaternionAproxMode{ Quaternion, Euler, Full };

	public static bool QuaternionAprox(Quaternion q1, Quaternion q2, QuaternionAproxMode mode)
	{
		if(mode == QuaternionAproxMode.Euler || mode == QuaternionAproxMode.Full)
		{
			for(int i = 0; i < 3; i++)
			{
				if( !Mathf.Approximately( q1.eulerAngles[i], q2.eulerAngles[i] ) )
					return false;
			}
		}
		if(mode == QuaternionAproxMode.Quaternion || mode == QuaternionAproxMode.Full)
		{
			for(int i = 0; i < 4; i++)
			{
				if( !Mathf.Approximately( q1[i], q2[i] ) )
					return false;
			}
		}
		return true;
	}

}
