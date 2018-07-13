using UnityEngine;
using System.Collections;

public class Magnet : MonoBehaviour
{

    // Use this for initialization

    public float shakeAmount;
    public float shakeLength;
    public float whenToShake;
    private float time;
    public CameraShake CamShake;
    void Start()
    {
        time = 0;
    }

    // Update is called once per frame
    void Update()
    {
        time += Time.deltaTime;
        if (time >= whenToShake)
        {
            CamShake.Shake(shakeAmount, shakeLength);
            time = -1000000000;
        }
    }
}
