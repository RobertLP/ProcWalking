using StateMachine;

public class PhysicsStateMachine : StateMachine<Physics>
{
    public PhysicsStateMachine(Physics owner) : base(owner) { }
}