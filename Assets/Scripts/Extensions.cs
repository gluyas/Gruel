using UnityEngine;

public static class Extensions
{
	public static T RandomElement<T>(this T[] array) where T : class
	{
		if (array.Length == 0) return null;
		return array[Mathf.FloorToInt(Random.Range(0, array.Length))];
	}
}
