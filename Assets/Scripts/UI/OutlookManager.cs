using UnityEngine;
using System.Collections.Generic;
using TMPro;
using UnityEngine.UI;
using BASimulator.Mission;

[System.Serializable]
public class EmailData
{
    public string emailId;
    public string senderName;
    public string senderEmail;
    public string subject;
    public string timestamp;
    [TextArea(5, 10)]
    public string body;
    public bool isClientEmail;
    public bool isRead;
}

public class OutlookManager : MonoBehaviour
{
    [Header("Email Data")]
    public List<EmailData> emails = new List<EmailData>();

    [Header("UI References - List")]
    public Transform emailListContainer;
    public GameObject emailItemPrefab;

    [Header("UI References - Content")]
    public TMP_Text contentSubject;
    public TMP_Text contentSender;
    public TMP_Text contentTimestamp;
    public TMP_Text contentBody;

    private WorkstationUI workstationUI;

    void Awake()
    {
        workstationUI = GetComponentInParent<WorkstationUI>();
        InitializeSampleEmails();
    }

    void Start()
    {
        RefreshEmailList();
    }

    private void InitializeSampleEmails()
    {
        if (emails.Count > 0) return;

        emails.Add(new EmailData 
        { 
            emailId = "001",
            senderName = "Mr. John",
            senderEmail = "john.client@globecorp.com",
            subject = "URGENT: New Project Requirements",
            timestamp = "09:15 AM",
            body = "Hi team,\n\nWe need to finalize the functional requirements for the new simulation module by EOD. Can you please review the attached draft and let me know if there are any gaps?\n\nBest regards,\nJohn",
            isClientEmail = true,
            isRead = false
        });

        emails.Add(new EmailData 
        { 
            emailId = "002",
            senderName = "Anna (PM)",
            senderEmail = "anna.pm@basim.com",
            subject = "Weekly Sync Meeting",
            timestamp = "Yesterday",
            body = "Hi everyone,\n\nJust a reminder that our weekly sync is tomorrow at 10 AM in Room 302. Please update your Jira tasks before the meeting.\n\nAnna",
            isClientEmail = false,
            isRead = false
        });

        emails.Add(new EmailData 
        { 
            emailId = "003",
            senderName = "David (Senior BA)",
            senderEmail = "david.ba@basim.com",
            subject = "Review: Process Flow Diagram",
            timestamp = "Monday",
            body = "Hey,\n\nI've left some comments on your process flow diagram in Draw.io. Let's discuss them once you've had a chance to look.\n\nDavid",
            isClientEmail = false,
            isRead = true
        });
    }

    public void RefreshEmailList()
    {
        foreach (Transform child in emailListContainer)
        {
            Destroy(child.gameObject);
        }

        foreach (var email in emails)
        {
            GameObject item = Instantiate(emailItemPrefab, emailListContainer);
            SetupEmailItem(item, email);
        }
    }

    private void SetupEmailItem(GameObject item, EmailData data)
    {
        TMP_Text[] texts = item.GetComponentsInChildren<TMP_Text>();
        // Assuming 0: Sender, 1: Subject
        if (texts.Length >= 2)
        {
            texts[0].text = data.senderName;
            texts[1].text = data.subject;
            
            // Visual distinction for unread
            texts[0].fontStyle = data.isRead ? FontStyles.Normal : FontStyles.Bold;
            texts[1].color = data.isRead ? Color.gray : Color.white;
        }

        Image bg = item.GetComponent<Image>();
        if (bg != null) bg.color = data.isRead ? new Color(0.2f, 0.2f, 0.2f) : new Color(0.3f, 0.3f, 0.3f);

        Button btn = item.GetComponent<Button>();
        if (btn != null)
        {
            btn.onClick.AddListener(() => OnEmailClicked(data));
        }
    }

    public void OnEmailClicked(EmailData data)
    {
        contentSubject.text = data.subject;
        contentSender.text = $"From: {data.senderName} <{data.senderEmail}>";
        contentTimestamp.text = data.timestamp;
        contentBody.text = data.body;

        if (!data.isRead)
        {
            data.isRead = true;
            RefreshEmailList();
        }

        if (data.isClientEmail && MissionManager.Instance != null)
        {
            MissionManager.Instance.CompleteObjective(2);
        }
    }

    [ContextMenu("Setup UI References")]
    public void SetupUIReferences()
    {
        // Search within children for specific names
        contentSubject = transform.Find("EmailContent/SubjectText")?.GetComponent<TMP_Text>();
        contentSender = transform.Find("EmailContent/SenderText")?.GetComponent<TMP_Text>();
        contentTimestamp = transform.Find("EmailContent/TimestampText")?.GetComponent<TMP_Text>();
        contentBody = transform.Find("EmailContent/BodyText")?.GetComponent<TMP_Text>();
        emailListContainer = transform.Find("EmailList/Scroll View/Viewport/Content");

        if (emailListContainer == null)
            emailListContainer = transform.Find("EmailList");

        Debug.Log("Outlook UI References hooked up!");
    }
}