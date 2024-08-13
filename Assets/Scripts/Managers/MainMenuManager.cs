using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenuManager : MonoBehaviour
{
    [SerializeField] private GameObject _player;
    [SerializeField] private RecordKeyConfig playerKeys;
    public bool _is_trigger_gate {  get; set; }
    public int _current_level { get; set; } = 1;


    // Update is called once per frame
    void Update()
    {
        HandleInput();
    }
    public void HandleInput()
    {
        // Player stuff
        if (_player == null) return;
        if (Input.GetKeyDown(playerKeys.jump))
        {
            _player.GetComponent<PlayerMovement>().HandleStartJump();
        }

        if (Input.GetKeyUp(playerKeys.jump))
        {
            _player.GetComponent<PlayerMovement>().HandleEndJump();
        }

        bool moveLeft = Input.GetKey(playerKeys.moveLeft);
        bool moveRight = Input.GetKey(playerKeys.moveRight);


        if (moveLeft ^ moveRight)
        {
            // Kiểm tra phím di chuyển trái
            if (moveLeft)
            {
                _player.GetComponent<PlayerMovement>().HandleMovement(-1);
                //Debug.Log("Left");
            }

            // Kiểm tra phím di chuyển phải
            if (moveRight)
            {
                _player.GetComponent<PlayerMovement>().HandleMovement(1);
                //Debug.Log("Right");
            }
        }
        if (Input.GetKeyDown(playerKeys.attack))
        {
            if(_is_trigger_gate)
            {
                GameManager.Instance.Current_level= _current_level;
                SceneManager.LoadSceneAsync("Level1 demo");
            }
            else
            {
                _player.GetComponent<PlayerAttack>().HandleAttack();
            }
        }
    }


}
