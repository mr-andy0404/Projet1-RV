using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AnimationAvion : MonoBehaviour
{

    public float dureeAnimationCollisionObstacle = 5f;
    private ParticleSystem particules;
    private bool fumer = false;

    public GameObject prefabExplosion;

    // Start is called before the first frame update
    void Start()
    {
        particules = GetComponent<ParticleSystem>();
    }

    // Update is called once per frame
    void Update()
    {
        if (fumer && !particules.isPlaying)
        {
            particules.Play();
            StartCoroutine(FaireDurerAnimation(dureeAnimationCollisionObstacle, particules));
        }
    }

    public void FaireFumer()
    {
        fumer = true;
        GetComponent<AirplaneController>().hud.GetComponent<Hud>().AnimerEcranRouge(dureeAnimationCollisionObstacle);
    }

    public void Exploser()
    {
        GetComponent<Rigidbody>().isKinematic = true;
        GameObject explosion = Instantiate<GameObject>(prefabExplosion, transform);
        ParticleSystem p = explosion.GetComponent<ParticleSystem>();
        if (!p.isPlaying)
        {
            p.Play();
        }
        StartCoroutine(FaireDurerAnimation(2f, p));
        //p.Stop();
        transform.GetChild(2).gameObject.SetActive(false);
    }

    IEnumerator FaireDurerAnimation(float secondes, ParticleSystem particleSystem)
    {
        yield return new WaitForSeconds(secondes);
        particleSystem.Stop();
        fumer = false;
    }
}
