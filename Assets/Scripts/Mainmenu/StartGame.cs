using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public enum SceneName
{
    MainMenu,
    InGame,
}

public class StartGame : MonoBehaviour
{
    [SerializeField]
    SceneName changeSceneName = SceneName.InGame;

    Button startButton;

    private void Awake()
    {
        startButton = GetComponent<Button>();
        startButton.onClick.AddListener(OnClickStartGameButton);
    }
    public void OnClickStartGameButton()
    {
        SceneManager.LoadScene(changeSceneName.ToString());
        startButton.onClick.RemoveAllListeners();
    }
}
