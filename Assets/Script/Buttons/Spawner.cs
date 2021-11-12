using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Spawner : MonoBehaviour    
{
    public GameObject Companion;
    public void ToSpawn()
    {
        Instantiate(Companion, transform.position, Quaternion.identity);
    }
}
