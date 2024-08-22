using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Spikes : MonoBehaviour
{
    [SerializeField] private IngameManager _in_game_manager;
    public IngameManager ingameManager
    {  
        get { return _in_game_manager; }
        set { _in_game_manager = value;}
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        _in_game_manager.CheckBulletSpikeTrigger(collision.gameObject);
    }
}
