using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EclairsAleatoires : MonoBehaviour
{
    [Range(0, 100)]
    public int frequenceEclair;
    private DigitalRuby.LightningBolt.LightningBoltScript eclair;
    // Start is called before the first frame update
    void Start()
    {
        eclair = transform.GetComponentInParent<DigitalRuby.LightningBolt.LightningBoltScript>();
    }

    // Update is called once per frame
    void Update()
    {
        if (0 < frequenceEclair && Random.Range(1, 100) < frequenceEclair)
        {
            eclair.Trigger();
        }
    }
}
