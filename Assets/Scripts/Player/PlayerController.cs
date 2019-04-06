using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(CharacterController))] //objekt s ovom skriptom zahtjeva imati CharacterController komponentu
public class PlayerController : MonoBehaviour {

    public float MovementSpeed = 5.0f;
    public bool IsParalized = false;
    public float NegativeGravity = -9.81f;
    private float _gravity;
    public float JumpSpeedMin = 2f; //kada "kratko" skočim, koliko maksimalno visoko idem
    public float JumpSpeedMax = 4f; //kada "dugo" skočim, koliko maksimalno visoko idem
    public float JumpLasting = 0.5f; //koliko dugo ostajem u zraku
    private float _jumpSpeedGrad = 0.0f;

    private CharacterController _characterController;

    private Vector3 _startingPosition;

    private float _movementH;
    private float _movementV;
    private bool _jump;
    private bool _isJumping = false;

    private AudioSource _myAudioSource;
    public AudioClip JumpSound;
    public AudioClip FallSound;

    private Vector3 movement;

    private void Awake()
    {
        _characterController = GetComponent<CharacterController>();
        _myAudioSource = GetComponent<AudioSource>();
        _jumpSpeedGrad = JumpSpeedMin;
        _gravity = NegativeGravity;
        IsParalized = false;
        _startingPosition = transform.position;
    }

    private void Update()
    {
        if(GameManager.GM.CurrentState == GameManager.GameState.Playing)
        {
            if (!IsParalized)
            {
                _movementH = Input.GetAxisRaw("Horizontal");
                _movementV = Input.GetAxisRaw("Vertical");
                _jump = Input.GetButton("Jump");

                //ovo je zapravo kretnja po globalnim osima, ovo su nam naša dva vektora smjera po dvije glavne globalne osi kretanja
                Vector3 movementX = _movementH * Vector3.right; //koristimo globalne osi (.right, vektor po X-u)
                Vector3 movementZ = _movementV * Vector3.forward; //koristimo globalne osi (.forward, vektor po Z-u)
                movement = movementX + movementZ;

                movement.Normalize();
                //dijagonalni vektor (1,0,1) duljine 1,41 pretvara u jedinični duljine 1 i vraća kao vrijednost vektora movement
                //(za razliku od movement.normalized, koja samo vraća normaliziranu vrijednost vektora movement)
                //tek sada kada smo normalizirali vektor smjera, SADA ga množimo s brzinom i time.deltatime

                //movement *= MovementSpeed * Time.deltaTime;
                //movement.x *= MovementSpeed;
                //movement.z *= MovementSpeed;

                //nama treba kretanje playera naspram njegovih lokalnih koordinata
                //ono što je lokalno naprijed je ono što je lokalna Z os
                //ono što je lokalno desno je ono što je lokalna X os
                //transform.TransformDirection() pretvara lokalni vektor u globalni...
                //...ali koristit ćemo to obrnuto...
                //...dat ćemo mu globalni vektor, što će Unity za TransformDirection pretpostavit da je lokalni...
                //...i pretvorit će ga u globalni, a to će nama sada biti naš lokalni vektor koji nam i treba
                //tako transform.TransformDirection funkcionira
                movement = transform.TransformDirection(movement);

                if (_jump && _characterController.isGrounded)
                {
                    _isJumping = true;
                    _myAudioSource.pitch = Random.Range(0.7f, 1.3f);
                    _myAudioSource.PlayOneShot(JumpSound);
                    StartCoroutine(JumpCo());
                }
                else if (_isJumping)
                {
                    _jumpSpeedGrad += Time.deltaTime * JumpSpeedMax * 2; // *JumpSpeedMax samo zato da ubrzam gradaciju
                    if (_jumpSpeedGrad < JumpSpeedMax && _isJumping)
                        movement = movement * MovementSpeed + Vector3.up * _jumpSpeedGrad;
                    else
                        _jumpSpeedGrad = JumpSpeedMin;
                }
                else if (!_isJumping)
                {
                    movement = movement * MovementSpeed + Vector3.up * _gravity;
                }

                _characterController.Move(movement * Time.deltaTime);
            }

            if (IsParalized && _gravity > 0f)
            {
                _characterController.Move(Vector3.up * _gravity * Time.deltaTime);
            }
        }
    }

    private IEnumerator JumpCo()
    {
        yield return new WaitForSeconds(JumpLasting);
        _isJumping = false;
        _myAudioSource.pitch = Random.Range(0.7f, 1.3f);
        _myAudioSource.PlayOneShot(FallSound);
    }

    public void Paralize(bool isParalized)
    {
        if (isParalized)
            IsParalized = true;
        if (!isParalized)
            IsParalized = false;
    }

    public void ResetGravity()
    {
        _gravity = NegativeGravity;
    }

    public void ChangeGravity(float changedGravity)
    {
        _gravity = changedGravity;
    }

    public void ResetTransform()
    {
        transform.position = _startingPosition;
    }

}