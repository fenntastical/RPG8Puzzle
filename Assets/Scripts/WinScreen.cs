using System.Collections;
using System.Collections.Generic;
using UnityEngine;


//
// Author: Fenn Edmonds autumne@unr.edu
// Purpose: Controls the winscreen and actually also used for defeat screens
//

public class WinScreen : MonoBehaviour
{
    Animator ani;
    public GameObject animation;
    bool cutinDone = false;

    public GameObject b1;
    public GameObject b2;
    // Start is called before the first frame update
    void Start()
    {
        ani = animation.GetComponent<Animator>();
        b1.SetActive(false);
        b2.SetActive(false);
    }

    // Update is called once per frame
    void Update()
    {
        AnimatorStateInfo aInfo = ani.GetCurrentAnimatorStateInfo(0);
        float NTime = aInfo.normalizedTime;

        if (NTime > 1.0f)
        {
            b1.SetActive(true);
            b2.SetActive(true);
            cutinDone = true;
        }
    }
}
