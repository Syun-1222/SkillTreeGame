using UnityEngine;

public class Unit : MonoBehaviour
{
    public int maxHp;       // 最大HP
    public int currentHp;   // 現在HP

    // ゲーム開始時
    protected virtual void Start()
    {
        currentHp = maxHp;
    }
    
    // ダメージを受けたらHPを減らす
    public virtual void TakeDamage(int damage)
    {
        currentHp -= damage;

        if(currentHp <= 0)
        {
            Die();
        }
    }

    // 死亡処理
    protected virtual void Die()
    {
        Destroy(gameObject);
    }
}
