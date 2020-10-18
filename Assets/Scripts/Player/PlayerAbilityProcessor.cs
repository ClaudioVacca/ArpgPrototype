using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using UnityEngine;

public class PlayerAbilityProcessor : MonoBehaviour
{
    public static PlayerAbilityProcessor Instance;

    private Queue<AbilityType> abilitiesQueue = new Queue<AbilityType>();

    private Dictionary<AbilityType, Ability> abilities = new Dictionary<AbilityType, Ability>();

    private bool _initialized;

    public bool IsExecutingAbility { get; set; }

    public Ability AbilityBeingExecuted { get; set; }

    private void Awake()
    {
        if (Instance == null)
            Instance = this;
        else
            Destroy(this.gameObject);

        Initialize();
    }

    private void Initialize()
    {
        abilities.Clear();

        var abilityTypes = Assembly.GetAssembly(typeof(Ability)).GetTypes().Where(t => typeof(Ability).IsAssignableFrom(t) && t.IsAbstract == false);
        foreach (var abilityType in abilityTypes)
        {
            Ability ability = Activator.CreateInstance(abilityType) as Ability;
            abilities.Add(ability.AbilityType, ability);
        }

        _initialized = true;
    }

    public void EnqueueAbility(AbilityType abilityType)
    {
        abilitiesQueue.Enqueue(abilityType);
    }

    public void DequeueAndUseAbility()
    {
        if (abilitiesQueue.Count == 0)
            return;

        UseAbility(abilitiesQueue.Dequeue());
    }

    public void UseAbility(AbilityType abilityType)
    {
        if (!_initialized)
            Initialize();

        AbilityBeingExecuted = abilities[abilityType];
        StartCoroutine(AbilityBeingExecuted.ProcessAbility());
    }


}
