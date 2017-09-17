using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameCamera : MonoBehaviour
{
	public float ShakeDecayRate = 1.5f;
	
	private static float _shake;
	private static float _freeze;
	
	private Vector3 _playerOffset;

	public static void AddShake(float shake)
	{
		_shake += shake;
	}
	
	private void Start()
	{
		_playerOffset = this.transform.position - Player.Instance.transform.position;
	}

	private void Update()
	{
		if (Input.GetButton("Restart"))
		{
			SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
		}
			
		if (Player.Instance.Entity.Dead) return;
		
		this.transform.position = Player.Instance.transform.position + _playerOffset;
		var shakeDir = (Vector3) Random.insideUnitCircle;
		this.transform.localPosition += shakeDir * _shake;

		_shake /= ShakeDecayRate;
	}
}


