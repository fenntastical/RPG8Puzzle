using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//
// Author: Fenn Edmonds autumne@unr.edu
// Purpose: Resets the donotload when you start a new game from the main menu
//

public class StoreDealer : MonoBehaviour
{
    public GameObject dataLoad;
    HintnSolveTracker hintLoader;
    public int bSolves;
    public int bHints;
    // Start is called before the first frame update
    void Start()
    {
        dataLoad = GameObject.Find("Stored");


        if (dataLoad != null)
        {
            hintLoader = dataLoad.GetComponent<HintnSolveTracker>();
            if (hintLoader != null)
            {
                hintLoader.baseHints = bHints;
                hintLoader.baseSolves = bSolves;
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
}
