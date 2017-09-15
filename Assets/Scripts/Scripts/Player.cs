using System;
using UnityEngine;
using Random = UnityEngine.Random;

[RequireComponent(typeof(Entity))]
public class Player : MonoBehaviour
{	
	public static Player Instance { get; private set; }

	public Rigidbody2D RigidBody { get; private set; }
	
	private Entity _ent;
	
	public int Hp
	{
		get { return _ent.Hp; }
	}
	
	private void Start()
	{
		Instance = this;
		_ent = GetComponent<Entity>();
		RigidBody = GetComponent<Rigidbody2D>();
	}	

	private void Update()
	{
		// MOVEMENT
		//_ent.Velocity = (new Vector2(Input.GetAxis("Horizontal"), Input.GetAxis("Vertical")));
		_ent.WishMovement = new Vector2(-Input.GetAxis("Horizontal"), Input.GetAxis("Vertical"));
	}
}
