using System;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneLoader : MonoBehaviour
{
    GameManager gameManager;

    private void Awake()
    {
        gameManager = FindFirstObjectByType<GameManager>();

        if(gameManager == null)
        {
            Debug.LogError("Cannot find any objcet of type GameManager!");
        }

        SceneManager.LoadSceneAsync(1,LoadSceneMode.Additive).completed += OnSceneLoadcompleted;
        
        
    }

    private void OnSceneLoadcompleted(AsyncOperation operation)
    {
        gameManager.SetUp();
    }
}
