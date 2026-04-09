using UnityEngine;
using UnityEngine.UI;

public class SkillLineView : MonoBehaviour
{
    [SerializeField] private Image image;

    [Header("Colors")]
    [SerializeField] private Color lockedColor = Color.black;   // 未開放
    [SerializeField] private Color availableColor = Color.white; // 解放可能
    [SerializeField] private Color unlockedColor = Color.cyan;   // 解放済み

    private RectTransform rect;

    private SkillNodeView from;
    private SkillNodeView to;
    private SkillTreeModel model;

    public void Init(SkillNodeView fromNode, SkillNodeView toNode, SkillTreeModel m)
    {
        from = fromNode;
        to = toNode;
        model = m;

        rect = GetComponent<RectTransform>();
    }

    public void UpdateLine()
    {
        Vector3 a = from.GetComponent<RectTransform>().position;
        Vector3 b = to.GetComponent<RectTransform>().position;

        Vector3 dir = b - a;
        float dist = dir.magnitude;

        rect.sizeDelta = new Vector2(dist, 3f);
        rect.pivot = new Vector2(0, 0.5f);

        rect.position = a;

        float angle = Mathf.Atan2(dir.y, dir.x) * Mathf.Rad2Deg;
        rect.rotation = Quaternion.Euler(0, 0, angle);

        UpdateColor();
    }

    private void UpdateColor()
    {
        var fromState = model.GetState(from.GetSkill());
        var toState   = model.GetState(to.GetSkill());

        // ■未開放（どちらか未開放なら黒）
        if (fromState == SkillNodeState.Locked || toState == SkillNodeState.Locked)
        {
            image.color = lockedColor;
            return;
        }

        // ■解放済み（終点が解放済み）
        if (toState == SkillNodeState.Unlocked)
        {
            image.color = unlockedColor;
            return;
        }

        // ■解放可能
        image.color = availableColor;
    }
}