using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class Candy : MonoBehaviour
{
    

    Vector2 offset, firstPos, secondPos;
   public float angle;
    public int col,row,targetX,targetY,revTargetX,revTargetY;
   public GameObject oppCandy;
    board bord;
    public GameObject rowBombPre, colBombPre, colorbombpre;
    public bool isMatch,isRowBomb,isColBomb,isColourBomb;
    MatchFinder matchFinder;
    private void Start()
    {
        matchFinder = FindAnyObjectByType<MatchFinder>();
        bord = FindObjectOfType<board>();
    }

    private void Update()
    {
        targetX = col;
        targetY=row;


        if (Mathf.Abs(targetX - transform.position.x) > 0.01f)
        {
            Vector2 newPos = new Vector2(targetX, targetY);
            transform.position = Vector2.Lerp(transform.position, newPos, 0.5f);
            if (bord.allCandy[col, row] != this.gameObject)
            {
                bord.allCandy[col, row] = this.gameObject;
            }
        }

        else if (Mathf.Abs(targetY - transform.position.y) > 0.01f)
        {
            Vector2 newPos = new Vector2(targetX, targetY);
            transform.position = Vector2.Lerp(transform.position, newPos, 0.5f);
            if (bord.allCandy[col, row] != this.gameObject)
            {
                bord.allCandy[col, row] = this.gameObject;
            }
        }
        else
        { 
            Vector2 newPos=new Vector2(targetX, targetY);
            transform.position = newPos;
        }

        matchFinder.findMatch();
    }


    private void OnMouseDown()
    {
        bord.curMoveCandy = this.gameObject;
        firstPos = Camera.main.WorldToScreenPoint(Input.mousePosition);
    }

    private void OnMouseUp()
    {
        secondPos = Camera.main.WorldToScreenPoint(Input.mousePosition);
        offset = new Vector2(secondPos.x - firstPos.x, secondPos.y - firstPos.y);

        if(Mathf.Abs(firstPos.x - secondPos.x) > 0.5f || Mathf.Abs(firstPos.y- secondPos.y) > 0.5f)
        { 
            angle = Mathf.Atan2(offset.y, offset.x) * Mathf.Rad2Deg;
            //print(angle);
            move();
        }
       

    }

    void move()
    {
        if (angle >= -45 && angle <= 45 && col < bord.colls - 1)
        {
            oppCandy = bord.allCandy[col + 1, row];
            // bord.allCandy[col + 1, row] = bord.allCandy[col, row];
            // bord.allCandy[col, row]=oppCandy;
            oppCandy.GetComponent<Candy>().col -= 1;
            col += 1;
            activateColourBomb();


        }

        else if (angle >= 45 && angle <= 135 && row<bord.rows-1)
        {
            oppCandy = bord.allCandy[col,row+1];
           // bord.allCandy[col , row+1] = bord.allCandy[col, row];
           // bord.allCandy[col, row] = oppCandy;
            oppCandy.GetComponent<Candy>().row -=1;
            row+= 1;
            activateColourBomb();

        }

        else if ((angle >= 135 || angle <= -135 ) && col>0)
        {
            oppCandy = bord.allCandy[col - 1, row];
          //  bord.allCandy[col-1, row ] = bord.allCandy[col, row];
          //  bord.allCandy[col, row] = oppCandy;
            oppCandy.GetComponent<Candy>().col += 1;
            col -= 1;
            activateColourBomb();

        }
        else if (angle <= -45 && angle >=-135 && row>0)
        {
            oppCandy = bord.allCandy[col, row - 1];
          //  bord.allCandy[col, row - 1] = bord.allCandy[col, row];
           // bord.allCandy[col, row] = oppCandy;
            oppCandy.GetComponent<Candy>().row += 1;
            row -= 1;
            activateColourBomb();

        }

        StartCoroutine(checkCandyMove());
    }

    void activateColourBomb()
    {
        if (gameObject.tag == "ColorBomb" && oppCandy.tag == "ColorBomb")
        {
            for (int i = 0; i < bord.colls; i++)
            {
                for (int j = 0; j < bord.rows; j++)
                {
                    bord.allCandy[i, j].GetComponent<Candy>().isMatch = true;
                }
            }
        }
        else if (gameObject.tag == "ColorBomb")
        {
            gameObject.GetComponent<Candy>().isColourBomb = true;
        }
        else if (oppCandy.tag == "ColorBomb")
        {
            oppCandy.GetComponent<Candy>().isColourBomb = true;
        }
        
    }

    void findmatch()
    {

        if (col > 0 && col < bord.colls - 1)
        {
            GameObject curCandy = bord.allCandy[col, row];
            GameObject leftCandy = bord.allCandy[col+1, row];
            GameObject rightCandy = bord.allCandy[col-1, row];

            if (curCandy != null && leftCandy != null && rightCandy!=null)
            if(curCandy.tag == leftCandy.tag && curCandy.tag==rightCandy.tag && leftCandy.tag==rightCandy.tag)
            {
                isMatch=true;
                leftCandy.GetComponent<Candy>().isMatch = true;
                rightCandy.GetComponent<Candy>().isMatch = true;
                /*curCandy.GetComponent<Candy>().col =7;
                curCandy.GetComponent<Candy>().row =7;
                leftCandy.GetComponent<Candy>().col =7;
                leftCandy.GetComponent<Candy>().row =7;
                rightCandy.GetComponent <Candy>().col =7;
                rightCandy.GetComponent<Candy>().row=7;
*/
            }
        }

        if (row > 0 && row < bord.rows - 1)
        {
            GameObject curCandy = bord.allCandy[col, row];
            GameObject upCandy = bord.allCandy[col , row+1];
            GameObject downCandy = bord.allCandy[col , row-1];

            if (curCandy != null && upCandy != null && downCandy != null)
            {
                if (curCandy.tag == upCandy.tag && curCandy.tag == downCandy.tag && upCandy.tag == downCandy.tag)
                {
                    isMatch = true;
                    upCandy.GetComponent<Candy>().isMatch = true;
                    downCandy.GetComponent<Candy>().isMatch = true;
                }
            }
        }
    }

    

    IEnumerator checkCandyMove()
    { 
    
        yield return new WaitForSeconds(0.5f);

        if (oppCandy != null)
        {
            if (!oppCandy.GetComponent<Candy>().isMatch && !isMatch)
            {
                revTargetX = oppCandy.GetComponent<Candy>().col;
                revTargetY = oppCandy.GetComponent<Candy>().row;
                oppCandy.GetComponent<Candy>().row = row;
                oppCandy.GetComponent<Candy>().col = col;
                row = revTargetY;
                col = revTargetX;
                oppCandy = null;
            }

            else {
                for (int i = 0; i < bord.colls; i++)
                {
                    for (int j = 0; j < bord.rows; j++)
                    {
                        if(bord.allCandy[i, j] != null)
                        if (bord.allCandy[i,j].GetComponent<Candy>().isMatch)
                        bord.destoryMatchCandy();
                    }
                }
            }
        }
    }

    public void colourBomb()
    { 
        Instantiate(colorbombpre, transform);
    }

    public void collBomber()
    {
        isColBomb=true;
        Instantiate(colBombPre,transform);
    }

    public void rowBomber()
    {
        isRowBomb = true;
        Instantiate(rowBombPre,transform);
    }

   
}
