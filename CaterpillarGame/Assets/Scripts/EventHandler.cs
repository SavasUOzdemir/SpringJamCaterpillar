using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EventHandler : MonoBehaviour
{
    private void OnEnable()
    {
        try
        {
        FindObjectOfType<PlayerStats>().PlayerDeathEvent += ActivatePanel;
        FindObjectOfType<PlayerCharacter>().OnWin += ActivateWinPanel;

        }
        catch (System.Exception)
        {

        }
    }
    private void OnDisable()
    {
        try
        {
        FindObjectOfType<PlayerStats>().PlayerDeathEvent -= ActivatePanel;
        FindObjectOfType<PlayerCharacter>().OnWin -= ActivateWinPanel;

        }
        catch (System.Exception)
        {

        }
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
