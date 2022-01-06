using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class AirplaneController : MonoBehaviour
{
    [SerializeField]
    List<AeroSurface> controlSurfaces = null;
    [SerializeField]
    List<WheelCollider> wheels = null;
    [SerializeField]
    float rollControlSensitivity = 0.2f;
    [SerializeField]
    float pitchControlSensitivity = 0.2f;
    [SerializeField]
    float yawControlSensitivity = 0.2f;

    [Range(-1, 1)]
    public float Pitch;
    [Range(-1, 1)]
    public float Yaw;
    [Range(-1, 1)]
    public float Roll;
    [Range(0, 1)]
    public float Flap;

    public bool reversePitch, reverseYaw, reverseRoll;

    public GameObject hud;
    private Hud affichage;

    public float thrustPercent;
    public float brakesTorque;
    public GameObject bullet;
    public float bulletSpeed;
    LineRenderer pointeur;

    //protected NowControlling myController;

    AircraftPhysics aircraftPhysics;
    Rigidbody rb;
    public GameObject cible;

    private void Start()
    {
        aircraftPhysics = GetComponent<AircraftPhysics>();
        rb = GetComponent<Rigidbody>();
        // myController = GetComponent<NowControlling>();
        pointeur = GetComponent<LineRenderer>();
        affichage = hud.GetComponent<Hud>();
    }

    private void Update()
    {
        //Pitch = Input.GetAxis("Vertical");
        //Roll = Input.GetAxis("Horizontal");
        //Yaw = Input.GetAxis("Yaw");

        //if (Input.GetKeyDown(KeyCode.Space))
        //{
        //    thrustPercent = thrustPercent > 0 ? 0 : 1f;
        //}

        if (Input.GetKeyDown(KeyCode.F))
        {
            Flap = Flap > 0 ? 0 : 0.3f;
        }

        //if (Input.GetKeyDown(KeyCode.Space))
        //{
        //    Point();
        //}

        //if (Input.GetKeyUp(KeyCode.Space))
        //{
        //    Fire();
        //}

        pointeur.SetPosition(0, transform.position);
        pointeur.SetPosition(1, cible.transform.position);

        //if (Input.GetKeyDown(KeyCode.B))
        //{
        //    brakesTorque = brakesTorque > 0 ? 0 : 100f;
        //}

        affichage.ParametresDeVol(rb.velocity.magnitude, aircraftPhysics.thrust);
    }

    private void FixedUpdate()
    {
        SetControlSurfecesAngles(Pitch, Roll, Yaw, Flap);
        aircraftPhysics.SetThrustPercent(thrustPercent);
        foreach (var wheel in wheels)
        {
            wheel.brakeTorque = brakesTorque;
            // small torque to wake up wheel collider
            wheel.motorTorque = 0.01f;
        }
    }

    public void SetControlSurfecesAngles(float pitch, float roll, float yaw, float flap)
    {
        foreach (var surface in controlSurfaces)
        {
            if (surface == null || !surface.IsControlSurface) continue;
            switch (surface.InputType)
            {
                case ControlInputType.Pitch:
                    surface.SetFlapAngle(pitch * pitchControlSensitivity * surface.InputMultiplyer);
                    break;
                case ControlInputType.Roll:
                    surface.SetFlapAngle(roll * rollControlSensitivity * surface.InputMultiplyer);
                    break;
                case ControlInputType.Yaw:
                    surface.SetFlapAngle(yaw * yawControlSensitivity * surface.InputMultiplyer);
                    break;
                case ControlInputType.Flap:
                    surface.SetFlapAngle(flap * surface.InputMultiplyer);
                    break;
            }
        }
    }

    private void OnDrawGizmos()
    {
        if (!Application.isPlaying)
            SetControlSurfecesAngles(Pitch, Roll, Yaw, Flap);
    }

    public void Point()
    {
        pointeur.enabled = true;
    }

    public void Fire()
    {
        GameObject myBullet = Instantiate(bullet);
        myBullet.transform.SetPositionAndRotation(transform.GetChild(5).position, transform.rotation);
        myBullet.GetComponent<Rigidbody>().velocity = myBullet.transform.forward * bulletSpeed + rb.velocity;
        pointeur.enabled = false;
    }
}
