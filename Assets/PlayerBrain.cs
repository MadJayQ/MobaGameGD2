using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using UnityEngine.Networking;

public class PlayerBrain : MobaAttackBase {

	private Vector2 m_InputVector;

	private Vector3 m_SpawnLocation;
	private Quaternion m_SpawnRotation;

	// Use this for initialization
	void Start () {
		if(isLocalPlayer) {
			StartCoroutine(SlowInputUpdate());
			var mainCamera = GameObject.Find("Main Camera");
			var mobaCamera = mainCamera.GetComponent<MobaCamera>();
			mobaCamera.followController = m_CharacterController;
		}

		m_SpawnLocation = transform.position;
		m_SpawnRotation = transform.rotation;
	}
	
	// Update is called once per frame
	void Update () {
		m_CharacterController.MovementVector = new Vector3(
			m_InputVector.x, 0f, m_InputVector.y);
	}

	IEnumerator SlowInputUpdate() {
		while(true) {
			yield return new WaitForSeconds(50f / 1000f);
			var tempVector = new Vector2(Input.GetAxisRaw("Horizontal"), Input.GetAxisRaw("Vertical")); 
			CmdMove(tempVector);
		}
	}

	[Command]
	public void CmdMove(Vector2 inputVector) {
		m_InputVector = inputVector;
	}

	public override void OnDeath(DamageSource source, Damageable damageable) {
		m_CharacterController.Motor.SetPositionAndRotation(m_SpawnLocation, m_SpawnRotation);
	}
}
