using UnityEngine;
using System.Collections;

public class PlayerController : MonoBehaviour
{
    [Header("이동 및 점프 설정")]
    public float moveSpeed = 7f;        // 걷는 속도
    public float jumpForce = 12f;       // 점프 힘
    public float dashSpeed = 18f;       // 대시 속도
    public float dashDuration = 0.2f;   // 대시 지속 시간
    public float dashCooldown = 1f;     // 대시 재사용 대기 시간

    [Header("지면 체크")]
    public Transform groundCheck;       // 발밑 위치 체크용 오브젝트 (Empty Object)
    public float checkRadius = 0.2f;    // 체크 범위
    public LayerMask groundLayer;       // 무엇을 바닥으로 인식할지 (Ground 레이어)
    private bool isGrounded;            // 땅에 닿아있는지 여부

    [Header("기본 이미지 (정지 상태)")]
    public Sprite idleFront; public Sprite idleBack;
    public Sprite idleLeft;  public Sprite idleRight;

    [Header("걷기용 이미지 (교체 모션)")]
    public Sprite walkFront; public Sprite walkBack;
    public Sprite walkLeft;  public Sprite walkRight;

    [Header("대시 전용 이미지 (좌우 분리)")]
    public Sprite dashLeft;  public Sprite dashRight;

    [Header("애니메이션 설정")]
    public float animationSpeed = 0.2f; // 발을 바꾸는 속도

    // 내부 컴포넌트 및 변수
    private Rigidbody2D rb;             // 물리 이동 처리
    private Vector2 moveInput;          // 키보드 입력값 (X, Y)
    private SpriteRenderer spriteRenderer; // 이미지를 그려주는 컴포넌트
    
    private bool isDashing;             // 현재 대시 중인지 체크
    private bool canDash = true;        // 대시를 사용할 수 있는 상태인지 체크
    private float animTimer;            // 이미지 교체 타이밍용 타이머
    private bool stepToggle;            // true면 기본 이미지, false면 걷기 이미지를 출력

    void Start()
    {
        // 컴포넌트 연결 및 초기 설정
        rb = GetComponent<Rigidbody2D>();
        spriteRenderer = GetComponent<SpriteRenderer>();
        
        // 캐릭터가 회전하지 않게 고정 및 물리 충돌 방식 설정
        rb.freezeRotation = true;
        rb.collisionDetectionMode = CollisionDetectionMode2D.Continuous;
    }

    void Update()
    {
        // 1. 대시 중에는 입력을 무시
        if (isDashing) return;

        // 2. 땅 체크 (발밑 센서 작동)
        isGrounded = Physics2D.OverlapCircle(groundCheck.position, checkRadius, groundLayer);

        // 3. 입력 받기 (WASD / 화살표)
        moveInput.x = Input.GetAxisRaw("Horizontal");
        moveInput.y = Input.GetAxisRaw("Vertical");

        // 4. 점프 실행 (Space 키 + 땅에 닿아있을 때)
        if (Input.GetKeyDown(KeyCode.Space) && isGrounded)
        {
            rb.linearVelocity = new Vector2(rb.linearVelocity.x, jumpForce);
        }

        // 5. 애니메이션 로직 실행 (이미지 교체)
        UpdateAnimation();

        // 6. 대시 실행 (왼쪽 Shift)
        if (Input.GetKeyDown(KeyCode.LeftShift) && canDash)
        {
            StartCoroutine(Dash());
        }
    }

    void UpdateAnimation()
    {
        // 캐릭터가 어느 방향으로든 움직이고 있을 때
        if (moveInput.sqrMagnitude > 0) 
        {
            animTimer += Time.deltaTime;
            if (animTimer >= animationSpeed)
            {
                stepToggle = !stepToggle; // 상태 반전
                animTimer = 0;
            }

            // 입력 방향에 따라 이미지 세트 선택
            if (moveInput.x < 0) // 왼쪽 이동
                spriteRenderer.sprite = stepToggle ? idleLeft : walkLeft;
            else if (moveInput.x > 0) // 오른쪽 이동
                spriteRenderer.sprite = stepToggle ? idleRight : walkRight;
            else if (moveInput.y > 0) // 위쪽 이동 (W키 - 뒷모습 출력!)
                spriteRenderer.sprite = stepToggle ? idleBack : walkBack;
            else if (moveInput.y < 0) // 아래쪽 이동 (S키 - 앞모습 출력)
                spriteRenderer.sprite = stepToggle ? idleFront : walkFront;
        }
        else if (isGrounded) // 멈춰 있고 땅에 있을 때
        {
            // 마지막 보던 방향에 맞는 '기본(Idle)' 이미지로 고정
            if (spriteRenderer.sprite == walkLeft) spriteRenderer.sprite = idleLeft;
            else if (spriteRenderer.sprite == walkRight) spriteRenderer.sprite = idleRight;
            else if (spriteRenderer.sprite == walkBack) spriteRenderer.sprite = idleBack;
            else if (spriteRenderer.sprite == walkFront) spriteRenderer.sprite = idleFront;
        }
    }

    void FixedUpdate()
    {
        // 물리 이동 처리 (중력을 방해하지 않도록 Y축 속도는 유지)
        if (isDashing) return;
        rb.linearVelocity = new Vector2(moveInput.x * moveSpeed, rb.linearVelocity.y);
    }

    // 대시 처리를 위한 코루틴
    IEnumerator Dash()
    {
        canDash = false;
        isDashing = true;
        
        float gravityBefore = rb.gravityScale;
        rb.gravityScale = 0; // 대시 중에는 중력 무시
        
        // 현재 바라보는 방향 혹은 입력 방향으로 대시 방향 결정
        Vector2 dashDir = moveInput.x == 0 ? GetFacingDirection() : new Vector2(moveInput.x, 0).normalized;
        
        // 대시 방향에 맞춰 이미지 변경
        spriteRenderer.sprite = dashDir.x < 0 ? dashLeft : dashRight;
        
        rb.linearVelocity = dashDir * dashSpeed;
        
        yield return new WaitForSeconds(dashDuration);
        
        rb.linearVelocity = Vector2.zero;
        rb.gravityScale = gravityBefore; // 중력 복구
        isDashing = false;

        yield return new WaitForSeconds(dashCooldown);
        canDash = true;
    }

    // 펭귄이 현재 보고 있는 방향을 체크하는 보조 함수
    Vector2 GetFacingDirection()
    {
        if (spriteRenderer.sprite == idleLeft || spriteRenderer.sprite == walkLeft) return Vector2.left;
        if (spriteRenderer.sprite == idleBack || spriteRenderer.sprite == walkBack) return Vector2.up;
        if (spriteRenderer.sprite == idleRight || spriteRenderer.sprite == walkRight) return Vector2.right;
        return Vector2.down;
    }
}