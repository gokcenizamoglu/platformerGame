using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    [SerializeField] float _speed = 1;
    [SerializeField] float _jumpVelocity = 200;
    [SerializeField] int _maxJumps = 2;
    [SerializeField] Transform _feet;
    [SerializeField] float _downPull = 5;

    Vector2 _startPosition;
    int _jumpRemaining;
    float _fallTimer;

    void Start()
    {
        _startPosition = transform.position;
        _jumpRemaining = _maxJumps;
    }

    internal void ResetToStart()
    {
        transform.position = _startPosition;
    }

    void Update()
    {
        var hit = Physics2D.OverlapCircle(_feet.position, 0.1f, LayerMask.GetMask("Default"));
        bool isGrounded = hit != null;

        var horizontal = Input.GetAxis("Horizontal") * _speed;
        var rigidbody2D = GetComponent<Rigidbody2D>();

        if(Mathf.Abs(horizontal) >= 1)
        {
            rigidbody2D.velocity = new Vector2(horizontal, rigidbody2D.velocity.y);
            Debug.Log($"Velocity = {rigidbody2D.velocity}");
        }
  
        var animator = GetComponent<Animator>();
        bool walking = horizontal != 0;
        animator.SetBool("Walk", walking);

        if (horizontal != 0 )
        {
            var spriteRenderer = GetComponent<SpriteRenderer>();
            spriteRenderer.flipX = horizontal < 0;
        }

        if (Input.GetKeyDown("space") && _jumpRemaining > 0)
        {
            rigidbody2D.velocity = new Vector2(rigidbody2D.velocity.x, _jumpVelocity);
            _jumpRemaining--;
        }

        if (!isGrounded)
        {
            _fallTimer = 0;
            _jumpRemaining = _maxJumps;

        }
        else
        {
            _fallTimer += Time.deltaTime;
            rigidbody2D.velocity = new Vector2(rigidbody2D.velocity.x, -_downPull * _fallTimer * _fallTimer);

        }
    }

    void OnCollisionEnter2D(Collision2D collision)
    {
        var hit = Physics2D.OverlapCircle(_feet.position, 0.1f, LayerMask.GetMask("Default"));
        if ( hit != null)
        {
            _jumpRemaining = _maxJumps;
        }
        
    }
}
