using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;

public class UIController : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI message;
    [SerializeField] private GameObject AIPicker;
    [SerializeField] private List<TextMeshProUGUI> AIList;
    [SerializeField] private List<TextMeshProUGUI> InitiativeList;
    public void ShowMessage(string m, Color c = default)
    {
        message.text = m;
        if (c == default)
        {
            message.color = Color.white;
        }
        else
        {
            message.color = c;
        }
    }

    public void TogglePicker(bool doesShow)
    {
        AIPicker.SetActive(doesShow);
        if (doesShow)
        {
            OnClickFool();
            OnClickFirst();
        }
    }

    public void OnClickFool()
    {
        AIList[0].color = Color.red;
        AIList[1].color = Color.white;
        GameData.AIPicked = 0;
    }
    public void OnClickDevil()
    {
        AIList[0].color = Color.white;
        AIList[1].color = Color.red;
        GameData.AIPicked = 1;
    }
    public void OnClickRestart()
    {
        GameManager.manager.ResetGame(GameData.IsPlayerFirst, GameData.AIPicked);
        TogglePicker(false);
    }
    public void OnClickFirst()
    {
        InitiativeList[0].color = Color.red;
        InitiativeList[1].color = Color.white;
        GameData.IsPlayerFirst = true;
    }
    public void OnClickSecond()
    {
        InitiativeList[0].color = Color.white;
        InitiativeList[1].color = Color.red;
        GameData.IsPlayerFirst = false;
    }
    public void OnClickQuit()
    {
        Application.Quit();
    }
}
