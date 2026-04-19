using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Game/EnemyDatabase")]
public class EnemyDatabase : ScriptableObject
{
    [SerializeField] private EnemyData[] datas;

    private Dictionary<int, EnemyData> map;

    public void Init()
    {
        map = new Dictionary<int, EnemyData>();

        foreach (var data in datas)
        {
            map[data.enemyID] = data;
        }
    }

    public EnemyData Get(int id)
    {
        if (map == null)
            Init();

        if (map.TryGetValue(id, out var data))
            return data;

        Debug.LogError("EnemyData not found: " + id);
        return null;
    }
}