using UnityEngine;

public class TacticPauseManager : MonoBehaviour
{
    public static TacticPauseManager Instance;
    PlayerController playerController;
    PlayerAbilityProcessor playerAbilityProcessor;

    public bool tacticPauseInput;
    private bool _isTacticPauseActive;

    public bool IsTacticPauseActive
    {
        get { return _isTacticPauseActive; }
        set
        {
            _isTacticPauseActive = value;
            if (value)
            {
                Time.timeScale = 0.01f;
                UiManager.Instance.OpenTacticPauseMenu();
            }
            else
            {
                Time.timeScale = 1f;
                UiManager.Instance.CloseTacticPauseMenu();
            }
        }
    }

    private void Awake()
    {
        if (Instance == null)
            Instance = this;
        else
            Destroy(this.gameObject);

        playerController = FindObjectOfType<PlayerController>();
        playerAbilityProcessor = FindObjectOfType<PlayerAbilityProcessor>();
    }

    private void Update()
    {
        if (playerController.playerInput.TacticPauseInput)
            IsTacticPauseActive = !IsTacticPauseActive;
    }


    internal void HurricaneAbilitySelected()
    {
        playerAbilityProcessor.EnqueueAbility(AbilityType.Hurricane);
    }

    
}
