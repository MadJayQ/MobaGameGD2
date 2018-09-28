using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using KinematicCharacterController;

public class MobaPlayer : BaseCharacterController {

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}


	public override void UpdateRotation(ref Quaternion currentRotation, float deltaTime) {

	}
	public override void UpdateVelocity(ref Vector3 currentVelocity, float deltaTime) {

	}

	public override void BeforeCharacterUpdate(float deltaTime) {

	}

	public override void AfterCharacterUpdate(float deltaTime) {

	}

	public override void PostGroundingUpdate(float deltaTime) {

	}

	public override bool IsColliderValidForCollisions(Collider coll) {
		return true;
	}

	public override void OnGroundHit(Collider hitCollider, Vector3 hitNormal, Vector3 hitPoint, ref HitStabilityReport hitStabilityReport) {

	}

	public override void OnMovementHit(Collider hitCollider, Vector3 hitNormal, Vector3 hitPoint, ref HitStabilityReport hitStabilityReport) {

	}

	public override void ProcessHitStabilityReport(Collider hitCollider, Vector3 hitNormal, Vector3 hitPoint, Vector3 atCharacterPosition, Quaternion atCharacterRotation, ref HitStabilityReport hitStabilityReport) {
		
	}

}
