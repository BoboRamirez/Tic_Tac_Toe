using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Welcome : MonoBehaviour
{
    public Audio a;
    public void OnClickFirst()
    {
        a.Play();
        GameData.IsPlayerFirst = true;
        GameData.a = a;
        SceneManager.LoadScene("Stage");
    }
    public void OnClickSecond()
    {
        a.Play();
        GameData.IsPlayerFirst = false;
        GameData.a = a;
        SceneManager.LoadScene("Stage");
    }
    public void OnClickQuit()
    {
        Application.Quit();
    }
}

public static class GameData
{
    public static bool IsPlayerFirst;
    public static int AIPicked = 0;
    public static Audio a;
}
