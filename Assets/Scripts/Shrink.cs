using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Shrink : MonoBehaviour {

	public Image cooldown1;
	public Image cooldown2;

	// Use this for initialization
	void Start () {
		
	}

	// Update is called once per frame
	void Update () {
		if (cooldown1.rectTransform.localScale.x > 1.0f) {
			cooldown1.rectTransform.localScale *= 0.98f;
			cooldown2.rectTransform.localScale *= 0.98f;
		} else if (cooldown1.rectTransform.localScale.x < 1.0f) {
			cooldown1.rectTransform.localScale = new Vector3(1.0f, 1.0f, 1.0f);
			cooldown2.rectTransform.localScale = new Vector3(1.0f, 1.0f, 1.0f);
		}
		if (cooldown2.color.g != 0.0f) {
			StartCoroutine ("ColChange");
		}
	}

	IEnumerator ColChange() {
		yield return new WaitForSeconds (0.02f);
		cooldown2.color = new Color (1.0f, 0.0f, 0.0f);
	}
}
