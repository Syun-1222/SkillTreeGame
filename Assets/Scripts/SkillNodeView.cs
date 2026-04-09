using UnityEngine;
using UnityEngine.UI;

public class SkillNodeView : MonoBehaviour
{
    [SerializeField] private Image frame;
    [SerializeField] private Button button;
    [SerializeField] private Image icon;
    [SerializeField] private Image glow;

    [SerializeField] private Color normalColor = Color.white; // 通常色
    [SerializeField] private Color grayColor = Color.gray;    // 無効色

    private SkillData skill;     // 表示しているSkill
    private SkillTreeModel model;// Node状態
    private SkillTreeView view;  // UI更新

    // SkillID取得
    public string SkillId => skill != null ? skill.id : string.Empty;

    // 初期化
    public void Init(SkillTreeModel m, SkillTreeView v)
    {
        model = m;
        view = v;

        if (button != null)
            button.interactable = true;
    }

    private void Awake()
    {
        if (button == null)
            button = GetComponent<Button>();

        button.interactable = true;
    }

    // データ呼び出し
    public void Setup(SkillData data, Vector2 iconOffset)
    {
        skill = data;
        icon.sprite = data.icon;                          // アイコン画像を差し替え
        icon.rectTransform.anchoredPosition = iconOffset; // アイコン位置調整
        button.onClick.RemoveAllListeners();              // イベントの設定 
        button.onClick.AddListener(OnClick);              // 初期状態更新

        UpdateState();
    }

    // 状態更新
    public void UpdateState()
    {
        if (model == null || skill == null) return;

        var state = model.GetState(skill);

        switch (state)
        {   
            // 未開放
            case SkillNodeState.Locked:
                SetFrame(true);
                SetIcon(true);
                glow.gameObject.SetActive(false);
                break;

            // 解放可能
            case SkillNodeState.Available:
                SetFrame(false);
                SetIcon(true);
                glow.gameObject.SetActive(false);
                break;

            // 解放済み
            case SkillNodeState.Unlocked:
                SetFrame(false);
                SetIcon(false);
                glow.gameObject.SetActive(true);
                break;
        }
    }

    // フレーム色変更
    private void SetFrame(bool gray)
    {
        if (frame == null) return;
        frame.color = gray ? grayColor : normalColor;
    }

    // アイコン色変更
    private void SetIcon(bool gray)
    {
        if (icon == null) return;
        icon.color = gray ? grayColor : normalColor;
    }

    // クリック処理
    private void OnClick()
    {
        if (model == null || skill == null) return;

        Debug.Log($"[SkillNodeView] {skill.id} clicked");

        // Skill解放処理
        if (model.Unlock(skill))
        {
            if (view != null)
                view.RefreshAll();
        }
    }

    // 位置取得
    public Vector2 GetPosition()
    {
        return ((RectTransform)transform).anchoredPosition;
    }

    public RectTransform GetRect()
    {
        return (RectTransform)transform;
    }

    public SkillData GetSkill()
    {
        return skill;
    }
}