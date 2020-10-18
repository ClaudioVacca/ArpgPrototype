using System.Collections;

public abstract class Ability
{
    public Ability() { }

    public abstract float? PlayerSpeedWhileUsingAbility { get; }

    public abstract AbilityType AbilityType { get; }

    public abstract IEnumerator ProcessAbility();
}
