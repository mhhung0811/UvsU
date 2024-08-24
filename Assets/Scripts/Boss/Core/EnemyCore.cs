using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyCore : MonoBehaviour
{
    private FSMState _curent_state;
    public string _init_state = "Idle";
    [SerializeField] private EnemyHealth _enemy_health;
    public List<FSMState> fsmStates = new List<FSMState>();
    private void Start()
    {
        LoadInitState();
    }
    private void Update()
    {
        Execute();
    }
    void LoadInitState()
    {
        _curent_state = GetState(_init_state);
    }
    public void ChangeState(string state_id)
    {
        FSMState state = GetState(state_id);
        if(state == null)
        {
            return;
        }
        _curent_state = state;
    }
    private FSMState GetState(string state_id)
    {
        foreach(FSMState state in fsmStates)
        {
            if(state.id_state == state_id)
            {
                return state;
            }
        }
        return null;
    }
    private void Execute()
    {
        if(CanExecuteState())
        {
            _curent_state?.UpdateStateEnemy(this);
        }
    }
    private bool CanExecuteState()
    {
        if(_enemy_health != null && _enemy_health.Health > 0)
        {
            return true;
        }
        else
        {
            return false;
        }
    }
}
