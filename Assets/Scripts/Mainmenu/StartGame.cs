using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public enum SceneName
{
    MainMenu,
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
