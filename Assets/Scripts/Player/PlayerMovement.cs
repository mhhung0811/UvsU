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

    private float _moveDirection;
    private bool _isGrounded;

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
        HandleMovement();
        HandleJump();
        SetYVelocity();
    }

    private void Initialize()
    {
        _runVelocity = 8f;
        _jumpForce = 30f;
    }

    private void HandleMovement()
    {
        float moveDirectionTemp = Input.GetAxisRaw("Horizontal");

        if (moveDirectionTemp == 0f && (_moveDirection != 0f))
        {
            _animations.SetBoolRunning(false);
        }
        else if (moveDirectionTemp != 0f)
        {
            _moveDirection = moveDirectionTemp;
            _model.SetDirection(_moveDirection);
            _animations.SetBoolRunning(true);
        }

        Move(moveDirectionTemp);
    }

    private void Move(float direction)
    {
        _rigidBody.velocity = new Vector2(_runVelocity * direction, _rigidBody.velocity.y);
    }

    private void HandleJump()
    {
        _isGrounded = GroundCheck();
        if( _isGrounded )
        {
            _animations.SetBoolGround(true);
        }
        if (Input.GetKeyDown(KeyCode.Space) && _isGrounded)
        {
            _animations.SetBoolGround(false);
            Jump();
        }
        
    }

    private void Jump()
    {
        _rigidBody.velocity = new Vector2(_rigidBody.velocity.x, _jumpForce);
    }

    private bool GroundCheck()
    {
        return Physics2D.OverlapCircle(_groundCheck.position, 0.1f, _groundLayer);
    }
    private void SetYVelocity()
    {
        _animations.SetVelocityY(_rigidBody.velocity.y);
    }
}
