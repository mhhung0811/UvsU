using System.Collections;
using System.Collections.Generic;
using System.Linq;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;
using static UnityEditor.Progress;

public class IngameManager : MonoBehaviour, IHub
{
    [SerializeField] private Grid _grid;

    private List<LevelConfig> list_level_configs = new List<LevelConfig>();
    private LevelConfig _current_level_config;
    private GameObject _current_map;

    private float _max_time;
    private float _timer;

    private int _max_iteration;
    private int _max_iterator;
    [SerializeField] private int _current_iteration;
    private int _current_iterator;

    [SerializeField] private LevelConfig _levelConfig;
    [SerializeField] private List<Peer> _peers;

    [SerializeField] private GameSceneUIManager _gameSceneUIManager;
    [SerializeField] private InputManager _inputManager;
    private RecordManager _recordManager;

    private List<GameObject> _iterators;
    private List<List<IAction>> _whiteRecords;
    private List<List<IAction>> _blackRecords;

    private List<Coroutine> _coroutines =  new List<Coroutine>();

    private Coroutine _timer_coroutine;

    
    void Start()
    {
        PrepareMap();
        LoadComponent();
        StartCoroutine(PrepareIteration());
        AudioManager.Instance.PlayBGM(0);
    }

    private void LoadComponent()
    {
        
        _current_iteration = 0;
        _current_iterator = -1;
        

        //Initiate list icon iteration
        _gameSceneUIManager.InitiateIconIter(_max_iteration);

        _recordManager = GetComponent<RecordManager>();
        _iterators = new List<GameObject>();
        _whiteRecords = new List<List<IAction>>();
        _blackRecords = new List<List<IAction>>();
        

        foreach (Peer item in _peers)
        {
            Register(item);
        }
    }

    IEnumerator CountDownTimer()
    {
        while (_timer > 0)
        {
            _timer -= Time.deltaTime;
            SetTimerUI();
            yield return new WaitForEndOfFrame();
        }
        if(_timer <= 0)
        {
            /*RestartIteration();*/
            StartCoroutine(IterationFailure());

        }
        yield return null;
    }

    private void SetTimerUI()
    {
        _gameSceneUIManager.SetTimerText(_timer);
        _gameSceneUIManager.SetTimeSlider(_timer, _max_time);
    }

    public IEnumerator PrepareIteration()
    {
        yield return new WaitForSeconds(0.25f);
        PrepareUI();
        //Reload component in map for new iteration
        _current_map.GetComponent<Map>()?.ReLoadMap();
        _current_iterator = (_current_iteration % 2 == 0) ? 0 : (_current_iteration + 1) / 2;

        // Clear old iterator
        foreach (GameObject iter in _iterators)
        {
            IterSpawner.Instance.Despawn(iter.transform);
        }
        _iterators.Clear();

        // Spawn new iterator
        for (int i = 0; i <= (_current_iteration + 1) / 2; i++)
        {
            GameObject obj = null;
            Transform spawn_pos = _current_map.GetComponent<Map>().list_spawn_point[i];
            Map test = _current_map.GetComponent<Map>();
            if(test == null)
            {
                Debug.Log("test is null");
            }
            if (spawn_pos == null)
            {
                Debug.Log("Spawn pos null");
            }
            else
            {
                //Debug.Log(spawn_pos.position);
            }
            //Debug.Log(i);
            if (i == 0)
            {
                obj = IterSpawner.Instance.Spawn("IterWhite", spawn_pos.position, Vector3.zero, 1);
                if (obj != null)
                {
                    if (_iterators.Count > 0)
                    {
                        _iterators[0] = obj;
                    }
                    else
                    {
                        _iterators.Add(obj);
                    }
                }
            }
            else
            {
                //Debug.Log("Spawn black");
                //Debug.Log(_iterators.Count);
                obj = IterSpawner.Instance.Spawn("IterBlack", spawn_pos.position, Vector3.zero, -1);
                if (obj != null)
                {
                    obj.GetComponent<PlayerModel>().LoadComponent();
                    _iterators.Add(obj);
                }
                else Debug.Log("Is null");

            }
            obj.transform.Find("Canvas").gameObject.SetActive(false);
        }
        _iterators[_current_iterator].transform.Find("Canvas").gameObject.SetActive(true);
        // Set player
        _inputManager.SetPlayer(GetCurrentPlayer());

        if (_current_iteration >= _max_iteration)
        {
        }
    }
    private void PrepareUI()
    {
        //Enable move to start
        _gameSceneUIManager.EnableUpperText();

        //Set iter text UI
        _gameSceneUIManager.SetIterText(_current_iteration + 1, _max_iteration);
        _gameSceneUIManager.SetIconIter(_current_iteration + 1);

        // Set timer
        _max_time = _current_level_config.iterTimes[_current_iteration];
        _timer = _max_time;
        SetTimerUI();
    }
    private void PrepareMap()
    {
        list_level_configs = GameManager.Instance.GetAllLevelConfigs();
        if(list_level_configs.Count == 0)
        {
            Debug.Log("Do not have any config for level");
            return;
        }
        foreach (LevelConfig level in list_level_configs)
        {
            if(level.level_id == GameManager.Instance.Current_level)
            {
                _current_level_config = level;
                _max_iteration = level.iterTimes.Count;
                _max_iterator = (level.iterTimes.Count) + 1 / 2;
                _current_map = Instantiate(level.map);
                _current_map.SetActive(true);
                _current_map.transform.parent = _grid.transform;
                var spikes = _current_map.GetComponentInChildren<Spikes>();
                if (spikes != null)
                {
                    spikes.ingameManager = this;
                    Debug.Log("Spikes == this");
                }
                _peers.Add(_current_map.GetComponentInChildren<Peer>());

                Debug.Log("Map Generated");
                break;
            }
        }
        
        
    }
    public void BackToPreviousIteration()
    {
        StopAnyLogicCoroutine();
        StartCoroutine(DelRecordIteraction());
        
        _current_iteration--;
        //_current_iterator = (_current_iteration % 2 == 0) ? 0 : (_current_iteration + 1) / 2;    
        StartCoroutine(PrepareIteration());
        StartCoroutine(_inputManager.WaitToStartGame());
    }
    private void StopAnyLogicCoroutine()
    {
        // Still bugging, can't fix & won't fix
        // Clear coroutines
        foreach (Coroutine record in _coroutines)
        {
            if (record != null)
                StopCoroutine(record);
        }
        _coroutines.Clear();

        // End timer
        StopCoroutine(_timer_coroutine);
        //End record 
        _recordManager.isStopRecording = true;
        // Disable keyboard
        _inputManager.FreePlayer();
        GameManager.Instance.SoftPause();

    }
    public void StartIteration()
    {
        GameManager.Instance.SoftContinue();

        //Debug.Log(_whiteRecords.Count);
        //Debug.Log(_blackRecords.Count);
        Debug.Log("Iteration: " +  _current_iteration);

        // Check records
        // White iteration
        if (_current_iteration % 2 == 0)
        {
            Debug.Log("Bug White" + _current_iteration);

            if (_whiteRecords.Count < _current_iteration / 2) Debug.Log("Bug at Start Iteration");
            while (_whiteRecords.Count > _current_iteration / 2)
            {
                _whiteRecords.RemoveAt(_whiteRecords.Count - 1);
            }
            //if (_whiteRecords.Count == _current_iteration / 2)
            //{
            //    Debug.Log("True white even");
            //}
            //else Debug.Log(_whiteRecords.Count);

            if (_blackRecords.Count < _current_iteration / 2) Debug.Log("Bug at Start Iteration");
            while (_blackRecords.Count > _current_iteration / 2)
            {
                _blackRecords.RemoveAt(_blackRecords.Count - 1);
            }
            //if (_blackRecords.Count == _current_iteration / 2)
            //{
            //    Debug.Log("True black even");
            //}
            //else Debug.Log(_blackRecords.Count);
        }
        // Black iteration
        else
        {
            Debug.Log("Bug Black" + _current_iteration);

            if (_whiteRecords.Count < _current_iteration / 2 + 1) Debug.Log("Bug at Start Iteration");
            while (_whiteRecords.Count > _current_iteration / 2 + 1)
            {
                _whiteRecords.RemoveAt(_whiteRecords.Count - 1);
            }
            //if (_whiteRecords.Count == _current_iteration / 2 + 1)
            //{
            //    Debug.Log("True white odd");
            //}
            //else Debug.Log(_whiteRecords.Count);

            if (_blackRecords.Count < _current_iteration / 2) Debug.Log("Bug at Start Iteration");
            while (_blackRecords.Count > _current_iteration / 2)
            {
                _blackRecords.RemoveAt(_blackRecords.Count - 1);
            }
            if (_blackRecords.Count == _current_iteration / 2)
            {
                Debug.Log("True black odd" + _blackRecords.Count);
            }
            else Debug.Log(_blackRecords.Count);
        }

        // Even is white
        if (_current_iteration % 2 == 0)
        {
            // Start recording
            StartCoroutine(_recordManager.StartRecord(_max_time, _whiteRecords));

            // Run record if has
            for (int i = 0; i < _blackRecords.Count; i++)
            {
                if (_iterators.Count > i)
                {
                    List<Coroutine> records = _recordManager.RunRecord(_blackRecords[i], _iterators[i + 1]);
                    //_black_records.Add(record);
                    //_coroutines.Add(record);
                    _coroutines = _coroutines.Concat(records).ToList();
                }
                else Debug.Log("Bug at StarIteration");
                
            }
        }
        // Odd is black
        else
        {
            // Start recording
            StartCoroutine(_recordManager.StartRecord(_max_time, _blackRecords));

            // Run record if has
            //_white_record =  _recordManager.RunRecord(_whiteRecords[0], _iterators[0]);
            List<Coroutine> records = _recordManager.RunRecord(_whiteRecords[^1], _iterators[0]);
            Debug.Log("record count return : " +  records.Count);
            _coroutines = _coroutines.Concat(records).ToList();
            Debug.Log("List coroutine size before: " + _coroutines.Count);
            for (int i = 0; i < _blackRecords.Count; i++)
            {
                List<Coroutine> records1 = _recordManager.RunRecord(_blackRecords[i], _iterators[i + 1]);
                //_black_records.Add(record);
                _coroutines = _coroutines.Concat(records1).ToList();
            }
        }
        Debug.Log("List coroutine size after: "+ _coroutines.Count);
        _gameSceneUIManager.DisableUpperText();
        _timer_coroutine = StartCoroutine(CountDownTimer());
    }

    public void EndIteration()
    {
        foreach (var iterator in _iterators)
        {
            iterator.SetActive(false);
        }
        BulletSpawner.Instance.DespawnAll();

        StopAnyLogicCoroutine();
        Debug.Log("list coroutine size in end : " + _coroutines.Count);
        _current_iteration++;
        if (_current_iteration == _max_iteration)
        {
            _gameSceneUIManager.LevelCompleted();
            _inputManager.WaitToReturnHub();
        }
        else
        {
            StartCoroutine(PrepareIteration());
            StartCoroutine(_inputManager.WaitToStartGame());
        }
    }

    public GameObject GetCurrentPlayer()
    {
        if (_iterators.Count >= _current_iterator)
        {
            //Debug.Log(_iterators.Count);
            //Debug.Log(_iterators[_current_iterator]);
            //Debug.Log("Current iterator : " + _current_iterator);
            return _iterators[_current_iterator];

        }
        else
        {
            Debug.Log("player null at getCurrentPlayer");
            return null;
        }
    }

    public void SendMessage(string message, Peer sender, GameObject obj)
    {
        // Resolve message here
        if (message == IngameMessage.Complete.ToString())
        {
            if(obj == _iterators[0])
            {
                //IterSpawner.Instance.Despawn(obj.transform);
                //_iterators.Remove(obj);
                if (_current_iteration % 2 == 0)
                {
                    EndIteration();
                }
                //Iter white in odd iteration reachs gate
                else
                {
                    RestartIteration();
                }
            }
        }
    }

    public void Register(Peer peer)
    {
        //_components.Add(component);
        peer.SetMediator(this);
    }
    IEnumerator IterationFailure()
    {
        foreach (var iterator in _iterators)
        {
            iterator.SetActive(false);
        }
        BulletSpawner.Instance.DespawnAll();

        StopAnyLogicCoroutine();
        _gameSceneUIManager.EnableFailedText();
        yield return new WaitForSeconds(1f);
        _gameSceneUIManager.DisableFailedText();
        StartCoroutine(PrepareIteration());
        StartCoroutine(_inputManager.WaitToStartGame());
        yield break;
    }
    public void RestartIteration()
    {
        StartCoroutine(IterationFailure());
    }
    public void RedoIteration()
    {
        StopAnyLogicCoroutine();
        StartCoroutine(PrepareIteration());
        StartCoroutine(_inputManager.WaitToStartGame());
    }
    public void CheckBulletSpikeTrigger(GameObject obj)
    {
        if(obj == _iterators[_current_iterator])
        {
            RestartIteration();
        }
        else if((obj != _iterators[_current_iterator]))
        {
            // EndIteration();
            //_whiteRecords.Clear();

            //Debug.Log("End iteration by shooting");
            if ((obj == _iterators[0]))
            {
                EndIteration();
                // _whiteRecords.Clear();
                Debug.Log("End iteration by shooting");
            }
            else
            {
                //IterSpawner.Instance.Despawn(obj.transform);
                //_iterators.Remove(obj);
            }
        }
    }
    public void RestartLevel()
    {
        StopAnyLogicCoroutine();
        _current_iteration = 0;
        StartCoroutine(PrepareIteration());
        StartCoroutine(_inputManager.WaitToStartGame());

    }

    // Dirty work here, pls don't interupt
    private IEnumerator DelRecordIteraction()
    {
        yield return new WaitForSeconds(0.7f);
        if (_whiteRecords.Count > 0)
        {
            _whiteRecords.RemoveAt(_whiteRecords.Count - 1);

        }
        if (_blackRecords.Count > 0)
        {
            _blackRecords.RemoveAt(_blackRecords.Count - 1);
        }
    }
    
}
