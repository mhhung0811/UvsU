using System.Collections;
using System.Collections.Generic;
using System.Net.NetworkInformation;
using Unity.VisualScripting;
using UnityEngine;

public class FSMAttackAction :  IFSMAction, IObserver
{
    [SerializeField] private GameObject _laser;
    [SerializeField] private float _delay;
    [SerializeField] private InputManager _inputManager;
    [SerializeField] private EnemyAnimation _enemyAnimation;
    [SerializeField] private LineRenderer _laser_render;
    [SerializeField] private LineRenderer _warning_line;
    [SerializeField] private Transform _fire_point;
    [SerializeField] private InGameEventCenter _gameEventCenter;
    private bool _skill_activating;
    private Vector2 hit_point;
    private Coroutine _coroutine;
    public float Delay
    { 
        get { return _delay; } 
        set {  _delay = value; } 
    }
    [SerializeField,Range(0.5f,3f)] private float _cool_down;

    // Start is called before the first frame update
    void Start()
    {
        _gameEventCenter.Attach(this);
        LoadComponent();
    }

    // Update is called once per frame
    void Update()
    {
        CountDownDelay();
    }
    public override void Action()
    {
        if(_skill_activating)
        {
            return;
        }
        if (_inputManager.Player == null)
        {
            return;
        }
        Vector2 playerPos = GetPlayerPosition();
        _coroutine =  StartCoroutine(ExecuteSkill(playerPos));
    }
    private void CountDownDelay()
    {
        _delay -= Time.deltaTime;
        _delay = Mathf.Max(0, _delay);
    }
    private void LoadComponent()
    {
        _cool_down = 2f;
        _delay = 0f;
        _skill_activating = false;
    }
    IEnumerator ExecuteSkill(Vector2 playerPos)
    {
        _skill_activating = true;
        Vector2 firePointPos = _fire_point.position;
        Vector2 direction = (playerPos - firePointPos).normalized;

        _warning_line.SetPosition(0, firePointPos);
        _laser_render.SetPosition(0, firePointPos);

        RaycastHit2D hit = Physics2D.Raycast(firePointPos, direction, Vector2.Distance(playerPos, firePointPos));
        if (hit)
        {
            _warning_line.SetPosition(0, hit_point);
            _laser_render.SetPosition(1, hit.point);

            hit_point = hit.point;
        }
        
        // Set particle system for laser
        _laser.GetComponent<Laser>().ActivateLaser(_fire_point.position, hit.point);
        _warning_line.enabled = true;
        
        yield return new WaitForSeconds(0.75f);


        _warning_line.enabled = false;
        _enemyAnimation.SetTriggerAttack();
        _laser_render.enabled = true;

        yield return new WaitForSeconds(1f);

        _delay = _cool_down;
        _skill_activating = false;
        _laser_render.enabled = false;
        _laser.GetComponent<Laser>().ResetSystems();
        yield return null;
    }
    private Vector2 GetPlayerPosition()
    {
        return _inputManager.Player.transform.position;
    }

    public void UpdateState()
    {
        if(_coroutine == null)
        {
            return;
        }
        StopCoroutine(_coroutine);
        _laser.GetComponent<Laser>().ResetSystems();
    }
}
