using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EventHandler : MonoBehaviour
{
    private void OnEnable()
    {
        FindObjectOfType<PlayerStats>().PlayerDeathEvent += ActivatePanel;
        FindObjectOfType<PlayerCharacter>().OnWin += ActivateWinPanel;
    }
    private void OnDisable()
    {
        FindObjectOfType<PlayerStats>().PlayerDeathEvent -= ActivatePanel;
        FindObjectOfType<PlayerCharacter>().OnWin -= ActivateWinPanel;
    }

    void ActivatePanel()
    {
        transform.GetChild(0).gameObject.SetActive(true);
    }
    void ActivateWinPanel()
    {
        transform.GetChild(1).gameObject.SetActive(true);
    }
}
