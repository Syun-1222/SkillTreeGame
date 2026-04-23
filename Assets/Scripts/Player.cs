using UnityEngine;

public class Player : MonoBehaviour
{
    public static Player Instance;
    [SerializeField] private int maxHp = 100;
    [SerializeField] private int attackPower = 100; // Playerの攻撃力

    private int currentHp;
    public int AttackPower => attackPower;
    public int skillPoint;

    private void Awake()
    {
        Instance = this;
        currentHp = maxHp;
    }

    // --------------------
    // ダメージ処理
    // --------------------
    public void TakeDamage(int damage)
    {
        currentHp -= damage;
        Debug.Log("Player HP: " + currentHp);

        if (currentHp <= 0)
        {
            Die();
        }
    }

    // --------------------
    // 死亡処理
    // --------------------
    private void Die()
    {
        Debug.Log("Player Dead");

        GameManager.Instance.GameOver();
    }

    // スキルポイントの加算処理
    public void AddSkillPoint(int amount)
    {
        skillPoint += amount;
        Debug.Log(skillPoint);
    }
}
