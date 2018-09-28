using UnityEngine;
using UnityEngine.Events;

public class DamageSource : MonoBehaviour {
	[System.Serializable]
	public class DamageEvent : UnityEvent<DamageSource, Damageable> {}

	public int damageAmount;
	public DamageEvent OnDamage;
}
