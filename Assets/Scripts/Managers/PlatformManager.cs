using UnityEngine;
using System.Collections.Generic;
using System.Collections;

public class PlatformManager : MonoBehaviour {

    public Booster booster;
    public Transform prefab;
    public int numberOfObjects;
    public float recycleOffset;
    public Vector3 startPosition;
    public Vector3 minSize, maxSize, minGap, maxGap;
    public float minY, maxY;

    public Transform parent;

    public Texture[] textures;
    public Material[] materials;    
    public PhysicMaterial[] physicMaterials;

    private float _minGapX, _maxGapX;
    private bool diffTick = false;

    private Vector3 nextPosition;
    private Queue<Transform> objectQueue;

    void Start() {
        // Store default diffulty
        _minGapX = minGap.x;
        _maxGapX = maxGap.x;

        objectQueue = new Queue<Transform>(numberOfObjects);
        for (int i = 0; i < numberOfObjects; i++) {
            objectQueue.Enqueue(Instantiate(prefab, new Vector3(0f, 0f, -100f), Quaternion.identity, parent));
        }
        enabled = false;

        // Event hooks
        GameEventManager.GameStart += GameStart;
        GameEventManager.GameOver += GameOver;
    }

    void Update() {
        if (objectQueue.Peek().localPosition.x + recycleOffset < Runner.DistanceTravelled) {
            Recycle();
        }

        if (Mathf.Round(Runner.DistanceTravelled) % 100 == 0 && !diffTick) {
            IncreaseDifficulty();
        }
    }

    private void IncreaseDifficulty() {
        Debug.Log("Increase Gap");        
        StartCoroutine(DifficultyTick());
        if (minGap.x < 10) {
            minGap.x += 0.15f;
        }

        if (maxGap.x < 15) {
            maxGap.x += 0.25f;
        }        
    }

    IEnumerator DifficultyTick() {
        diffTick = true;
        yield return new WaitForSeconds(1.0f);
        diffTick = false;
    }
    
    private void GameStart() {
        minGap.x = _minGapX;
        maxGap.x = _maxGapX;

        // StartCoroutine(IncreaseDifficulty());

        nextPosition = startPosition;
        for (int i = 0; i < numberOfObjects; i++) {
            Recycle();
        }

        enabled = true;
    }

    private void GameOver() {
        enabled = false;
    }

    private void Recycle() {
        Vector3 scale = new Vector3(
            Random.Range(minSize.x, maxSize.x),
            Random.Range(minSize.y, maxSize.y),
            Random.Range(minSize.z, maxSize.z));

        Vector3 position = nextPosition;
        position.x += scale.x * 0.5f;
        position.y += scale.y * 0.5f;

        booster.SpawnIfAvailable(position);

        Transform o = objectQueue.Dequeue();
        o.localScale = scale;
        o.localPosition = position;

        int materialIndex = Random.Range(0, materials.Length);
        int textureIndex = Random.Range(0, textures.Length);
        int textureScale = Random.Range(1, 5);

        o.GetComponent<Collider>().material = physicMaterials[materialIndex];

        Renderer renderer = o.GetComponent<Renderer>();        
        renderer.material = materials[materialIndex];        
        renderer.material.SetTexture("_MainTex", textures[textureIndex]);        
        renderer.material.mainTextureScale = new Vector2(textureScale, textureScale);

        objectQueue.Enqueue(o);

        nextPosition += new Vector3(
            Random.Range(minGap.x, maxGap.x) + scale.x,
            Random.Range(minGap.y, maxGap.y),
            Random.Range(minGap.z, maxGap.z));

        if (nextPosition.y < minY) {
            nextPosition.y = minY + maxGap.y;
        } else if (nextPosition.y > maxY) {
            nextPosition.y = maxY - maxGap.y;
        }
    }

}