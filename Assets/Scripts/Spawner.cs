using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Spawner : MonoBehaviour
{
	public GameObject Stomper;
	public GameObject Swarmer;

	public float SpawnDelay = 1.5f;
	public float SpawnOffset = 2f;
	
	public float WaveTime = 10;
	public float WaveTimeDecay = 1;
	public float WaveTimeMin = 2;
	
	public float NextWave = 3;
	
	public float StomperChance = 0;
	public float StomperChangeIncrease = 0.1f;
	public float StomperChanceMax = 1;
	
	public float SwarmerWaveSize = 10;
	public float SwarmerWaveIncrement = 1;
	
	public GameObject InactiveIcon;
	public GameObject ActiveIcon;
	public Vector3 IconOffset;
	
	public Vector2[] SpawnPoints;
	private List<SpawnIcon> _icons = new List<SpawnIcon>();

	// Use this for initialization
	public void Start () {
		foreach (var spawnPoint in SpawnPoints)
		{
			var active = Instantiate(ActiveIcon);
			active.transform.position = (Vector3) spawnPoint + IconOffset;
			var inactive = Instantiate(InactiveIcon);
			inactive.transform.position = (Vector3) spawnPoint + IconOffset;
			_icons.Add(new SpawnIcon(this, active, inactive));
		}
	}

	private void FixedUpdate()
	{
		NextWave -= Time.deltaTime;
		if (NextWave <= 0)
		{
			// spawn
			_icons[Mathf.FloorToInt(Random.Range(0, _icons.Count))].DoSpawn();
			
			// adjust
			WaveTime = Mathf.Clamp(WaveTime - WaveTimeDecay, WaveTimeMin, WaveTime);
			NextWave += WaveTime;

			StomperChance = Mathf.Clamp(StomperChance + StomperChangeIncrease, StomperChance, StomperChanceMax);

			SwarmerWaveSize += SwarmerWaveIncrement;
		}
	}

	private class SpawnIcon
	{
		private Spawner _parent;
		private GameObject _active;
		private GameObject _inactive;

		public SpawnIcon(Spawner parent, GameObject active, GameObject inactive)
		{
			_parent = parent;
			_active = active;
			_inactive = inactive;
			SetActive(false);
		}

		public void DoSpawn()
		{
			SetActive(true);
			_parent.StartCoroutine(SpawnAfterDelay());
		}

		private IEnumerator SpawnAfterDelay()
		{
			yield return new WaitForSeconds(_parent.SpawnDelay);
			for (var i = 0; i < _parent.SwarmerWaveSize; i++)
			{
				var pos = _active.transform.position - _parent.IconOffset +
				          (Vector3) Random.insideUnitCircle * _parent.SpawnOffset;
				var swarmer = Instantiate(_parent.Swarmer);
				swarmer.transform.position = pos;
			}
			if (Random.value >= 1 - _parent.StomperChance)
			{
				var stomper = Instantiate(_parent.Stomper);
				stomper.transform.position = _active.transform.position - _parent.IconOffset;
			}
			SetActive(false);
		}
		
		private void SetActive(bool active)
		{
			_active.SetActive(active);
			_inactive.SetActive(!active);
		}
	}
}
