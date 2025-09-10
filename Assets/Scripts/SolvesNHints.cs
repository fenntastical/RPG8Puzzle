using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class SolvesNHints : MonoBehaviour
{

    public int solves;
    public int hints;

    public TextMeshProUGUI solveText;
    public TextMeshProUGUI hintText;

    public GameMgr gameMgr;

    public BossMgr bossM;

    public bool boss;
    public GameObject dataLoad;
    HintnSolveTracker hintLoader;
    // Start is called before the first frame update
    void Start()
    {
        dataLoad = GameObject.Find("Stored");


        if (dataLoad != null)
        {
            hintLoader = dataLoad.GetComponent<HintnSolveTracker>();
            if (hintLoader != null)
            {
                Debug.Log(hintLoader.baseSolves);
                Debug.Log(hintLoader.baseHints);

                solves = hintLoader.baseSolves;
                hints = hintLoader.baseHints;
                solveText.text = solves.ToString();
                hintText.text = hints.ToString();
            }
            else
            {
                Debug.LogError("HintnSolveTracker not found on Stored object!");
            }
        }
        else
        {
            Debug.LogError("Stored object not found!");
        }
    }

    // Update is called once per frame
    void Update()
    {
    }

    public void UpdateText()
    {
        solveText.text = solves.ToString();
        hintText.text = hints.ToString();
    }

    public void SolvePuz()
    {
        if (solves > 0)
        {
            solves -= 1;
            solveText.text = solves.ToString();
            if (!boss)
                gameMgr.SolvePuzzle();
            if (boss)
                bossM.SolvePuzzle();
        }
    }

    public void HintGiver()
    {
        if (hints > 0)
        {
            hints -= 1;
            hintText.text = hints.ToString();
            if (!boss)
                gameMgr.getHint();
            if (boss)
                bossM.getHint();
        }
    }
    
    public void UpdateStores()
    {
        hintLoader.baseSolves = solves;
        hintLoader.baseHints = hints;
    }
}
