public interface IPlayerAbilityProcessor 
{
    float PlayerSpeedWhileUsingAbility { get; }

    void EnqueueAbility();
    void DequeueAndUseAbility();
}
