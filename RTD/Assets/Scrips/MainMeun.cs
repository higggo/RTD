using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMeun : MonoBehaviour
{
    public void ChangefirstScene()
    {
        SceneManager.LoadScene("Main");
    }

    public void changeSecondScen()
    {
        SceneManager.LoadScene("Play");
    }

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void OnClickNewGame()
    {

    }

    public void OnClickLoad()
    {


    }

    public void OnClickOption()
    {


    }

    public void OnClickQuit()
    {
#if UNITY_EDITOR
        UnityEditor.EditorApplication.isPlaying = false;
#else
        Application.Quit();
#endif
    }
}
