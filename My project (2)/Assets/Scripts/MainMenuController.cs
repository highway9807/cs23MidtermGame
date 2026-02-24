using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class MainMenuController : MonoBehaviour
{
    [Header("Scene Names")]
    [SerializeField] private string mainHubSceneName = "Main_Hub";
    [Header("Buttons")]
    [SerializeField] private Button newGameButton;
    [SerializeField] private Button quitButton;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        // make sure there are no active pauses in the main menu
        PauseManager.ClearAllPauses();
        Time.timeScale = 1f;
         // Hook up button click events.
        if (newGameButton != null) 
            newGameButton.onClick.AddListener(OnNewGamePressed);

        if (quitButton != null)
            quitButton.onClick.AddListener(OnQuitPressed);

    }

    public void OnNewGamePressed()
    {
        PauseManager.ClearAllPauses();
        Time.timeScale = 1f;
        SceneManager.LoadScene(mainHubSceneName);
    }

    public void OnQuitPressed()
    {
        Application.Quit();
        Debug.Log("Quit pressed.");
    }

}
