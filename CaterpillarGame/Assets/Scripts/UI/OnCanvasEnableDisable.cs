using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OnCanvasEnableDisable : MonoBehaviour 
{
    static bool _gamePaused = false;
    public static bool _GamePaused { get => _gamePaused; }

    private void OnEnable()
    {
        _gamePaused = true;
    }

    private void OnDisable()
    {
        _gamePaused = false;
    }
}
