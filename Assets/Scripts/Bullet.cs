using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Rigidbody2D))]
[RequireComponent(typeof(DamageTrigger))]
public class Bullet : MonoBehaviour
{
	public float MaxDistance;
	public float Speed;
	public bool InheritVelocity;

	private Rigidbody2D _rb;
	private Vector2 _initPos;

	public void Init(Vector2 pos, Vector2 dir, Entity owner = null)
	{
		_initPos = pos;
		_rb = GetComponent<Rigidbody2D>();
		_rb.position = pos;
		_rb.velocity = dir.normalized * Speed;
		
		if (owner != null && InheritVelocity)
		{
			_rb.velocity += owner.Velocity;
		}
	}
	
	private void FixedUpdate()
	{
		if ((_rb.position - _initPos).magnitude >= MaxDistance) Destroy(this.gameObject);
	}
}
