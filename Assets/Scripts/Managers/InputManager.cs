using System.Collections;
using System.Collections.Generic;
using System.Data;
using UnityEngine;
using UnityEngine.SceneManagement;
using static System.Runtime.CompilerServices.RuntimeHelpers;
using static UnityEngine.Rendering.DebugUI;

public class InputManager : MonoBehaviour
{
    //[SerializeField] private RecordManager recordManager;
    [SerializeField] private IngameManager ingameManager;
    [SerializeField] private GameSceneUIManager gameSceneUIManager;
    private GameObject player;
    [SerializeField] private RecordKeyConfig playerKeys;
    [SerializeField] private InputRouter _inputRouter;

    // Start is called before the first frame update
    void Start()
    {
        LoadComponent();
        
    }
    public void LoadComponent()
    {
        StartCoroutine(WaitToStartGame());
        //GameManager.Instance.Pause();
    }

    // Update is called once per frame
    void Update()
    {
        HandleInput();
    }

    public void SetPlayer(GameObject player)
    {
        this.player = player;
    }

    public void FreePlayer()
    {
        this.player = null;
    }

    public void HandleInput()
    {
        // Pause game
        if (Input.GetKeyDown(KeyCode.P))
        {
            GameManager.Instance.Pause();
        }
        if (GameManager.Instance.isPauseGame ||
            GameManager.Instance.isSoftPauseGame) 
            return;


        if (Input.GetKeyDown(KeyCode.T))
        {
            ingameManager.StartIteration();
        }
        //else if (Input.GetKeyDown(KeyCode.Y))
        //{
        //    recordManager.EndRecord();
        //    Debug.Log("End Record");
        //}
        if (Input.GetKeyDown(KeyCode.Y))
        {
            //iterator.SetActive(true);
            //recordManager.RunRecord(recordManager.Records[0], iterator);
        }


        // Player stuff
        if (player == null) return;
        if (Input.GetKeyDown(playerKeys.jump))
        {
            player.GetComponent<PlayerMovement>().HandleStartJump();
        }

        if (Input.GetKeyUp(playerKeys.jump))
        {
            player.GetComponent<PlayerMovement>().HandleEndJump();
        }

        bool moveLeft = Input.GetKey(playerKeys.moveLeft);
        bool moveRight = Input.GetKey(playerKeys.moveRight);


        if (moveLeft ^ moveRight)
        {
            // Kiểm tra phím di chuyển trái
            if (moveLeft)
            {
                _inputRouter.PressArrowLeft(player);
            }

            // Kiểm tra phím di chuyển phải
            if (moveRight)
            {
                _inputRouter.PressArrowRight(player);
            }
        }
        else
        {
           _inputRouter.PressLeftAndRight(player);
        }
        if (Input.GetKeyDown(playerKeys.attack))
        {
            _inputRouter.PressX(player);
        }

        if(Input.GetKeyDown(KeyCode.Escape))
        {
            _inputRouter.PressESC();
        }

        if(Input.GetKeyDown(playerKeys.moveDown))
        {
            _inputRouter.PressArowDown(player);
        }
        if (Input.GetKeyDown(KeyCode.UpArrow))
        {
            _inputRouter.PressArrowUp();
        }
    }

    public IEnumerator WaitToStartGame()
    {
        //Debug.Log("wait");
        GameManager.Instance.SoftPause();
        yield return new WaitForSeconds(0.75f);
        while (true)
        {
            if (Input.GetKeyDown(playerKeys.jump) ||
                Input.GetKeyDown(playerKeys.moveLeft) ||
                Input.GetKeyDown(playerKeys.moveRight) ||
                Input.GetKeyDown(playerKeys.attack))
            {
                ingameManager.StartIteration();
                GameManager.Instance.SoftContinue();
                yield break;
            }
            yield return null;
        }
    }
    public void WaitToReturnHub()
    {
        StartCoroutine(WaitToReturnHubCoroutine());
    }
    public IEnumerator WaitToReturnHubCoroutine()
    {
        while (true)
        {
            if(Input.GetKeyDown(KeyCode.X))
            {
                AudioManager.Instance.AudioSourceBGM.Stop();
                SceneManager.LoadSceneAsync("Main Menu");   
                yield break;
            }
            yield return null;
        }
        
    }
}
