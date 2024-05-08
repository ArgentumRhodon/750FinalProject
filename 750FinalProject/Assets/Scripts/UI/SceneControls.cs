using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public enum Scene
{
    MainMenu = 0,
    Game = 1,
}

public class SceneControls : MonoBehaviour
{
    public void LoadGame()
    {
        SceneManager.LoadScene((int)Scene.Game);
    }

    public void LoadMainMenu()
    {
        SceneManager.LoadScene((int)Scene.MainMenu);
    }
}
