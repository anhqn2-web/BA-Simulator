using UnityEngine;

public class ComputerInteractable : MonoBehaviour, IInteractable
{
    public WorkstationUI workstationUI;

    public string GetPrompt()
    {
        return "Press [E] to use Computer";
    }

    public void Interact()
    {
        if (workstationUI != null)
        {
            workstationUI.OpenWorkstation();
        }
    }
}