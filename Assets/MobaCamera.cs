using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MobaCamera : MonoBehaviour {


	public MobaCharacterController followController;

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		if(followController == null) {
			return;
		}
		Vector3 position = followController.transform.position;
		position += followController.Motor.CharacterUp.normalized * 15;
		Quaternion rot = Quaternion.LookRotation(-followController.Motor.CharacterUp);
		transform.position = position;
		transform.rotation = rot;
	}
}
