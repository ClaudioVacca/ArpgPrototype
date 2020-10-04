using Rewired;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using static Rewired.Controller;

public class UiManager : MonoBehaviour
{
    Player player;
    EventSystem es;

    [SerializeField]
    GameObject PanelTacticPause;
    [SerializeField]
    GameObject PanelTacticPauseAbilities;

    [SerializeField]
    GameObject[] tacticMenuButtons;
    [SerializeField]
    GameObject[] abilitiesButtons;

    private bool isPanelAbilitiesOpen = false;
    private bool isPanelTacticMenuOpen = false;
    private int currentTacticMenuButtonSelected = 0;
    private int currentAbilitiesButtonSelected = 0;


    public static UiManager Instance;

    private void Awake()
    {
        player = ReInput.players.GetPlayer(0);

        if (Instance == null)
            Instance = this;
        else
            Destroy(this.gameObject);
    }

    // Start is called before the first frame update
    void Start()
    {
        es = GameObject.Find("EventSystem").GetComponent<EventSystem>();
        es.SetSelectedGameObject(null);
        es.SetSelectedGameObject(tacticMenuButtons[0]);
        currentTacticMenuButtonSelected = 0;
    }

    // Update is called once per frame
    void Update()
    {
        if (isPanelTacticMenuOpen && !isPanelAbilitiesOpen)
        {
            if (player.GetButtonDown("MenuDown"))
            {
                currentTacticMenuButtonSelected++;
                if (currentTacticMenuButtonSelected >= tacticMenuButtons.Length)
                    currentTacticMenuButtonSelected = 0;

                es.SetSelectedGameObject(null);
                es.SetSelectedGameObject(tacticMenuButtons[currentTacticMenuButtonSelected]);
            }

            if (player.GetButtonDown("MenuUp"))
            {
                currentTacticMenuButtonSelected--;
                if (currentTacticMenuButtonSelected < 0)
                    currentTacticMenuButtonSelected = tacticMenuButtons.Length - 1;

                es.SetSelectedGameObject(null);
                es.SetSelectedGameObject(tacticMenuButtons[currentTacticMenuButtonSelected]);
            }
        }
        else if (isPanelAbilitiesOpen)
        {
            if (player.GetButtonDown("MenuDown"))
            {
                currentAbilitiesButtonSelected++;
                if (currentAbilitiesButtonSelected >= abilitiesButtons.Length)
                    currentAbilitiesButtonSelected = 0;

                es.SetSelectedGameObject(null);
                es.SetSelectedGameObject(abilitiesButtons[currentAbilitiesButtonSelected]);
            }

            if (player.GetButtonDown("MenuUp"))
            {
                currentAbilitiesButtonSelected--;
                if (currentAbilitiesButtonSelected < 0)
                    currentAbilitiesButtonSelected = abilitiesButtons.Length - 1;

                es.SetSelectedGameObject(null);
                es.SetSelectedGameObject(abilitiesButtons[currentAbilitiesButtonSelected]);
            }
        }
    }

    public void OpenTacticPauseMenu()
    {
        PanelTacticPause.SetActive(true);
        es.SetSelectedGameObject(null);
        es.SetSelectedGameObject(tacticMenuButtons[0]);
        currentTacticMenuButtonSelected = 0;
        isPanelTacticMenuOpen = true;
    }

    public void CloseTacticPauseMenu()
    {
        PanelTacticPause.SetActive(false);
        PanelTacticPauseAbilities.SetActive(false);
        isPanelTacticMenuOpen = false;
        isPanelAbilitiesOpen = false;
    }

    public void OpenTacticPauseMenuAbilities()
    {
        PanelTacticPauseAbilities.SetActive(true);
        es.SetSelectedGameObject(null);
        es.SetSelectedGameObject(abilitiesButtons[0]);
        currentAbilitiesButtonSelected = 0;
        isPanelAbilitiesOpen = true;
    }

    public void CloseTacticPauseMenuAbilities()
    {
        PanelTacticPauseAbilities.SetActive(false);
        es.SetSelectedGameObject(null);
        es.SetSelectedGameObject(tacticMenuButtons[0]);
        currentTacticMenuButtonSelected = 0;
        isPanelAbilitiesOpen = false;
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
