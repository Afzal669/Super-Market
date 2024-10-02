using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UIController : MonoBehaviour
{
    public void OnExitButtonClick()
    {
        // If we are running in the Unity Editor, just stop playing
#if UNITY_EDITOR
        UnityEditor.EditorApplication.isPlaying = false;
#else
        // If running in a build, quit the application
        Application.Quit();
#endif
    }

    public void OnPauseButtonclick()
    {
        Time.timeScale = 0;
    }
    public void OnResumeButtonclick()
    {
        Time.timeScale = 1;
    }
}
