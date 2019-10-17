using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TrailEmitter : MonoBehaviour
{
    TrailRenderer tr;
    private bool gameStarted = false;

    // Start is called before the first frame update
    void Start()
    {
        tr = GetComponent<TrailRenderer>();
        GameEventManager.GameStart += GameStart;
    }

    private void Update() {
        if (Runner.DistanceTravelled <= 1 && gameStarted) {
            Debug.Log("Clear trail");
            tr.Clear();
        }
    }    

    private void GameStart() {
        gameStarted = true;
    }
}
