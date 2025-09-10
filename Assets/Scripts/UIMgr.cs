using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

//
// Author: Fenn Edmonds
// Purpose: Controls the ui at some points
//

public class UIMgr : MonoBehaviour
{

    public List<Bobber> items;

    public TextMeshProUGUI textBox;
    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            Vector3 mousePos = Input.mousePosition;
            Vector3 worldPos = Camera.main.ScreenToWorldPoint(mousePos);

            RaycastHit2D hit = Physics2D.Raycast(worldPos, Vector2.zero);

            Debug.Log("Reg");
            if (hit.collider != null)
            {
                Debug.Log("hit");
                textBox.text = "Choose A Path";

                Bobber tile = hit.collider.GetComponent<Bobber>();

                if (tile != null)
                    textBox.text = tile.pathDescription;

            }
        }
    }
}
