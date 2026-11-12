using UnityEngine;

public class WorkstationUI : MonoBehaviour
{
    [Header("Panels trong Workstation")]
    public GameObject workstationPanel;
    public GameObject desktopPanel;
    public GameObject outlookPanel;
    public GameObject emailContentPanel;

    [Header("Tham chiếu Mission")]
    public MissionManager missionManager;

    public bool IsOpen => workstationPanel != null && workstationPanel.activeSelf;

    void Start()
    {
        if (workstationPanel != null) workstationPanel.SetActive(false);
    }

    public void OpenWorkstation()
    {
        if (workstationPanel == null) return;

        workstationPanel.SetActive(true);
        if (desktopPanel != null) desktopPanel.SetActive(true);
        if (outlookPanel != null) outlookPanel.SetActive(false);
        if (emailContentPanel != null) emailContentPanel.SetActive(false);

        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;

        if (missionManager != null) missionManager.CompleteObjective(0);
    }

    public void CloseWorkstation()
    {
        if (workstationPanel != null) workstationPanel.SetActive(false);

        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
    }

    public void OpenOutlook()
    {
        if (outlookPanel != null) outlookPanel.SetActive(true);
        if (missionManager != null) missionManager.CompleteObjective(1);
    }

    public void ReadClientEmail()
    {
        if (emailContentPanel != null) emailContentPanel.SetActive(true);
        if (missionManager != null) missionManager.CompleteObjective(2);
    }
}