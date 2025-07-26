using System;
using System.Collections;
using System.Collections.Generic;
using System.Numerics;
using UnityEngine;
using Vector2 = UnityEngine.Vector2;

public class Player : MonoBehaviour
{
    [Header("Input")]
    public KeyCode jumpKey =  KeyCode.Space;
    public KeyCode attackKey = KeyCode.Mouse0;
    private const string XAxis = "Horizontal";
    
    [Header("Movement")]
    public float speed = 5f;
    public float jumpForce = 6f;
    public float groundedLeeway = 0.1f;
    
    private BoxCollider2D _boxCollider;
    private Rigidbody2D _rb;
    private float _mvmtX = 0;
    private bool _attemptJump = false;
    private int _noOfJumps = 0;
    private bool _attemptAttack = false;
    
    
    // Start is called before the first frame update
    void Start()
    {
        if (GetComponent<Rigidbody2D>()) _rb = GetComponent<Rigidbody2D>();
        _boxCollider = GetComponent<BoxCollider2D>();
    }

    // Update is called once per frame
    void Update()
    {
        GetInput();
        HandleRun();
        HandleJump();
    }

    private void HandleJump()
    {
        
        if (_attemptJump && IsGrounded() && _noOfJumps == 0)
        {
            _rb.velocity = new Vector2(_rb.velocity.x, jumpForce);
            _noOfJumps++;
        } 
        
        else if (_attemptJump && _noOfJumps is >= 1 and < 2)
        {
            _rb.velocity = new Vector2(_rb.velocity.x, jumpForce);
            _noOfJumps++;
        }
        
        else if (_noOfJumps >= 2)
        {
            _noOfJumps = 0;
        }
        
    }

    private void HandleRun()
    {
        _rb.velocity = new Vector2(_mvmtX * speed, _rb.velocity.y);
    }

    void FixedUpdate()
    {
        
    }

    private void GetInput()
    {
        _mvmtX = Input.GetAxis(XAxis);
        _attemptJump = Input.GetKeyDown(jumpKey);
        _attemptAttack = Input.GetKeyDown(attackKey);
    }

    private bool IsGrounded()
    {
        LayerMask floorLayer = LayerMask.GetMask("Floor");
        RaycastHit2D raycastHit = Physics2D.BoxCast(_boxCollider.bounds.center, _boxCollider.bounds.size,
            0f, Vector2.down, groundedLeeway, floorLayer);
        
        return raycastHit.collider;
    }
}
