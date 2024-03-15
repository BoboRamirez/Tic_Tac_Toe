using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    //[SerializeField] List<GameObject> blocks;
    [SerializeField] Camera mainCamera;
    private RaycastHit raycastHit;
    private Ray ray;
    private Mino mino = null;
    [SerializeField] bool _isPlayerFirst = true;
    [SerializeField] bool _isPlayerOn = true;
    public List<Mino> minos = new List<Mino>();
    private event Action ActivateAI;
    public List<int> availableMinos =
        new List<int> {0, 1, 2, 3, 4, 5, 6, 7, 8 };
    public static GameManager manager;
    public UIController UICtrl;
    private int lastPlay = -1;
    public int LastPlay {  get { return lastPlay; } }
    private AITheDevil AI_TheDevil;
    // Start is called before the first frame update
    void Start()
    {
        if (manager == null)
            manager = this;
        else if (manager != this)
            Destroy(this);
        AI_TheDevil = new AITheDevil();
        AI_TheDevil.Initialize();
        ActivateAI += AI_TheFool;
        UnityEngine.Random.InitState((int)Time.time);
        _isPlayerFirst = PlayerPick.IsPlayerFirst;
        _isPlayerOn = _isPlayerFirst;
        if (!_isPlayerFirst)
        {
            ActivateAI();
        }
    }

    // Update is called once per frame
    void Update()
    {
        if (_isPlayerOn)
        {
            HandlePlayerOn();
        }

    }

    private void HandlePlayerOn()
    {
        //UICtrl.ShowMessage("Your Turn...");
        UICtrl.ShowMessage("Here?...");
        ray = mainCamera.ScreenPointToRay(Input.mousePosition);
        if (Physics.Raycast(ray, out raycastHit))
        {
            if (mino != raycastHit.transform.gameObject.GetComponent<Mino>()
                && mino != null && mino.State == MinoState.Ghost)
                mino.SetState(MinoState.Empty);
            mino = raycastHit.transform.gameObject.GetComponent<Mino>();
            if (mino.State == MinoState.Empty)
                mino.SetState(MinoState.Ghost);

            if (Input.GetMouseButtonUp(0))
            {
                PlayAt(mino.Index);
                if (!_isPlayerOn)
                    ActivateAI();
            }
        }
        else if (mino != null && mino.State == MinoState.Ghost)
        {
            mino.SetState(MinoState.Empty);
            mino = null;
        }
    }
    //plays randomly. no intention to win what so ever.
    private void AI_TheFool()
    {
        if (availableMinos.Count <= 0)
            return;
        UICtrl.ShowMessage("Fool's Turn...");
        int n = UnityEngine.Random.Range(0, availableMinos.Count);
        //TODO:switch to AI_PlayAt
        minos[availableMinos[n]].SetState(MinoState.Ghost);
        StartCoroutine(AI_Animation(0.3f, availableMinos[n]));
    }
    private IEnumerator AI_Animation(float time, int index)
    {
        yield return new WaitForSeconds(time);
        PlayAt(index);
    }
    //aims at winning with no mercy.
    
    public void AI_PlayAt(int index, float time = 0.3f)
    {
        minos[index].SetState(MinoState.Ghost);
        StartCoroutine(AI_Animation(time, index));
    }

    /// <summary>
    /// Place move. used for both AI and Player
    /// </summary>
    /// <param name="index">the position to play at</param>
    private void PlayAt(int index)
    {
        minos[index].
           SetState(_isPlayerFirst ^ _isPlayerOn ? MinoState.Second : MinoState.First);
        availableMinos.Remove(index);
        if (CheckBoard(index))
        {
            Debug.Log(_isPlayerOn ? "PlayerWin" : "AIWin");
            EndGame(_isPlayerOn ? EndGameState.PlayerWin : EndGameState.AIWin);
            return;
        }
        if (availableMinos.Count <= 0)
        {
            EndGame(EndGameState.Draw);
            return;
        }
        lastPlay = index;
        _isPlayerOn = !_isPlayerOn;
    }

    private bool CheckBoard(int index)
    {
        if (minos[index].State < MinoState.First)
        {
            return false;
        }
        if (index == 4)//center
        {
            return (minos[0].State == minos[4].State && minos[4].State == minos[8].State) ||
                    (minos[1].State == minos[4].State && minos[4].State == minos[7].State) ||
                    (minos[2].State == minos[4].State && minos[4].State == minos[6].State) ||
                    (minos[3].State == minos[4].State && minos[4].State == minos[5].State);
        }
        /*else if (index % 3 == 0 && minos[index].State == minos[index + 1].State && minos[index + 1].State == minos[index + 2].State)
        {
            return true;
        }
        else if (index % 3 == 1 && minos[index].State == minos[index + 1].State && minos[index].State == minos[index - 1].State)
        {
            return true;
        }
        else if (index % 3 == 2 && minos[index].State == minos[index - 1].State && minos[index - 1].State == minos[index - 2].State)
        {
            return true;
        }*/
        else if (minos[index / 3 * 3].State == minos[index / 3 * 3 + 1].State &&
                minos[index / 3 * 3 + 1].State == minos[index / 3 * 3 + 2].State)//rows
        {

            return true;
        }
        else if (minos[index].State == minos[(index + 3) % 9].State && minos[index].State == minos[(index + 6) % 9].State)//columns
        {
            return true;
        }
        else if (index % 2 == 0 && minos[index].State == minos[4].State && minos[4].State == minos[8 - index].State)//diagnal
        {
            return true;
        }
        else
        {
            return false;
        }
    }
    private void EndGame(EndGameState s)
    {
        switch (s)
        {
            case EndGameState.PlayerWin: 
                Debug.Log("Player win");
                UICtrl.ShowMessage("You Win!\nAnother round?");
                ActivateAI -= PlayerPick.AIPicked == 0 ? AI_TheFool : AI_TheDevil.AI_TheDevil;
                UICtrl.TogglePicker(true);
                _isPlayerOn = false;
                break;
            case EndGameState.AIWin: 
                Debug.Log("AI win");
                UICtrl.ShowMessage($"{(PlayerPick.AIPicked == 0? "Fool" : "Devil")} Wins!\nAnother round?");
                UICtrl.TogglePicker(true);
                _isPlayerOn = false;
                break;
            default: 
                Debug.Log("Draw game");
                UICtrl.ShowMessage("Draw Game!\nAnother round?");
                UICtrl.TogglePicker(true);
                _isPlayerOn = false;
                break;
        }

    }
    public void ResetGame(bool isPlayerFirst = true, int AIPicked = 0)
    {
        foreach (var mino in minos)
        {
            mino.SetState(MinoState.Empty);
            availableMinos = new List<int> { 0, 1, 2, 3, 4, 5, 6, 7, 8 };
        }
        _isPlayerFirst = isPlayerFirst;
        _isPlayerOn = _isPlayerFirst;
        ActivateAI -= AI_TheDevil.AI_TheDevil;
        ActivateAI -= AI_TheFool;
        switch (AIPicked)
        {
            case 0: ActivateAI += AI_TheFool; break;
            case 1: 
                ActivateAI += AI_TheDevil.AI_TheDevil;
                AI_TheDevil.Initialize();
                break;
            default: break;
        }
        if (!_isPlayerOn)
            ActivateAI();
    }
}
public enum EndGameState
{
    PlayerWin, AIWin, Draw
}