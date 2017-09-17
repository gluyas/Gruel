using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Bomber : MonoBehaviour
{
	public GameObject Bomb;
	public float bombDelay;
	public bool bombAvailable = true;
	public Image cooldown;
	public Image cooldown2;
	
	private void Update () {
		if (Input.GetMouseButtonDown (0) && bombAvailable) {
			bombAvailable = false;
			StartCoroutine ("Recharge");
			var mouseRay = Camera.main.ScreenPointToRay (Input.mousePosition);
			var playerPlane = new Plane (new Vector3 (0, 0, 1), Vector3.zero);
			
			float toPlane;
			if (playerPlane.Raycast (mouseRay, out toPlane)) {
				var targetPos = mouseRay.GetPoint (toPlane);
				var bomb = Instantiate (Bomb);
				bomb.transform.position = targetPos;
			}
		} else if (Input.GetMouseButtonDown (0) && !bombAvailable) {
			cooldown.color = new Color (1.0f, 1.0f, 0.0f);
			cooldown.rectTransform.localScale *= 1.18f;
			cooldown2.rectTransform.localScale *= 1.18f;
		}
	}

	IEnumerator Recharge() {
		yield return new WaitForSeconds (bombDelay);
		bombAvailable = true;
	}
}
