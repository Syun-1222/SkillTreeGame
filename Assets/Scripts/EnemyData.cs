using UnityEngine;

[CreateAssetMenu(menuName = "Game/EnemyData")]
public class EnemyData : ScriptableObject
{
    public int enemyID;
    public int maxHp;
    public int skillPoint;
}