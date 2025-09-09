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
    // Start is called before the first frame update
    void Start()
    {
        solveText.text = solves.ToString();
        hintText.text = hints.ToString();
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
            if(!boss)
                gameMgr.SolvePuzzle();
            if(boss)
                bossM.SolvePuzzle();
        }
    }

    public void HintGiver()
    {
        if (hints > 0)
        {
            hints -= 1;
            hintText.text = hints.ToString();
            if(!boss)
                gameMgr.getHint();
            if(boss)
                bossM.getHint();
        }
    }
}
