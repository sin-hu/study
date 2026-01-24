using UnityEngine;
using System.Collections;

public class PlayerController : MonoBehaviour
{
    [Header("이동 및 대시 설정")]
    public float moveSpeed = 5f;
    public float dashSpeed = 15f;
    public float dashDuration = 0.2f;
    public float dashCooldown = 1f;

    private Rigidbody2D rb;
    private Vector2 moveInput;
    private Animator anim;
    private SpriteRenderer spriteRenderer;
    
    private bool isDashing;
    private bool canDash = true;

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        anim = GetComponent<Animator>();
        spriteRenderer = GetComponent<SpriteRenderer>();
        rb.freezeRotation = true;
    }

    void Update()
    {
        if (isDashing) return; // 대시 중에는 조작 불가

        // 1. 입력 받기
        moveInput.x = Input.GetAxisRaw("Horizontal");
        moveInput.y = Input.GetAxisRaw("Vertical");

        // 2. 애니메이션 파라미터 전달 (속도가 0보다 크면 걷기 애니메이션 재생)
        anim.SetBool("isMoving", moveInput.sqrMagnitude > 0);

        // 3. 좌우 반전
        if (moveInput.x > 0) spriteRenderer.flipX = false;
        else if (moveInput.x < 0) spriteRenderer.flipX = true;

        // 4. 대시 입력 (Left Shift)
        if (Input.GetKeyDown(KeyCode.LeftShift) && canDash)
        {
            StartCoroutine(Dash());
        }
    }

    void FixedUpdate()
    {
        if (isDashing) return;

        rb.MovePosition(rb.position + moveInput.normalized * moveSpeed * Time.fixedDeltaTime);
    }

    IEnumerator Dash()
    {
        canDash = false;
        isDashing = true;
        
        // 바라보는 방향 혹은 입력 방향으로 대시
        Vector2 dashDir = moveInput == Vector2.zero ? (spriteRenderer.flipX ? Vector2.left : Vector2.right) : moveInput.normalized;
        
        rb.linearVelocity = dashDir * dashSpeed;
        
        yield return new WaitForSeconds(dashDuration);
        
        rb.linearVelocity = Vector2.zero;
        isDashing = false;

        yield return new WaitForSeconds(dashCooldown);
        canDash = true;
    }
}