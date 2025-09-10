using UnityEngine;
using System;
using System.Collections;
using TMPro;

//
// Author: Fenn Edmonds
// Purpose: Makes the UI float
//

public class Float : MonoBehaviour
{

    float originalY;
    public float floatStrength = 1;
    // Start is called before the first frame update
    void Start()
    {
        this.originalY = this.transform.position.y;
    }

    void Update()
    {
        transform.position = new Vector3(transform.position.x,
            originalY + ((float)Math.Sin(Time.time) * floatStrength),
            transform.position.z);
    }
}
