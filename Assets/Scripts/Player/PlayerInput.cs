using Rewired;
using UnityEngine.EventSystems;

public class PlayerInput : IPlayerInput
{
    Player player;

    public float HorizontalMov { get; private set; }
    public float VerticalMov { get; private set; }
    public bool RunInput { get; private set; }
    public bool CrouchInput { get; private set; }
    public bool RollInput { get; private set; }
    public bool LightAttackInput { get; private set; }
    public bool HeavyAttackInput { get; private set; }
    public bool TacticPauseInput { get; private set; }

    public PlayerInput()
    {
        player = ReInput.players.GetPlayer(0);
    }

    public void ProcessInput()
    {
        HorizontalMov = player.GetAxis("HorizontalMov");
        VerticalMov = player.GetAxis("VerticalMov");
        RunInput = player.GetButton("Run") ? true : false;
        CrouchInput = player.GetButton("Crouch") ? true : false;
        RollInput = player.GetButtonDown("Roll") ? true : false;

        if (!EventSystem.current.IsPointerOverGameObject() && !TacticPauseManager.Instance.IsTacticPauseActive)
        {
            LightAttackInput = player.GetButtonDown("LightAttack");
            HeavyAttackInput = player.GetButtonDown("HeavyAttack");
        }

        TacticPauseInput = player.GetButtonDown("TacticPause");
    }
}
