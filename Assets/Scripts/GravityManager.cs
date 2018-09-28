using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using KinematicCharacterController;
using KinematicCharacterController.Examples;
using System;

public class GravityManager : BaseMoverController
{
    public SphereCollider GravityField;
    public float GravityStrength = 10;
    public Vector3 OrbitAxis = Vector3.forward;
    public float OrbitSpeed = 10;
    private List<MobaCharacterController> _characterControllersOnPlanet = new List<MobaCharacterController>();
    private Vector3 _savedGravity;
    private Quaternion _lastRotation;

    private void Start()
    {

    }

    public void OnCharacterSpawned(MobaCharacterController cc) {
        if(cc.ControlGravity) {
            ControlGravity(cc);
        }
    }

    public void OnDeath(DamageSource source, Damageable damageable) {
        if(damageable.GetComponent<PlayerBrain>()) {
            return; //Hack
        }
        var cc = damageable.GetComponent<MobaCharacterController>();
        if(cc != null) {
            UnControlGravity(cc); //No longer controlling gravity
        }
    }

    public override void UpdateMovement(out Vector3 goalPosition, out Quaternion goalRotation, float deltaTime)
    {
        goalPosition = Mover.Rigidbody.position;
        goalRotation = Quaternion.identity;

        /*
        // Rotate
        Quaternion targetRotation = Quaternion.Euler(OrbitAxis * OrbitSpeed * deltaTime) * _lastRotation;
        goalRotation = targetRotation;
        _lastRotation = targetRotation;
        */

        // Apply gravity to characters
        foreach (var cc in _characterControllersOnPlanet)
        {
            cc.Gravity = (transform.position - cc.transform.position).normalized * GravityStrength;
        }
    }

    void ControlGravity(MobaCharacterController cc)
    {
        _savedGravity = cc.Gravity;
        _characterControllersOnPlanet.Add(cc);
    }

    void UnControlGravity(MobaCharacterController cc)
    {
        cc.Gravity = _savedGravity;
        _characterControllersOnPlanet.Remove(cc);
    }
}