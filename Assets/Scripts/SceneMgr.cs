using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

//
// Author: Fenn Edmonds autumne@unr.edu
// Purpose: Controls the scene switching mostly on buttons
//

public class SceneMgr : MonoBehaviour
{
    public int SceneToLoad;
    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {

    }

    public void LoadScene()
    {
        SceneManager.LoadScene(SceneToLoad);
    }

}
