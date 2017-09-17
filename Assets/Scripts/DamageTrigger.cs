using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Collider2D))]
public class DamageTrigger : MonoBehaviour
{
	public bool RemoveOnTrigger;
	public int Damage;
	public ParticleSystem Explosion;
	
	private void OnTriggerEnter2D(Collider2D other)
	{
		var entity = other.GetComponent<Entity>();
		if (entity != null)
		{
			var dmgExplosion = Instantiate (Explosion);
			dmgExplosion.transform.position = this.transform.position;
			entity.Damage(Damage);
			if (RemoveOnTrigger) Destroy(this.gameObject);
		}
	}
}
