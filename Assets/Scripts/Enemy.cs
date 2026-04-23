using UnityEngine;

public class Enemy : Unit
{
    [SerializeField] private int enemyID;
    [SerializeField] private EnemyDatabase database;

    private EnemyData data;

    // 敵IDからHP情報取得
    protected override void Start()
    {
        data = database.Get(enemyID);

        maxHp = data.maxHp;
        base.Start();
    }

    public override void TakeDamage(int damage)
    {
        base.TakeDamage(damage);
    }

    // 敵が死亡した場合、SkillPointを渡す
    protected override void Die()
    {
        Player.Instance.AddSkillPoint(data.skillPoint);
        base.Die();
    }
}