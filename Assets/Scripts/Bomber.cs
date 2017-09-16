using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bomber : MonoBehaviour
{
	public GameObject Bomb;
	
	private void Update () {
		if (Input.GetMouseButtonDown(0))
		{
			var mouseRay = Camera.main.ScreenPointToRay(Input.mousePosition);
			var playerPlane = new Plane(new Vector3(0, 0, 1), Vector3.zero);
			
			float toPlane;
			if (playerPlane.Raycast(mouseRay, out toPlane))
			{
				var targetPos = mouseRay.GetPoint(toPlane);
				var bomb = Instantiate(Bomb);
				bomb.transform.position = targetPos;
			}
		}
	}
}
