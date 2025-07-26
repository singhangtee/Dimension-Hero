using System;
using System.Collections;
using System.Collections.Generic;
using System.Numerics;
using UnityEngine;
using Quaternion = UnityEngine.Quaternion;
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
    
    [Header("Physics")]
    private BoxCollider2D _boxCollider;
    private Rigidbody2D _rb;
    private float _mvmtX;
    private bool _attemptJump;
    private int _noOfJumps;
    private bool _attemptAttack;
    
    
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
        HandleRealitySwitch();

    }
    
    private void HandleJump()
    {
        if (_attemptJump && IsGrounded())
        {
            _rb.velocity = new Vector2(_rb.velocity.x, jumpForce);
            _noOfJumps++;
            return;
        }

        if (_attemptJump && _noOfJumps == 1)
        {
            _rb.velocity = new Vector2(_rb.velocity.x, jumpForce);
            _noOfJumps++;
            return;
        }

        if (_noOfJumps == 2)
        {
            _noOfJumps++;
        }
    }

    private void HandleRun()
    {
        if (_mvmtX > 0 && transform.rotation.y == 0) transform.rotation = Quaternion.Euler(0, 0, 0);
        else if (_mvmtX < 0 && transform.rotation.y != 0) transform.rotation = Quaternion.Euler(0, 180, 0);
        
        _rb.velocity = new Vector2(_mvmtX * speed, _rb.velocity.y);
    }

    private void HandleRealitySwitch()
    {
        if (Input.GetKeyDown(KeyCode.CapsLock))
        {
            RealityManager.SwitchReality();
        }
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
