using System.Collections.Generic;

public class SkillTreeModel
{
    private HashSet<string> unlockedSkills = new();
    private int skillPoint = 5;

    public System.Action OnChanged;

    public bool CanUnlock(SkillData skill)
    {
        if (unlockedSkills.Contains(skill.id)) return false;

        foreach (var req in skill.requires)
        {
            if (!unlockedSkills.Contains(req.id))
                return false;
        }

        if (skillPoint < skill.cost) return false;

        return true;
    }

    public bool Unlock(SkillData skill)
    {
        if (!CanUnlock(skill)) return false;

        unlockedSkills.Add(skill.id);
        skillPoint -= skill.cost;
        OnChanged?.Invoke();

        return true;
    }

    public bool IsUnlocked(string id)
    {
        return unlockedSkills.Contains(id);
    }

    public SkillNodeState GetState(SkillData skill)
    {
        if (unlockedSkills.Contains(skill.id))
            return SkillNodeState.Unlocked;

        foreach (var req in skill.requires)
        {
            if (!unlockedSkills.Contains(req.id))
                return SkillNodeState.Locked;
        }

        if (skillPoint < skill.cost)
            return SkillNodeState.Locked;

        return SkillNodeState.Available;
    }
}