using System.Collections;
using System.Collections.Generic;
using System.Data;
using UnityEngine;
using UnityEngine.SceneManagement;
using static System.Runtime.CompilerServices.RuntimeHelpers;

public class InputManager : MonoBehaviour
{
    //[SerializeField] private RecordManager recordManager;
    [SerializeField] private IngameManager ingameManager;
    private GameObject player;
    [SerializeField] private RecordKeyConfig playerKeys;
    
    private GameObject actor;

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
                player.GetComponent<PlayerMovement>().HandleMovement(-1);
                //Debug.Log("Left");
            }

            // Kiểm tra phím di chuyển phải
            if (moveRight)
            {
                player.GetComponent<PlayerMovement>().HandleMovement(1);
                //Debug.Log("Right");
            }
        }
        if (Input.GetKeyDown(playerKeys.attack))
        {
            player.GetComponent<PlayerAttack>().HandleAttack();
        }

        

    }

    public IEnumerator WaitToStartGame()
    {
        Debug.Log("wait");
        //yield return new WaitForSeconds(0.5f);
        while (true)
        {
            if (Input.GetKeyDown(playerKeys.jump) ||
                Input.GetKeyDown(playerKeys.moveLeft) ||
                Input.GetKeyDown(playerKeys.moveRight) ||
                Input.GetKeyDown(playerKeys.attack))
            {
                ingameManager.StartIteration();
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
        Debug.Log("Press X to return Hub");
        while (true)
        {
            if(Input.GetKeyDown(KeyCode.X))
            {
                Debug.Log("X pressed");
                AudioManager.Instance.AudioSourceBGM.Stop();
                SceneManager.LoadSceneAsync("Main Menu");   
                yield break;
            }
            yield return null;
        }
        
    }
}
