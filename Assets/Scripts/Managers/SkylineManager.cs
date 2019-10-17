using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SkylineManager : MonoBehaviour
{    
    public Transform prefab;
    public int numberOfObjects;
    public float recycleOffset;
    public Vector3 startPosition;
    public Vector3 minSize, maxSize, minGap, maxGap;
    
    public Material[] materials;
    public Transform parent;

    private Vector3 nextPosition;
    private Queue<Transform> objectQueue;

    void Start() {
        GameEventManager.GameStart += GameStart;
        GameEventManager.GameOver += GameOver;        

        objectQueue = new Queue<Transform>(numberOfObjects);
        for (int i = 0; i < numberOfObjects; i++) {
            objectQueue.Enqueue((Transform)Instantiate(prefab, new Vector3(0f, 0f, -100f), Quaternion.identity, parent));
        }

        GameStart();

        // enabled = false;
    }

    // Update is called once per frame
    void Update()
    {
        if (Runner.DistanceTravelled > 2) {
            if (objectQueue.Peek().localPosition.x + recycleOffset < Runner.DistanceTravelled) {
                Recycle();
            }
        }        
    }

    private void Recycle() {
        Vector3 scale = new Vector3(
            Random.Range(minSize.x, maxSize.x),
            Random.Range(minSize.y, maxSize.y),
            Random.Range(minSize.z, maxSize.z));

        Vector3 position = nextPosition;
        position.x += scale.x * 0.5f;
        position.y += scale.y * 0.5f;

        Transform o = objectQueue.Dequeue();
        o.localScale = scale;
        o.localPosition = position;

        if (materials.Length > 0) {
            int materialIndex = Random.Range(0, materials.Length);
            Renderer renderer = o.GetComponent<Renderer>();
            renderer.material = materials[materialIndex];
            int textureScale = Random.Range(10, 50);
            renderer.material.mainTextureScale = new Vector2(scale.x/textureScale, scale.y/textureScale);
        }        

        nextPosition += new Vector3(
            Random.Range(minGap.x, maxGap.x) + scale.x,
            Random.Range(minGap.y, maxGap.y),
            Random.Range(minGap.z, maxGap.z));
        objectQueue.Enqueue(o);
    }

    private void GameStart() {
        nextPosition = startPosition;
        for (int i = 0; i < numberOfObjects; i++) {
            Recycle();
        }
        enabled = true;
    }

    private void GameOver() {
        enabled = false;
    }
}
