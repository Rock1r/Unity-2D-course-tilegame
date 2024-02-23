using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerMovement : MonoBehaviour
{
    [SerializeField] private float _playerRunSpeed = 5f;
    [SerializeField] private float _playerWalkSpeed = 2f;

    private Vector2 _moveInput;
    private Rigidbody2D _rb;
    private Animator _animator;
    private bool _isRunning = false;

    private void Start()
    {
        _rb = GetComponent<Rigidbody2D>();
        _animator = GetComponent<Animator>();
    }

    private void Update()
    {
        Move();
        UpdateAnimation();
    }

    private void OnMove(InputValue value)
    {
        _moveInput = value.Get<Vector2>();
        Debug.Log(_moveInput);
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

    

    private void FlipSprite()
    {
        bool _playerHasHorizontalSpeed = Mathf.Abs(_rb.velocity.x) > Mathf.Epsilon;
        if (_playerHasHorizontalSpeed)
        {
            transform.localScale = new Vector2(Mathf.Sign(_rb.velocity.x), 1f);
        }
    }

    private void UpdateAnimation()
    {
        bool isWalking = Mathf.Abs(_rb.velocity.x) >= Mathf.Epsilon && !_isRunning;
        _animator.SetBool("IsRunning", _isRunning);
        _animator.SetBool("IsWalking", isWalking);
    }

}
