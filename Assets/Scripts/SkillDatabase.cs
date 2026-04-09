using UnityEngine;
using System.Collections.Generic;
using System.Linq;

[CreateAssetMenu(menuName = "SkillTree/SkillDatabase")]
public class SkillDatabase : ScriptableObject
{
    public List<SkillNodeEntry> nodes = new();

    // IDからノード取得
    public SkillNodeEntry GetNode(string id)
    {
        return nodes.FirstOrDefault(n => n.skill != null && n.skill.id == id);
    }

    // スキル取得
    public SkillData GetSkill(string id)
    {
        var node = GetNode(id);
        return node != null ? node.skill : null;
    }

    // 存在チェック
    public bool Contains(string id)
    {
        return nodes.Any(n => n.skill != null && n.skill.id == id);
    }
}
