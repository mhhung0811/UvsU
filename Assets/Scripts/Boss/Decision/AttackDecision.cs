using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AttackDecision :  IFSMDecision
{
    public FSMAttackAction _attack_action;
    [SerializeField] private InputManager _inputManager;
    [SerializeField] private IngameManager _ingameManager;
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
        if(_attack_action.Delay <= 0 && !_attack_action.SkillActivating && _inputManager.Player != null && _ingameManager.IsPlaying)
        {
            return true;
        }
        else
        {
            return false;
        }
        
    }
}
