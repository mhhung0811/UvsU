using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AttackDecision :  IFSMDecision
{
    public FSMAttackAction _attack_action;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    public override bool Decision()
    {
        return _attack_action.Delay <= 0;
    }
}
