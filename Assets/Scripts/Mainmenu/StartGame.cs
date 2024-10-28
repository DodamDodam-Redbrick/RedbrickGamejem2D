using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

enum SceneName
{
    InGame,
}

public class StartGame : MonoBehaviour
{
    [SerializeField]
    SceneName changeSceneName = SceneName.InGame;

    public void OnClickStartGameButton()
    {
        SceneManager.LoadScene(changeSceneName.ToString());
    }
}
