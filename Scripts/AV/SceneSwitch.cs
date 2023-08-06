using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.Timeline;

// Testing the ITimeControl Interface
public class SceneSwitch : MonoBehaviour, ITimeControl
{
    public void OnControlTimeStart()
    {
        throw new System.NotImplementedException();
    }

    public void OnControlTimeStop()
    {
        throw new System.NotImplementedException();
    }

    public void SetTime(double time)
    {
        throw new System.NotImplementedException();
    }

    // Switch that scene
    public void SwitchScene(string sceneName)
    {
        SceneManager.LoadScene(sceneName);
    }
}
