using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class OnSceneLoaded : MonoBehaviour
{
    bool _checkDone = false;
    [SerializeField] AudioSource _gothMothSound;
    [SerializeField] AudioSource _music;

    private void Update()
    {
        if (!_checkDone)
            OnSceneLoadedMethod(gameObject.scene,LoadSceneMode.Single);
    }

    private void Start()
    {
        _gothMothSound.PlayDelayed(.7f);
        _music.PlayDelayed(2f);

        Cursor.lockState = CursorLockMode.Confined;
    }

    private void OnEnable()
    {
        SceneManager.sceneLoaded += OnSceneLoadedMethod;
    }

    private void OnDisable()
    {
        SceneManager.sceneLoaded -= OnSceneLoadedMethod;
    }

    private void OnSceneLoadedMethod(Scene scene, LoadSceneMode mode)
    {
        gameObject.transform.GetChild(0).gameObject.SetActive(true);

        _checkDone = true;
    }
}
