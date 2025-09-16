using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

//
// Author: Fenn Edmonds autumne@unr.edu
// Purpose: Controls the health of dagger enemies, Sorry this is also named poorly
//

public class DaggerHealth : MonoBehaviour
{
    // public GameObject healthSprite;
    public List<GameObject> healthList;
    public int health;
    public DaggerGameMgr gameMgr;

    public DaggerBossMgr BMgr;

    public bool boss;


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
            if (!boss)
            {
                gameMgr.monsterDefeated = true;
                gameObject.SetActive(false);
            }
            if (boss)
            {
                BMgr.monsterDefeated = true;
                gameObject.SetActive(false);
            }

        }

    }
}
