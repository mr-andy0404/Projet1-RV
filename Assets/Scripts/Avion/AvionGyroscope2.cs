using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// Ancienne version pour un contrôle de la rotation en position. Efficace pour faire des tests.
public class AvionGyroscope2 : MonoBehaviour
{
    [Range(-1, 0)]
    [Tooltip("Caractérise l’évolution du coefficient d’interpolation : pour k = 0 le poids de la nouvelle rotation est égal à l’augmentation relative de l’angle. " +
        "Pour k = -1, les petites rotations sont peu prises en compte et si l’augmentation relative de l’angle dépasse 0,5 le poids de la nouvelle rotation augmente brutalement.")]
    public float k = -0.5f;
    [Range(1, 90)]
    public float angleGrandeRotation = 45;
    public Vector3 angleDeDepart;
    protected Vector3 previousRotation;
    protected Quaternion positionOrigineGyro;


    public GameObject hud;
    private Hud affichage;


    // Start is called before the first frame update
    void Start()
    {
        Input.gyro.enabled = true;
        positionOrigineGyro = Quaternion.Inverse(Input.gyro.attitude);
        angleDeDepart = new Vector3(0, 0, 0);
        transform.rotation = Quaternion.Euler(angleDeDepart);
        previousRotation = angleDeDepart;


        affichage = hud.GetComponent<Hud>();
    }

    //public float sensibiliteGyroscope = 0.02f;
    public float speed = 20.0f;
    private float vitesse, poussee;
    public bool bloque = false;
    // Update is called once per frame
    void Update()
    {
        if (!bloque)
        {
            // Move object
            transform.Translate(new Vector3(0, 0, Input.touchCount) * speed * Time.deltaTime);

            poussee = Input.touchCount;
            vitesse = poussee * speed;
            poussee *= 100;
            affichage.ParametresDeVol(vitesse, poussee);

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

            transform.rotation = Quaternion.Euler(rotationFinale);
        }
    }


    private Vector3 filtrePetitesRotations(Vector3 rotPrec, Vector3 rotCourante)
    {
        // Calcule un coefficient d'interpolation dépendant de l'amplitude de la rotation :
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
