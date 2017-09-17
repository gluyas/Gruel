using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HUD : MonoBehaviour {

	public Bomber bomber;
	public Image cooldown;
	private float timer;
	private bool recharging = false;

	// Use this for initialization
	void Start () {
		timer = bomber.bombDelay;
	}

	// Update is called once per frame
	void Update () {
		if (cooldown.fillAmount < 1.0f && recharging) {
			cooldown.fillAmount += 1.0f / timer * Time.deltaTime;
		} else if (recharging && cooldown.fillAmount >= 1.0f) {
			cooldown.fillAmount = 1.0f;
			recharging = false;
		} else if (!bomber.bombAvailable && !recharging) {
			cooldown.fillAmount = 0f;
			recharging = true;
		}
	}
}
