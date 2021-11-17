using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Spawner : MonoBehaviour    
{
    public GameObject Companion;
    public void ToSpawn()
    {
         GameObject a = Instantiate(Companion, transform.position, Quaternion.identity);
         GameObject b = Instantiate(Companion, transform.position, Quaternion.identity);

        GameController.GetGameController().GetLevelData().m_CompanionsSpawned.Add(a.GetComponent<Companion>());
        GameController.GetGameController().GetLevelData().m_CompanionsSpawned.Add(b.GetComponent<Companion>());
    }
}
