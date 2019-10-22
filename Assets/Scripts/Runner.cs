using UnityEngine;
using System.Collections;

public class Runner : MonoBehaviour {

    public static float DistanceTravelled;
    private static int boosts;

    public AudioClip jumpSound, boostSound, climbSound, deathSound;
    public AudioSource footsteps;

    public float gameOverY;
    public float Acceleration;    
    public Vector3 boostVelocity, jumpVelocity, climbVelocity;
    
    private bool touchingPlatform, canBoost, canClimb;
    private Vector3 StartPosition;

    private Rigidbody rb;
    private Animator animator;
    private AudioSource AudioSource;
    private SkinnedMeshRenderer meshRenderer;

    void Start() {
        // Set variables        
        rb = GetComponent<Rigidbody>();       
        animator = GetComponent<Animator>();
        AudioSource = GetComponent<AudioSource>();
        meshRenderer = GetComponentInChildren<SkinnedMeshRenderer>();

        // Start defaults
        StartPosition = transform.localPosition;
        meshRenderer.enabled = false;
        rb.isKinematic = true;
        canClimb = true;
        enabled = false;

        // Event hooks
        GameEventManager.GameStart += GameStart;
        GameEventManager.GameOver += GameOver;        
    }

    void Update() {
        // Jump with Space
        if (Input.GetButtonDown("Jump")) {
            if (touchingPlatform) {
                if (rb.velocity.x > 0) {
                    PlayerJump();
                } else {
                    PlayerClimb();                    
                }
                
            } else if (boosts > 0 && canBoost) {
                PlayerBoost();
            }
        }

        // Jump with Touch
        if (Input.touchCount > 0 && !UIManager.IsTouchingUI()) {
            Touch touch = Input.GetTouch(0);

            if (touch.phase == TouchPhase.Began) {
                if (touchingPlatform) {
                    if (rb.velocity.x > 0) {
                        PlayerJump();
                    } else {
                        PlayerClimb();
                    }                    
                } else if (boosts > 0 && canBoost) {
                    PlayerBoost();
                }
            }
        }
        
        DistanceTravelled = transform.localPosition.x; // Save distance travelled
        UIManager.SetDistance(DistanceTravelled);

        // Check for gameoverY
        if (transform.localPosition.y < gameOverY) {
            GameEventManager.TriggerGameOver();
        }
    }

    void PlayerClimb() {
        if (canClimb) {            
            canClimb = false;
            rb.AddForce(climbVelocity, ForceMode.VelocityChange);
            AudioSource.PlayOneShot(climbSound);
            animator.SetBool("isClimbing", true);
            StartCoroutine(StopClimb());
        }        
    }

    void PlayerJump() {
        rb.AddForce(jumpVelocity, ForceMode.VelocityChange);
        animator.SetBool("isJumping", true);
        AudioSource.PlayOneShot(jumpSound);
        touchingPlatform = false;
    }

    void PlayerBoost() {
        rb.AddForce(boostVelocity, ForceMode.VelocityChange);
        animator.SetBool("isBoosting", true);
        AudioSource.PlayOneShot(boostSound);
        StartCoroutine(StopBoost());
        canBoost = false;
        boosts -= 1;

        UIManager.SetBoosts(boosts);
    }

    void FixedUpdate() {
        // Move runnner by Acceleration
        if (touchingPlatform) {
            rb.AddForce(Acceleration, 0f, 0f, ForceMode.Acceleration);
        }
    }

    void OnCollisionEnter(Collision collision) {
        footsteps.Play();
        animator.SetBool("isJumping", false);
        touchingPlatform = true;
        canBoost = true;
    }

    void OnCollisionExit(Collision collision) {
        footsteps.Stop();
        touchingPlatform = false;        
    }

    private void GameStart() {
        boosts = 0;
        UIManager.SetBoosts(boosts);
        DistanceTravelled = 0f;
        UIManager.SetDistance(DistanceTravelled);
        transform.localPosition = StartPosition;
        meshRenderer.enabled = true;
        rb.isKinematic = false;
        enabled = true;
    }

    private void GameOver() {
        PlayerPrefs.SetFloat("LastRun", DistanceTravelled); // Set last run
        
        // Update highscore
        if (!PlayerPrefs.HasKey("HighScore") || DistanceTravelled > PlayerPrefs.GetFloat("HighScore")) {
            PlayerPrefs.SetFloat("HighScore", DistanceTravelled);
        }

        AudioSource.PlayOneShot(deathSound);
        meshRenderer.enabled = false;
        rb.isKinematic = true;
        enabled = false;
    }

    public static void AddBoost() {
        boosts += 1;
        UIManager.SetBoosts(boosts);
    }

    IEnumerator StopBoost() {
        yield return new WaitForSeconds(1.5f);
        animator.SetBool("isBoosting", false);
    }

    IEnumerator StopClimb() {
        yield return new WaitForSeconds(1.36f); // HACK: Magic number
        animator.SetBool("isClimbing", false);
        canClimb = true;
    }
}