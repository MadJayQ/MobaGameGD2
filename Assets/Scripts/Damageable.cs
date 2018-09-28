using UnityEngine;
using UnityEngine.Events;
using UnityEngine.SceneManagement;

public class Damageable : MonoBehaviour {

	[System.Serializable]
	public class DamageEvent : UnityEvent<DamageSource, Damageable> {}

	[System.Serializable]
	public class HealthSetEvent : UnityEvent<int, DamageSource> {}

	public int maxHealth;
	public DamageEvent OnTakeDamage;
	public DamageEvent OnDie;
	public HealthSetEvent OnHealthSet;

	protected int m_CurrentHealth;

	public int GetHealth() {
		return m_CurrentHealth;
	}

	void Start () {
		m_CurrentHealth = maxHealth;
	}

	public void OnPlayerDie(DamageSource source, Damageable damageable) {
		SceneManager.LoadScene(5, LoadSceneMode.Single);
	}
	public void TakeDamage(DamageSource source) {
		m_CurrentHealth -= source.damageAmount;

		if(m_CurrentHealth <= 0) {
            m_CurrentHealth = 0;
            OnDie.Invoke(source, this);
		}

        OnHealthSet.Invoke(m_CurrentHealth, source);

        OnTakeDamage.Invoke(source, this);
    }
}
