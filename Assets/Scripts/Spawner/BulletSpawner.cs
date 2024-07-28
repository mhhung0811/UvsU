using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BulletSpawner : Spawner 
{
    public static BulletSpawner Instance;
    // Start is called before the first frame update
    private void Awake()
    {
        if(Instance == null)
        {
            Instance = this;
        }
        else 
        {
            Debug.Log("More than one BulletSpawner");
            Destroy(gameObject);
        }

    }
    
}
