using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MovingPlatform : MonoBehaviour
{
    [SerializeField] Transform _targetA;
    [SerializeField] Transform _targetB;
    [SerializeField] float _speed = 1.0f;

    private bool _switching = false;    

    // Update is called once per frame
    void FixedUpdate()
    {
        if (!_switching) {
            transform.position = Vector3.MoveTowards(transform.position, _targetB.position, _speed * Time.deltaTime);
        } else {
            transform.position = Vector3.MoveTowards(transform.position, _targetA.position, _speed * Time.deltaTime);
        }

        if (transform.position == _targetB.position) {
            _switching = true;
        } else if (transform.position == _targetA.position) {
            _switching = false;
        }
    }

    private void OnTriggerEnter(Collider other) {
        if (other.CompareTag("Player")) {
            other.transform.parent = this.transform;
        }
    }

    private void OnTriggerExit(Collider other) {
        if (other.CompareTag("Player")) {
            other.transform.parent = null;
        }
    }
}
