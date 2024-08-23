using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    [SerializeField] private PlayerManager _player_manager;
    [SerializeField] private CapsuleCollider2D _capsule_collider_2d;
    [SerializeField] private PlayerAnimation _animations;
    [SerializeField] private PlayerModel _model;
    [SerializeField] private Rigidbody2D _rigidBody;
    [SerializeField, Range(1f, 10f)] private float _runVelocity = 7f;
    [SerializeField, Range(1f, 100f)] private float _jumpForce = 50f;
    [SerializeField] private Transform _groundCheck;
    [SerializeField] private LayerMask _groundLayer;
    [SerializeField] private Transform _wallCheck;

    private float _moveDirection;
    [SerializeField] private bool _isGrounded;

    [SerializeField] private float _timer;
    [SerializeField, Range(50f, 200f)] private float _multi;
    [SerializeField] private bool _is_jumping;

    // jump stuff
    private float jumpMaxTime = 0.15f;
    private float jumpMinTime = 0.03f;
    private float jumpTimer = 0f;

    private void Awake()
    {
        _rigidBody = GetComponent<Rigidbody2D>();
        _model = GetComponent<PlayerModel>();
        _animations = GetComponent<PlayerAnimation>();
    }

    void Start()
    {
        Initialize();
    }

    void Update()
    {
        _isGrounded = GroundCheck();
        SetYVelocity();

        if (_is_jumping)
        {
            jumpTimer += Time.deltaTime;
        }

        Jumpping();
    }

    private void Initialize()
    {
        _runVelocity = 8f;
        _jumpForce = 20f;
        _moveDirection = 0;
        _timer = 0;
        _multi = 115f;
        _is_jumping = false;
    }

    public void HandleMovement(float value)
    {
        float moveDirectionTemp = value;

        _moveDirection = moveDirectionTemp;
        _model.SetDirection(_moveDirection);


        Move(moveDirectionTemp);
    }
    
    private void Move(float direction)
    {
        if ((_rigidBody.velocity.x * direction) < 0 || WallCheck())
        {
            _rigidBody.velocity = new Vector2(0, _rigidBody.velocity.y);
            _animations.SetBoolRunning(false);
        }
        else
        {
            _rigidBody.velocity = new Vector2(_runVelocity * direction, _rigidBody.velocity.y);
            _animations.SetBoolRunning(true);
        }
    }

    public void HandleMoveLeftAndRight()
    {
        _animations.SetBoolRunning(false);
    }

    public void HandleStartJump()
    {
        // Initial jump
        if (GroundCheck() && !_is_jumping)
        {
            //Debug.Log(Mathf.Min(Mathf.Max(_timer, 0.19f), 0.3f));
            jumpTimer = 0;
            _is_jumping = true;
            //_rigidBody.velocity = new Vector2(_rigidBody.velocity.x, Mathf.Min(Mathf.Max(_timer, 0.19f), 0.3f) * _multi);
            _rigidBody.velocity = new Vector2(_rigidBody.velocity.x, _jumpForce);

            //Debug.Log(transform.position.y);
            //Debug.Log(jumpFixedY + jumpMaxY);
        }
        //_timer += Time.deltaTime;
        //if (_timer >= 0.3f)
        //{
        //    Jumpping();
        //}
    }
    public void HandleEndJump()
    {
        //Jumpping();
        //_timer = 0;
        _is_jumping = false;
    }
    public void Jumpping()
    {
        // Jump float
        if (!GroundCheck() && _is_jumping && jumpTimer < jumpMinTime)
        {
            _rigidBody.velocity = new Vector2(_rigidBody.velocity.x, _jumpForce);
        }
        if (!GroundCheck() && _is_jumping)
        {
            if (jumpTimer < jumpMaxTime)
                _rigidBody.velocity = new Vector2(_rigidBody.velocity.x, _jumpForce);
            else
                _is_jumping = false;
        }

        // Jump drop
        //if (GroundCheck() && _is_jumping)
        //{
        //    _is_jumping = false;
        //}
        //if (transform.position.y > jumpFixedY + jumpMaxY)
        //{
        //    _is_jumping = false;
        //}
    }

    public bool GroundCheck()
    {
        bool check = Physics2D.OverlapCircle(_groundCheck.position, 0.01f, _groundLayer);
        _animations.SetBoolGround(check);
        return check;
    }
    private void SetYVelocity()
    {
        _animations.SetVelocityY(_rigidBody.velocity.y);
    }

    public bool WallCheck()
    {
        return Physics2D.OverlapCircle(_wallCheck.position, 0.1f, _groundLayer);
    }

    public void FallDown()
    {
        GameObject obj = _model._current_one_way_platfrom;
        if (obj != null)
        {
            StartCoroutine(DisableCollision(obj));
        }
    }
    IEnumerator DisableCollision(GameObject obj)
    {
        BoxCollider2D boxCollider = obj.GetComponent<BoxCollider2D>();
        Physics2D.IgnoreCollision(_capsule_collider_2d, boxCollider);

        yield return new WaitForSeconds(0.25f);

        Physics2D.IgnoreCollision(_capsule_collider_2d, boxCollider, false);

        yield return null;
    }
}
