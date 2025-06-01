using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MatchFinder : MonoBehaviour
{
    board bord;
    public List<GameObject> matchCandy = new List<GameObject>();
    GameObject curCandy;
    private void Start()
    {
        bord = FindAnyObjectByType<board>();
    }

    public void findMatch()
    {
        StartCoroutine(checkMatches());
        
    }


    IEnumerator checkMatches()
    {
        yield return new WaitForSeconds(0.2f);

        for (int i = 0; i < bord.colls; i++)
        {
            for (int j = 0; j < bord.rows; j++)
            {
                if (i > 0 && i < bord.colls - 1)
                {
                     curCandy = bord.allCandy[i, j];
                    if (curCandy != null)
                    {
                        GameObject leftCandy = bord.allCandy[i - 1, j];
                        GameObject rightCandy = bord.allCandy[i + 1, j];
                        if (leftCandy != null && rightCandy != null)
                        {
                            if (leftCandy.tag == curCandy.tag && rightCandy.tag == curCandy.tag)
                            {
                                leftCandy.GetComponent<Candy>().isMatch = true;
                                curCandy.GetComponent<Candy>().isMatch = true;
                                rightCandy.GetComponent<Candy>().isMatch = true;


                                if (leftCandy.GetComponent<Candy>().isColBomb)
                                {
                                    collectColumnCandies(leftCandy.GetComponent<Candy>().col);
                                }
                                if (curCandy.GetComponent<Candy>().isColBomb)
                                {
                                    collectColumnCandies(curCandy.GetComponent<Candy>().col);
                                }
                                if (rightCandy.GetComponent<Candy>().isColBomb)
                                {
                                    collectColumnCandies(rightCandy.GetComponent<Candy>().col);
                                }

                                if (leftCandy.GetComponent<Candy>().isRowBomb)
                                {
                                    collectRowCandies(leftCandy.GetComponent<Candy>().row);
                                }
                                if (curCandy.GetComponent<Candy>().isRowBomb)
                                {
                                    collectRowCandies(curCandy.GetComponent<Candy>().row);
                                }
                                if (rightCandy.GetComponent<Candy>().isRowBomb)
                                {
                                    collectRowCandies(rightCandy.GetComponent<Candy>().row);
                                }


                                if (!matchCandy.Contains(leftCandy))
                                    matchCandy.Add(leftCandy);

                                if (!matchCandy.Contains(curCandy))
                                    matchCandy.Add(curCandy);

                                if (!matchCandy.Contains(rightCandy))
                                    matchCandy.Add(rightCandy);
                            }
                        }
                    }
                }


                if (j > 0 && j < bord.rows - 1)
                {
                     curCandy = bord.allCandy[i, j];
                    if (curCandy != null)
                    {
                        GameObject upCandy = bord.allCandy[i, j + 1];
                        GameObject downCandy = bord.allCandy[i, j - 1];
                        if (upCandy != null && downCandy != null)
                        {
                            if (upCandy.tag == curCandy.tag && downCandy.tag == curCandy.tag)
                            {  
                                upCandy.GetComponent<Candy>().isMatch = true;
                                curCandy.GetComponent<Candy>().isMatch = true;
                                downCandy.GetComponent<Candy>().isMatch = true;



                                if (upCandy.GetComponent<Candy>().isColBomb)
                                {
                                    collectColumnCandies(upCandy.GetComponent<Candy>().col);
                                }
                                if (curCandy.GetComponent<Candy>().isColBomb)
                                {
                                    collectColumnCandies(curCandy.GetComponent<Candy>().col);
                                }
                                if (downCandy.GetComponent<Candy>().isColBomb)
                                {
                                    collectColumnCandies(downCandy.GetComponent<Candy>().col);
                                }

                                if (upCandy.GetComponent<Candy>().isRowBomb)
                                {
                                    collectRowCandies(upCandy.GetComponent<Candy>().row);
                                }
                                if (curCandy.GetComponent<Candy>().isRowBomb)
                                {
                                    collectRowCandies(curCandy.GetComponent<Candy>().row);
                                }
                                if (downCandy.GetComponent<Candy>().isRowBomb)
                                {
                                    collectRowCandies(downCandy.GetComponent<Candy>().row);
                                }




                                if (!matchCandy.Contains(upCandy))
                                    matchCandy.Add(upCandy);

                                if (!matchCandy.Contains(curCandy))
                                    matchCandy.Add(curCandy);

                                if (!matchCandy.Contains(downCandy))
                                    matchCandy.Add(downCandy);

                            }

                        }

                        
                    }
                }


                if (curCandy != null)
                { 
                    if(curCandy.GetComponent<Candy>().isColourBomb)
                    {
                        GameObject oppCandy= bord.curMoveCandy.GetComponent<Candy>().oppCandy;
                        if(oppCandy!=null)
                        {
                            collectAllCandy(oppCandy);
                            curCandy.GetComponent<Candy>().isMatch=true;
                        }
                    }
                
                }

            }
        }

    }

    void collectAllCandy(GameObject g)
    {
        for(int i=0;i<bord.colls;i++)
        {
            for (int j = 0; j < bord.rows; j++)
            {
                if(bord.allCandy[i, j]!=null && g!=null)
                if (g.tag == bord.allCandy[i,j].tag)
                {
                    bord.allCandy[i, j].GetComponent<Candy>().isMatch = true;
                }
            }
        }

    }
    

    void collectColumnCandies(int col)
    {
        for (int j = 0; j < bord.rows; j++)
        {
            if(bord.allCandy[col, j]!=null)
                bord.allCandy[col, j].GetComponent<Candy>().isMatch = true;
        }
    }

    void collectRowCandies(int row)
    {
        for (int j = 0; j < bord.colls; j++)
        {
            if (bord.allCandy[j,row] != null)
                bord.allCandy[j, row].GetComponent<Candy>().isMatch = true;
        }
    }

    

    public void checkForColourBomb()
    {
        if (bord.curMoveCandy != null)
        {
            if (bord.curMoveCandy.GetComponent<Candy>().isMatch)
            {
                bord.curMoveCandy.GetComponent<Candy>().isMatch = false;
                Candy curCandy = bord.curMoveCandy.GetComponent<Candy>();
                curCandy.colourBomb();
                curCandy.tag="ColorBomb";
            }
            else if (bord.curMoveCandy.GetComponent<Candy>().oppCandy != null)
            {
                GameObject oppCandyObj = bord.curMoveCandy.GetComponent<Candy>().oppCandy;
                Candy curCandy = bord.curMoveCandy.GetComponent<Candy>();

                
                if (oppCandyObj.GetComponent<Candy>().isMatch)
                {
                    oppCandyObj.GetComponent<Candy>().isMatch = false;
                    oppCandyObj.GetComponent<Candy>().colourBomb();
                    oppCandyObj.tag = "ColorBomb";
                }
            }
        }
    }

    public void checkForBomb()
    {
        if (bord.curMoveCandy != null)
        {
            if (bord.curMoveCandy.GetComponent<Candy>().isMatch)
            {
                bord.curMoveCandy.GetComponent<Candy>().isMatch = false;
                Candy curCandy=bord.curMoveCandy.GetComponent<Candy>();
                if ((curCandy.angle >= -45f && curCandy.angle <= 45f) || (curCandy.angle <= -135f || curCandy.angle >= 135f))
                {
                    curCandy.collBomber();
                }
                else
                {
                    curCandy.rowBomber();
                }
            }
            else if (bord.curMoveCandy.GetComponent<Candy>().oppCandy != null)
            {
                GameObject oppCandyObj = bord.curMoveCandy.GetComponent<Candy>().oppCandy;
                Candy curCandy = bord.curMoveCandy.GetComponent<Candy>();
                if (oppCandyObj.GetComponent<Candy>().isMatch)
                {
                    oppCandyObj.GetComponent<Candy>().isMatch = false;

                    if ((curCandy.angle >= -45f && curCandy.angle <= 45f) || (curCandy.angle <= -135f || curCandy.angle >= 135f))
                    {
                        oppCandyObj.GetComponent<Candy>().rowBomber();
                    }
                    else
                    {
                        oppCandyObj.GetComponent<Candy>().collBomber();
                    }
                }
            }
        }
    }

}
