using UnityEngine;
using TMPro;

public class MissionManager : MonoBehaviour
{
    public static MissionManager Instance;

    [Header("Tham chiếu hệ thống")]
    public ExperienceManager experienceManager;
    public TextMeshProUGUI missionText;

    [Header("Thông tin nhiệm vụ BA001")]
    public string missionTitle = "BA001: Check Your Email";
    public int rewardEXP = 20;

    private string[] objectives = new string[]
    {
        "Open workstation",
        "Open Outlook",
        "Read client email"
    };

    private bool[] completedObjectives = new bool[3];
    private bool isMissionCompleted = false;

    void Awake()
    {
        Instance = this;
    }

    void Start()
    {
        UpdateMissionUI();
    }

    public void CompleteObjective(int index)
    {
        if (isMissionCompleted || index < 0 || index >= completedObjectives.Length) return;
        if (completedObjectives[index]) return;

        completedObjectives[index] = true;
        Debug.Log($"Hoàn thành mục tiêu {index}: {objectives[index]}");

        CheckMissionCompletion();
        UpdateMissionUI();
    }

    void CheckMissionCompletion()
    {
        for (int i = 0; i < completedObjectives.Length; i++)
        {
            if (!completedObjectives[i]) return;
        }

        isMissionCompleted = true;
        if (experienceManager != null)
        {
            experienceManager.AddEXP(rewardEXP);
        }
    }

    void UpdateMissionUI()
    {
        if (missionText == null) return;

        if (isMissionCompleted)
        {
            missionText.text = $"<color=#2E7D32>[Completed] {missionTitle} (+{rewardEXP} EXP)</color>";
            return;
        }

        string status = $"<b>{missionTitle}</b>\n";
        for (int i = 0; i < objectives.Length; i++)
        {
            string mark = completedObjectives[i] ? "[x]" : "[ ]";
            status += $"{mark} {objectives[i]}\n";
        }
        missionText.text = status;
    }
}