using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(CircleCollider2D))]
public class Bomb : MonoBehaviour
{
	public float ExplosionDelay;
	public float ExplosionRadius;
	public float ExplosionMaxForce;
	public float ExplosionExponent;
	
	public float RemainingTime { get; private set; }

	private Collider2D _explosionTrigger;
	
	private void Awake()
	{
		RemainingTime = ExplosionDelay;
		
		var explosionTrigger = GetComponent<CircleCollider2D>();
		explosionTrigger.enabled = false;
		explosionTrigger.radius = ExplosionRadius;
		explosionTrigger.isTrigger = true;
		_explosionTrigger = explosionTrigger;
	}

	private void FixedUpdate()
	{
		RemainingTime -= Time.deltaTime;
		if (RemainingTime <= 0)
		{
			_explosionTrigger.enabled = true;
			StartCoroutine(DestroyAfterFixedUpdate());
		}
	}

	private void OnTriggerEnter2D(Collider2D other)
	{
		var entity = other.gameObject.GetComponent<Entity>();
		if (entity != null)
		{
			var rb = entity.GetComponent<Rigidbody2D>();
			var toEntity = rb.position - new Vector2(transform.position.x, transform.position.y);
			if (toEntity.magnitude <= ExplosionRadius)
			{
				var force = ExplosionMaxForce * Mathf.Pow(1 - toEntity.magnitude / ExplosionRadius, ExplosionExponent);
				rb.AddForce(toEntity.normalized * force, ForceMode2D.Impulse);
			}
		}
	}

	private IEnumerator DestroyAfterFixedUpdate()
	{
		yield return new WaitForFixedUpdate();
		Destroy(this.gameObject);
	}
}
