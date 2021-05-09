using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MenuBehavior : MonoBehaviour
{
    public void PlayButton()
    {
        PlayerData.level = 1;
        SceneManager.LoadScene(PlayerData.level);
    }

    public void ContinueButton()
    {
        SceneManager.LoadScene(PlayerData.level);
    }

    public void QuitButton()
    {
        Application.Quit();
    }

    public void ToMenuButton()
    {
        SceneManager.LoadScene(0);
    }
}
