using System;
using UnityEngine;
using UnityEngine.SceneManagement;


public class Player : MonoBehaviour
{
    // Hashcodes for faster performance according to Rider
    private static readonly int IsJumping = Animator.StringToHash("IsJumping");
    private static readonly int Speed = Animator.StringToHash("Speed");

    [Header("Input")]
    public KeyCode jumpKey = KeyCode.Space;
    public KeyCode attackKey = KeyCode.Mouse0;
    private const string XAxis = "Horizontal";

    [Header("Movement")]
    public float speed = 5f;
    public float jumpForce = 6f;
    public float groundedLeeway = 0.1f;
    public int maxJumps = 2; // Made this configurable for double jump

    [Header("Physics")]
    private BoxCollider2D _boxCollider;
    private Rigidbody2D _rb;
    private float _mvmtX;
    private bool _attemptJump;
    private int _jumpsUsed; // Renamed for clarity
    private bool _attemptAttack;

    [Header("Animation")]
    private Animator _animator;
    private bool _facingRight = true; // Assume facing right initially
    private bool _wasGrounded; // Track previous grounded state

    void Start()
    {
        _rb = GetComponent<Rigidbody2D>();
        _boxCollider = GetComponent<BoxCollider2D>();
        _animator = GetComponent<Animator>();
    }

    void Update()
    {
        GetInput();
        
        // Check grounded state first
        bool isGrounded = IsGrounded();
        
        // Reset jumps when landing
        if (isGrounded && !_wasGrounded) _jumpsUsed = 0;
        
        HandleRun();
        HandleJump(isGrounded);
        HandleRealitySwitch();
        HandleRestart();

        // Update animator params every frame
        _animator.SetFloat(Speed, Mathf.Abs(_mvmtX));
        _animator.SetBool(IsJumping, !isGrounded);
        
        // Store grounded state for next frame
        _wasGrounded = isGrounded;
    }

    private void HandleRestart()
    {
        if (!Input.GetKeyDown(KeyCode.R)) return;
        
        Scene currentScene = SceneManager.GetActiveScene();
        SceneManager.LoadScene(currentScene.name);

    }
    private void HandleJump(bool isGrounded)
    {
        // Allow jumping if we haven't used all jumps
        if (_attemptJump && _jumpsUsed < maxJumps)
        {
            _jumpsUsed++;
            _rb.velocity = new Vector2(_rb.velocity.x, jumpForce);
        }

        // Reset attempt jump to prevent multiple jumps in one frame
        _attemptJump = false;
    }

    private void HandleRun()
    {
        // Move the player
        _rb.velocity = new Vector2(_mvmtX * speed, _rb.velocity.y);

        // Flip the sprite depending on direction
        if (_mvmtX > 0 && !_facingRight) Flip();
        else if (_mvmtX < 0 && _facingRight) Flip();
    }

    private void Flip()
    {
        _facingRight = !_facingRight;
        Vector3 scale = transform.localScale;
        scale.x *= -1;
        transform.localScale = scale;
    }

    private void HandleRealitySwitch()
    {
        if (Input.GetKeyDown(KeyCode.CapsLock)) RealityManager.SwitchReality();
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

        // Get the bottom center of the collider
        Vector2 boxCenter = new Vector2(_boxCollider.bounds.center.x, _boxCollider.bounds.min.y);

        // Create a smaller box size for just the bottom check
        // Keep the width the same but make height very small
        Vector2 boxSize = new Vector2(_boxCollider.bounds.size.x, 0.1f);

        RaycastHit2D raycastHit = Physics2D.BoxCast(boxCenter, boxSize,
            0f, Vector2.down, groundedLeeway, floorLayer);

        return raycastHit.collider;
    }
}