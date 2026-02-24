using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class PauseMenuController : MonoBehaviour
{
    [Header("Scene Name(s)")]
    [SerializeField] private string mainMenuSceneName = "Main_Menu";

    [Header("Wiring")]
    [SerializeField] private GameObject pauseRoot;   // pause menu panel
    [SerializeField] private Button resumeButton;
    [SerializeField] private Button mainMenuButton;

    // the reason we would pause from this file (always menu pause)
    private const PauseReason PauseCode = PauseReason.PauseMenu;

    private bool isOpen = false;

    private void SetOpen(bool open)
    {
        isOpen = open;

        if (pauseRoot != null) pauseRoot.SetActive(isOpen);

        if (isOpen) PauseManager.RequestPause(PauseCode);
        else PauseManager.ReleasePause(PauseCode);
    }

    void Start()
    {
        // Start closed and ensure we are not paused.
        SetOpen(false);

        if (resumeButton != null)
            resumeButton.onClick.AddListener(OnResumePressed);

        if (mainMenuButton != null)
            mainMenuButton.onClick.AddListener(OnMainMenuPressed);
        
    }

    private void OnResumePressed()
    {
        SetOpen(false);
    }

    private void OnMainMenuPressed()
    {
        // clear pauses again before going to the menu
        PauseManager.ClearAllPauses();
        Time.timeScale = 1f;

        SceneManager.LoadScene(mainMenuSceneName);
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            SetOpen(!isOpen);
        }
    }
    private void OnDestroy()
    {
        // safety: do not leave the game paused if this object is destroyed mid-pause.
        PauseManager.ReleasePause(PauseReason.PauseMenu);
    }
}
