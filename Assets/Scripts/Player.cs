using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
public class Player : MonoBehaviour
{

    [SerializeField] float _speed = 5.0f;
    [SerializeField] float _gravity = 1.0f;
    [SerializeField] float _jumpHeight = 25.0f;
    [SerializeField] int _coins = 0;
    [SerializeField] int _lives = 3;

    Animator _animator;
    CharacterController _controller;
    UIManager _uiManager;
    
    float _yVelocity;
    bool _canDoubleJump = false;

    // Start is called before the first frame update
    void Start()
    {
        _animator = GetComponent<Animator>();
        _controller = GetComponent<CharacterController>();
        _uiManager = GameObject.Find("Canvas").GetComponent<UIManager>();        

        if (_uiManager == null) {
            Debug.Log("UI Manager is null on Player");
        }

        _uiManager.UpdateLivesDisplay(_lives);
    }

    // Update is called once per frame
    void Update()
    {        
        float horizontalInput = Input.GetAxis("Horizontal");        
        Vector3 direction = new Vector3(horizontalInput, 0f, 0f);
        Vector3 velocity = direction * _speed;

        // Rotate player left and right
        if (horizontalInput > 0) {
            transform.rotation = Quaternion.Euler(0, 90, 0);
        } else if (horizontalInput < 0) {
            transform.rotation = Quaternion.Euler(0, -90, 0);
        }
        
        if (_controller.isGrounded == true) {
            _animator.SetBool("isJumping", false);
            // Jump            
            if (Input.GetButtonDown("Jump")) {
                _animator.SetBool("isJumping", true);
                _yVelocity = _jumpHeight;
                _canDoubleJump = true;
            }
        } else {
            // Double jump
            if (_canDoubleJump) {
                if (Input.GetButtonDown("Jump")) {
                    _yVelocity = _jumpHeight;
                    _canDoubleJump = false;
                }
            }            
            _yVelocity -= _gravity;
        }

        velocity.y = _yVelocity;
        _controller.Move(velocity * Time.deltaTime);
    }

    public void AddCoins() {
        _coins++;
        if (_uiManager != null) {
            _uiManager.UpdateCoinDisplay(_coins);
        }        
    }

    public void Damage() {
        _lives--;
        _uiManager.UpdateLivesDisplay(_lives);

        if (_lives < 1) {
            SceneManager.LoadScene(0);
        }
    }
}
