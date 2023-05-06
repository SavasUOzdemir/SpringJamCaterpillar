using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class OnSceneLoaded : MonoBehaviour
{
    bool _checkDone = false;
    [SerializeField]AudioSource _audioSource;

    private void Update()
    {
        if (!_checkDone)
            OnSceneLoadedMethod(gameObject.scene,LoadSceneMode.Single);
    }

    private void Start()
    {
        _audioSource.PlayDelayed(1.5f);
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
