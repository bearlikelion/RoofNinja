﻿using UnityEngine;
using TMPro;
using UnityEngine.EventSystems;

public class UIManager : MonoBehaviour {

    private static UIManager instance;

    public TextMeshProUGUI boostText, distanceText, gameOverText, instructionsText, runnerText, highscoreText, muteButtonText, fpsText;

    private AudioSource[] music;
    private EventSystem es;

    private bool gameisRunning = false;

    void Start() {
        instance = this;

        music = Camera.main.GetComponents<AudioSource>();
        es = GetComponentInChildren<EventSystem>();

        gameOverText.enabled = false;
        distanceText.text = "";
        boostText.text = "";

        UpdateHighScore();

        // Event Hooks
        GameEventManager.GameStart += GameStart;
        GameEventManager.GameOver += GameOver;
    }

    void Update() {
        // Calculate framerate 
        int current = 0;
        current = (int)(1f / Time.unscaledDeltaTime);
        fpsText.text = current.ToString() + "FPS";

        if (!gameisRunning) {
            if (Input.GetButtonDown("Jump")) {
                GameEventManager.TriggerGameStart();
            }

            if (Input.touchCount > 0 && !IsTouchingUI()) {
                Touch touch = Input.GetTouch(0);

                if (touch.phase == TouchPhase.Began) {
                    GameEventManager.TriggerGameStart();
                }
            }
        }

        // Live update highscore
        if (PlayerPrefs.HasKey("HighScore")) {
            if (Runner.DistanceTravelled > PlayerPrefs.GetFloat("HighScore")) {
                highscoreText.text = "High Score: " + Runner.DistanceTravelled.ToString("f0");
            }
        }
    }

    void UpdateHighScore() {
        // Show Highschore
        if (PlayerPrefs.HasKey("HighScore")) {
            highscoreText.text = "High Score: " + PlayerPrefs.GetFloat("HighScore").ToString("f0");
        } else {
            highscoreText.text = "";
        }
    }

    public void ToggleMusic() {
        foreach(AudioSource sound in music) {
            if (sound.mute) {
                sound.mute = false;
                muteButtonText.color = new Color(255f, 255f, 255f, 255f);
            } else {
                sound.mute = true;
                muteButtonText.color = new Color(255f, 255f, 255f, 0.5f);
            }
        }        
    }    

    public static bool IsTouchingUI() {
        if (instance.es.IsPointerOverGameObject(Input.GetTouch(0).fingerId)) {
            return true;
        } else {
            return false;
        }        
    }

    public static void SetBoosts(int boosts) {
        instance.boostText.text = "Boosts: " + boosts.ToString();
    }

    public static void SetDistance(float distance) {
        instance.distanceText.text = distance.ToString("f0");
    }

    // Events
    private void GameStart() {
        UpdateHighScore();
        gameOverText.enabled = false;
        instructionsText.enabled = false;
        runnerText.enabled = false;        
        gameisRunning = true;
    }

    private void GameOver() {
        gameOverText.enabled = true;
        instructionsText.enabled = true;        
        gameisRunning = false;
    }
}