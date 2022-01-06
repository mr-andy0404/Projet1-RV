using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class staticNetInfo : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        //Instantiate(NetworkInfo.networkManager);
        Instantiate(NetworkInfo.player);
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
