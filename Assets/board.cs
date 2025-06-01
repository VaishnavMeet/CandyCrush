using System.Collections;
using System.Collections.Generic;
using System.Data;
using UnityEngine;

public class board : MonoBehaviour
{
    public int rows=0, colls=0;
    public GameObject[] prefabCandy;
    public GameObject[,] allCandy;
    public GameObject keyPrefab;
    public GameObject curMoveCandy;
    MatchFinder matchFinder;
    int keyCnt=3;
    void Start()
    {
        matchFinder = FindAnyObjectByType<MatchFinder>();
        allCandy = new GameObject[colls, rows];
        generate();
    }

    void generate()
    {

        int radomCall = Random.Range(0, 5);
        for (int i = 0; i < colls; i++)
        {
            for (int j = 0; j < rows; j++)
            {

                if (!GameObject.FindGameObjectWithTag("key") && keyCnt > 0 && j == 5 && i==radomCall)
                {
                    GameObject g= Instantiate(keyPrefab, new Vector2(radomCall, j), Quaternion.identity);
                    allCandy[i, j] = g;
                    g.GetComponent<Candy>().col = i;
                    g.GetComponent<Candy>().row = j;
                    keyCnt--;
                }
                else
                {
                    int r = Random.Range(0, prefabCandy.Length);
                    Vector2 vect2 = new Vector2(i, j);
                    while (checkRepete(i, j, prefabCandy[r]))
                    {
                        r = Random.Range(0, prefabCandy.Length);
                    }
                    GameObject g = Instantiate(prefabCandy[r], vect2, Quaternion.identity);
                    allCandy[i, j] = g;
                    g.GetComponent<Candy>().col = i;
                    g.GetComponent<Candy>().row = j;
                }
            }
        }
    }

  

    bool checkRepete(int rows,int cols,GameObject cur)
    {
        if (rows > 1 && cols > 1)
        {
            if(allCandy[rows - 1, cols].tag == cur.tag && allCandy[rows - 2, cols].tag == cur.tag)
            {
                return true;
            }
            if(allCandy[rows, cols - 1].tag == cur.tag && allCandy[rows, cols - 2].tag == cur.tag)
            {
                return true;
            }
        }
        else if (rows > 1)
        {
            if(allCandy[rows - 1, cols].tag == cur.tag && allCandy[rows - 2, cols].tag == cur.tag)
            {
                return true;
            }
        }
        else if (cols > 1)
        {
            if(allCandy[rows, cols - 1].tag == cur.tag && allCandy[rows, cols - 2].tag == cur.tag)
            {
                return true;
            }
        }
        return false;
    }

    public void destoryMatchCandy()
    {
        if (matchFinder.matchCandy.Count == 4)
        {
            matchFinder.checkForBomb();
        }
        else if (matchFinder.matchCandy.Count >= 5)
        {
            matchFinder.checkForColourBomb();
        }

        for (int i = 0; i < colls; i++)
        {
            for (int j = 0; j < rows; j++)
            {

                if (allCandy[i, j] != null)
                {
                    if (allCandy[i,j].GetComponent<Candy>().isMatch || (j == 0 && allCandy[i,j].tag == "key".ToString()))
                    {
                        Destroy(allCandy[i,j]);
                        allCandy[i,j]=null;
                    }

                }
                
            }
        }
        StartCoroutine(shiftingRaw());
    
    }

    IEnumerator shiftingRaw()
    {
        yield return new WaitForSeconds(0.4f);
        int nallCount = 0;

        for (int i = 0; i < colls; i++)
        {
            for (int j = 0; j < rows; j++)
            {

                if (allCandy[i, j] == null)
                {
                    nallCount++;

                }
                else if (nallCount > 0)
                {
                    allCandy[i, j].GetComponent<Candy>().row-=nallCount;
                    allCandy[i, j] = null;
                }

            }
            nallCount = 0;
        }
        StartCoroutine(OnDestroyGenerate());
    }

    IEnumerator OnDestroyGenerate()
    {
        matchFinder.matchCandy.Clear();
        yield return new WaitForSeconds(0.5f);

        for (int i = 0; i < colls; i++)
        {
            for (int j = 0; j < rows; j++)
            {

                if (allCandy[i, j] == null)
                {

                    if (!GameObject.FindGameObjectWithTag("key") && keyCnt > 0 && j == 5)
                    {
                        GameObject g = Instantiate(keyPrefab, new Vector2(i, j), Quaternion.identity);
                        allCandy[i, j] = g;
                        g.GetComponent<Candy>().col = i;
                        g.GetComponent<Candy>().row = j;
                        keyCnt--;
                    }
                    else
                    {
                        int r = Random.Range(0, prefabCandy.Length);
                        Vector2 vect2 = new Vector2(i, j);


                        GameObject g = Instantiate(prefabCandy[r], vect2, Quaternion.identity);
                        allCandy[i, j] = g;
                        g.GetComponent<Candy>().col = i;
                        g.GetComponent<Candy>().row = j;

                    }
                }
            }
        }

        yield return new WaitForSeconds(0.5f);
        for (int i = 0; i < colls; i++)
        {
            for (int j = 0; j < rows; j++)
            {

                if (allCandy[i, j] != null)
                {
                    if (allCandy[i, j].GetComponent<Candy>().isMatch)
                    {
                        destoryMatchCandy();
                    }
                }
            }
        }
    }
}
