using UnityEngine;

public class PlayerController : MonoBehaviour
{
    [Header("Move")]
    [SerializeField] private float moveSpeed = 5f;
    [SerializeField] private float jumpForce = 20f;

    [Header("Attack")]
    [SerializeField] private AttackHitbox hitBox;
    [SerializeField] private AttackData attack1;
    [SerializeField] private MotionPreviewData attack1Motion;
    [SerializeField] private AttackData attack2;
    [SerializeField] private AttackData attack3;
    [SerializeField] private AttackData dashAttack;

    [Header("Sliding")]
    [SerializeField] private MotionPreviewData slidMotion;

    [Header("AirDash")]
    [SerializeField] private MotionPreviewData airDashMotion;

    [Header("BackStep")]
    [SerializeField] private MotionPreviewData backStepMotion;

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
    private MotionPreviewData currentMotion;
    private float defaultGravityScale;
    private float motionTime;
    private float previousFixedTime;
    private string prevState;
    private float debugT;

    private int slideCount;
    private int airDashCount;
    private int backStepCount;
    private int animPlayId;

    private float inputLockTimer;
    private bool isMotionPlaying;   // モーション動き
    private float motionTimer;      // モーション時間
    private Vector2 motionStartPos; // モーション位置

    private bool isGrounded;        // 地面判定：現在地面に接地しているか
    private bool landLocked;        // 着地ロック
    private bool wasGrounded;       // 前フレームの地面状態
    private float landLockTime;     // 着地ロックの解除

    private float moveInput;        // 移動入力値
    private bool jumpRequest;       // ジャンプ入力の一時保持
    private bool isAttacking;       // 攻撃中の制御

    private bool isBackStep;      
    private bool isSliding;
    private bool isAirDashing;
    private bool airDashUsed;       // 空中ダッシュの使用制限
    private bool canAirDash = true; // 空中ダッシュを使用可能判定     

    private void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
        spriteRenderer = GetComponent<SpriteRenderer>();
        animator = GetComponent<Animator>();
        player = GetComponent<Player>();
        defaultGravityScale = rb.gravityScale;
    }

    private void Update()
    {

        if (inputLockTimer > 0f)
        inputLockTimer -= Time.deltaTime;

        HandleInput();
        HandleJump();
        UpdateState();
        AnimationUpdate();
        LogAnimatorState();
    }

    private void FixedUpdate()
    {
        GroundCheck();
        CheckLanding();

        if (isMotionPlaying)
        {
            motionTimer += Time.fixedDeltaTime;
            debugT = motionTimer / currentMotion.duration;
            debugT = Mathf.Clamp01(debugT);
            ApplyMotion(debugT);
            
            if (debugT >= 1f)
            {
                isMotionPlaying = false;
                currentMotion = null;

                if (isAirDashing)
                {
                    EndAirDash();
                }
            }
        }
        Move();

        if (!isGrounded)
        {
            jumpRequest = false;
        }
    }

    private void UpdateState()
    {
        if (isAirDashing)
        {
            if (isGrounded || rb.linearVelocity.y < 0f)
            {
                EndAirDash();
            }
        }      
    }

    // --------------------
    // 入力
    // --------------------
    private void HandleInput()
    {

        if (IsInputLocked())
        {
            moveInput = 0f;
            Debug.Log("入力ロック中");
            return;
        }

        if (isAttacking)
        {
            Debug.Log("入力無効（攻撃中）");
            return;
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
            Debug.Log("スペース入力");
            jumpRequest = true;
        }

        if (Input.GetKeyDown(KeyCode.LeftShift))
        {
            Debug.Log("Shift入力");
            HandleShift();
        }
        
        if (Input.GetKeyDown(KeyCode.LeftControl))
        {
            Debug.Log("LeftControlを入力");
            HandleBackStep();
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
    
    private bool IsInputLocked()
    {
        return isMotionPlaying || isAttacking;
    }

    private void ApplyMotion(float t)
    {
        float dir = spriteRenderer.flipX ? -1f : 1f;
        if (currentMotion.directionType == MotionDirectionType.Backward)
            dir *= -1f;

        float x = currentMotion.curveX.Evaluate(t)
                * currentMotion.distanceX
                * dir;

        float y = currentMotion.curveY.Evaluate(t)
                * currentMotion.distanceY;

        Vector2 pos = motionStartPos + new Vector2(x, y);
        rb.MovePosition(pos);
    }


    //　モーション動かす
    public void PlayMotion(MotionPreviewData motion)
    {
        animPlayId++;
        int id = animPlayId;
        currentMotion = motion;
        motionTimer = 0f;
        isMotionPlaying = true;
        inputLockTimer = motion.inputLockTime;
        motionStartPos = rb.position;
    }

    private void ClearAllTriggers()
    {
        animator.ResetTrigger("Jump");
        animator.ResetTrigger("Land");
        animator.ResetTrigger("airdash");
        animator.ResetTrigger("slid");
        animator.ResetTrigger("backStep");
        animator.ResetTrigger("Attack");
    }

    // ジャンプ入力された時、かつ、地面接触判定が付いている時にジャンプ実行する
    private void HandleJump()
    {
        if (IsInputLocked())
            return;

        if (!isGrounded)
            return;
            
        if (isAttacking)
            return;
        
        if (isMotionPlaying)
            return;

        if (jumpRequest)
        {
            jumpRequest = false;
            Debug.Log("ジャンプ実行");
            Jump();   
        }
    }

    // Shiftを押すとスライディングor空中ダッシュ
    private void HandleShift()
    {
        if (IsInputLocked()) 
            return;
        
        if (landLocked)
            return;

        if (isAirDashing)
            return;
        
        if (isMotionPlaying)
            return;

        if (isSliding || isBackStep)
            return;
            
        // スライディング
        if (isGrounded && Mathf.Abs(moveInput) > 0.1f)
        {
            Debug.Log("スライディング実行");
            StartSliding();
            return;
        }
        // 空中ダッシュ
        else if (!isGrounded && canAirDash && !airDashUsed)
        {
            Debug.Log("空中ダッシュ実行");
            airDashUsed = true;
            StartAirDash();
        }
    }

    private void HandleBackStep()
    {
        if (!isGrounded) 
            return;
        
        if (isMotionPlaying) 
            return;
        
        var state = animator.GetCurrentAnimatorStateInfo(0);
        if (!state.IsName("Run")) 
            return;

        bool isRunState = state.IsName("Run");

        if (!isRunState)
        {
            Debug.Log("Run状態以外なのでBackStep不可");
            return;
        }

        Debug.Log("バックステップ実行");
        StartBackStep();
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
        if (isMotionPlaying)
        {
            rb.linearVelocity = Vector2.zero;
            return;
        }
        rb.linearVelocity = new Vector2(moveInput * moveSpeed, rb.linearVelocity.y);
    }

    // --------------------
    // ジャンプ
    // --------------------
    private void Jump()
    {
        ClearAllTriggers();
        rb.linearVelocity = new Vector2(rb.linearVelocity.x, 0f);
        rb.AddForce(Vector2.up * jumpForce, ForceMode2D.Impulse);

        animator.SetTrigger("Jump");
    }

    // --------------------
    // スライディング
    // --------------------
    private void StartSliding()
    {
        ClearAllTriggers();
        if (isSliding) return;
        isSliding = true;
        slideCount++;
        Debug.Log($"[SLIDE] 発動回数: {slideCount}");
        PlayMotion(slidMotion);
        animator.SetTrigger("slid");
    }

    public void EndSlide()
    {
        Debug.Log($"[MOTION END] Slide time={Time.time}");
        isSliding = false;
    }
    // --------------------
    // 空中ダッシュ
    // --------------------
    private void StartAirDash()
    {
        ClearAllTriggers();
        if (isAirDashing) return;
        isAirDashing = true;
        airDashUsed = true;

        airDashCount++;
        Debug.Log($"[AIR DASH] 発動回数: {airDashCount}");

        rb.linearVelocity = Vector2.zero;
        rb.gravityScale = 0f;

        PlayMotion(airDashMotion);
        animator.ResetTrigger("airdash");
        animator.SetTrigger("airdash");
    }

    public void EndAirDash()
    {
        if (!isAirDashing) return;
        isAirDashing = false;
        isMotionPlaying = false;
        currentMotion = null;
        Debug.Log("AirDashアニメーション終了");
        Debug.Log($"[MOTION END] AirDash time={Time.time}");
        rb.gravityScale = defaultGravityScale;
    }

    // --------------------
    // バックステップ
    // --------------------
    private void StartBackStep()
    {
        ClearAllTriggers();
        if (isBackStep) return;
        Debug.Log($"[BACKSTEP INPUT STATE] motionStartPos={motionStartPos} flipX={spriteRenderer.flipX}");

        isBackStep = true;
        backStepCount++;

        motionTimer = 0f;
        motionStartPos = rb.position;

        Debug.Log($"[BACK STEP] 発動回数: {backStepCount}");

        PlayMotion(backStepMotion);
        animator.SetTrigger("backStep");
    }

    public void EndBackStep()
    {
        Debug.Log($"[MOTION END] BackStep time={Time.time}");
        isBackStep = false;
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

    // 2回再生か確認
    private void LogAnimatorState()
    {
        var state = animator.GetCurrentAnimatorStateInfo(0);

        string name = state.IsName("slid") ? "slid"
                    : state.IsName("airdash") ? "airdash"
                    : state.IsName("backStep") ? "backStep"
                    : "other";

        if (name != prevState)
        {
            Debug.Log($"[ANIM STATE CHANGE] {prevState} -> {name} time={Time.time}");
            prevState = name;
        }
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

    /*
    public void Stop(SEType type)
    {
        seManager.PlaySE(SEType type);
    }
    */

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