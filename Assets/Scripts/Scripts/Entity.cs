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
	
	public float MovementSpeed;
	public int MaxHp;

	public Vector2 Velocity { get; private set; }
	public int Hp { get; private set; }

	private Vector2 _facing = Vector2.up;

	private bool _dead = false;

	private Rigidbody2D _rb;
	
	private void Start()
	{
		Hp = MaxHp;
		_rb = GetComponent<Rigidbody2D>();
	}

	public Vector3 Facing
	{
		get { return _facing; }
		set
		{
			_facing = value.normalized;
			transform.rotation = Quaternion.FromToRotation(Vector3.forward, _facing);
		}
	}

	/// <summary>
	/// Set the Character's horizontal movement
	/// </summary>
	/// <param name="movement">Vector to move in, as a coefficient of the Character's movement speed</param>
	/// <param name="truncate">If true, then the new speed will not exceed MovemenSpeed</param>
	public void SetMovement(Vector2 movement, bool truncate = true)
	{
		if (truncate && movement.magnitude > 1)
		{
			movement.Normalize();
		}
		movement *= MovementSpeed;
		Velocity = movement;
	}
	
	/// <summary>
	/// Adjust the Character's movement. Effectively acceleration
	/// </summary>
	/// <param name="delta">Vector to adjust movment in, as a coefficient of the Character's movement speed</param>
	/// <param name="truncate">If true, then the new speed will not exceed MovemenSpeed</param>
	public void AddMovement(Vector2 delta, bool truncate = true)
	{
		SetMovement(Velocity + delta, truncate);	
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
	
	private void FixedUpdate()
	{
		_rb.MovePosition(_rb.position + Velocity * Time.deltaTime);
	}
}
