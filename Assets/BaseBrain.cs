using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using UnityEngine.Networking;

public class BaseBrain : MobaAttackBase {

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}

	protected override void SetTarget(NetworkIdentity target) {
		//Don't do anything with our target
	} 

	public override void OnDeath(DamageSource source, Damageable damageable) {
		if(isServer) {
			var nm = GameObject.Find("NetworkManager");
			nm.GetComponent<MobaNetworkManager>().ServerChangeScene("OfflineScene");
		}
		base.OnDeath(source, damageable);
	}
}
