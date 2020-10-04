using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UiManager : MonoBehaviour
{
    [SerializeField]
    GameObject PanelTacticPause;
    [SerializeField]
    GameObject PanelTacticPauseAbilities;

    public static UiManager Instance;

    private void Awake()
    {
        if (Instance == null)
            Instance = this;
        else
            Destroy(this.gameObject);
    }

    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {

    }

    public void OpenTacticPauseMenu()
    {
        PanelTacticPause.SetActive(true);
    }

    public void CloseTacticPauseMenu()
    {
        PanelTacticPause.SetActive(false);
        PanelTacticPauseAbilities.SetActive(false);
    }

    public void OpenTacticPauseMenuAbilities()
    {
        PanelTacticPauseAbilities.SetActive(true);
    }

    public void CloseTacticPauseMenuAbilities()
    {
        PanelTacticPauseAbilities.SetActive(false);
    }

    public void OnTacticPauseMenuAbilitiesClick()
    {
        OpenTacticPauseMenuAbilities();
    }

    public void OnTacticPauseMenuAbilitiesHurricaneClick()
    {
        TacticPauseManager.Instance.IsTacticPauseActive = false;
        TacticPauseManager.Instance.HurricaneAbilityUsed();
    }

    public void OnTacticPauseMenuAbilitiesBackClick()
    {
        CloseTacticPauseMenuAbilities();
    }

    public void OnTacticPauseMenuExitClick()
    {
        TacticPauseManager.Instance.IsTacticPauseActive = false;
    }
}
