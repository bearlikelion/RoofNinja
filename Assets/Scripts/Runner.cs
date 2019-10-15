using UnityEngine;
using System.Collections;

public class Runner : MonoBehaviour {

    public static float DistanceTravelled;
    private static int boosts;

    public float gameOverY;
    public float Acceleration;
    public Vector3 boostVelocity, jumpVelocity;

    private bool touchingPlatform;
    private Vector3 StartPosition;

    private Rigidbody rb;
    private Animator animator;
    private SkinnedMeshRenderer meshRenderer;

    void Start() {
        // Set variables
        rb = GetComponent<Rigidbody>();
        animator = GetComponent<Animator>();
        meshRenderer = GetComponentInChildren<SkinnedMeshRenderer>();

        // Start defaults
        StartPosition = transform.localPosition;
        meshRenderer.enabled = false;
        rb.isKinematic = true;
        enabled = false;

        // Event hooks
        GameEventManager.GameStart += GameStart;
        GameEventManager.GameOver += GameOver;        
    }

    void Update() {
        // Jump
        if (Input.GetButtonDown("Jump")) {
            if (touchingPlatform) {
                PlayerJump();
            } else if (boosts > 0) {
                PlayerBoost();
            }
        }

        if (Input.touchCount > 0) {
            Touch touch = Input.GetTouch(0);

            if (touch.phase == TouchPhase.Began) {
                if (touchingPlatform) {
                    PlayerJump();
                } else if (boosts > 0) {
                    PlayerBoost();
                }
            }
        }
        
        DistanceTravelled = transform.localPosition.x; // Save distance travelled
        GUIManager.SetDistance(DistanceTravelled);

        // Check for gameoverY
        if (transform.localPosition.y < gameOverY) {
            GameEventManager.TriggerGameOver();
        }
    }

    void PlayerJump() {
        rb.AddForce(jumpVelocity, ForceMode.VelocityChange);
        animator.SetBool("isJumping", true);
        touchingPlatform = false;
    }

    void PlayerBoost() {
        rb.AddForce(boostVelocity, ForceMode.VelocityChange);
        animator.SetBool("isBoosting", true);
        StartCoroutine(StopBoost());
        boosts -= 1;
        GUIManager.SetBoosts(boosts);
    }

    void FixedUpdate() {
        // Move runnner by Acceleration
        if (touchingPlatform) {
            rb.AddForce(Acceleration, 0f, 0f, ForceMode.Acceleration);
        }
    }

    void OnCollisionEnter(Collision collision) {
        animator.SetBool("isJumping", false);
        touchingPlatform = true;
    }

    void OnCollisionExit(Collision collision) {
        touchingPlatform = false;
    }

    private void GameStart() {
        boosts = 0;
        GUIManager.SetBoosts(boosts);
        DistanceTravelled = 0f;
        GUIManager.SetDistance(DistanceTravelled);
        transform.localPosition = StartPosition;
        meshRenderer.enabled = true;
        rb.isKinematic = false;
        enabled = true;
    }

    private void GameOver() {
        PlayerPrefs.SetFloat("LastRun", DistanceTravelled); // Set last run
        
        if (!PlayerPrefs.HasKey("HighScore") || DistanceTravelled > PlayerPrefs.GetFloat("HighScore")) {
            PlayerPrefs.SetFloat("HighScore", DistanceTravelled);
        }

        meshRenderer.enabled = false;
        rb.isKinematic = true;
        enabled = false;
    }

    public static void AddBoost() {
        boosts += 1;
        GUIManager.SetBoosts(boosts);
    }

    IEnumerator StopBoost() {
        yield return new WaitForSeconds(1.5f);
        animator.SetBool("isBoosting", false);
    }
}