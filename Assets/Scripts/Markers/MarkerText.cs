using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MarkerText : MonoBehaviour
{
    private GameObject player;
    private Renderer meshRenderer;

    // Start is called before the first frame update
    void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player");
        meshRenderer = GetComponent<Renderer>();
    }

    // Update is called once per frame
    void Update()
    {
        if (!meshRenderer.isVisible) {
            transform.position = new Vector3(transform.position.x, player.transform.position.y + 4, transform.position.z);
        }        
    }
}
