using UnityEngine;

public class Player : MonoBehaviour
{
    public static Player Instance;
    [SerializeField] private int attackPower = 10; // Playerの攻撃力

    public int AttackPower => attackPower;
    public int skillPoint;

    private void Awake()
    {
        Instance = this;
    }

    // スキルポイントの加算処理
    public void AddSkillPoint(int amount)
    {
        skillPoint += amount;
        Debug.Log(skillPoint);
    }
}
