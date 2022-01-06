using System.Collections.Generic;
using UnityEngine;
using Mirror;

/*
	Documentation: https://mirror-networking.gitbook.io/docs/guides/networkbehaviour
	API Reference: https://mirror-networking.com/docs/api/Mirror.NetworkBehaviour.html
*/

// NOTE: Do not put objects in DontDestroyOnLoad (DDOL) in Awake.  You can do that in Start instead.

public class BeControlled : NetworkBehaviour
{
	public GameObject controlledBy;
	private AirplaneController AC;
    private PositionControl PC;
    private AudioSource sound;
    private Rigidbody RB;
    public float tauxDiminution = 0.0001f;
    public bool fire = false;
    public float vitesseMax = 150f;

    public bool engineStart = false;

    private void Start()
    {
        AC = GetComponent<AirplaneController>();
        sound = GetComponent<AudioSource>();
        RB = GetComponent<Rigidbody>();
    }

    private void Update()
    {
		if (controlledBy == null) return;
        if (PC == null) PC = controlledBy.GetComponent<PositionControl>();

        AC.Pitch = controlledBy.transform.position.x * ((AC.reversePitch ? 0 : 1) * 2 - 1);
        AC.Roll = controlledBy.transform.position.y * ((AC.reverseRoll ? 0 : 1) * 2 - 1);
        AC.Yaw = controlledBy.transform.position.z * ((AC.reverseYaw ? 0 : 1) * 2 - 1);

        if (PC.touch > 4) {
            engineStart = !engineStart;
        }

        if (engineStart && !sound.isPlaying) sound.Play();
        else if (!engineStart && sound.isPlaying) sound.Stop();

        if (PC.MicLoudness > 0.001 && engineStart)
            AC.thrustPercent = 1;
        else if (PC.MicLoudness <= 0.001 && AC.thrustPercent > 0.1 && engineStart)
            AC.thrustPercent -= tauxDiminution;
        else if (PC.MicLoudness <= 0.001 && AC.thrustPercent <= 0.1 && engineStart)
            AC.thrustPercent = 0;
        else if (!engineStart)
            AC.thrustPercent = 0;

        sound.pitch = RB.velocity.magnitude / 50 + 0.1f;

        if (PC.touch > 2 && PC.touch < 5) { AC.brakesTorque = 200; AC.thrustPercent = 0; }
        else { AC.brakesTorque = 0; }

        if (PC.touch > 0 && PC.touch < 3 && fire == false) { AC.Point(); fire = true; }
        else if (PC.touch == 0 && fire) { AC.Fire(); fire = false; }

        if(RB.velocity.magnitude > vitesseMax)
        {
            AC.thrustPercent = 0;
        }
    }
}
