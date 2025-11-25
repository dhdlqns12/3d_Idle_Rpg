public interface IState
{
    public void Enter(); // 상태 진입 시
    public void Exit(); // 상태 나갈때
    public void OnStateUpdate(); // 상태 중 업데이트
    public void OnStatePhysicsUpdate(); // 상태 중 물리 업데이트(Fixed Update)
}
