public interface IPlayerRoll
{
    bool CanRoll { get; }
    bool IsRolling { get; }
    bool IsRollInCooldown { get; }

    void ProcessRoll();
}