using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Spin : MonoBehaviour
{
	public float SpinRate;

	private void FixedUpdate()
	{
		this.transform.Rotate(new Vector3(0, 0, 1), SpinRate * Time.deltaTime);
	}
}
