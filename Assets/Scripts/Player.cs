﻿using System;
using UnityEngine;
using Random = UnityEngine.Random;

[RequireComponent(typeof(Entity))]
public class Player : MonoBehaviour
{	

	public AudioClip[] IdleSound;
	public AudioClip[] VocalsIdleSound;
	public AudioClip[] ShootingSoftSounds;
	public AudioClip[] ShootingSounds;
	public static Player Instance { get; private set; }

	public AudioSource Audio;

	public Rigidbody2D RigidBody { get; private set; }	
	public Entity Entity { get; private set; }

	public GameObject Bullet;
	public float FireScreenShake = 0.5f;
	public float FireRateAuto;
	private float _nextAuto;
	
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
		Audio = GetComponent<AudioSource>();
	}	
	
	private void FixedUpdate()
	{
		if (Entity.Dead) return;
		
		// MOVEMENT
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
		
		// AIM
		var aim = new Vector2(-Input.GetAxis("AimX"), Input.GetAxis("AimY"));
		if (aim.magnitude > 0.1)
		{
			Entity.AutoFacing = false;
			Entity.Facing = aim;
		}
		else
		{
			Entity.AutoFacing = true;
		}

//		var animationDirection = Mathf.Sign(Vector2.Dot(Entity.WishMovement, Entity.Facing));
//		foreach (var animator in _animators)
//		{
//			animator.speed = animationDirection;
//		}
		
		// SHOOTING
		if (Input.GetButton("Fire1"))
		{
			_nextAuto -= Time.deltaTime;
			if (_nextAuto <= 0 || Input.GetButtonDown("Fire1"))
			{
				Audio.pitch = Random.Range(0.3f, 1.2f);
				Audio.PlayOneShot(ShootingSoftSounds.RandomElement(), 0.4f);
				var bullet = Instantiate(Bullet).GetComponent<Bullet>();
				bullet.Init(RigidBody.position, Entity.Facing, this.Entity);
				_nextAuto = 1 / FireRateAuto;
				GameCamera.AddShake(FireScreenShake);
			}
		}
		
	}

	
}
