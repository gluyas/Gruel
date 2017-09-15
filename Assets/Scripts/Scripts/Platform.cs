using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Collider2D))]
public class Platform : MonoBehaviour
{
	private Collider2D _collider;

	private void Start()
	{
		_collider = GetComponent<Collider2D>();
	}

	private void OnTriggerEnter2D(Collider2D other)
	{
		other.SendMessage("OnPlatformEnter");
	}

	private void OnTriggerExit2D(Collider2D other)
	{
		other.SendMessage("OnPlatformExit");
	}
}
