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
                rb.AddForce(jumpVelocity, ForceMode.VelocityChange);
                animator.SetBool("isJumping", true);
                touchingPlatform = false;
            } else if (boosts > 0) {                
                rb.AddForce(boostVelocity, ForceMode.VelocityChange);
                animator.SetBool("isBoosting", true);
                StartCoroutine(StopBoost());
                boosts -= 1;
            }
        }
        
        DistanceTravelled = transform.localPosition.x; // Save distance travelled

        // Check for gameoverY
        if (transform.localPosition.y < gameOverY) {
            GameEventManager.TriggerGameOver();
        }
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
        DistanceTravelled = 0f;
        transform.localPosition = StartPosition;
        meshRenderer.enabled = true;
        rb.isKinematic = false;
        enabled = true;
    }

    private void GameOver() {
        meshRenderer.enabled = false;
        rb.isKinematic = true;
        enabled = false;
    }

    public static void AddBoost() {
        boosts += 1;
    }

    IEnumerator StopBoost() {
        yield return new WaitForSeconds(1.5f);
        animator.SetBool("isBoosting", false);
    }
}