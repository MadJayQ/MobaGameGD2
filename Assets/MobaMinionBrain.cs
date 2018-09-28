using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using UnityEngine.Networking;

public class MobaMinionBrain : MobaAttackBase {

	[SyncVar] public Vector3 AIMovementDirection;

	private Vector3 m_SavedDirection = Vector3.zero;

	void SetMovementDirection(Vector3 direction) {
		AIMovementDirection = direction;
	}
	// Update is called once per frame
	void Update () {
		m_CharacterController.MovementVector = AIMovementDirection;
	}

	protected override void OnAttackStateChanged(AttackState oldState, AttackState newState) {
		if(newState == AttackState.ATTACK_STATE_TARGETFOUND) {
			m_SavedDirection = AIMovementDirection;
			SetMovementDirection(Vector3.zero);
		}
		base.OnAttackStateChanged(oldState, newState);
	}

	public override void OnTargetSlain(NetworkIdentity other) {
		if(m_Alive) {
			Debug.Log("I AM VICTORIOUS!");
			AIMovementDirection = m_SavedDirection;
			m_SavedDirection = Vector3.zero;
		}
	}


}
