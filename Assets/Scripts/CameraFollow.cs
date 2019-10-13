using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraFollow : MonoBehaviour
{
    private GameObject player;
    private Vector3 offset;

    // Start is called before the first frame update
    void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player");
        if (player == null) {
            Debug.LogError("Player not tagged for camera");
        } else {
            offset = transform.position - player.transform.position;
        }        
    }

    // Update is called once per frame
    void LateUpdate()
    {
        if (offset != null) {
            transform.position = player.transform.position + offset;
        }        
    }
}
