using UnityEngine;
using System;
using System.Collections;
using TMPro;

public class Bobber : MonoBehaviour
{
    float originalY;

    public TextMeshProUGUI textbox;
    public float floatStrength = 1; // You can change this in the Unity Editor to 
                                    // change the range of y positions that are possible.


    public string pathDescription;

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

    public void ChangeText()
    {
        textbox.text = pathDescription;
        textbox.fontSize = 100;
        // Vector3 pos = textbox.transform.position;
        //             pos.y = 0;
        //             textbox.transform.position = pos;
    }
}
