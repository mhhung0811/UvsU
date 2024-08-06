using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RecordManager : MonoBehaviour
{
    [SerializeField] private RecordKeyConfig recordKeys;

    private Dictionary<KeyCode, int> keyPos;
    private Dictionary<KeyCode, bool> keyPressed;
    // public List<IAction> actions { get; private set; }
    //private float timer;
    //private bool isRecord;

    public List<List<IAction>> Records { get; private set; }
    private List<float> recordTimes;
    private List<IAction> actions;

    
    // Start is called before the first frame update
    void Start()
    {
        //timer = 0;
        //isRecord = false;

        Records = new List<List<IAction>>();
        actions = new List<IAction>();

        recordTimes = new List<float>();
            
        keyPos = new Dictionary<KeyCode, int>()
        {
            {recordKeys.moveLeft, 0 },
            {recordKeys.moveRight, 0 },
            {recordKeys.jump, 0 },
            {recordKeys.attack, 0 }
        };
        keyPressed = new Dictionary<KeyCode, bool>()
        {
            {recordKeys.moveLeft, false },
            {recordKeys.moveRight, false },
            {recordKeys.jump, false },
            {recordKeys.attack, false }
        };      
    }

    // Update is called once per frame
    void Update()
    {
        //if (!isRecord) return;

        //timer += Time.deltaTime;

        //if (Input.GetKeyDown(recordKeys.moveLeft))
        //{
        //    StartCoroutine(EndAction(recordOrder, recordKeys.moveLeft, timer));
        //}
        //if (Input.GetKeyDown(recordKeys.moveRight))
        //{
        //    StartCoroutine(EndAction(recordOrder, recordKeys.moveRight, timer));
        //}
        //if (Input.GetKeyDown(recordKeys.jump))
        //{
        //    StartCoroutine(EndAction(recordOrder, recordKeys.jump, timer));
        //}
        //if (Input.GetKeyDown(recordKeys.attack))
        //{
        //    StartCoroutine(EndAction(recordOrder, recordKeys.attack, timer));
        //}
    }

    public IEnumerator StartRecord(float time)
    {
        float timer = 0;

        Debug.Log("Start Record");
        recordTimes.Add(time);

        //Debug.Log(recordTimes.Count);
        while (timer < time)
        {
            //Debug.Log(timer);
            if (Input.GetKey(recordKeys.moveLeft) && !keyPressed[recordKeys.moveLeft])
            {
                keyPressed[recordKeys.moveLeft] = true;
                StartCoroutine(EndAction(recordKeys.moveLeft, timer, time));
            }
            if (Input.GetKey(recordKeys.moveRight) && !keyPressed[recordKeys.moveRight])
            {
                Debug.Log("Running");
                keyPressed[recordKeys.moveRight] = true;
                StartCoroutine(EndAction(recordKeys.moveRight, timer, time));
            }
            if (Input.GetKey(recordKeys.jump) && !keyPressed[recordKeys.jump])
            {
                keyPressed[recordKeys.jump] = true;
                StartCoroutine(EndAction(recordKeys.jump, timer, time));
            }
            if (Input.GetKey(recordKeys.attack) && !keyPressed[recordKeys.attack])
            {
                keyPressed[recordKeys.attack] = true;  
                StartCoroutine(EndAction(recordKeys.attack, timer, time));
            }
            timer += Time.deltaTime;
            yield return new WaitForEndOfFrame();
        }

        // error occur when hold key out of end time
        // hot fix, might need improve
        yield return new WaitForSeconds(0.5f);
        EndRecord();

        Debug.Log("End Record");
    }

    public void EndRecord()
    {
        Records.Add(new List<IAction>(actions));
        actions.Clear();
    }

    public void RunRecord(List<IAction> listAction, GameObject actor)
    {
        Debug.Log(listAction.Count);
        if (listAction.Count > 0)
        {
            StartCoroutine(Run(listAction, actor));
        }
    }

    IEnumerator StartAction(KeyCode keycode, float startTime, float endTime, Action<float> callback)
    {
        IAction action;
        float timer = 0f;
        Debug.Log("Recording");
        if (keycode == recordKeys.moveLeft)
        {
            action = new MoveLeftAction(startTime, 0f);
            keyPos[keycode] = actions.Count;
            actions.Add(action);
        }
        else if (keycode == recordKeys.moveRight)
        {
            action = new MoveRightAction(startTime, 0f);
            keyPos[keycode] = actions.Count;
            actions.Add(action);
        }
        else if (keycode == recordKeys.jump)
        {
            action = new JumpAction(startTime, 0f);
            keyPos[keycode] = actions.Count;
            actions.Add(action);
        }
        else if (keycode == recordKeys.attack)
        {
            action = new AttackAction(startTime, 0f);
            keyPos[keycode] = actions.Count;
            actions.Add(action);
        }
        
        while (Input.GetKey(keycode) && timer < endTime)
        {
            timer += Time.deltaTime;
            yield return new WaitForEndOfFrame();
        }
        //yield return new WaitUntil(() => Input.GetKeyUp(keycode));
        callback(timer);
    }

    IEnumerator EndAction(KeyCode keycode, float startTime, float endTime)
    {
        float timer = 0;
        yield return StartCoroutine(StartAction(keycode, startTime, endTime, result => { timer = result; }));
        if (keycode == recordKeys.moveLeft ||
            keycode == recordKeys.moveRight ||
            keycode == recordKeys.jump ||
            keycode == recordKeys.attack)
        {
            actions[keyPos[keycode]].actionTime = timer;
            keyPressed[keycode] = false;
        }
        Debug.Log("End action");
    }

    IEnumerator Run(List<IAction> actions, GameObject actor)
    {
        Debug.Log("Run");

        float timer = 0;
        int i = 0;

        while (i < actions.Count)
        {
            timer += Time.deltaTime;
            if (timer >= actions[i].startTime)
            {
                StartCoroutine(actions[i].Execute(actor));
                i++;
            }
            yield return new WaitForEndOfFrame();
        }

        Debug.Log("End Run");

        //foreach (var action in actions)
        //{
        //    yield return new WaitForSeconds(action.startTime);
        //    StartCoroutine(action.Execute());
        //}
    }
}
