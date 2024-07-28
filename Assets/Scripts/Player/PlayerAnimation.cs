using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerAnimation : MonoBehaviour
{
    [SerializeField] private Animator _animator;

    private readonly int _running = Animator.StringToHash("Running");
    private readonly int _is_ground = Animator.StringToHash("Isground");
    private readonly int _velocity_y = Animator.StringToHash("VelocityY");
    private readonly int _attack = Animator.StringToHash("Attack");
    private readonly int _run_attack = Animator.StringToHash("RunAttack");
    private readonly int _air_attack = Animator.StringToHash("AirAttack");


    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    public void SetBoolRunning(bool value)
    {
        _animator.SetBool(_running, value);
    }
    public void SetBoolGround(bool value)
    {
        _animator.SetBool(_is_ground, value);
    }
    public void SetVelocityY(float value)
    {
        _animator.SetFloat(_velocity_y, value);
    }
    public void SetTriggerAttack()
    {
        _animator.SetTrigger(_attack);
    }
    public void SetTriggerRunAttack()
    {
        _animator.SetTrigger(_run_attack);
    }
    public void SetTriggerAirAttack()
    {
        _animator.SetTrigger(_air_attack);
    }
}
