using UnityEngine;
using TMPro;

public class GUIManager : MonoBehaviour {

    private static GUIManager instance;

    public TextMeshProUGUI boostText, distanceText, gameOverText, instructionsText, runnerText, highscoreText;

    void Start() {
        instance = this;

        // Event Hooks
        GameEventManager.GameStart += GameStart;
        GameEventManager.GameOver += GameOver;

        gameOverText.enabled = false;
        distanceText.text = "";
        boostText.text = "";               
    }

    void Update() {
        if (Input.GetButtonDown("Jump")) {
            GameEventManager.TriggerGameStart();
        }

        if (Input.touchCount > 0) {
            Touch touch = Input.GetTouch(0);

            if (touch.phase == TouchPhase.Began) {
                GameEventManager.TriggerGameStart();
            }
        }
    }

    public static void SetBoosts(int boosts) {
        instance.boostText.text = boosts.ToString();
    }

    public static void SetDistance(float distance) {
        instance.distanceText.text = distance.ToString("f0");
    }

    // Events
    private void GameStart() {
        gameOverText.enabled = false;
        instructionsText.enabled = false;
        runnerText.enabled = false;
        enabled = false;

        if (PlayerPrefs.HasKey("HighScore")) {
            highscoreText.text = "High Score: " + PlayerPrefs.GetFloat("HighScore").ToString("f0");
        }
    }

    private void GameOver() {
        gameOverText.enabled = true;
        instructionsText.enabled = true;
        enabled = true;
    }
}