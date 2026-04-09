using UnityEngine;
using System.Collections.Generic;

[CreateAssetMenu(menuName = "SkillTree/SkillData")]
public class SkillData : ScriptableObject
{
    [Header("基本情報")]
    public string id;
    public string skillName; 
    public string description;
    public Sprite icon;

    [Header("解放条件")]
    public List<SkillData> requires; //前提解放Skill

    [Header("コスト")]
    public int cost = 1;
}

