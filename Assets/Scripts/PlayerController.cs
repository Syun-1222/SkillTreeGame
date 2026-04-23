using UnityEngine;

public class PlayerController : MonoBehaviour
{
    [Header("Move")]
    [SerializeField] private float moveSpeed = 5f;
    [SerializeField] private float jumpForce = 20f;

    [Header("Attack")]
    [SerializeField] private Collider2D hitBox; 

    [Header("Ground Check")]
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

    private float moveInput;
    private bool jumpRequest;
    private bool isGrounded;
    private bool isAttacking;
    private bool wasGrounded;

    private void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
        spriteRenderer = GetComponent<SpriteRenderer>();
        animator = GetComponent<Animator>();
        player = GetComponent<Player>();

        hitBox.enabled = false;
    }

    private void Update()
    {
        HandleInput();
        HandleJump();
        AnimationUpdate();
    }

    private void FixedUpdate()
    {
        Move();
        GroundCheck();
        CheckLanding();
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
        moveInput = 0f;

        if (Input.GetKey(KeyCode.A))
        {
            moveInput = -1f;
        }
        else if (Input.GetKey(KeyCode.D))
        {
            moveInput = 1f;
        }

        if (Input.GetKeyDown(KeyCode.Space))
        {
            Debug.Log("ジャンプ入力");
            if (isGrounded)
            {
                jumpRequest = true;
            }
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
        
        if (jumpRequest)
        {
            Debug.Log("ジャンプ実行");
            Jump();
            jumpRequest = false;
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

    // --------------------
    // HitBox制御
    // --------------------
    public void EnableHitBox()
    {
        hitBox.enabled = true;
    }

    public void DisableHitBox()
    {
        hitBox.enabled = false;
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
        
        if (!hitBox.enabled) return;

        Enemy enemy = collision.GetComponent<Enemy>();

        if (enemy != null)
        {
            Debug.Log("敵にヒット");
            enemy.TakeDamage(Player.Instance.AttackPower);
        }
    }

    // --------------------
    // 地面判定
    // --------------------
    private void GroundCheck()
    {
        bool prev = isGrounded;
        
        isGrounded = Physics2D.OverlapCircle(
            groundCheck.position,
            groundRadius,
            groundLayer
        );
        if (!prev && isGrounded)
        {
            Debug.Log("[Ground] 地面に接触");
        }

        if (prev && !isGrounded)
        {
            Debug.Log("[Ground] 地面から離れた");
        }
    }
    
    // 着地判定
    private void CheckLanding()
    {
        if (!wasGrounded && isGrounded)
        {
            Debug.Log("[LAND] triggered");
            animator.SetTrigger("Land");
        }
        wasGrounded = isGrounded;
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