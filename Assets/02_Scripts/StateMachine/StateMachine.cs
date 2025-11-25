public abstract class StateMachine
{
    protected IState curState;

    public void ChangeState(IState state)
    {
        curState?.Exit();
        curState = state;
        curState?.Enter();
    }

    public void OnStateUpdate()
    {
        curState?.OnStateUpdate();
    }

    public void OnStatePhysicsUpdate()
    {
        curState?.OnStatePhysicsUpdate();
    }
}
