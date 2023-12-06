using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Menu : MonoBehaviour
{
    bool isPaused;
    [SerializeField] private GameObject mainMenu;
    [SerializeField] private GameObject _pauseMenu;
    [SerializeField] private GameObject gameOverScreen;


    public void StartGame()
    {
        SceneManager.LoadScene("MainMenu");
    }
    private void Awake()
    {
        gameOverScreen.SetActive(false);
    }

    public void GameOver()
    {
        //gameOverScreen.SetActive(true);
    }

    public void PlayGame()
    {
        SceneManager.LoadScene(1);
    }

    public void LeaderBoard()
    {
        SceneManager.LoadScene(4);
    }

    public void Restart()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }

    public void MainMenu()
    {
        SceneManager.LoadScene(0);
    }

    void Update()
    {

        if (Input.GetKeyDown(KeyCode.Escape))
        {
            isPaused = !isPaused;
            Time.timeScale = isPaused ? 0 : 1;
            _pauseMenu.SetActive(isPaused);
        }
    }

    public void Quit()
    {
        Application.Quit();

#if UNITY_EDITOR
        UnityEditor.EditorApplication.isPlaying = false;
#endif
    }

}
