using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class LevelGate : MonoBehaviour
{
    [SerializeField] private MainMenuManager _mainMenuManager;
    [SerializeField] private int _gate_level;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        //if (collision.GetComponent<BulletImpart>()) return;
        GameManager.Instance.Current_level = _gate_level;
        _mainMenuManager._is_trigger_gate = true;
    }
    private void OnTriggerExit2D(Collider2D collision)
    {
        _mainMenuManager._is_trigger_gate = false;
    }
}
