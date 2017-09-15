using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraTracking : MonoBehaviour
{
	private Vector3 _posOffset;
	
	private void Start()
	{
		_posOffset = this.transform.position - Player.Instance.transform.position;
	}

	private void Update()
	{
		this.transform.position = Player.Instance.transform.position + _posOffset;
	}
	
}


