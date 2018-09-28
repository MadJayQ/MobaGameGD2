using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using UnityEngine.Networking;

[RequireComponent(typeof(MobaCharacterController))]
public class MobaMinionBrain : NetworkBehaviour {

	[SyncVar] public Vector3 AIMovementDirection;


	private MobaCharacterController m_CharacterController;

	// Use this for initialization
	public void Awake() {
		Initialize();
	}
	public void Initialize() {
		m_CharacterController = GetComponent<MobaCharacterController>();
		m_CharacterController.OnSpawnEvent.Invoke(m_CharacterController);
	}

	public void SetTeam(MobaTeam team) {
		m_CharacterController.Team.CloneFromOther(team);
	}

	public void OnTriggerEnter(Collider col) {
		var controller = col.gameObject.GetComponent<MobaCharacterController>();
		if(controller == null) {
			return;
		}

		if(controller.Team.IsFriendly(m_CharacterController.Team)) {
			return;
		} else {
			SetMovementDirection(Vector3.zero);
		}
	}

	void SetMovementDirection(Vector3 direction) {
		AIMovementDirection = direction;
	}
	// Update is called once per frame
	void Update () {
		m_CharacterController.MovementVector = AIMovementDirection;
	}
}
