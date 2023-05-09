using UnityEngine;
using UnityEngine.SceneManagement;

public class ButtonsScript : MonoBehaviour
{
    [SerializeField]GameObject _panelGameObject;


    private void Start()
    {
        if (_panelGameObject == null)
            return;
        if (_panelGameObject.activeSelf)
            _panelGameObject.SetActive(false);
    }

    public void PlayButtonPress()
    {
        SceneManager.LoadScene(1);
    }

    public void QuitButtonPress()
    {
        Application.Quit();
    }

    public void ResumeButtonPressed()
    {
        Time.timeScale = 1;
        _panelGameObject.SetActive(false);
    }

    public void MainMenuButton()
    {
        Time.timeScale = 1;
        SceneManager.LoadScene(0);
    }

    public void PauseMenuHandler()
    {
        if (SceneManager.GetActiveScene().buildIndex!=1)
        {
            return;
        }
        if (!_panelGameObject.activeSelf)
        {
            _panelGameObject.SetActive(true);
            Time.timeScale = 0;
        }
        else
        {
            _panelGameObject.SetActive(false);
            Time.timeScale = 1;
        }
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
            PauseMenuHandler();
    }
}
