using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

[RequireComponent(typeof(CharacterController))]

public class Controller : MonoBehaviour
{
    [Header("Movement"),Space(10)]
    public float walkingSpeed = 7.5f;
    public float runningSpeed = 11.5f;
    public float jumpSpeed = 8.0f;
    public float gravity = 20.0f;
    public Camera playerCamera;
    public float lookSpeed = 2.0f;
    public float lookXLimit = 45.0f;
    public AudioSource footsteps;

    [Header("Fire System"), Space(10)]
    public GameObject hitBoxSword;
    public Animator swordAnimator;
    public GameObject hitBoxPike;
    public Animator pikeAnimator;
    public GameObject swordMash, pikeMash;
    public int health = 100;
    public Slider healthSlider;
    int numWeapon;
    bool isFire;

    [Header("EXP System"), Space(10)]
    public TextMeshProUGUI exptxt;
    public Slider expSlider;
    int exp;
    int level;

    CharacterController characterController;
    Vector3 moveDirection = Vector3.zero;
    float rotationX = 0;

    public bool canMove = true;

    void Start()
    {
        characterController = GetComponent<CharacterController>();

        Cursor.visible = false;
        Cursor.lockState = CursorLockMode.Locked;

    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            Application.Quit();
        }

        if (Input.GetKeyDown(KeyCode.Alpha1))
        {
            numWeapon = 0;
            swordMash.SetActive(true);
            pikeMash.SetActive(false);
        }
        else if (Input.GetKeyDown(KeyCode.Alpha2))
        {
            numWeapon = 1;
            swordMash.SetActive(false);
            pikeMash.SetActive(true);
        }

        if (Input.GetMouseButtonDown(0) && !isFire)
        {
            StartCoroutine(FireEnum());
            switch (numWeapon)
            {
                case 0:
                    swordAnimator.Play("SwordFire");
                    break;
                case 1:
                    pikeAnimator.Play("PikeFire");
                    break;
            }

            isFire = true;
        }

        if (health <= 0)
        {
            SceneManager.LoadScene(0);
        }

        MovementAndJump();
    }

    public void SetEXP()
    {
        exp += Random.Range(5,11);
        if (exp >= 100)
        {
            exp -= 100;
            level += 1;
        }
        exptxt.text = "EXP : " + exp + " / 100 | Lvl : " + level;
        expSlider.value = exp / 100f;
    }

    public void SetHealthProgress(int HealthDemage)
    {
        health -= HealthDemage;
        healthSlider.value = health / 100f;
    }

    private IEnumerator FireEnum()
    {
        yield return new WaitForSeconds(0.05f);
        switch (numWeapon)
        {
            case 0:
                hitBoxSword.SetActive(true);
                break;
            case 1:
                hitBoxPike.SetActive(true);
                break;
        }
        yield return new WaitForSeconds(0.03f);
        switch (numWeapon)
        {
            case 0:
                hitBoxSword.SetActive(false);
                break;
            case 1:
                hitBoxPike.SetActive(false);
                break;
        }
        yield return new WaitForSeconds(0.13f);
        isFire = false;
    }

    private void MovementAndJump()
    {


        Vector3 forward = transform.TransformDirection(Vector3.forward);
        Vector3 right = transform.TransformDirection(Vector3.right);

        bool isRunning = Input.GetKey(KeyCode.LeftShift);
        float curSpeedX = canMove ? (isRunning ? runningSpeed : walkingSpeed) * Input.GetAxis("Vertical") : 0;
        float curSpeedY = canMove ? (isRunning ? runningSpeed : walkingSpeed) * Input.GetAxis("Horizontal") : 0;
        float movementDirectionY = moveDirection.y;
        moveDirection = (forward * curSpeedX) + (right * curSpeedY);

        if (Input.GetButton("Jump") && canMove && characterController.isGrounded)
        {
            moveDirection.y = jumpSpeed;
        }
        else
        {
            moveDirection.y = movementDirectionY;
        }

        if (!characterController.isGrounded)
        {
            moveDirection.y -= gravity * Time.deltaTime;
        }

        characterController.Move(moveDirection * Time.deltaTime);

        _playAudio();

        if (canMove)
        {
            rotationX += -Input.GetAxis("Mouse Y") * lookSpeed;
            rotationX = Mathf.Clamp(rotationX, -lookXLimit, lookXLimit);
            playerCamera.transform.localRotation = Quaternion.Euler(rotationX, 0, 0);
            transform.rotation *= Quaternion.Euler(0, Input.GetAxis("Mouse X") * lookSpeed, 0);
        }
    }

    private void _playAudio(){

        if(moveDirection != Vector3.zero && footsteps.isPlaying == false && characterController.isGrounded){

            footsteps.Play();

        }
        if((characterController.isGrounded == false || characterController.velocity == Vector3.zero) && footsteps.isPlaying == true){

            footsteps.Pause();

        }
    }
}

//Pusuu Games Jam