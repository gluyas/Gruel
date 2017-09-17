using System.Collections;
using System.Collections.Generic;
using JetBrains.Annotations;
using UnityEngine;

[RequireComponent(typeof(CircleCollider2D))]
public class Bomb : MonoBehaviour
{
	public AudioClip[] ExplosionSounds;

	public float ExplosionDelay;
	public float ExplosionRadius;
	public float ExplosionMaxForce;
	public float ExplosionExponent;
	
	public float ScreenShakeStrength;
	public float ScreenShakeRadius;
	public float ScreenShakeExponent;

	public ParticleSystem parts;
	
	public float RemainingTime { get; private set; }
	
	public Vector2 Position { get {return new Vector2(transform.position.x, transform.position.y);} }

	private Collider2D _explosionTrigger;
	private GameObject _drop;
	private InstantiateDropMethod _dropMethod;

	public delegate void InstantiateDropMethod(GameObject drop, Bomb bomb);
	
	
	public void SetDrop(GameObject drop, InstantiateDropMethod instantiateMethod = null )
	{
		_drop = drop;
		_dropMethod = instantiateMethod;
	}

	private AudioSource _audio;
	
	private void Awake()
	{
		RemainingTime = ExplosionDelay;
		
		var explosionTrigger = GetComponent<CircleCollider2D>();
		explosionTrigger.enabled = false;
		explosionTrigger.radius = ExplosionRadius;
		explosionTrigger.isTrigger = true;
		_explosionTrigger = explosionTrigger;
		_audio = GetComponent<AudioSource>();
	}

	private float ExplosionStrength(Vector2 pos, float radius, float exponent)
	{
		var distance = (pos - Position).magnitude;
		if (distance >= radius) return 0;
		else return Mathf.Pow(1 - distance / radius, exponent);
	}
	
	private void FixedUpdate()
	{
		RemainingTime -= Time.deltaTime;
		if (RemainingTime <= 0)
		{
			var expParts = Instantiate (parts);
			expParts.transform.position = 
				new Vector3(this.transform.position.x, this.transform.position.y, this.transform.position.z+0.45f);
			_explosionTrigger.enabled = true;
			GameCamera.AddShake(ScreenShakeStrength * 
			                    ExplosionStrength(Player.Instance.RigidBody.position, 
				                    ScreenShakeRadius, ScreenShakeExponent));
			Debug.Log(_audio);
			_audio.PlayOneShot(ExplosionSounds.RandomElement());
			StartCoroutine(DestroyAfterFixedUpdate());
		}
	}

	private void OnTriggerEnter2D(Collider2D other)
	{
		var entity = other.gameObject.GetComponent<Entity>();
		if (entity != null)
		{
			var rb = entity.GetComponent<Rigidbody2D>();
			var toEntity = rb.position - Position;
			if (toEntity.magnitude <= ExplosionRadius)
			{
				var force = ExplosionMaxForce * ExplosionStrength(rb.position, ExplosionRadius, ExplosionExponent);
				rb.AddForce(toEntity.normalized * force, ForceMode2D.Impulse);
			}
		}
	}

	private IEnumerator DestroyAfterFixedUpdate()
	{
		yield return new WaitForFixedUpdate();
		if (_drop != null)
		{
			var drop = Instantiate(_drop);
			if (_dropMethod != null) _dropMethod(drop, this);
		}
		Destroy(this.gameObject);
	}
}
