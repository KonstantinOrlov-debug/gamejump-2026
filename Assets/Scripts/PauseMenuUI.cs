using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UI;

public class PauseMenuUI : MonoBehaviour
{
    public GameObject pausePanel;         // already set in scene: Menu GameObject
    public GameObject settingsSubPanel;   // SetingsPanel
    public Slider masterSlider;
    public Slider musicSlider;
    public Slider sfxSlider;

    private PlayerInput _input;
    private bool _isPaused = false;

    void Awake()
    {
        _input = new PlayerInput();
    }

    void OnEnable()
    {
        _input.Main.Pause.performed += OnPausePressed;
        _input.Main.Enable();
    }

    void OnDisable()
    {
        _input.Main.Pause.performed -= OnPausePressed;
        _input.Main.Disable();
    }

    private void OnPausePressed(InputAction.CallbackContext ctx)
    {
        if (_isPaused) OnResumeButton();
        else OpenPauseMenu();
    }

    private void OpenPauseMenu()
    {
        _isPaused = true;
        pausePanel.SetActive(true);
        settingsSubPanel.SetActive(false);
        Time.timeScale = 0f;
        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;
    }

    public void OnResumeButton()
    {
        _isPaused = false;
        pausePanel.SetActive(false);
        Time.timeScale = 1f;
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
    }

    public void OnSettingsButton()
    {
        settingsSubPanel.SetActive(true);
    }

    public void OnBackSettingsButton()
    {
        settingsSubPanel.SetActive(false);
    }

    public void OnMainMenuButton()
    {
        Time.timeScale = 1f;
        // Replace with however your GameManager loads scenes:
        UnityEngine.SceneManagement.SceneManager.LoadScene("MainMenu");
    }

    public void OnQuitButton()
    {
        Time.timeScale = 1f;
        Application.Quit();
    }
}