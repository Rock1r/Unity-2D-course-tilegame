using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerMovement : MonoBehaviour
{
    [SerializeField] private float _playerRunSpeed = 5f;
    [SerializeField] private float _playerWalkSpeed = 2f;
    [SerializeField] private float _jumpForce = 1f;
    [SerializeField] private float _climbSpeed = 1f;

    private Vector2 _moveInput;
    private Rigidbody2D _rb;
    private Animator _animator;
    private bool _isRunning = false;
    private CapsuleCollider2D _collider;
    private float _gravityScale = 1f;

    private void Start()
    {
        _rb = GetComponent<Rigidbody2D>();
        _animator = GetComponent<Animator>();
        _collider = GetComponent<CapsuleCollider2D>();
        _gravityScale = _rb.gravityScale;
    }

    private void Update()
    {
        Move();
        UpdateAnimation();
        Climb();
    }

    private void OnMove(InputValue value)
    {
        _moveInput = value.Get<Vector2>();
    }

    private void Move()
    {
        _isRunning = false;
        float _speed = _playerWalkSpeed;
        if (Keyboard.current.leftShiftKey.isPressed && _moveInput.magnitude > 0)
        {
            _isRunning = true;
            _speed = _playerRunSpeed;
        }
            Vector2 _playerVelocity = new Vector2(_moveInput.x * _speed, _rb.velocity.y);
            _rb.velocity = _playerVelocity;
            FlipSprite();
    }

    private void OnJump(InputValue value)
    {
        if (_collider.IsTouchingLayers(LayerMask.GetMask("Ground")) && value.isPressed)
        {
            _animator.SetTrigger("IsJumping");
            _rb.velocity += new Vector2(0f, _jumpForce);
        }
    }

    private void Climb()
    {
        if (_collider.IsTouchingLayers(LayerMask.GetMask("Liana")))
        {
            _rb.gravityScale = 0f;
            Vector2 climbVelocity = new Vector2(_rb.velocity.x , _moveInput.y * _climbSpeed);
            _rb.velocity = climbVelocity;
        }
        else
        {
            _rb.gravityScale = _gravityScale;
        }
    }

    private void UpdateAnimation()
    {
        bool isWalking = Mathf.Abs(_rb.velocity.x) >= Mathf.Epsilon && !_isRunning;
        _animator.SetBool("IsRunning", _isRunning);
        _animator.SetBool("IsWalking", isWalking);
        _animator.SetBool("WasMoving", _isRunning || isWalking);
    }

    private void FlipSprite()
    {
        bool _playerHasHorizontalSpeed = Mathf.Abs(_rb.velocity.x) > Mathf.Epsilon;
        if (_playerHasHorizontalSpeed)
        {
            transform.localScale = new Vector2(Mathf.Sign(_rb.velocity.x), 1f);
        }
    }

    public void ResetJumpTrigger()
    {
        _animator.ResetTrigger("IsJumping");
    }

}
