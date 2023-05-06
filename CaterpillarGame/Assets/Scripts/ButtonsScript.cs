using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class ButtonsScript : MonoBehaviour
{
    GameObject _panelGameObject;
    private void Awake()
    {
        _panelGameObject = FindObjectOfType<Canvas>().transform.GetChild(0).gameObject;
    }

    private void Start()
    {
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
