using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Disappear : MonoBehaviour
{
    private float startTime;
    private Quaternion startRot;
    // Start is called before the first frame update
    void Start()
    {
        startTime = Time.unscaledTime;
        transform.Rotate(90, 0, 0);
        startRot = transform.rotation;
        

    }

    // Update is called once per frame
    void Update()
    {
        transform.rotation = startRot;
        if (Time.unscaledTime - startTime > 3)
            Destroy(gameObject);
    }
}
