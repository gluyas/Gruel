using System;
using UnityEngine;
using Random = UnityEngine.Random;

[RequireComponent(typeof(Entity))]
public class Player : MonoBehaviour
{	
	public static Player Instance { get; private set; }

	public Rigidbody2D RigidBody { get; private set; }	
	public Entity Entity { get; private set; }
	
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
	}	

	private void Update()
	{
		// MOVEMENT
		//_ent.Velocity = (new Vector2(Input.GetAxis("Horizontal"), Input.GetAxis("Vertical")));
		Entity.WishMovement = new Vector2(-Input.GetAxis("Horizontal"), Input.GetAxis("Vertical"));
	}
}
