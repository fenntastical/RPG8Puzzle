using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

//
// Author: Fenn Edmonds autumne@unr.edu
// Purpose: Controls player health
//

public class PlayerHealth : MonoBehaviour
{

    public List<GameObject> healthList;
    public int health;
    // public BossMgr BMgr;
    // public DaggerBossMgr dBMgr;

    public bool dagger;
    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {

    }

    public void DealDamage(int damage)
    {
        int newHealth = health - damage;
        if (newHealth > 0)
        {
            while (health != newHealth)
            {
                healthList[health - 1].SetActive(false);
                health -= 1;
            }
        }

        if (newHealth <= 0)
        {

            if (dagger == false)
                SceneManager.LoadScene(6);

            if (dagger)
                SceneManager.LoadScene(8);


        }

    }
}
