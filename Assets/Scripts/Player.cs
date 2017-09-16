using System;
using UnityEngine;
using Random = UnityEngine.Random;

[RequireComponent(typeof(Entity))]
public class Player : MonoBehaviour
{	
	public static Player Instance { get; private set; }

	public Rigidbody2D RigidBody { get; private set; }	
	public Entity Entity { get; private set; }

	private Animator[] _animators;
	private bool _walking;
	
	public int Hp
	{
		get { return Entity.Hp; }
	}

	private void Awake()
	{
		Instance = this;
	}

	private void Start()
	{
		Entity = GetComponent<Entity>();
		RigidBody = GetComponent<Rigidbody2D>();
		_animators = GetComponentsInChildren<Animator>();
	}	

	private void FixedUpdate()
	{
		Entity.WishMovement = new Vector2(-Input.GetAxis("Horizontal"), Input.GetAxis("Vertical"));
		if (Entity.Velocity.magnitude > 0.2 && !_walking)
		{
			foreach (var animator in _animators)
			{
				animator.SetTrigger("walk");
			}
			_walking = true;
		} 
		else if (Entity.Velocity.magnitude <= 0.2 &&_walking)
		{
			foreach (var animator in _animators)
			{
				animator.SetTrigger("idle");
			}
			_walking = false;
		}
	}
}
