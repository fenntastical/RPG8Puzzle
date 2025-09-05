using System.Collections;
using System.Collections.Generic;
using UnityEngine;

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
        mousePos = Input.mousePosition;

        Ray mouseRay = Camera.main.ScreenPointToRay(mousePos);

        if (Input.GetMouseButtonDown(0))
        {
            rayHit = Physics2D.Raycast(mouseRay.origin, mouseRay.direction);
            clickObject = rayHit ? rayHit.collider.transform : null;

            if (clickObject)
            {
                gameMgr.selectedTile = tileNumber;
            }
        }

    }
}
