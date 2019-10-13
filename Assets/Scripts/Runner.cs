using UnityEngine;

public class Runner : MonoBehaviour {

    public static float distanceTraveled;

    public float gameOverY;
    public float acceleration;
    public Vector3 jumpVelocity;

    private bool touchingPlatform;
    private Vector3 startPosition;

    private Rigidbody rb;
    private Animator animator;
    private SkinnedMeshRenderer renderer;

    void Start() {
        animator = GetComponent<Animator>();
        renderer = GetComponentInChildren<SkinnedMeshRenderer>();
        rb = GetComponent<Rigidbody>();

        GameEventManager.GameStart += GameStart;
        GameEventManager.GameOver += GameOver;
        startPosition = transform.localPosition;
        renderer.enabled = false;
        rb.isKinematic = true;
        enabled = false;
    }

    void Update() {
        if (touchingPlatform && Input.GetButtonDown("Jump")) {
            rb.AddForce(jumpVelocity, ForceMode.VelocityChange);
            animator.SetBool("isJumping", true);
            touchingPlatform = false;
        }
        distanceTraveled = transform.localPosition.x;

        if (transform.localPosition.y < gameOverY) {
            GameEventManager.TriggerGameOver();
        }
    }

    void FixedUpdate() {
        if (touchingPlatform) {
            rb.AddForce(acceleration, 0f, 0f, ForceMode.Acceleration);
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
        distanceTraveled = 0f;
        transform.localPosition = startPosition;
        renderer.enabled = true;
        rb.isKinematic = false;
        enabled = true;
    }

    private void GameOver() {
        renderer.enabled = false;
        rb.isKinematic = true;
        enabled = false;
    }
}