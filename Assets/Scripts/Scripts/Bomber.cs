using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bomber : MonoBehaviour
{
	public float BombRadius;
	public float BombMaxForce;
	public float BombExponent;
	
	void Update () {
		if (Input.GetMouseButtonDown(0))
		{
			var mouseRay = Camera.main.ScreenPointToRay(Input.mousePosition);
			var playerPlane = new Plane(new Vector3(0, 0, 1), Vector3.zero);
			
			float toPlane;
			if (playerPlane.Raycast(mouseRay, out toPlane))
			{
				var targetPos = mouseRay.GetPoint(toPlane);
				var toPlayer = Player.Instance.RigidBody.position - new Vector2(targetPos.x, targetPos.y);
				if (toPlayer.magnitude <= BombRadius)
				{
					var force = BombMaxForce * Mathf.Pow(1 - toPlayer.magnitude / BombRadius, BombExponent);
					Player.Instance.RigidBody.AddForce(toPlayer.normalized * force, ForceMode2D.Impulse);
				}
			}
		}
	}
}
