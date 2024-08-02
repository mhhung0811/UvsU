using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RecordManager : MonoBehaviour
{
    [SerializeField] private RecordKeyConfig recordKeys;
    [SerializeField] private GameObject player;

    private Dictionary<KeyCode, int> keyPos;
    // public List<IAction> actions { get; private set; }
    //private float timer;
    //private bool isRecord;

    public List<List<IAction>> Records { get; private set; }
    private int recordOrder;
    private List<float> recordTime;

    
    // Start is called before the first frame update
    void Start()
    {
        //timer = 0;
        //isRecord = false;

        // actions = new List<IAction>();
        recordOrder = 0;
        Records = new List<List<IAction>>();

        recordTime = new List<float>();

        keyPos = new Dictionary<KeyCode, int>()
        {
            {recordKeys.moveLeft, 0 },
            {recordKeys.moveRight, 0 },
            {recordKeys.jump, 0 },
            {recordKeys.attack, 0 }
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
        recordTime.Add(time);
        //isRecord = true;

        while (timer < time)
        {
            timer += Time.deltaTime;
            if (Input.GetKeyDown(recordKeys.moveLeft))
            {
                StartCoroutine(EndAction(recordOrder, recordKeys.moveLeft, timer, time));
            }
            if (Input.GetKeyDown(recordKeys.moveRight))
            {
                StartCoroutine(EndAction(recordOrder, recordKeys.moveRight, timer, time));
            }
            if (Input.GetKeyDown(recordKeys.jump))
            {
                StartCoroutine(EndAction(recordOrder, recordKeys.jump, timer, time));
            }
            if (Input.GetKeyDown(recordKeys.attack))
            {
                StartCoroutine(EndAction(recordOrder, recordKeys.attack, timer, time));
            }
            yield return new WaitForEndOfFrame();
        }
        //EndRecord();
    }

    public void EndRecord()
    {
        //isRecord = false;
    }

    public void RunRecord(List<IAction> actions, GameObject actor)
    {
        if (actions.Count > 0)
        {
            StartCoroutine(Run(actions, actor));
        }
    }

    IEnumerator StartAction(int recordOrder, KeyCode keycode, float startTime, float endTime, Action<float> callback)
    {
        IAction action;
        float timer = 0f;
        Debug.Log("Recording");
        if (keycode == recordKeys.moveLeft)
        {
            action = new MoveLeftAction(startTime, 0f);
            keyPos[keycode] = Records[recordOrder].Count;
            Records[recordOrder].Add(action);
        }
        else if (keycode == recordKeys.moveRight)
        {
            action = new MoveRightAction(startTime, 0f);
            keyPos[keycode] = Records[recordOrder].Count;
            Records[recordOrder].Add(action);
        }
        else if (keycode == recordKeys.jump)
        {
            action = new JumpAction(startTime, 0f);
            keyPos[keycode] = Records[recordOrder].Count;
            Records[recordOrder].Add(action);
        }
        else if (keycode == recordKeys.attack)
        {
            action = new AttackAction(startTime, 0f);
            keyPos[keycode] = Records[recordOrder].Count;
            Records[recordOrder].Add(action);
        }
        
        while (Input.GetKey(keycode) && timer < endTime)
        {
            timer += Time.deltaTime;
            yield return new WaitForEndOfFrame();
        }
        //yield return new WaitUntil(() => Input.GetKeyUp(keycode));
        callback(timer);
    }

    IEnumerator EndAction(int recordOrder, KeyCode keycode, float startTime, float endTime)
    {
        float timer = 0;
        yield return StartCoroutine(StartAction(recordOrder, keycode, startTime, endTime, result => { timer = result; }));
        if (keycode == recordKeys.moveLeft ||
            keycode == recordKeys.moveRight ||
            keycode == recordKeys.jump ||
            keycode == recordKeys.attack)
        {

            Records[recordOrder][keyPos[keycode]].actionTime = timer;
        }
        Debug.Log("End record");
    }

    IEnumerator Run(List<IAction> actions, GameObject actor)
    {
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

        //foreach (var action in actions)
        //{
        //    yield return new WaitForSeconds(action.startTime);
        //    StartCoroutine(action.Execute());
        //}
    }
}
