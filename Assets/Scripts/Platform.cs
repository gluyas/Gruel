using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Collider2D))]
public class Platform : MonoBehaviour
{
	private static HashSet<Platform> _all = new HashSet<Platform>();
	
	private Collider2D _collider;

	private void Start()
	{
		_collider = GetComponent<Collider2D>();
		_all.Add(this);
	}

	public static float? DistanceFromPlatform(Vector2 pos)
	{
		float minSqrDistance = float.MaxValue;
		foreach (var platform in _all)
		{
			if (platform._collider.OverlapPoint(pos)) return null;
			else minSqrDistance = Mathf.Min(minSqrDistance, platform._collider.bounds.SqrDistance(pos));
		}
		return Mathf.Sqrt(Mathf.Sqrt(minSqrDistance));
	}

	private void OnTriggerEnter2D(Collider2D other)
	{
		other.SendMessage("OnPlatformEnter", SendMessageOptions.DontRequireReceiver);
	}

	private void OnTriggerExit2D(Collider2D other)
	{
		other.SendMessage("OnPlatformExit", SendMessageOptions.DontRequireReceiver);
	}
}
