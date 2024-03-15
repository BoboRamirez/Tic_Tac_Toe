using System.Collections;
using System.Collections.Generic;
using Unity.Mathematics;
using UnityEngine;
//tricky, aims at killing.
public class AITheDevil
{
    private GameManager gm = GameManager.manager;
    private UIController UICtrl;
    private List<int> kills = new List<int>();
    private int place = -1;
    private List<int> playerMoves = new List<int>();
    private bool _isKillMode;
    private float delay = 1.5f;
    public void Initialize()
    {
        UICtrl = gm.UICtrl;
        playerMoves.Clear();
        kills.Clear();
        place = -1;
        _isKillMode = false;
    }

    public void AI_TheDevil()
    {
        if (gm.availableMinos.Count <= 0)
            return;
        UICtrl.ShowMessage("Devil's Turn...");
        switch (gm.availableMinos.Count)
        {
            //AI first. dominance.
            case 9: gm.AI_PlayAt(4); break;
            case 7:
                playerMoves.Add(gm.LastPlay);
                if (gm.LastPlay % 2 == 0)
                    for (int i = 0; i <= 8; i+= 2)
                    {
                        if (i != gm.LastPlay && i != 8 - gm.LastPlay && i != 4)
                        {
                            UICtrl.ShowMessage("Boring...");
                            gm.AI_PlayAt(i);
                            kills.Add(8 - i);
                            break;
                        }
                    }
                else
                {
                    _isKillMode = true;
                    //Debug.Log("Kill mode");
                    UICtrl.ShowMessage("Your Soul...", Color.red);
                    place = UnityEngine.Random.Range(0, 2) == 0 ?
                        gm.LastPlay + 4 - math.abs(gm.LastPlay - 4) :
                        gm.LastPlay - 4 + math.abs(gm.LastPlay - 4);
                    gm.AI_PlayAt(place, delay);
                    //Debug.Log("7: " + place.ToString());
                    kills.Add(8 - place);
                }
                break;
            case 5:
                playerMoves.Add(gm.LastPlay);
                if (gm.availableMinos.Contains(kills[0]))
                {
                    UICtrl.ShowMessage("Feast...", Color.red);
                    gm.AI_PlayAt(kills[0], delay);
                }
                else if (!_isKillMode)
                {
                    UICtrl.ShowMessage("Boring...");
                    gm.AI_PlayAt((playerMoves[0] + playerMoves[1]) / 2);
                    kills[0] = 8 - (playerMoves[0] + playerMoves[1]) / 2;
                }
                else
                {
                    UICtrl.ShowMessage("Shall be...", Color.red);

                    //Debug.Log("5: " + (8 - 2 * playerMoves[0] + place).ToString());
                    gm.AI_PlayAt(8 - 2 * playerMoves[0] + place, delay);
                    kills[0] = 2 * playerMoves[0] - place;
                    kills.Add(4 - playerMoves[0] + place);
                }
                break;
            case 3:
                playerMoves.Add(gm.LastPlay);
                if (_isKillMode)
                {
                    UICtrl.ShowMessage("Mine...", Color.red);
                    foreach (int kill in kills)
                    {
                        if (gm.availableMinos.Contains(kill))
                            gm.AI_PlayAt(kill, delay);
                    }
                }
                else if (gm.availableMinos.Contains(kills[0]))
                {
                    UICtrl.ShowMessage("Feast...", Color.red);
                    gm.AI_PlayAt(kills[0], delay);
                }
                else// if (!_isKillMode)
                {
                    UICtrl.ShowMessage("Boring...");
                    gm.AI_PlayAt(gm.availableMinos[0]);
                }
                break;
            case 1:
                UICtrl.ShowMessage("Feast...", Color.red);
                gm.AI_PlayAt(gm.availableMinos[0], delay);
                break;
            //AI second. defense.
            case 8:
                UICtrl.ShowMessage("Hmmm...");
                if (gm.LastPlay == 4)
                {
                    gm.AI_PlayAt(6, delay);
                }
                else if(gm.LastPlay %2 == 0)
                {
                    gm.AI_PlayAt(4, delay);
                }
                else
                {
                    gm.AI_PlayAt(8 - gm.LastPlay, delay);
                }
                break;
            default:
                //to kill
                foreach (int i in gm.availableMinos)
                {
                    //check center block for kill
                    if (i == 4)
                    {
                        for (int j = 0; j <= 3; j++)
                        {
                            if (gm.minos[j].state == MinoState.Second &&
                                gm.minos[j].state == gm.minos[8 - j].state)
                            {
                                UICtrl.ShowMessage("Nihil...", Color.red);
                                gm.AI_PlayAt(4, delay);
                                return;
                            }

                        }
                    }
                    //check corner blocks for kill
                    else if (i % 2 == 0)
                    {
                        int sign1 = i % 3 == 0 ? 1 : -1;
                        int sign3 = i < 4 ? 1 : -1;
                        if ((gm.minos[i + sign1 * 1].state == MinoState.Second &&
                            gm.minos[i + sign1 * 2].state == MinoState.Second) ||
                            (gm.minos[i + sign3 * 3].state == MinoState.Second &&
                            gm.minos[i + sign3 * 6].state == MinoState.Second) ||
                            (gm.minos[4].state == MinoState.Second &&
                            gm.minos[8 - i].state == MinoState.Second))
                        {
                            UICtrl.ShowMessage("Nihil...", Color.red);
                            gm.AI_PlayAt(i, delay);
                            return;
                        }
                    }
                    //check edge blocks for kill
                    else
                    {
                        int delta = 4 - math.abs(i - 4);
                        if ((gm.minos[i + delta].state == MinoState.Second &&
                            gm.minos[i - delta].state == MinoState.Second) ||
                            (gm.minos[4].state == MinoState.Second &&
                            gm.minos[8 - i].state == MinoState.Second))
                        {
                            UICtrl.ShowMessage("Nihil...", Color.red);
                            gm.AI_PlayAt(i, delay);
                            return;
                        }
                    }
                }
                //to defense
                foreach (int i in gm.availableMinos)
                {
                    //check center block for defense
                    if (i == 4)
                    {
                        for (int j = 0; j <= 3; j++)
                        {
                            if (gm.minos[j].state == MinoState.First &&
                                gm.minos[8 - j].state == MinoState.First)
                            {
                                UICtrl.ShowMessage("OK...");
                                gm.AI_PlayAt(4, delay);
                                return;
                            }

                        }
                    }
                    //check corner blocks for defense
                    else if (i % 2 == 0)
                    {
                        int sign1 = i % 3 == 0 ? 1 : -1;
                        int sign3 = i < 4 ? 1 : -1;
                        if ((gm.minos[i + sign1 * 1].state == MinoState.First &&
                            gm.minos[i + sign1 * 2].state == MinoState.First) ||
                            (gm.minos[i + sign3 * 3].state == MinoState.First &&
                            gm.minos[i + sign3 * 6].state == MinoState.First) ||
                            (gm.minos[4].state == MinoState.First &&
                            gm.minos[8 - i].state == MinoState.First))
                        {
                            UICtrl.ShowMessage("OK...");
                            gm.AI_PlayAt(i, delay);
                            return;
                        }
                    }
                    //check edge blocks for defense
                    else
                    {
                        int delta = 4 - math.abs(i - 4);
                        if ((gm.minos[i + delta].state == MinoState.First &&
                            gm.minos[i - delta].state == MinoState.First) ||
                            (gm.minos[4].state == MinoState.First &&
                            gm.minos[8 - i].state == MinoState.First))
                        {
                            UICtrl.ShowMessage("OK...");
                            gm.AI_PlayAt(i, delay);
                            return;
                        }
                    }
                }
                //ramdom pick, chance to be defeated
                place = gm.availableMinos[UnityEngine.Random.Range(0, gm.availableMinos.Count)];
                UICtrl.ShowMessage("Bored...");
                gm.AI_PlayAt(place, delay);
                break;
        }
    }
}
