using UnityEngine;

public class PlayerController : MonoBehaviour
{
    [Header("Move")]
    [SerializeField] private float moveSpeed = 5f;
    [SerializeField] private float jumpForce = 20f;

    [Header("Attack")]
    [SerializeField] private AttackHitbox hitBox;
    [SerializeField] private AttackData attack1;
    [SerializeField] private AttackData attack2;
    [SerializeField] private AttackData attack3;
    [SerializeField] private AttackData dashAttack;

    [Header("Sliding")]
    [SerializeField] private float slideTime = 0.5f;
    [SerializeField] private float slideDistance = 10f;
    [SerializeField] private float slideInputLockTime = 0.5f;

    [Header("AirDash")]
    [SerializeField] private float airDashTime = 0.3f;
    [SerializeField] private float airDashDistance = 10f;
    [SerializeField] private float airDashInputLockTime = 0.5f;

    [Header("Ground Check")]
    [SerializeField] private float landCooldown = 0.1f;
    [SerializeField] private Transform groundCheck;
    [SerializeField] private float groundRadius = 0.1f;
    [SerializeField] private LayerMask groundLayer;

    [Header("Game")]
    [SerializeField] private GameManager gameManager;

    [Header("SE")]
    [SerializeField] private SEManager seManager;

    private Rigidbody2D rb;
    private SpriteRenderer spriteRenderer;
    private Animator animator;
    private Player player;

    private bool isGrounded;        // 地面判定：現在地面に接地しているか
    private bool landLocked;        // 着地ロック
    private bool wasGrounded;       // 前フレームの地面状態
    private float landLockTime;     // 着地ロックの解除

    private float moveInput;        // 移動入力値
    private bool jumpRequest;       // ジャンプ入力の一時保持
    private bool isAttacking;       // 攻撃中の制御

    private bool isSliding;         // スライディングの制御
    private int slideDir;           // スライディング方向
    private Vector2 slideStartPos;  // スライディング方向
    private Vector2 slideTargetPos; // スライディング終了位置 
    private float slideElapsed;     // スライディング経過時間
    private float slideTimer;       // スライディングタイマー
    private float slideInputLockTimer;// スライディング中の入力受付 

    private bool isAirDashing;      // 空中ダッシュ中の制御
    private bool airDashUsed;       // 空中ダッシュの使用制限
    private Vector2 airDashStartPos;// 空中ダッシュの方向
    private Vector2 airDashTargetPos;// 空中ダッシュの終了位置
    private float airDashElapsed;   //　空中ダッシュ経過時間
    private bool canAirDash = true; // 空中ダッシュを使用可能判定 
    private float airDashTimer;     // 空中ダッシュタイマー
    private float airDashInputLockTimer;//スライディング中の入力受付

    private void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
        spriteRenderer = GetComponent<SpriteRenderer>();
        animator = GetComponent<Animator>();
        player = GetComponent<Player>();
    }

    private void Update()
    {
        HandleInput();
        HandleJump();
        AnimationUpdate();

        if (slideInputLockTimer > 0f)
            slideInputLockTimer -= Time.deltaTime;

        if (airDashInputLockTimer > 0f)
            airDashInputLockTimer -= Time.deltaTime;
    }

    private void FixedUpdate()
    {
        Move();
        GroundCheck();
        CheckLanding();

        // スライディングでキャラを滑らせる処理
        if (isSliding)
        {
            slideTimer -= Time.fixedDeltaTime;
            slideElapsed += Time.fixedDeltaTime;

            float t = slideElapsed / slideTime;

            Vector2 newPos = Vector2.Lerp(slideStartPos, slideTargetPos, t);
            rb.MovePosition(newPos);

            float movedDistance = Vector2.Distance(slideStartPos, newPos);

            Debug.Log($"start:{slideStartPos} target:{slideTargetPos} current:{rb.position}");
            Debug.Log($"[Slide] 移動距離: {movedDistance:F3}");
            if (slideInputLockTimer > 0f)
            {
                slideInputLockTimer -= Time.deltaTime;
            }

            if (movedDistance >= slideDistance || slideTimer <= 0f)
            {
                isSliding = false;
            }
        }

        // 空中ダッシュでキャラを滑らせる処理
        if (isAirDashing)
        {
            airDashTimer -= Time.fixedDeltaTime;
            airDashElapsed += Time.fixedDeltaTime;

            float t = airDashElapsed / airDashTime;

            Vector2 newPos = Vector2.Lerp(airDashStartPos, airDashTargetPos, t);
            rb.MovePosition(newPos);

            float movedDistance = Vector2.Distance(airDashStartPos, newPos);

            Debug.Log($"start:{airDashStartPos} target:{airDashTargetPos} current:{rb.position}");
            Debug.Log($"[AirDash] 移動距離: {movedDistance:F3}");

            rb.linearVelocity = Vector2.zero;
            if (airDashInputLockTimer > 0f)
            {
                airDashInputLockTimer -= Time.deltaTime;
            }

            if (movedDistance >= 10f || airDashTimer <= 0f)
            {
                isAirDashing = false;
            }
        }
    }

    // --------------------
    // 入力
    // --------------------
    private void HandleInput()
    {
        if (isAttacking)
        {
            Debug.Log("入力無効（攻撃中）");
            return;
        }
        if (slideInputLockTimer > 0f || airDashInputLockTimer > 0f)
        {
            moveInput = 0f;
        }
        else
        {
            moveInput = 0f;

            if (Input.GetKey(KeyCode.A))
                moveInput = -1f;
            else if (Input.GetKey(KeyCode.D))
                moveInput = 1f;
        }

        if (Input.GetKeyDown(KeyCode.Space))
        {
            Debug.Log("ジャンプ入力");

            if (slideInputLockTimer > 0f || airDashInputLockTimer > 0f)
                return;

            if (isGrounded)
                jumpRequest = true;
        }

        if (Input.GetKeyDown(KeyCode.LeftShift))
        {
            Debug.Log("Shift入力");
            HandleShift();
        }

        if (Input.GetMouseButtonDown(0))
        {
            Debug.Log("左クリック入力");
            Attack();
        }

        if (moveInput > 0)
            spriteRenderer.flipX = false;
        else if (moveInput < 0)
            spriteRenderer.flipX = true;
    }

    // ジャンプ入力された時、かつ、地面接触判定が付いている時にジャンプ実行する
    private void HandleJump()
    {
        if (!isGrounded)
            return;

        if (isSliding) 
            return;

        if (isAirDashing)
            return;

        if (isAttacking)
            return;

        if (jumpRequest)
        {
            Debug.Log("ジャンプ実行");
            Jump();
            jumpRequest = false;
        }
    }

    // Shiftを押すとスライディングor空中ダッシュ
    private void HandleShift()
    {
        if (landLocked)
            return;

        if (isAttacking || isSliding || isAirDashing)
            return;

        // スライディング
        if (isGrounded && Mathf.Abs(moveInput) > 0.1f)
        {
            Debug.Log("スライディング実行");
            StartSliding();
        }
        // 空中ダッシュ
        else if (!isGrounded && canAirDash && !airDashUsed)
        {
            Debug.Log("空中ダッシュ実行");
            airDashUsed = true;
            StartAirDash();
        }
    }

    // --------------------
    // 移動
    // --------------------
    private void Move()
    {
        if (isAttacking)
        {
            Debug.Log("攻撃中（移動停止）");
            rb.linearVelocity = new Vector2(0, rb.linearVelocity.y);
            return;
        }

        if (isSliding || isAirDashing)
        {
            return;
        }

        rb.linearVelocity = new Vector2(moveInput * moveSpeed, rb.linearVelocity.y);
    }

    // --------------------
    // ジャンプ
    // --------------------
    private void Jump()
    {
        rb.linearVelocity = new Vector2(rb.linearVelocity.x, 0f);
        rb.AddForce(Vector2.up * jumpForce, ForceMode2D.Impulse);

        animator.SetTrigger("Jump");
    }

    // --------------------
    // スライディング
    // --------------------
    private void StartSliding()
    {
        isSliding = true;
        slideTimer = slideTime;
        slideElapsed = 0f;
        slideStartPos = rb.position;

        slideDir = spriteRenderer.flipX ? -1 : 1;
        slideTargetPos = slideStartPos + new Vector2(slideDir * slideDistance, 0f);
        slideInputLockTimer = slideInputLockTime;
        jumpRequest = false;

        animator.SetTrigger("slid");
    }

    // --------------------
    // 空中ダッシュ
    // --------------------
    private void StartAirDash()
    {
        isAirDashing = true;
        canAirDash = false;
        airDashTimer = airDashTime;
        airDashElapsed = 0f;
        airDashStartPos = rb.position;

        int dir = spriteRenderer.flipX ? -1 : 1;
        airDashTargetPos = airDashStartPos + new Vector2(dir * airDashDistance, 0f);
        rb.linearVelocity = Vector2.zero; 
        airDashInputLockTimer = airDashInputLockTime;
        jumpRequest = false;

        animator.SetTrigger("airdash");
    }

    public void EndAirDash()
    {
        Debug.Log("AirDashアニメーション終了");
        isAirDashing = false;
    }

    // --------------------
    // 攻撃
    // --------------------
    private void Attack()
    {
        if (isAttacking)
        {
            Debug.Log("攻撃入力無効");
            return;
        }
        isAttacking = true;

        Debug.Log("Attackアニメーション開始");
        animator.SetTrigger("Attack");
    }

    public void PlayAttack1()
    {
        Debug.Log("PlayAttack1 呼ばれた");
        hitBox.Play(attack1);
    }

    public void EndAttack()
    {
        Debug.Log("Attackアニメーション終了");
        isAttacking = false;
    }

    // --------------------
    // 当たり判定
    // --------------------
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("OutZone"))
        {
            Debug.Log("ゲームオーバー（場外）");
            GameManager.Instance.GameOver();
            return;
        }

        if (collision.CompareTag("Finish"))
        {
            Debug.Log("ゲームクリア");
            GameManager.Instance.GameClear();
            return;
        }
    }

    // --------------------
    // 地面判定
    // --------------------
    private void GroundCheck()
    {
        wasGrounded = isGrounded;
        
        isGrounded = Physics2D.OverlapCircle(
            groundCheck.position,
            groundRadius,
            groundLayer
        );

        if (!wasGrounded && isGrounded)
        {
            Debug.Log("[Ground] 地面に接触");
        }

        if (wasGrounded && !isGrounded)
        {
            Debug.Log("[Ground] 地面から離れた");
        }

        if (!wasGrounded && isGrounded)
        {
            Debug.Log("着地した瞬間");
            canAirDash = true;
            airDashUsed = false;
        }
    }
    
    // 着地判定
    private void CheckLanding()
    {
        if (!wasGrounded && isGrounded && !landLocked)
        {
            Debug.Log("[LAND] triggered");
            animator.SetTrigger("Land");

            landLocked = true;
            landLockTime = Time.time;
        }

        if (Time.time - landLockTime > landCooldown)
        {
            landLocked = false;
        }
    }

    // --------------------
    // アニメ
    // --------------------
    private void AnimationUpdate()
    {
        animator.SetFloat("Speed", Mathf.Abs(moveInput));
        animator.SetBool("IsGrounded", isGrounded);
        animator.SetFloat("VerticalVelocity", rb.linearVelocity.y);
        bool isFalling = rb.linearVelocity.y < -0.1f && !isGrounded;
        animator.SetBool("IsFalling", isFalling);
    }

    // --------------------
    // SE
    // --------------------
    public void PlayFootstepRight()
    {
        seManager.PlayFootstep(true);
    }

    public void PlayFootstepLeft()
    {
        seManager.PlayFootstep(false);
    }

    public void PlayArmor()
    {
        seManager.PlayArmor();
    }

    public void PlaySE(int type)
    {
        seManager.Play((SEType)type);
    }

    // --------------------
    // デバッグ：接地面判定の範囲描画
    // --------------------
    private void OnDrawGizmos()
    {
        if (groundCheck == null) return;

        Gizmos.color = Color.green;
        Gizmos.DrawWireSphere(groundCheck.position, groundRadius);
    }
}