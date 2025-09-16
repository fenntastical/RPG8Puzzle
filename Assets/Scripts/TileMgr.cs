using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//
// Author: Fenn Edmonds autumne@unr.edu
// Purpose: Controls the tiles
//

public class TileMgr : MonoBehaviour
{

    public int tileNumber;
    Vector3 mousePos;
    RaycastHit2D rayHit;
    Transform clickObject;

    public GameMgr gameMgr;
    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
    }
}
