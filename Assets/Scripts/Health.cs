using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Health : MonoBehaviour
{
    public GameObject healthSprite;
    public List<GameObject> healthList;
    public int health;
    public GameMgr gameMgr;


    // Start is called before the first frame update
    void Start()
    {
        // for (int i = 0; i <= health; i++)
        //     healthList.Add(healthSprite);

        // int spawnTracker = 0;
        // foreach (GameObject hp in healthList)
        // {

        //     Vector3 spawnpos = this.transform.position;
        //     spawnpos.y += 20;
        //     switch(spawnTracker)
        //     {
        //         case 0:
        //             spawnpos.x -= 20;
        //         break;
        //         case 1:
        //             spawnpos.x += 0;
        //         break;
        //         case 2:
        //             spawnpos.x += 20;
        //         break;
        //     }
            
        // }
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    void DealDamage(int damage)
    {
        int newHealth = health - damage;
        
    }
}
