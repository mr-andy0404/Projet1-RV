using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CollisionOiseaux : Obstacle
{
    // Start is called before the first frame update
    void Start()
    {
        animationAvion = avion.GetComponent<AnimationAvion>();
    }

    // Update is called once per frame
    void Update()
    {

    }
    private void OnTriggerEnter(Collider other)
    {
        if (other.name == avion.transform.GetChild(0).name)
        {
            animationAvion.FaireFumer();
            //avion.GetComponent<Rigidbody>().AddForce(0, 0, -100);
            avion.GetComponent<Rigidbody>().velocity /= 2;
        }
        if (other.name == bullet.transform.GetChild(0).name)
        {
            gameObject.SetActive(false);
        }
    }
}
