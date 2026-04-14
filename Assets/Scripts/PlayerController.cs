using UnityEngine;

public class PlayerController : MonoBehaviour
{
    [SerializeField] private float moveSpeed = 5f;
    [SerializeField] private float jumpForce = 20f;

    [SerializeField] private Transform groundCheck;
    [SerializeField] private float groundRadius = 0.1f;
    [SerializeField] private LayerMask groundLayer;

    private Rigidbody2D rb;
    private SpriteRenderer spriteRenderer;
    private Animator animator;

    private float moveInput;
    private bool isGrounded;
    private bool wasGrounded;
    private bool isAttacking;

    private void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
        spriteRenderer = GetComponent<SpriteRenderer>();
        animator = GetComponent<Animator>();
    }

    private void Update()
    {
        // 接地判定（1回だけ）
        bool currentGrounded = Physics2D.OverlapCircle(
            groundCheck.position,
            groundRadius,
            groundLayer
        );
        isGrounded = currentGrounded;

        if (currentGrounded && !wasGrounded)
        {
            Debug.Log("地面接触");
            animator.SetTrigger("Land");
        }
        if (!currentGrounded && wasGrounded)
        {
            Debug.Log("地面離れた");
        }
        wasGrounded = currentGrounded;

        // 入力
        moveInput = 0f;

        if (Input.GetKey(KeyCode.A))
            moveInput = -1f;
        else if (Input.GetKey(KeyCode.D))
            moveInput = 1f;

        if (moveInput > 0)
            spriteRenderer.flipX = false;
        else if (moveInput < 0)
            spriteRenderer.flipX = true;

        // ジャンプ
        if (Input.GetKeyDown(KeyCode.Space))
        {
            Debug.Log("Spaceキーが押されました");

            if (isGrounded)
            {
                Debug.Log("ジャンプ実行");
                Jump();
            }
            else
            {
                Debug.Log("ジャンプ不可（空中）");
            }
        }

        // 攻撃
        if (Input.GetMouseButtonDown(0))
        {
            if (!isAttacking)
            {
                Attack();
            }
        }

        animator.SetFloat("Speed", Mathf.Abs(moveInput));
        animator.SetBool("IsGrounded", isGrounded);
    }

    // ジャンプの処理
    private void Jump()
    {
        rb.linearVelocity = new Vector2(rb.linearVelocity.x, 0f);
        rb.AddForce(Vector2.up * jumpForce, ForceMode2D.Impulse);
    }

    // 攻撃
    private void Attack()
    {
        Debug.Log("左クリック入力");
        isAttacking = true;

        Debug.Log("Attackアニメーション開始");
        animator.SetTrigger("Attack");
    }

    // 攻撃終了
    public void EndAttack()
    {
        Debug.Log("Attackアニメーション終了");
        isAttacking = false;
    }

    // 接地面判定の範囲描画
    private void OnDrawGizmos()
    {
        if (groundCheck == null) return;

        Gizmos.color = Color.green;
        Gizmos.DrawWireSphere(groundCheck.position, groundRadius);
    }

    // 移動の処理
    private void FixedUpdate()
    {
        if (isAttacking)
        {
            rb.linearVelocity = new Vector2(0, rb.linearVelocity.y);
            return;
        }

        rb.linearVelocity = new Vector2(moveInput * moveSpeed, rb.linearVelocity.y);
    }
}