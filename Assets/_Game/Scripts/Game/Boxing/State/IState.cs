public interface IState
{
    void OnEnter(Fighter fighter);
    void OnUpdate();
    void OnExit();
}