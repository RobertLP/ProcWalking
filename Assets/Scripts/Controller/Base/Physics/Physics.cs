using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using StateMachine;

[RequireComponent(typeof(Rigidbody))]
public class Physics : PhysicsBase
{
    public bool linearDisabled;
    public bool angularDisabled;

    public enum DragMode { linear, speed, full };
    public enum AngularDragMode { linear, speed, full };
    public enum ForceMode { MaxForce_Direct, TopSpeed };
    public enum ThrusterMode { Absolute, Dynamic };

    private Rigidbody engineRB;

    private PhysicsStateMachine stateMachine;
    public PhysicsStateMachine StateMachine
    { get { return stateMachine; } }

    [Header("EngineData Linear")]
    public ForceMode forceMode;
    [SerializeField] private float maxForce;
    public float MaxForce
    {
        get { return maxForce; }
        set
        {
            ForceCalculator();
            maxForce = value;
        }
    }
    [SerializeField] private float topSpeed;
    private float TopSpeed
    {
        get { return topSpeed; }
        set
        {
            ForceCalculator();
            topSpeed = value;
        }
    }

    [Space(7)]
    [SerializeField] private DragMode dragMode;
    [SerializeField] private float drag;
    [SerializeField] private float mass;

    [Header("EngineData Angular")]
    [SerializeField] private AngularDragMode angularDragMode;
    [SerializeField] private Vector3 steeringTorque; // excess force
    [SerializeField] private Vector3 angular_drag;
    [SerializeField] private Vector3 rotationInertia; // mass
    
    [Header("ReadOut Engine")]
    [ReadOnly] [SerializeField] private Vector3 excessForce;
    [ReadOnly] [SerializeField] private Vector3 measuredDrag;
    [ReadOnly] [SerializeField] private Vector3 acceleration;
    [ReadOnly] [SerializeField] private Vector3 velocity;
    [Space(7)]
    [ReadOnly] [SerializeField] private Vector3 ExcessTorque;
    [ReadOnly] [SerializeField] private Vector3 measuredAngularDrag;
    [ReadOnly] [SerializeField] private Vector3 angularAcceleration;
    [ReadOnly] [SerializeField] private Vector3 angularVelocity;

    [Header("ReadOut Controls")]
    [ReadOnly] [SerializeField] private Vector3 rotationInput; // pitch yaw roll
    [ReadOnly] [SerializeField] private Vector3 throttleInput; // right up forward
    [ReadOnly] public bool throttleInputReceived; // TODO: Make me nice and private


    #region collision variables
    private bool collosionStay;
    private bool collisionDetected;
    private Vector3 avaragehitNormalCollection;
    private Vector3 avarageHitNormal;
    private int hitNormalCounter;
    #endregion

    #region Getters
    public Vector3 RotationInput
    {
        get { return rotationInput; }
    }
    public Vector3 ThrottleInput
    {
        get { return throttleInput; }
    }
    public Vector3 Velocity
    {
        get { return velocity; }
    }
    public Rigidbody EngineRB
    {
        get { return engineRB; }
    }
    public Vector3 AngularVelocity
    {
        get { return angularVelocity; }
    }
    public float RotationApproximativeTopSpeed
    {
        get { return ((Vector2)steeringTorque).magnitude / ((Vector2)angular_drag).magnitude; }
    }
    public float LinearTopSpeed
    {
        get { return TopSpeed; }
    }

    #endregion

    protected override void Awake()
    {
        base.Awake();
        stateMachine = new PhysicsStateMachine(this);
        stateMachine.RegisterState(new PhysicsState_Default());
        stateMachine.ChangeState<PhysicsState_Default>();
    }

    private void Start()
    {
        ForceCalculator();
        engineRB = this.GetComponent<Rigidbody>();
        engineRB.useGravity = false;
    }

    private void FixedUpdate()
    {
        if (this.gameObject.activeSelf)
        {
            stateMachine.FixedUpdate();
            throttleInputReceived = false;
            throttleInput.x = 0;
            throttleInput.y = 0;
            rotationInput = Vector3.zero;
        }
    }
    private void Update()
    {
        if (this.gameObject.activeSelf)
            stateMachine.Update();
    }
    private void LateUpdate()
    {
        if (this.gameObject.activeSelf)
            stateMachine.LateUpdate();
    }

    private void ForceCalculator()
    {
        if (forceMode == ForceMode.TopSpeed)
            maxForce = topSpeed * drag;
        else if (forceMode == ForceMode.MaxForce_Direct)
            topSpeed = maxForce / drag;
    }

    public Vector3 CalcForce()
    {
        return new Vector3(throttleInput.x * MaxForce, throttleInput.y * MaxForce, throttleInput.z * MaxForce);
    }

    #region VelocityCalculation
    public void CalcLinearVelocity(Vector3 force, Quaternion direction, float engineKillDragMultiplier = 1)
    {
        measuredDrag = Drag() * engineKillDragMultiplier; // need to do fresh drag
        excessForce = (direction * force) + measuredDrag;
        acceleration = excessForce / mass;
        acceleration *= Time.fixedDeltaTime;
        velocity += acceleration;
        engineRB.velocity = velocity;
    }

    public void CalcAngularVelocity()
    {
        measuredAngularDrag = AngularDrag();
        Vector3 appliedSteeringTorque = Vector3.Scale(steeringTorque, engineRB.rotation * FinalSteeringInput());
        ExcessTorque = appliedSteeringTorque + measuredAngularDrag;
        angularAcceleration = ExcessTorque.Devide(rotationInertia);
        angularVelocity += angularAcceleration * Time.fixedDeltaTime;
        if (angularVelocity.x == float.NaN)
            angularVelocity.x = 0;
        if (angularVelocity.y == float.NaN)
            angularVelocity.y = 0;
        if (angularVelocity.z == float.NaN)
            angularVelocity.z = 0;
        engineRB.angularVelocity = angularVelocity;
    }
    #endregion

    #region inputReceivers
    public void LinearInput(Vector3 linearInput)
    {
        if (!linearDisabled)
        {
            throttleInputReceived = true;
            throttleInput = linearInput;
            if (linearInput != Vector3.zero)
                throttleInput = ClampedInput(throttleInput);
        }
    }
    public void LinearInputAdd(float linearInput, GeneralInfo.Axis axis)
    {
        if (!linearDisabled)
        {
            throttleInputReceived = true;
            throttleInput = GeneralInfo.Axis_V_Add_Clamp(axis, throttleInput, linearInput, axis == GeneralInfo.Axis.Z ? 0 : -1, 1);
            throttleInput = ClampedInput(throttleInput);
        }
        else
            throttleInput = Vector3.zero;
    }
    public void LinearInput(float linearInput, GeneralInfo.Axis axis)
    {
        if (!linearDisabled)
        {
            throttleInputReceived = true;
            throttleInput = GeneralInfo.Axis_V_Clamp(axis, throttleInput, linearInput, axis == GeneralInfo.Axis.Z ? 0 : -1, 1);
            throttleInput = ClampedInput(throttleInput);
        }
        else
            throttleInput = Vector3.zero;
    }
    public void AngularInput(Vector3 angularInput)
    {
        if (!angularDisabled)
            rotationInput = ClampedInput(angularInput);
    }

    public void AngularInputZ(float Z_Input)
    {
        rotationInput.z = Z_Input;
    }
    #endregion

    #region InputModifer
    public Vector3 FinalSteeringInput()
    {
        return rotationInput;
    }
    #endregion

    #region DragCalculation
    private Vector3 Drag()
    {
        switch (dragMode)
        {
            case DragMode.speed:
                return -drag * velocity * velocity.magnitude;
            case DragMode.linear:
                return -drag * velocity;
            case DragMode.full:
                return (-drag * velocity) + (-drag * velocity * velocity.magnitude);
            default:
                Debug.LogError("UnspecifiedDrag");
                return default(Vector3);
        }
    }

    private Vector3 AngularDrag()
    {
        switch (angularDragMode)
        {
            case AngularDragMode.speed:
                return -Vector3.Scale(angular_drag, angularVelocity) * angularVelocity.magnitude;
            case AngularDragMode.linear:
                return -Vector3.Scale(angular_drag, angularVelocity);
            case AngularDragMode.full:
                return (-Vector3.Scale(angular_drag, angularVelocity)) + -(Vector3.Scale(angular_drag, angularVelocity) * angularVelocity.magnitude);
            default:
                Debug.LogError("UnspecifiedAngularDrag");
                return default(Vector3);
        }
    }
    #endregion

    private Vector3 ClampedInput(Vector3 input)
    {
        Vector2 newInput = new Vector2(input.x, input.y);
        newInput = Vector2.ClampMagnitude(newInput, 1);
        input.x = newInput.x;
        input.y = newInput.y;
        input.z = Mathf.Clamp(input.z, -1, 1); // TODO: Check if this is correct. dont forget
        return input;
    }

    public Vector3 HeadingToAngularInput(Vector3 heading)
    {
        Vector3 input = this.transform.InverseTransformDirection(heading);
        input.z = 0;
        input = input.normalized;
        return input;
    }

    public void ZeroVelocity()
    {
        this.velocity = Vector3.zero;
    }
}