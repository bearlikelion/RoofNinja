using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MarkerSound : MonoBehaviour
{
    private AudioSource audio;

    // Start is called before the first frame update
    void Start()
    {
        audio = GetComponent<AudioSource>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnTriggerEnter(Collider other) {
        audio.Play();
    }
}
