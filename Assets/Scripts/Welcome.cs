using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Welcome : MonoBehaviour
{
    public void OnClickFirst()
    {
        PlayerPick.IsPlayerFirst = true;
        SceneManager.LoadScene("Stage");
    }
    public void OnClickSecond()
    {
        PlayerPick.IsPlayerFirst = false;
        SceneManager.LoadScene("Stage");
    }
    public void OnClickQuit()
    {
        Application.Quit();
    }
}

public static class PlayerPick
{
    public static bool IsPlayerFirst;
    public static int AIPicked = 0;
}
