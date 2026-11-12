using UnityEngine;
using UnityEngine.SceneManagement;

public class UIManager : MonoBehaviour
{
    [Header("Giao diện UI")]
    public GameObject hudUI;
    public GameObject pauseMenuUI;
    public WorkstationUI workstationUI;

    public bool isUIOpen => (pauseMenuUI != null && pauseMenuUI.activeSelf) || 
                            (workstationUI != null && workstationUI.IsOpen);

    void Start()
    {
        if (hudUI != null) hudUI.SetActive(true);
        if (pauseMenuUI != null) pauseMenuUI.SetActive(false);

        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
        Time.timeScale = 1f;
    }

    void Update()
    {
        if (!isUIOpen)
        {
            if (Input.GetKeyDown(KeyCode.LeftAlt))
            {
                Cursor.lockState = CursorLockMode.None;
                Cursor.visible = true;
            }
            else if (Input.GetKeyUp(KeyCode.LeftAlt))
            {
                Cursor.lockState = CursorLockMode.Locked;
                Cursor.visible = false;
            }
        }

        if (Input.GetKeyDown(KeyCode.Escape))
        {
            if (workstationUI != null && workstationUI.IsOpen)
            {
                workstationUI.CloseWorkstation();
                return;
            }

            TogglePauseMenu();
        }
    }

    public void TogglePauseMenu()
    {
        if (pauseMenuUI == null) return;

        bool nextState = !pauseMenuUI.activeSelf;
        pauseMenuUI.SetActive(nextState);
        
        // Ẩn HUD khi mở Pause Menu và hiện lại khi đóng
        if (hudUI != null) hudUI.SetActive(!nextState);

        if (nextState)
        {
            Cursor.lockState = CursorLockMode.None;
            Cursor.visible = true;
            Time.timeScale = 0f;
        }
        else
        {
            Cursor.lockState = CursorLockMode.Locked;
            Cursor.visible = false;
            Time.timeScale = 1f;
        }
    }

    public void ResumeGame()
    {
        if (pauseMenuUI != null) pauseMenuUI.SetActive(false);
        if (hudUI != null) hudUI.SetActive(true);

        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
        Time.timeScale = 1f;
    }

    public void RestartGame()
    {
        Time.timeScale = 1f;
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }

    public void LoadMainMenu()
    {
        Time.timeScale = 1f;
        SceneManager.LoadScene("MainMenu");
    }

    public void QuitGame()
    {
        Application.Quit();
#if UNITY_EDITOR
        UnityEditor.EditorApplication.isPlaying = false;
#endif
    }
}