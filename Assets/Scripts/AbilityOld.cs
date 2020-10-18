using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AbilityOld
{
    int id;
    string name;

    public AbilityOld(int id, string name)
    {
        this.id = id;
        this.name = name;
    }

    public void Execute()
    {
        switch (id)
        {
            case 1:
                //PlayerController.Instance.HurricanAbilityUsed();
                break;
        }
    }
}
