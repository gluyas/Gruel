using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Collider2D))]
public class DamageTrigger : MonoBehaviour
{
	public bool RemoveOnTrigger;
	public int Damage;
	
	private void OnTriggerEnter2D(Collider2D other)
	{
		var entity = other.GetComponent<Entity>();
		if (entity != null)
		{
			entity.Damage(Damage);
			if (RemoveOnTrigger) Destroy(this.gameObject);
		}
	}
}
