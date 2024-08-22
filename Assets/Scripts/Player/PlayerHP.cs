using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerHP : MonoBehaviour, IDamageable
{
    [SerializeField] private IngameManager _in_game_manager;

    private int _health;

    private void Start()
    {
        LoadComponent();
    }
    private void Update()
    {
        CheckPlayerDeath();
    }
    public void ReceiveDamage(int value)
    {
        _health -= value;
    }
    private void LoadComponent()
    {
        _health = 1;
    }
    private void CheckPlayerDeath()
    {
        if( _health <= 0 )
        {
            _in_game_manager.CheckBulletSpikeTrigger(gameObject);
            _health = 1;
        }
    }
}
