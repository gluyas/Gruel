using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

[RequireComponent(typeof(Entity))]
public class Swarmer : MonoBehaviour
{
	public static readonly HashSet<Swarmer> All = new HashSet<Swarmer>();

	public float AnimationSpeed;
	
	// AI property weighting
	public float SwarmRadius;
	
	public float AvoidanceRadius;
	public float AvoidanceExponent;
	public float AvoidanceWeight = 1f;

	public float AlignmentWeight = 1f;
	
	public float CohesionWeight = 1f;
	
	public float PursuitWeight = 1f;

	public float EdgeWeight = 1f;
	public float EdgeDistance = 5f;
	public float EdgeExponent = 2f;

	public float NoiseWeight;
	public float NoiseDeltaMin;
	public float NoiseDeltaMax;
	private Vector2 _noise;

	public float TotalWeight
	{
		get { return AlignmentWeight + CohesionWeight + AvoidanceWeight + PursuitWeight + NoiseWeight; }
	}
	
	private Entity _entity;
	private Rigidbody2D _rb;
	private Animator _animator;
	
	private void OnEnable()
	{
		_animator = GetComponentInChildren<Animator>();
		_entity = GetComponent<Entity>();
		_rb = GetComponent<Rigidbody2D>();
		_noise = Random.insideUnitCircle;
		//_entity.OnDeath.AddListener(() => Destroy(gameObject));
		All.Add(this);
	}
	
	// Update is called once per frame
	private void Update()
	{
		var swarmCentre = Vector2.zero;
		var swarmDirection = Vector2.zero;
		var swarmCount = 0;

		var avoidance = Vector2.zero;
		var avoidanceCount = 0;
		
		foreach (var other in All)
		{
			if (other == this) continue;

			var toOther = other._rb.position - this._rb.position;
			var distance = toOther.magnitude;
			
			if (distance <= SwarmRadius)
			{
				swarmCentre += other._rb.position;
				swarmDirection += other._entity.Movement;
				
				swarmCount += 1;

				if (distance <= AvoidanceRadius)
				{
					toOther.Normalize();
					avoidance -= toOther * Mathf.Pow(1 - distance / AvoidanceRadius, AvoidanceExponent);
					
					avoidanceCount += 1; 
				}
			}
		}

		var evasion = Vector2.zero;
		var evasionCount = 0;

		// all metrics are mapped to the range [0, 1], to simplify tweaking by the designer
		
		// COHESION
		swarmCentre /= swarmCount;
		var cohesion = (swarmCentre - _rb.position) / SwarmRadius;
		
		// ALIGNMENT
		swarmDirection /= swarmCount;
		var alignment = swarmDirection - _entity.Movement;		// using 2*speed as scale factor means that this factor
		alignment /= 2;											// is only at max when boids move opposite directions
		if (alignment.magnitude > 1) alignment.Normalize();            

		// AVOIDANCE
		if (avoidanceCount > 0) avoidance /= avoidanceCount;
		
		// EVASION
		if (evasionCount > 0) evasion /= evasionCount;
		
		// PURSUIT
		var pursuit = Player.Instance.RigidBody.position - _rb.position;
		pursuit.Normalize();		// constant accelleration towards player
	
		// EDGE
		var edgeExceed = Platform.DistanceFromPlatform(_rb.position + _entity.Movement * EdgeDistance)?? 0;
		Debug.Log(edgeExceed);
		var edge = -_entity.Movement.normalized * edgeExceed / EdgeDistance;
		if (_entity.Velocity.magnitude < 0.5) edge = Vector2.zero;
		
		// EDGE CASES
		if (swarmCount == 0)
		{
			alignment = Vector2.zero;
			cohesion = Vector2.zero;
		}
		
		// NOISE
		var sign = Random.value > 0.5 ? -1 : 1;
		var noiseDelta = Quaternion.AngleAxis(
			sign * Random.Range(NoiseDeltaMin, NoiseDeltaMax) * Time.deltaTime, Vector3.forward);
		_noise = noiseDelta * _noise;
		          
		// FINAL CALCULATION
		var result = CohesionWeight * cohesion + AlignmentWeight * alignment + AvoidanceWeight * avoidance +
		             PursuitWeight * pursuit + NoiseWeight * _noise + EdgeWeight * edge;
		_entity.WishMovement = result.normalized;

		_animator.speed = _entity.Movement.magnitude * AnimationSpeed;
	}

	private void OnDestroy()
	{
		All.Remove(this);
	}
}
