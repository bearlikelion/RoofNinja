using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MarkerSound : MonoBehaviour
{
    private AudioSource AudioSource;

    // Start is called before the first frame update
    void Start()
    {
        AudioSource = GetComponent<AudioSource>();
        if (AudioSource == null) {
            Debug.Log("MarkerSound Missing AudioSource on " + gameObject.name);
        }
    }

    private void OnTriggerEnter(Collider other) {
        if (AudioSource != null) {
            AudioSource.Play();
        }        
    }
}
