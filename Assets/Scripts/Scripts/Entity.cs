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

	private int _platforms = 0;
	public bool HasPlatforms { get { return _platforms > 0; }}
	
	private Rigidbody2D _rb;
	
	private void Start()
	{
		Hp = MaxHp;
		_rb = GetComponent<Rigidbody2D>();
	}
	
	private void FixedUpdate()
	{	
		if (Vector2.Dot(WishMovement, Movement) <= WishMovement.magnitude)
		{
			var force = MaxAcceleration * _rb.mass;
			_rb.AddForce((WishMovement - Movement).normalized * force);
			if (_rb.velocity.magnitude < 0.1) _rb.velocity = Vector2.zero;
		}
	}

	private void OnPlatformExit()
	{
		_platforms--;
		if (_platforms <= 0)
		{
			Debug.Log("You fell!");
		}
	}

	private void OnPlatformEnter()
	{
		_platforms++;
	}

//	public void Damage(int damage)
//	{
//		Hp -= damage;
//		OnDamage.Invoke();
//		if (Hp <= 0 && !_dead)
//		{
//			OnDeath.Invoke();
//			_dead = true;
//		}
//	}
}
