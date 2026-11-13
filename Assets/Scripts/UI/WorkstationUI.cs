using UnityEngine;
using UnityEngine.UI;
using TMPro;
using BASimulator.Mission;

public class WorkstationUI : MonoBehaviour
{
    [Header("Main Panels")]
    public GameObject workstationPanel;
    public GameObject desktopPanel;
    public GameObject outlookPanel;
    public GameObject jiraPanel;
    public GameObject confluencePanel;

    [Header("App Managers")]
    public OutlookManager outlookManager;

    [Header("Confluence Components")]
    public TMP_InputField documentEditor;

    public bool IsOpen => workstationPanel != null && workstationPanel.activeSelf;

    void Start()
    {
        if (workstationPanel != null) workstationPanel.SetActive(false);
    }

    public void OpenWorkstation()
    {
        if (workstationPanel == null) return;

        workstationPanel.SetActive(true);
        ShowPanel(desktopPanel);

        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;

        if (MissionManager.Instance != null)
            MissionManager.Instance.CompleteObjective(0);
    }

    public void CloseWorkstation()
    {
        if (workstationPanel != null) workstationPanel.SetActive(false);

        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
    }

    public void ShowPanel(GameObject panelToShow)
    {
        if (desktopPanel != null) desktopPanel.SetActive(desktopPanel == panelToShow);
        if (outlookPanel != null) outlookPanel.SetActive(outlookPanel == panelToShow);
        if (jiraPanel != null) jiraPanel.SetActive(jiraPanel == panelToShow);
        if (confluencePanel != null) confluencePanel.SetActive(confluencePanel == panelToShow);
    }

    public void OpenOutlook()
    {
        ShowPanel(outlookPanel);
        if (outlookManager != null) outlookManager.RefreshEmailList();

        if (MissionManager.Instance != null)
            MissionManager.Instance.CompleteObjective(1);
    }

    public void OpenJira() => ShowPanel(jiraPanel);
    public void OpenConfluence() => ShowPanel(confluencePanel);
    public void OpenDesktop() => ShowPanel(desktopPanel);

    public void SaveDocument()
    {
        Debug.Log("Document Saved: " + documentEditor.text);
    }

    public void SubmitDocument()
    {
        Debug.Log("Document Submitted: " + documentEditor.text);
    }
}