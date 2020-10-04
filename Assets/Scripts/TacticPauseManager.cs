using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TacticPauseManager : MonoBehaviour
{
    public static TacticPauseManager Instance;
    private Queue<Ability> abilitiesQueue = new Queue<Ability>();

    public bool tacticPause;
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
    }

    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void LateUpdate()
    {
    }

    internal void HurricaneAbilityUsed()
    {
        EnqueueAbility(1, "Hurricane");
    }

    void EnqueueAbility(int id, string name)
    {
        abilitiesQueue.Enqueue(new Ability(id, name));
    }

    public void DequeueAbility()
    {
        if (abilitiesQueue.Count == 0)
            return;

        Ability ability = abilitiesQueue.Dequeue();
        ability.Execute();
    }
}
