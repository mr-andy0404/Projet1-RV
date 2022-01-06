using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AvionGyroscope : MonoBehaviour
{
    [Range(-1, 0)]
    [Tooltip("Caract?rise l??volution du coefficient d?interpolation : pour k = 0 le poids de la nouvelle rotation est ?gal ? l?augmentation relative de l?angle. " +
        "Pour k = -1, les petites rotations sont peu prises en compte et si l?augmentation relative de l?angle d?passe 0,5 le poids de la nouvelle rotation augmente brutalement.")]
    public float k = -0.5f;
    [Range(1, 90)]
    public float angleGrandeRotation = 45;
    public Vector3 angleDeDepart;
    protected Vector3 previousRotation;
    protected Quaternion positionOrigineGyro;
    public float pitch, roll, yaw;

    // Start is called before the first frame update
    void Start()
    {
        Screen.sleepTimeout = SleepTimeout.NeverSleep;
        Input.gyro.enabled = true;
        positionOrigineGyro = Quaternion.Inverse(Input.gyro.attitude);
        angleDeDepart = new Vector3(0, 0, 0);
        transform.rotation = Quaternion.Euler(angleDeDepart);
        previousRotation = angleDeDepart;
    }

    //public float sensibiliteGyroscope = 0.02f;
    public float speed = 20.0f;
    // Update is called once per frame
    void Update()
    {
        // Move object
        //transform.Translate(new Vector3(0, 0, Input.touchCount) * speed * Time.deltaTime);

        Quaternion r = Input.gyro.attitude * positionOrigineGyro;
        Vector3 rotEuler = r.eulerAngles;
        float temp = rotEuler.z;
        rotEuler.x = -rotEuler.x;
        rotEuler.z = -rotEuler.y;
        rotEuler.y = -temp;
        //rotEuler += angleDeDepart;

        Vector3 rotationFinale = filtrePetitesRotations(previousRotation, rotEuler);
        previousRotation = rotationFinale;

        //Debug.Log(rotationFinale.x + " , " + rotationFinale.y + " , " + rotationFinale.z);

        //transform.rotation = Quaternion.Euler(rotationFinale);

        pitch = rotationFinale.x + 360;
        if (pitch < 180) pitch = -pitch / 90;
        else pitch = (360 - pitch) / 90;

        roll = rotationFinale.z + 360;
        if (roll < 180) roll = -roll / 90;
        else roll = (360 - roll) / 90;

        yaw = rotationFinale.y + 360;
        if (yaw < 180) yaw = -yaw / 90;
        else yaw = (360 - yaw) / 90;
    }


    private Vector3 filtrePetitesRotations(Vector3 rotPrec, Vector3 rotCourante)
    {
        // Calcule un coefficient d'interpolation d?pendant de l'amplitude de la rotation :
        // une rotation forte provoque une interpolation plus proche de la nouvelle rotation
        float ecartDeRotation = Vector3.Dot(rotCourante - rotPrec, rotCourante - rotPrec);
        float amplitudeRotationRelative = Mathf.Min(1, ecartDeRotation / angleGrandeRotation);
        float coefInterpolation = ntsNormee(amplitudeRotationRelative);

        return Vector3.Lerp(rotPrec, rotCourante, coefInterpolation);
    }

    private float nts(float x)
    {
        return (x - k * x) / (k - 2 * k * Mathf.Abs(x) + 1);
    }

    private float ntsNormee(float x)
    {
        return (1 + nts(2 * x - 1)) / 2;
    }
}
