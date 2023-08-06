using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneSkip : MonoBehaviour
{
    [SerializeField] 
    private string _sceneName;
    private Scene _currentSceneName;

    private void Start()
    {
        Scene _currentSceneName = SceneManager.GetActiveScene();
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space))
        {
            SceneManager.LoadScene(_sceneName);
        }
        else if (_currentSceneName.isLoaded)
        {
            SceneManager.LoadScene(_sceneName);
        }
    }
}
