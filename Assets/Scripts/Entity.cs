using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

/// <summary>
/// Component for objects to move around within the world
/// Provides a simpler movement model than Unity's physics engine.
/// </summary>
[RequireComponent(typeof(Rigidbody2D))]
[RequireComponent(typeof(Collider2D))]
public class Entity : MonoBehaviour
{
	public readonly UnityEvent OnDamage = new UnityEvent(); 
	public readonly UnityEvent OnDeath = new UnityEvent(); 
	
	public float MaxSpeed;
	public float MaxAcceleration;

	public bool AutoFacing = true;
	public bool DestroyOnDeath = false;

	private bool _flying;
	public bool Flying
	{
		get { return _flying; }
		set
		{
			_flying = false;
			CheckFall();
		}
	}

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

	public Vector2 Facing { get; set; }

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
		OnDeath.AddListener(() =>
		{
			if (DestroyOnDeath) Destroy(this.gameObject);
		});
	}
	
	private void FixedUpdate()
	{	
		if (Vector2.Dot(WishMovement, Movement) <= WishMovement.magnitude)
		{
			var force = MaxAcceleration * _rb.mass;
			_rb.AddForce((WishMovement - Movement).normalized * force);
			if (_rb.velocity.magnitude < 0.1) _rb.velocity = Vector2.zero;
		}

		if (AutoFacing)
		{
			if (WishMovement.magnitude > 0.1) Facing = WishMovement;
		}
		var sign = Mathf.Sign(Vector3.Cross(Vector2.down, Facing).z);
		_rb.rotation = sign * Vector2.Angle(Vector2.down, Facing);
	}

	private void OnPlatformExit()
	{
		_platforms--;
		CheckFall();
	}
	
	private void CheckFall()
	{
		if (!HasPlatforms && !Flying)
		{
			var newBody = GameObject.CreatePrimitive(PrimitiveType.Sphere);
			newBody.GetComponent<Renderer>().enabled = false;
			var newRb = newBody.AddComponent<Rigidbody>();
		
			newBody.transform.position = this.transform.position;
			newRb.velocity = this._rb.velocity;

			for (var i = transform.childCount-1; i >= 0 ; i--)
			{
				transform.GetChild(i).SetParent(newBody.transform);
			}
			OnDeath.Invoke();
		}
	}
	
	private void OnPlatformEnter()
	{
		_platforms++;
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
