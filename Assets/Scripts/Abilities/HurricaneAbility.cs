using System.Collections;
using UnityEngine;

public class HurricaneAbility : Ability
{
    public float HurricaneTime = 4.5f;

    public override float? PlayerSpeedWhileUsingAbility { get; } = 10f;

    public override AbilityType AbilityType => AbilityType.Hurricane;

    public override IEnumerator ProcessAbility()
    {
        PlayerAbilityProcessor.Instance.IsExecutingAbility = true;
        PlayerController.Instance.playerAttacksProcessor.AttacksResets();
        PlayerController.Instance.playerAnimatorProcessor.SetHurricane(true);
        yield return new WaitForSeconds(HurricaneTime);
        PlayerController.Instance.playerAnimatorProcessor.SetHurricane(false);
        PlayerAbilityProcessor.Instance.IsExecutingAbility = false;
    }
}
