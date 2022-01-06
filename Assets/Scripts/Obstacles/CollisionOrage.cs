using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CollisionOrage : Obstacle
{
    private DigitalRuby.LightningBolt.LightningBoltScript eclair;

    // Start is called before the first frame update
    void Start()
    {
        animationAvion = avion.GetComponent<AnimationAvion>();
        eclair = transform.GetChild(9).GetComponent<DigitalRuby.LightningBolt.LightningBoltScript>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.name == avion.transform.GetChild(0).name)
        {
            eclair.Trigger();
            Debug.Log(animationAvion);
            //avion.GetComponent<Rigidbody>().AddForce(0, 0, -1000);
            avion.GetComponent<Rigidbody>().velocity /= 2;
            animationAvion.FaireFumer();
        }
        if (other.name == bullet.transform.GetChild(0).name)
        {
            gameObject.SetActive(false);
        }
    }
}
