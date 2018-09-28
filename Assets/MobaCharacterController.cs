using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

using KinematicCharacterController;

public class MobaCharacterController : BaseCharacterController
{

    [System.Serializable]
    public class SpawnEvent : UnityEvent<MobaCharacterController> { }

    public SpawnEvent OnSpawnEvent;

    public Vector3 Gravity = new Vector3(0, -30f, 0);

    public Vector3 MovementVector = new Vector3(0f, 0f, 0f);

    private Vector3 _movementVector;

    public bool ControlGravity = true;

    public GravityManager GravityManager;

    public MobaTeam Team;

    // Use this for initialization
    void Awake()
    {
        GravityManager = GameObject.Find("Planet").GetComponent<GravityManager>();
        GravityManager.OnCharacterSpawned(this);
        var damageable = GetComponent<Damageable>();
        if(damageable != null) {
            damageable.OnDie.AddListener(GravityManager.OnDeath);
        }
    }

    // Update is called once per frame
    void Update()
    {
        Vector3 planarDirection = Vector3.ProjectOnPlane(transform.rotation * Vector3.forward, Motor.CharacterUp).normalized;
        if (planarDirection.sqrMagnitude == 0f)
        {
            planarDirection = Vector3.ProjectOnPlane(transform.rotation * Vector3.up, Motor.CharacterUp).normalized;
        }
        Quaternion planarRotation = Quaternion.LookRotation(planarDirection, Motor.CharacterUp);
        _movementVector = planarRotation * MovementVector;
    }

    public override void UpdateRotation(ref Quaternion currentRotation, float deltaTime)
    {
        currentRotation = Quaternion.FromToRotation((currentRotation * Vector3.up), -Gravity) * currentRotation;
    }
    public override void UpdateVelocity(ref Vector3 currentVelocity, float deltaTime)
    {
        Vector3 targetMovementVelocity = Vector3.zero;

        // Ground movement
        if (Motor.GroundingStatus.IsStableOnGround)
        {
            Vector3 effectiveGroundNormal = Motor.GroundingStatus.GroundNormal;
            if (currentVelocity.sqrMagnitude > 0f && Motor.GroundingStatus.SnappingPrevented)
            {
                // Take the normal from where we're coming from
                Vector3 groundPointToCharacter = Motor.TransientPosition - Motor.GroundingStatus.GroundPoint;
                if (Vector3.Dot(currentVelocity, groundPointToCharacter) >= 0f)
                {
                    effectiveGroundNormal = Motor.GroundingStatus.OuterGroundNormal;
                }
                else
                {
                    effectiveGroundNormal = Motor.GroundingStatus.InnerGroundNormal;
                }
            }

            // Reorient velocity on slope
            currentVelocity = Motor.GetDirectionTangentToSurface(currentVelocity, effectiveGroundNormal) * currentVelocity.magnitude;

            // Calculate target velocity
            Vector3 inputRight = Vector3.Cross(_movementVector, Motor.CharacterUp);
            Vector3 reorientedInput = Vector3.Cross(effectiveGroundNormal, inputRight).normalized * _movementVector.magnitude;
            targetMovementVelocity = reorientedInput * 3f;

            // Smooth movement Velocity
            currentVelocity = Vector3.Lerp(currentVelocity, targetMovementVelocity, 1 - Mathf.Exp(-15 * deltaTime));
        }
        else
        {
            // Add move input
            if (MovementVector.sqrMagnitude > 0f)
            {
                targetMovementVelocity = MovementVector * 5f;

                // Prevent climbing on un-stable slopes with air movement
                if (Motor.GroundingStatus.FoundAnyGround)
                {
                    Vector3 perpenticularObstructionNormal = Vector3.Cross(Vector3.Cross(Motor.CharacterUp, Motor.GroundingStatus.GroundNormal), Motor.CharacterUp).normalized;
                    targetMovementVelocity = Vector3.ProjectOnPlane(targetMovementVelocity, perpenticularObstructionNormal);
                }

                Vector3 velocityDiff = Vector3.ProjectOnPlane(targetMovementVelocity - currentVelocity, Gravity);
                currentVelocity += velocityDiff * 1f * deltaTime;
            }

            // Gravity
            currentVelocity += Gravity * deltaTime;

            // Drag
            currentVelocity *= (1f / (1f + (0.1f * deltaTime)));
        }
    }


    public override void BeforeCharacterUpdate(float deltaTime)
    {

    }

    public override void AfterCharacterUpdate(float deltaTime)
    {

    }

    public override void PostGroundingUpdate(float deltaTime)
    {

    }

    public override bool IsColliderValidForCollisions(Collider coll)
    {
        var controller = coll.GetComponent<MobaCharacterController>();
        if(controller == null) {
            return true;
        }
        if(Team.IsFriendly(controller.Team)) {
            return false;
        }
        return true;
    }

    public override void OnGroundHit(Collider hitCollider, Vector3 hitNormal, Vector3 hitPoint, ref HitStabilityReport hitStabilityReport)
    {

    }

    public override void OnMovementHit(Collider hitCollider, Vector3 hitNormal, Vector3 hitPoint, ref HitStabilityReport hitStabilityReport)
    {

    }

    public override void ProcessHitStabilityReport(Collider hitCollider, Vector3 hitNormal, Vector3 hitPoint, Vector3 atCharacterPosition, Quaternion atCharacterRotation, ref HitStabilityReport hitStabilityReport)
    {

    }
}
