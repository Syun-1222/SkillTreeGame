using UnityEngine;
using System.Collections.Generic;

public class SkillTreeView : MonoBehaviour
{
    [SerializeField] private SkillDatabase database;
    [SerializeField] private Transform nodesRoot;
    [SerializeField] private SkillNodeView nodePrefab;
    [SerializeField] private RectTransform lineRoot;
    [SerializeField] private SkillLineView linePrefab;


    private SkillTreeModel model;
    private List<SkillNodeView> views = new();
    private List<SkillLineView> lines = new();

    private void Start()
    {
        model = new SkillTreeModel();
        model.OnChanged += RefreshAll;// 状態変化時にUI更新を呼ぶ

        // UI生成
        Generate();
        GenerateLines();
    }

    // SkillNode生成
    private void Generate()
    {
        foreach (var node in database.nodes)
        {
            //prefab生成
            var view = Instantiate(nodePrefab, nodesRoot);

            view.Init(model, this);
            view.Setup(node.skill, node.iconOffset);

            var rt = view.GetComponent<RectTransform>();
            rt.anchoredPosition = node.position;

            views.Add(view);
        }
    }

    // SkillLine生成
    private void GenerateLines()
    {
        foreach (var node in database.nodes)
        {
            foreach (var req in node.skill.requires)
            {
                var from = views.Find(v => v.GetSkill().id == req.id);
                var to = views.Find(v => v.GetSkill().id == node.skill.id);

                if (from == null || to == null) continue;

                var line = Instantiate(linePrefab, lineRoot);

                line.Init(from, to, model);
                line.UpdateLine();

                lines.Add(line);
            }
        }
    }

    // 状態変更の更新
    public void RefreshAll()
    {
        foreach (var v in views)
        {
            v.UpdateState();
        }

        foreach (var l in lines)
        {
            l.UpdateLine();
        }
    }
} 

