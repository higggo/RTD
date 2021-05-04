using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Pause : MonoBehaviour
{
    bool bPause = false;
    public void PauseGame()
    {
        if(!bPause)
        {
            Time.timeScale = 0;
            bPause = true;
        }
        else
        {
            Time.timeScale = 1;
            bPause = false;
        }
    }
    private void Start()
    {
        gameObject.GetComponent<Button>().onClick.AddListener(PauseGame);
    }
}
