using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Noise : MonoBehaviour
{
    // Start is called before the first frame update
    public Rigidbody rig;
    public AudioSource sound;
    PumpkinHeadAI pumpkin;
    void Start()
    {
        pumpkin = FindObjectOfType<PumpkinHeadAI>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    private void OnCollisionEnter(Collision collision)
    {
        if (rig.velocity.magnitude > 0.001f)
        {
            if(!sound.isPlaying) sound.Play();
            pumpkin.noise = true;
            pumpkin.Noise(transform.position);
        }
    }
}
