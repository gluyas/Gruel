using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

/// <summary>
/// Component for objects to move around within the world
/// Provides a simpler movement model than Unity's physics engine.
/// </summary>
[RequireComponent(typeof(Rigidbody2D))]
public class Entity : MonoBehaviour
{
	public readonly UnityEvent OnDeath = new UnityEvent();
	public readonly UnityEvent OnDamage = new UnityEvent();
	public readonly UnityEvent OnFall = new UnityEvent();
	
	public float MaxSpeed;
	public float MaxAcceleration;

	private Vector2 _wishMovement;
	public Vector2 WishMovement
	{
		get { return _wishMovement; } 
		set { _wishMovement = Vector2.ClampMagnitude(value, 1); }
	}

	public Vector2 Movement
	{
		get { return Velocity / MaxSpeed; }
	}
	
	public Vector2 WishVelocity
	{
		get { return WishMovement * MaxSpeed; }
	}

	public Vector2 Velocity
	{
		get { return _rb.velocity; }
	}

	public int MaxHp;
	public int Hp { get; private set; }

	private bool _dead = false;
	private bool _moving = false;
	
	private Rigidbody2D _rb;
	
	private void FixedUpdate()
	{	
		if (Vector2.Dot(WishMovement, Movement) <= WishMovement.magnitude)
		{
			var force = MaxAcceleration * _rb.mass;
			_rb.AddForce((WishMovement - Movement).normalized * force);
			if (_rb.velocity.magnitude < 0.1) _rb.velocity = Vector2.zero;
		}
	}
	
	private void Start()
	{
		Hp = MaxHp;
		_rb = GetComponent<Rigidbody2D>();
	}

	public void Damage(int damage)
	{
		Hp -= damage;
		OnDamage.Invoke();
		if (Hp <= 0 && !_dead)
		{
			OnDeath.Invoke();
			_dead = true;
		}
	}
}
