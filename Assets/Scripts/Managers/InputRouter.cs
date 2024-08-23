using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static UnityEngine.Rendering.DebugUI;

public class InputRouter : MonoBehaviour
{
    [SerializeField] private GameSceneUIManager _gameSceneUIManager;

    public void PressX(GameObject player)
    {
        if(_gameSceneUIManager.PausePanel.gameObject.activeSelf)
        {
            _gameSceneUIManager.ChooseCurrentOption();
        }
        else
        {
            player.GetComponent<PlayerAttack>().HandleAttack();
        }
    }
    public void PressArrowLeft(GameObject player)
    {
        if (_gameSceneUIManager.PausePanel.gameObject.activeSelf)
        {
            _gameSceneUIManager.ChangeVolume(-1f);
        }
        else
        {
            player.GetComponent<PlayerMovement>().HandleMovement(-1);
        }
    }
    public void  PressArrowRight(GameObject player) 
    {
        if (_gameSceneUIManager.PausePanel.gameObject.activeSelf)
        {
            _gameSceneUIManager.ChangeVolume(1f);
        }
        else
        {
            player.GetComponent<PlayerMovement>().HandleMovement(1);
        }
    }
    public void PressLeftAndRight(GameObject player)
    {
        if (!_gameSceneUIManager.PausePanel.gameObject.activeSelf)
        {
            player.GetComponent<PlayerMovement>().HandleMoveLeftAndRight();
        }
    }
    public void PressArrowUp()
    {
        _gameSceneUIManager.HandleOption(-1);
    }
    public void PressArowDown(GameObject player)
    {
        if (!_gameSceneUIManager.PausePanel.gameObject.activeSelf)
        {
            Debug.Log("down");
            player.GetComponent<PlayerMovement>().FallDown();
        }
        else
        {
            _gameSceneUIManager.HandleOption(1);
        }
    }
    public void PressESC() 
    {
        _gameSceneUIManager.ChangePauseState();
    }
}
