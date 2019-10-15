using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MarkerManager : MonoBehaviour
{
    public GameObject lastRunMarker;
    public GameObject highScoreMarker;

    private GameObject _lastRun;
    private GameObject _highScore;

    // Start is called before the first frame update
    void Start()
    {
        // Event hooks
        GameEventManager.GameStart += GameStart;
        GameEventManager.GameOver += GameOver;        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    void GameStart() {
        if (PlayerPrefs.HasKey("HighScore")) {
            _highScore = Instantiate(highScoreMarker, new Vector3(PlayerPrefs.GetFloat("HighScore"), 0f, 0f), Quaternion.identity);
        } else {
            Debug.Log("No high score");
        }

        if (PlayerPrefs.HasKey("LastRun")) {
            if (PlayerPrefs.HasKey("HighScore") && PlayerPrefs.GetFloat("HighScore") != PlayerPrefs.GetFloat("LastRun")) {
                _lastRun = Instantiate(lastRunMarker, new Vector3(PlayerPrefs.GetFloat("LastRun"), 0f, 0f), Quaternion.identity);
            }            
        } else {
            Debug.Log("No last run");
        }

        
    }

    void GameOver() {
        if (_highScore != null) {
            Destroy(_highScore);
        }
        if (_lastRun != null) {
            Destroy(_lastRun);
        }                
    }
}
