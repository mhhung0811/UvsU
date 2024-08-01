using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
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
        _isGrounded =  GroundCheck();
        SetYVelocity();
    }

    private void Initialize()
    {
        _runVelocity = 8f;
        _jumpForce = 30f;
        _moveDirection = 0;
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

    public void HandleJump()
    {
        
        if( _isGrounded )
        {
            _animations.SetBoolGround(true);
            Jump();
        }
        else
        {
            _animations.SetBoolGround(false);
        }
        
    }

    private void Jump()
    {
        _rigidBody.velocity = new Vector2(_rigidBody.velocity.x, _jumpForce);
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
}
