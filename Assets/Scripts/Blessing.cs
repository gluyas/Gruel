using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Collider2D))]
public abstract class Blessing : MonoBehaviour
{
	public float DespawnTime = 5;
	public float Duration = 1;
	
	protected abstract void OnApply(Player player);
	protected abstract void OnExpire(Player player);

	private void Awake()
	{
		StartCoroutine(Despawn());
	}

	private void OnTriggerEnter2D(Collider2D other)
	{
		var player = other.GetComponent<Player>();
		if (player != null) {
			OnApply(player);
			player.StartCoroutine(ExpireEffect(player, this));
			Destroy(this.gameObject);
		}
	}

	private IEnumerator Despawn()
	{
		yield return new WaitForSeconds(DespawnTime);
		Destroy(this.gameObject);
	}
	
	private static IEnumerator ExpireEffect(Player player, Blessing blessing)
	{
		yield return new WaitForSeconds(blessing.Duration);
		Debug.Log("Expire");
		blessing.OnExpire(player);
	}
}
