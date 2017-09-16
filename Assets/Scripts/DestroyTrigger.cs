using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DestroyTrigger : MonoBehaviour {
	
	private void OnTriggerEnter(Collider other)
	{
		Destroy(other.transform.root.gameObject);
	}
}
