using System.Collections;
using System.Collections.Generic;
using UnityEngine;


using UnityEngine.Networking;

public class MobaSpawner : NetworkBehaviour {


	[SerializeField] private MobaTeam m_Team;

	public GameObject MinionLeftPrefab;
	public GameObject MinionRightPrefab;
	public GameObject MinionUpPrefab;

	// Use this for initialization
	void Start () {
		if(isServer) {
			StartCoroutine(SpawnMinions());
		}
	}
	
	// Update is called once per frame
	void Update () {
		
	}

	void SpawnMinionSet() {
		var minionUp = GameObject.Instantiate(MinionUpPrefab, transform.position, transform.rotation);
		var minionBrain = minionUp.GetComponent<MobaMinionBrain>();
		NetworkServer.Spawn(minionUp);
		var minionLeft = GameObject.Instantiate(MinionLeftPrefab, transform.position, transform.rotation);
		minionBrain = minionLeft.GetComponent<MobaMinionBrain>();
		NetworkServer.Spawn(minionLeft);
		var minionRight = GameObject.Instantiate(MinionRightPrefab, transform.position, transform.rotation);
		minionBrain = minionRight.GetComponent<MobaMinionBrain>();
		NetworkServer.Spawn(minionRight);
	}

	private IEnumerator SpawnMinions() {
		while(true) {
			SpawnMinionSet();
			yield return new WaitForSeconds(5f);
		}
	}
}
