using UnityEngine;
using UnityEngine.InputSystem;
using TMPro;
using UnityEngine.UI;

public class playercontroller : MonoBehaviour
{
    private CharacterController controller;
    private Animator animator;
    private AudioSource audioSource;
    [Header("Audio Clips")]
    public AudioClip coinCollectSound;

    [Header("Movement Settings")]
    [SerializeField] private float walkSpeed;
    [SerializeField] private float runSpeed;
    [SerializeField] private float rotationSpeed;
    [SerializeField] private float rotationSmoothTime;


    [Header("key binds")]
    public InputAction moveAction;
    public InputAction runAction;
    public InputAction jumpAction;

    [Header("Gravity & Jump")]
    [SerializeField] private float gravity = -9.81f;
    [SerializeField] private float jumpHeight = 1.5f;
    [SerializeField] private LayerMask jumpableLayer;
    [SerializeField] private float groundCheckDistance;
    [SerializeField] private Transform groundCheckPointds;

    [Header("UI")]
    [SerializeField] private TextMeshProUGUI scoreTextObject;
    [SerializeField] private Image healthFGImage;

    [Header("Game Manager")]
    [SerializeField] private GameManager gameManager;

    private int score;
    private float maxHealth,currentHealth;

    private Camera mainCam;
    private Vector3 velocity;
    private bool isGrounded;

    private void OnEnable()
    {
        moveAction.Enable();
        runAction.Enable();
        jumpAction.Enable();
    }

    private void OnDisable()
    {
        moveAction.Disable();
        runAction.Disable();
        jumpAction.Disable();
    }
    private void Start()
    {
        controller = GetComponent<CharacterController>();
        animator = GetComponent<Animator>();
        audioSource = GetComponent<AudioSource>();
        score = 0;
        scoreTextObject.text = "score : " + score;
        maxHealth = 100;
        currentHealth = maxHealth;
        healthFGImage.fillAmount = currentHealth / maxHealth;

        mainCam = Camera.main;

        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;

    }

    // Update is called once per frame
    private void Update()
    {
        HandleMovement();
    }
    private void HandleMovement()
    {
        isGrounded = Physics.CheckSphere(groundCheckPointds.position, groundCheckDistance, jumpableLayer);
        if (isGrounded && velocity.y < 0)
        {
            velocity.y = -2;
        }

        Vector2 input = moveAction.ReadValue<Vector2>();
        Vector3 direction = new Vector3(input.x, 0, input.y).normalized;
        bool isRunning = runAction.IsPressed();
        float currentSpeed = isRunning ? runSpeed : walkSpeed;

        if (direction.magnitude >= 0.1f)
        {
            float targetAngle = Mathf.Atan2(direction.x, direction.z) * Mathf.Rad2Deg + mainCam.transform.eulerAngles.y;
            float angle = Mathf.SmoothDampAngle(transform.eulerAngles.y, targetAngle, ref rotationSpeed, rotationSmoothTime);

            transform.rotation = Quaternion.Euler(0, angle, 0);
            Vector3 newDir = Quaternion.Euler(0, targetAngle, 0) * Vector3.forward;
            controller.Move(newDir.normalized * currentSpeed * Time.deltaTime);
        }
        //animation
        animator.SetFloat("Speed", direction.magnitude*currentSpeed, 0.1f, Time.deltaTime);

        //jump
        if (jumpAction.WasPressedThisFrame() && isGrounded)
        {
            velocity.y = Mathf.Sqrt(jumpHeight * -2 * gravity);
            animator.SetTrigger("Jump");
        }

        velocity.y += gravity * Time.deltaTime;
        controller.Move(velocity * Time.deltaTime);
    }
    private void OnTriggerEnter(Collider other) //this is for collectors
    {

        if (other.gameObject.tag == "Coins")
        {
            score++;
            scoreTextObject.text = "score : " + score;
            gameManager.GameWin(score);
            Debug.Log(score);
            audioSource.PlayOneShot(coinCollectSound);
            Destroy(other.gameObject);
            //collision.gameObject.SetActive(false);
        }
        if (other.gameObject.tag == "AIEnemy")
        {
            currentHealth -= maxHealth;
            Debug.Log("Hit Enemy + Health =" + currentHealth);
            healthFGImage.fillAmount = currentHealth / maxHealth;
            if (currentHealth <= 0)
            {
                gameManager.GameLose();
            }
        }
    }
    private void OnControllerColliderHit(ControllerColliderHit hit)
    {
        if(hit.gameObject.tag == "healths")
        {
            currentHealth -= 20;
            Debug.Log("Hit Enemy + Health = " + currentHealth);
            healthFGImage.fillAmount = currentHealth / maxHealth;
            if (currentHealth <= 0)
            {
                gameManager.GameLose();
            }
            Destroy(hit.gameObject);
        }
        
    }
}
