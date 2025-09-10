using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HintnSolveTracker : MonoBehaviour
{
    public int baseHints = 3;
    public int baseSolves = 2;
    // Start is called before the first frame update
    void Start()
    {
        DontDestroyOnLoad(gameObject);
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
