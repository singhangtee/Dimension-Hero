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
    
    private BoxCollider2D boxCollider;
    private Rigidbody2D rb;
    private float mvmtX = 0;
    private bool attemptJump = false;
    private int noOfJumps = 0;
    private bool attemptAttack = false;
    
    
    // Start is called before the first frame update
    void Start()
    {
        if (GetComponent<Rigidbody2D>()) rb = GetComponent<Rigidbody2D>();
        boxCollider = GetComponent<BoxCollider2D>();
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
        if (attemptJump && IsGrounded() && noOfJumps == 0)
        {
            rb.velocity = new Vector2(rb.velocity.x, jumpForce);
            noOfJumps++;
        } 
        
        else if (attemptJump && noOfJumps is >= 1 and < 2)
        {
            rb.velocity = new Vector2(rb.velocity.x, jumpForce);
            Debug.Log("Double Jump");
            noOfJumps++;
        }

        else if (noOfJumps >= 2)
        {
            noOfJumps = 0;
        }
        
    }

    private void HandleRun()
    {
        rb.velocity = new Vector2(mvmtX * speed, rb.velocity.y);
    }

    void FixedUpdate()
    {
        
    }

    private void GetInput()
    {
        mvmtX = Input.GetAxis(XAxis);
        attemptJump = Input.GetKeyDown(jumpKey);
        attemptAttack = Input.GetKeyDown(attackKey);
    }

    private bool IsGrounded()
    {
        LayerMask floorLayer = LayerMask.GetMask("Floor");
        RaycastHit2D raycastHit = Physics2D.BoxCast(boxCollider.bounds.center, boxCollider.bounds.size,
            0f, Vector2.down, groundedLeeway, floorLayer);
        
        return raycastHit.collider;
    }
}
