using UnityEngine;

public class PlayerController : MonoBehaviour
{
    [SerializeField] private float moveSpeed = 5f;
    [SerializeField] private float jumpForce = 20f;

    [SerializeField] private Transform groundCheck;
    [SerializeField] private float groundRadius = 0.1f;
    [SerializeField] private LayerMask groundLayer;

    private Rigidbody2D rb;
    private float moveInput;
    private bool isGrounded;
    private bool wasGrounded;

    private void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
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
    }

    // 移動の処理
    private void FixedUpdate()
    {
        rb.linearVelocity = new Vector2(moveInput * moveSpeed, rb.linearVelocity.y);
    }

    // ジャンプの処理
    private void Jump()
    {
        rb.AddForce(Vector2.up * jumpForce, ForceMode2D.Impulse);
    }

    // 接地面判定の範囲描画
    private void OnDrawGizmos()
    {
        if (groundCheck == null) return;

        Gizmos.color = Color.green;
        Gizmos.DrawWireSphere(groundCheck.position, groundRadius);
    }
}