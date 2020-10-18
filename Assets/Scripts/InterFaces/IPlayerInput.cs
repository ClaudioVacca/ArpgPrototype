public interface IPlayerInput
{
    float HorizontalMov { get; }
    float VerticalMov { get; }
    bool RunInput { get; }
    bool CrouchInput { get; }
    bool RollInput { get; }
    bool LightAttackInput { get; }
    bool HeavyAttackInput { get; }
    bool TacticPauseInput { get; }

    void ProcessInput();
}
