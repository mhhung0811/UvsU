using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerManager : MonoBehaviour
{
    [SerializeField] private PlayerAnimation _player_animation;
    [SerializeField] private PlayerAttack _player_attack;
    [SerializeField] private PlayerHP _player_hp;
    [SerializeField] private PlayerModel _player_model;
    [SerializeField] private PlayerMovement _player_movement;
    [SerializeField] private PlayerUI _player_ui;

    /*public GameObject GetOneWayPlatform()
    {
        return  _player_model._current_one_way_platfrom;
    }*/
}
