using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerAttack : MonoBehaviour
{
    [SerializeField] private Transform _spawn_pos;
    [SerializeField] private PlayerModel _model;
    [SerializeField, Range(0.1f, 1f)] private float _cool_down;
    [SerializeField] private float _cool_down_count;
    [SerializeField] private PlayerAnimation _animation;
    private PlayerHP _playerHP;

    private void Awake()
    {
        this._model = GetComponent<PlayerModel>();
        this._animation = GetComponent<PlayerAnimation>();
        this._playerHP = GetComponent<PlayerHP>();
    }
    // Start is called before the first frame update
    void Start()
    {
        this.LoadComponent();
    }

    // Update is called once per frame
    void Update()
    {
        ResetSkill();
    }
    private void LoadComponent()
    {
        this._cool_down = 0.3f;
        this._cool_down_count = 0f;
    }
    public void HandleAttack()
    {
        if (_playerHP._type == 0) return;
        if((_cool_down_count <= 0))
        {
            _animation.SetTriggerAttack();
            Attack();
            this._cool_down_count = _cool_down;
        }
    }
    private void Attack()
    {
        AudioManager.Instance.PlayFX(4);
        BulletSpawner.Instance.Spawn("Bullet", _spawn_pos.position, new Vector3(0, 0, 0), _model.GetDirection());
    }
    private void ResetSkill()
    {
        if(_cool_down_count > 0)
        {
            _cool_down_count -= Time.deltaTime;
        }
    }
}
