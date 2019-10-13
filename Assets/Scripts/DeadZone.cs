using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DeadZone : MonoBehaviour
{
    [SerializeField] GameObject _respawnPoint;

    private void OnTriggerEnter(Collider other) {        
        if (other.CompareTag("Player")) {
            Player player = other.GetComponent<Player>();
            if (player != null) {
                player.Damage();                
            }

            CharacterController cc = other.GetComponent<CharacterController>();
            if (cc != null) {
                cc.enabled = false;
            }

            other.transform.position = _respawnPoint.transform.position;

            StartCoroutine(CCEnableRoutine(cc));
        }
    }

    IEnumerator CCEnableRoutine(CharacterController cc) {
        yield return new WaitForSeconds(0.5f);
        cc.enabled = true;
    }
}
