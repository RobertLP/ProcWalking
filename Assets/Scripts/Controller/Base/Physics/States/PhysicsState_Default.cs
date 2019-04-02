using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using StateMachine;

public class PhysicsState_Default : StateBase<Physics>
{
    public override void Initialize()
    {

    }
    public override void Exit()
    {
    }
    public override void OnFixedUpdate()
    {
        actor.CalcLinearVelocity(actor.CalcForce(), actor.EngineRB.rotation);
        actor.CalcAngularVelocity();
    }
    public override void OnUpdate()
    {
    }
    public override void OnLateUpdate()
    {
    }
}
