using UnityEngine;

public class UIController : MonoBehaviour
{
    [SerializeField] private GameObject rootPanel;
    [SerializeField] private GameObject skillTreePanel;
    [SerializeField] private GameObject statusPanel;
    [SerializeField] private GameObject allSkillPanel;

    private GameObject currentPanel;

    private void Start()
    {
        CloseAll();
    }

    private void Update()
    {
        // 全部閉じる
        if (Input.GetKeyDown(KeyCode.Tab))
        {
            // 何か開いているなら全部閉じる
            if (currentPanel != null || allSkillPanel.activeSelf)
            {
                CloseAll();
            }
            else
            {
                // 何も開いてない時だけStatusを開く
                Open(statusPanel);
            }
        }

        // RでSkillTreeを開く
        if (Input.GetKeyDown(KeyCode.R))
        {
            Open(skillTreePanel);
        }

        // SkillTree画面中にEでStatus画面を開く
        if (currentPanel == skillTreePanel)
        {
            if (Input.GetKeyDown(KeyCode.E))
            {
                Open(statusPanel);
            }
        }
    }

    //========================
    // 基本操作
    //========================

    public void Toggle(GameObject panel)
    {
        if (currentPanel == panel)
        {
            CloseAll();
        }
        else
        {
            Open(panel);
        }
    }

    public void Open(GameObject panel)
    {
        rootPanel.SetActive(true);

        // 全部閉じる
        skillTreePanel.SetActive(false);
        statusPanel.SetActive(false);
        allSkillPanel.SetActive(false);

        // 開く
        panel.SetActive(true);

        currentPanel = panel;
    }

    public void OpenAllSkillOverlay()
    {
        rootPanel.SetActive(true);
        allSkillPanel.SetActive(true);
    }

    public void CloseAllSkill()
    {
        allSkillPanel.SetActive(false);
    }

    public void CloseAll()
    {
        rootPanel.SetActive(false);

        skillTreePanel.SetActive(false);
        statusPanel.SetActive(false);
        allSkillPanel.SetActive(false);

        currentPanel = null;
    }

    //========================
    // ボタン用
    //========================

    public void OpenSkillTree() => Open(skillTreePanel);
    public void OpenStatus() => Open(statusPanel);
    public void OpenAllSkill() => Open(allSkillPanel);
    public void CloseAllSkillButton() => CloseAllSkill();
}