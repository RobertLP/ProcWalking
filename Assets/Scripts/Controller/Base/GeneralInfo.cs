using UnityEngine;
using System.Collections.Generic;

public static class GeneralInfo 
{
	public enum Axis { X, Y, Z };
	public enum Direction { Forward = 0, Backward = 180, Left = 270, Right = 90, Up = 450, Down = 630 };

	public static Vector3 Axis_V( Axis axis )
	{
		switch( axis )
		{
		case Axis.X:
			return Vector3.right;
		case Axis.Y:
			return Vector3.up;
		case Axis.Z:
			return Vector3.forward;
		default:
			return default(Vector3);
		}
	}

	public static Vector3 Axis_V_Add( Axis axis, Vector3 current, float toAdd )
	{
		switch( axis )
		{
		case Axis.X:
			current.x += toAdd; 
			return current;
		case Axis.Y:
			current.y += toAdd; 
			return current;
		case Axis.Z:
			current.z += toAdd; 
			return current;
		default:
			return default(Vector3);
		}
	}

	public static Vector3 Axis_V_Clamp( Axis axis, Vector3 current, float value, float min, float max )
	{
		switch( axis )
		{
		case Axis.X:
			current.x = Mathf.Clamp( value, min, max );
			return current;
		case Axis.Y:
			current.y = Mathf.Clamp( value, min, max );
			return current;
		case Axis.Z:
			current.z = Mathf.Clamp( value, min, max );
			return current;
		default:
			return default(Vector3);
		}
	}

	public static Vector3 Axis_V_Add_Clamp( Axis axis, Vector3 current, float toAdd, float min, float max )
	{
		switch( axis )
		{
		case Axis.X:
			current.x = Mathf.Clamp( current.x + toAdd, min, max );
			return current;
		case Axis.Y:
			current.y = Mathf.Clamp( current.y + toAdd, min, max );
			return current;
		case Axis.Z:
			current.z = Mathf.Clamp( current.z + toAdd, min, max );
			return current;
		default:
			return default(Vector3);
		}
	}

	public static Vector3 Direction_V( Direction dir )
	{
		switch( dir )
		{
		case Direction.Right:
			return Vector3.right;
		case Direction.Left:
			return Vector3.left;
		case Direction.Forward:
			return Vector3.forward;
		case Direction.Backward:
			return Vector3.back;
		case Direction.Up:
			return Vector3.up;
		case Direction.Down:
			return Vector3.down;
		default:
			return default(Vector3);
		}
	}

	public static string GetUIDistanceValue(float dist)
	{
		float distance =  Mathf.RoundToInt( dist );
		string distText = "";
		if(distance >= 1000)
		{
			distance /= 1000;

			distText = distance.ToString ();
			int i = distText.IndexOf ('.');
			if(distText.Length > i + 2)
				distText = distText.Remove (i + 2);
			distText += "k";
		}
		else
			distText = distance.ToString() + "m";
		return distText;
	}
}
