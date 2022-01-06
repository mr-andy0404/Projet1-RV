using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CollisionAvion : MonoBehaviour
{

    public GameObject gestionCourse;
    bool startup = true;
    float chrono = 0;

    // Start is called before the first frame update
    void Start()
    {
        GetComponent<AircraftPhysics>().thrust = 10000;
    }

    // Update is called once per frame
    void Update()
    {
        if(startup && chrono < 15)
        {
            chrono += Time.deltaTime;
        }
        if(chrono >= 15)
        {
            startup = false;
            GetComponent<AircraftPhysics>().thrust = 5000;
        }
    }

    public void OnCollisionEnter(Collision other)
    {
        //Debug.Log('a');
        if (other.collider.GetComponent<Terrain>() != null && !startup)
        {
            //Debug.Log('b');
            GetComponent<AnimationAvion>().Exploser();
            gestionCourse.GetComponent<GestionCourse>().DeclencherDefaite();
            //Destroy(transform.parent.gameObject);
        }

    }
}
