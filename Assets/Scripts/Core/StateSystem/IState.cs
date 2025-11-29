/// <summary>
/// 状态接口 - 状态模式的核心
/// </summary>
public interface IState
{
    void Enter(PlayerController player);      
    void Update(PlayerController player);    
    void FixedUpdate(PlayerController player); 
    void Exit(PlayerController player);
    string GetStateName();
}