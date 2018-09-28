using System;

using System.Collections;
using System.Collections.Generic;

using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.Events;

[RequireComponent(typeof(SphereCollider))]
[RequireComponent(typeof(DamageSource))]
[RequireComponent(typeof(Damageable))]
[RequireComponent(typeof(MobaCharacterController))]
public class MobaAttackBase : NetworkBehaviour {

    public enum AttackState {
        ATTACK_STATE_IDLE = 0,
        ATTACK_STATE_TARGETFOUND = 1,
        ATTACK_STATE_ATTACKING = 2
    };

    [System.Serializable]
    public class OnKillEvent : UnityEvent<NetworkIdentity> {}

    public OnKillEvent OnKilledTarget;

    protected bool m_Alive = true;

    [SyncVar] public AttackState AttackingState = AttackState.ATTACK_STATE_IDLE;
    protected MobaCharacterController m_CharacterController;
    protected DamageSource m_DamageDealer;
    protected Damageable m_Damageable;
    protected MobaTeam m_Team;
    [SyncVar] private NetworkIdentity m_CurrentTarget;

    [SerializeField] protected float m_AttackTimer = 0.4f;

    private Coroutine m_AttackCoroutine;
    public void Awake() {
        Initialize();
    }

    public virtual void Initialize() {
        m_CharacterController = GetComponent<MobaCharacterController>();
        m_Team = m_CharacterController.Team;
        m_CharacterController.OnSpawnEvent.Invoke(m_CharacterController);
        m_DamageDealer = GetComponent<DamageSource>();
        m_Damageable = GetComponent<Damageable>();

        OnKilledTarget.AddListener(OnTargetSlain);
        m_Damageable.OnDie.AddListener(OnDeath);
    }

    private void OnTriggerEnter(Collider collider) {
        if(collider.isTrigger) {
            return; //Ignore trigger colliders
        }
        if(IsColliderTarget(collider)) {
            if(AttackingState == AttackState.ATTACK_STATE_IDLE) {
                SetTarget(collider.gameObject.GetComponent<NetworkIdentity>());
            }
        }
    }

    protected virtual void SetTarget(NetworkIdentity target) {
        CmdSetTarget(target);
    }

    [Command]
    public void CmdSetTarget(NetworkIdentity other) {
        m_CurrentTarget = other;
        if(isServer) {
            OnAttackStateChanged(AttackingState, AttackState.ATTACK_STATE_TARGETFOUND);
        }
    }

    protected virtual void OnAttackStateChanged(AttackState oldState, AttackState newState) {
        switch(newState) {
            case AttackState.ATTACK_STATE_TARGETFOUND: {
                StartCoroutine(StartAttacking());
                break;
            }
            case AttackState.ATTACK_STATE_ATTACKING: {
                m_AttackCoroutine = StartCoroutine(AttackTimer());
                break;
            }
            case AttackState.ATTACK_STATE_IDLE: {
                if(oldState == AttackState.ATTACK_STATE_ATTACKING) {
                    StopCoroutine(m_AttackCoroutine);
                }
                if(m_Alive == false) {
                    StartCoroutine(DeathTimer());
                }
                break;
            }
        }
        AttackingState = newState;
    }

    protected virtual void OnAttack(DamageSource source, ref NetworkIdentity target) {
        if(!isServer) {
            return; //Only attack on the server.
        }
        if(target == null) {
            return; //Mission failed
        }
        var damageable = target.GetComponent<Damageable>();
        if(damageable == null) {
            return;
        }
        source.OnDamage.Invoke(source, damageable);
    }

    protected IEnumerator DeathTimer() {
        yield return new WaitForEndOfFrame();
        NetworkServer.Destroy(this.gameObject);
        yield return null;
    }

    protected IEnumerator AttackTimer() {
        while(true) {
            yield return new WaitForSeconds(m_AttackTimer);
            OnAttack(m_DamageDealer, ref m_CurrentTarget);
        }
    }

    protected IEnumerator StartAttacking() {
        yield return new WaitForSeconds(0.5f);
        OnAttackStateChanged(AttackingState, AttackState.ATTACK_STATE_ATTACKING);
        yield return null;
    }

    protected bool IsColliderTarget(Collider collider) {
        if(collider.gameObject.GetComponent<NetworkIdentity>() == null) {
            return false; //Only attack networked objects
        }
        var controller = collider.gameObject.GetComponent<MobaCharacterController>();
        if(controller == null || controller.Team.IsFriendly(m_CharacterController.Team)) {
            return false;
        }

        return true;
    }

    public virtual void OnDeath(DamageSource source, Damageable damageable) {
        m_Alive = false;
        OnAttackStateChanged(AttackingState, AttackState.ATTACK_STATE_IDLE); //Dont attack if dead.
        var attackBase = source.GetComponent<MobaAttackBase>();
        var target = damageable.GetComponent<NetworkIdentity>();
        if(attackBase == null) {
            return;
        }
        if(target == null) {
            return;
        }
        attackBase.OnKilledTarget.Invoke(target);
    }

    public virtual void OnTargetSlain(NetworkIdentity other) {
        OnAttackStateChanged(AttackingState, AttackState.ATTACK_STATE_IDLE);
    }

}