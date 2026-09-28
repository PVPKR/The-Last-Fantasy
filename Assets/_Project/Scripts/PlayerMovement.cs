using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    [Header("Movement Settings")]
    public float moveSpeed = 5f;
    public float rotationSpeed = 15f;

    [Header("References")]
    public FloatingJoystick joystick;
    public Transform characterModel;
    public Animator animator;

    private Rigidbody rb;
    private Vector3 moveDirection;

    private GameAudioManager audioManager;
    private bool wasMoving = false;

    void Start()
    {
        rb = GetComponent<Rigidbody>();

        audioManager =
            FindFirstObjectByType<GameAudioManager>();
    }

    void Update()
    {
        moveDirection = new Vector3(
            joystick.Horizontal,
            0f,
            joystick.Vertical
        );

        bool isMoving =
            moveDirection.magnitude > 0.1f;

        if (animator != null)
        {
            animator.SetBool(
                "isRun",
                isMoving
            );
        }

        if (isMoving)
        {
            Vector3 rotDir =
                moveDirection.normalized;

            Quaternion targetRotation =
                Quaternion.LookRotation(rotDir);

            characterModel.rotation =
                Quaternion.Slerp(
                    characterModel.rotation,
                    targetRotation,
                    rotationSpeed * Time.deltaTime
                );
        }

        HandleFootstepSound(isMoving);
    }

    void FixedUpdate()
    {
        if (moveDirection.magnitude > 0.1f)
        {
            Vector3 moveDir =
                moveDirection.normalized;

            rb.MovePosition(
                rb.position +
                moveDir *
                moveSpeed *
                Time.fixedDeltaTime
            );
        }
    }

    private void HandleFootstepSound(bool isMoving)
    {
        if (audioManager == null)
            return;

        if (isMoving && !wasMoving)
        {
            audioManager.StartFootstepSound();
        }
        else if (!isMoving && wasMoving)
        {
            audioManager.StopFootstepSound();
        }

        wasMoving = isMoving;
    }

    private void OnDisable()
    {
        if (audioManager != null)
        {
            audioManager.StopFootstepSound();
        }

        wasMoving = false;
    }
}