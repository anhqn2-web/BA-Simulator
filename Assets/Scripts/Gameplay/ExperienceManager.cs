using UnityEngine;
using TMPro;

public class ExperienceManager : MonoBehaviour
{
    [Header("Hồ sơ người chơi")]
    public string playerName = "Alex Nguyen";
    public int currentEXP = 0;
    public int currentLevel = 1;
    public string currentTitle = "Junior BA";

    [Header("UI - Profile Popup (Góc trên trái)")]
    public GameObject profilePopupPanel;
    public TextMeshProUGUI rankText;
    public TextMeshProUGUI expDetailText;

    [Header("UI - Pause Menu (Dưới nhân vật 3D)")]
    public TextMeshProUGUI pauseStatsText;

    void Start()
    {
        // Lấy tên người chơi nếu đã nhập từ màn MainMenu (mặc định dùng playerName)
        playerName = PlayerPrefs.GetString("PlayerName", playerName);

        if (profilePopupPanel != null) profilePopupPanel.SetActive(false);
        CalculateRank();
        UpdateUI();
    }

    public void AddEXP(int amount)
    {
        currentEXP += amount;
        CalculateRank();
        UpdateUI();
    }

    public void ToggleProfilePopup()
    {
        if (profilePopupPanel == null) return;

        bool nextState = !profilePopupPanel.activeSelf;
        profilePopupPanel.SetActive(nextState);

        if (nextState) UpdateUI();
    }

    void CalculateRank()
    {
        if (currentEXP >= 10000) { currentLevel = 6; currentTitle = "Lead BA"; }
        else if (currentEXP >= 6000) { currentLevel = 5; currentTitle = "Senior BA"; }
        else if (currentEXP >= 3000) { currentLevel = 4; currentTitle = "Business Analyst"; }
        else if (currentEXP >= 1500) { currentLevel = 3; currentTitle = "Junior BA+"; }
        else if (currentEXP >= 500) { currentLevel = 2; currentTitle = "BA Intern Completed"; }
        else { currentLevel = 1; currentTitle = "Junior BA"; }
    }

    int GetNextLevelEXP()
    {
        if (currentEXP < 500) return 500;
        if (currentEXP < 1500) return 1500;
        if (currentEXP < 3000) return 3000;
        if (currentEXP < 6000) return 6000;
        if (currentEXP < 10000) return 10000;
        return currentEXP;
    }

    void UpdateUI()
    {
        int nextEXP = GetNextLevelEXP();
        string expString = currentLevel < 6 ? $"{currentEXP}/{nextEXP}" : $"{currentEXP} (MAX)";

        // 1. Cập nhật bảng Profile khi click Avatar
        if (rankText != null)
            rankText.text = $"<b>{playerName}</b> — {currentTitle} (Lv.{currentLevel})";

        if (expDetailText != null)
            expDetailText.text = $"EXP tích lũy: <b>{expString}</b>";

        // 2. Cập nhật dòng chữ dưới nhân vật 3D trong Pause Menu
        if (pauseStatsText != null)
            pauseStatsText.text = $"{playerName} | {currentTitle} — EXP {expString}";
    }
}