public interface IPlayerAttacksProcessor
{
    bool IsLightAttacking { get; set; }
    bool IsHeavyAttacking { get; set; }

    void ProocessAttacks();
    void AttacksResets();
}
