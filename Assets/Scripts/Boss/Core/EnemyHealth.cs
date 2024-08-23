using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyHealth : MonoBehaviour
{
    private float _enemy_hp = 1f;
    
    public float Health
    {
        get { return  _enemy_hp; }
        set { _enemy_hp = value; }
    }
    public void AddHP(float value)
    {
        _enemy_hp += value;
    }
    public void MinusHP(float value)
    {
        _enemy_hp -= value;
    }
}
