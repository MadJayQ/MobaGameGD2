using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using UnityEngine.Networking;

public class TowerBrain : MobaAttackBase {

	private List<NetworkIdentity> m_Targets = new List<NetworkIdentity>();

	private bool CanAttackTarget(NetworkIdentity target) {
		return true;
	}

	protected override void OnAttack(DamageSource source, ref NetworkIdentity target) {
		foreach(var cachedTarget in m_Targets) {
			if(CanAttackTarget(cachedTarget)) {
				target = cachedTarget;
				break;
			}
		}
		base.OnAttack(source, ref target);
	}

	protected override void SetTarget(NetworkIdentity target) {
		m_Targets.Add(target);
		base.SetTarget(target);
	}

	public override void OnTargetSlain(NetworkIdentity other) {
		m_Targets.Remove(other);

		base.OnTargetSlain(other);
	}
}
