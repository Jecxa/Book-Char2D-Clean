using UnityEngine;
using UnityEngine.InputSystem;

[RequireComponent(typeof(Rigidbody2D))]
[RequireComponent(typeof(Animator))]
[RequireComponent(typeof(Collider2D))]
public class PlayerController2D : MonoBehaviour
{
    [Header("Movimiento")]
    public float moveSpeed = 5f;
    public float jumpForce = 8f;

    [Header("Ground Check")]
    public Transform groundCheck;     
    public LayerMask groundMask;      
    public float groundRadius = 0.25f; 

    [Header("Buffers (opcional)")]
    public float coyoteTime = 0.1f;   
    public float jumpBuffer = 0.1f;   

    private Rigidbody2D rb;
    private Animator anim;
    private Collider2D col;

    private bool facingRight = true;
    private bool isGrounded;
    private float coyoteTimer;
    private float jumpBufferTimer;

    void Awake()
    {
        rb  = GetComponent<Rigidbody2D>();
        anim = GetComponent<Animator>();
        col = GetComponent<Collider2D>();
    }

    void Update()
    {
       
        float x = 0f;
        bool jumpPressed = false;

        if (Keyboard.current != null)
        {
            if (Keyboard.current.leftArrowKey.isPressed || Keyboard.current.aKey.isPressed)  x -= 1f;
            if (Keyboard.current.rightArrowKey.isPressed || Keyboard.current.dKey.isPressed) x += 1f;
            jumpPressed = Keyboard.current.spaceKey.wasPressedThisFrame;
        }

        rb.linearVelocity = new Vector2(x * moveSpeed, rb.linearVelocity.y);

        
        if (x > 0 && !facingRight) Flip();
        else if (x < 0 && facingRight) Flip();

        if (isGrounded) coyoteTimer = coyoteTime;
        else            coyoteTimer -= Time.deltaTime;

        if (jumpPressed) jumpBufferTimer = jumpBuffer;
        else             jumpBufferTimer -= Time.deltaTime;

    
        if (jumpBufferTimer > 0f && coyoteTimer > 0f)
        {
            rb.linearVelocity = new Vector2(rb.linearVelocity.x, jumpForce);
            jumpBufferTimer = 0f;
            coyoteTimer = 0f;
        }

     
        if (anim.runtimeAnimatorController != null)
        {
          
            float speedParam = Mathf.Abs(x);
            anim.SetFloat("Speed", speedParam);
            anim.SetBool("IsGrounded", isGrounded);
        }
    }

    void FixedUpdate()
    {
       
        bool circleHit = false;
        if (groundCheck != null)
            circleHit = Physics2D.OverlapCircle(groundCheck.position, groundRadius, groundMask);

        bool layerHit = col.IsTouchingLayers(groundMask);

        isGrounded = circleHit || layerHit;
    }

    void Flip()
    {
        facingRight = !facingRight;
        var s = transform.localScale;
        s.x *= -1f;
        transform.localScale = s;
    }

    void OnDrawGizmosSelected()
    {
        if (!groundCheck) return;
        Gizmos.color = Color.yellow;
        Gizmos.DrawWireSphere(groundCheck.position, groundRadius);
    }
}
