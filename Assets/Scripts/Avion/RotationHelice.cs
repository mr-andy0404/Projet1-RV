using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RotationHelice : MonoBehaviour
{
    private float Acceleration;
    public float rotationSpeed = 1;
    private GameObject AC;
    // Start is called before the first frame update
    void Start()
    {
        AC = transform.parent.parent.parent.gameObject;
    }

    // Update is called once per frame
    void Update()
    {
        Acceleration = AC.GetComponent<AirplaneController>().thrustPercent;
        if (AC.GetComponent<BeControlled>().engineStart)
            transform.Rotate(1 + Acceleration * rotationSpeed * Time.deltaTime * AC.GetComponent<Rigidbody>().velocity.magnitude + AC.GetComponent<Rigidbody>().velocity.magnitude * 0.01f, 0, 0);
        else
            transform.rotation = transform.rotation;
    }
}
