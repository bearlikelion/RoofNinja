using UnityEngine;

public class Booster : MonoBehaviour {

    public Vector3 offset, rotationVelocity;
    public float recycleOffset, spawnChance;

    public AudioClip pickupSound;

    private AudioSource _audioSource;

    void Start() {
        _audioSource = GameObject.FindGameObjectWithTag("Player").GetComponent<AudioSource>();
        GameEventManager.GameOver += GameOver;
        gameObject.SetActive(false);
    }

    void Update() {
        if (transform.localPosition.x + recycleOffset < Runner.DistanceTravelled) {
            gameObject.SetActive(false);
            return;
        }
        transform.Rotate(rotationVelocity * Time.deltaTime);
    }

    public void SpawnIfAvailable(Vector3 position) {
        if (gameObject.activeSelf || spawnChance <= Random.Range(0f, 100f)) {
            return;
        }
        transform.localPosition = position + offset;
        gameObject.SetActive(true);
    }

    private void GameOver() {
        gameObject.SetActive(false);
    }

    void OnTriggerEnter() {
        Runner.AddBoost();
        _audioSource.PlayOneShot(pickupSound);
        gameObject.SetActive(false);
    }
}