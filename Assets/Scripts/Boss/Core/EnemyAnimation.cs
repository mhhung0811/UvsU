using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyAnimation : MonoBehaviour
{
    [SerializeField] private Animator _animator;

    private readonly int attack = Animator.StringToHash("AttackLeft");
    
    public void SetTriggerAttack()
    {
        _animator.SetTrigger(attack);
    }
    
}
