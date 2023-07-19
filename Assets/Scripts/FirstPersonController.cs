using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class FirstPersonController : MonoBehaviour
{
    
    private bool IsSprinting => canSprint && Input.GetKey(sprintKey);
    private bool ShouldJump => Input.GetKeyDown(jumpKey) && CharacCtrl.isGrounded;
    private bool ShouldCrouch => Input.GetKeyDown(crouchKey) && !inCrouchAnim && CharacCtrl.isGrounded;

    [Header("Function Options")]
    [SerializeField] private bool canMove = true;
    [SerializeField] private bool canLook = true;
    [SerializeField] private bool canSprint = true;
    [SerializeField] private bool canJump = true;
    [SerializeField] private bool canCrouch = true;
    [SerializeField] private bool canHeadBob = true;
    [SerializeField] private bool canInteract = true;
    [SerializeField] private bool useFootsteps = true;
    [SerializeField] private bool useStamina = true;

    [Header("Controls")]
    [SerializeField] private KeyCode sprintKey = KeyCode.LeftShift;
    [SerializeField] private KeyCode jumpKey = KeyCode.Space;
    [SerializeField] private KeyCode crouchKey = KeyCode.C;
    [SerializeField] private KeyCode interactKey = KeyCode.Mouse0;

    [Header("Movement Parameters")]
    [SerializeField] private float WalkSpeed = 3.0f;
    [SerializeField] private float SprintSpeed = 6.0f;
    [SerializeField] private float crouchSpeed = 0.25f;

    [Header("Look Parameters")]
    [SerializeField, Range(1, 10)] private float LookSpeedX = 2.0f;
    [SerializeField, Range(1, 10)] private float LookSpeedY = 2.0f;
    [SerializeField, Range(1, 180)] private float UpperLookLimit = 80.0f;
    [SerializeField, Range(1, 180)] private float LowerLookLimit = 80.0f;

    [Header("Jumping Parameters")]
    [SerializeField] private float jumpForce = 8.0f;
    [SerializeField] private float gravity = 30.0f;

    [Header("Crouch Parameters")]
    [SerializeField] private float crouchHeight = 0.5f;
    [SerializeField] private float standHeight = 2f;
    [SerializeField] private float timeToCrouch = 0.25f;
    [SerializeField] private Vector3 crouchingCenter = new Vector3(0, 0.5f, 0);
    [SerializeField] private Vector3 standingCenter = new Vector3(0, 0, 0);

    private bool isCrouching;
    private bool inCrouchAnim;

    [Header("Stamina Parameters")]
    [SerializeField] private float maxStamina = 100;
    [SerializeField] private float staminaDrain = 5;
    [SerializeField] private float timeBeforeStart = 5;
    [SerializeField] private float staminaRegenValue = 2;
    [SerializeField] private float staminaRegenTime = 0.1f;
    private float currentStamina;
    private Coroutine regeneratingStamina;

    [Header("Headbob Parameters")]
    [SerializeField] private float walkBobSpeed = 14f;
    [SerializeField] private float walkBobAmount = 0.05f;
    [SerializeField] private float SprintBobSpeed = 18f;
    [SerializeField] private float SprintBobAmount = 0.11f;
    [SerializeField] private float crouchBobSpeed = 7f;
    [SerializeField] private float crouchBobAmount = 0.02f;
    private float defaultYPos = 0;
    private float timer = 20;

    [Header("Footstep Parameters")]
    [SerializeField] private float baseStepSpeed = 0.1f;
    [SerializeField] private float crouchStepMultiplier = 1.5f;
    [SerializeField] private float SprintStepMultiplier = 0.5f;
    [SerializeField] private AudioSource footstepAudioSource = default;
    [SerializeField] private AudioClip[] woodClips = default;
    [SerializeField] private AudioClip[] stoneClips = default;
    [SerializeField] private AudioClip[] tileClips = default;
    private float footstepTimer = 0;
    private float GetCurrentOffset => isCrouching ? baseStepSpeed * crouchStepMultiplier : IsSprinting ? baseStepSpeed * SprintStepMultiplier : baseStepSpeed;

    [Header("Interaction")]
    [SerializeField] private Vector3 interactionRayPoint = default;
    [SerializeField] private float interactionDistance = default;
    [SerializeField] private LayerMask interactionLayer = default;
    public Interactable currentInteractable;
    private Interactable previousInteractable;

    private Camera playerCamera;
    private CharacterController CharacCtrl;

    private Vector3 MoveDir;
    private Vector2 CurrentInput;

    private float rotationX = 0;

    public static FirstPersonController instance;

    private bool isOnDialogue = false;


    void Awake()
    {
        instance = this;

        playerCamera = GetComponentInChildren<Camera>();
        CharacCtrl = GetComponent<CharacterController>();
        defaultYPos = playerCamera.transform.localPosition.y;
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
    }

    void Update()
    {
        
        if (isOnDialogue) return;
        
            
        if (canMove)
        {
            MovementInput();
        }
            
        // Should stop ability to move player head when in dialogue
        CursorHandler();
        MouseLook();
        //

        ApplyMovement();

        if (canJump)
            HandleJump();

        if (canCrouch)
            HandleCrouch();

        if (canHeadBob)
            HandleHeadBob();

        if (useFootsteps)
            HandleFootstep();

        if (useStamina)
            HandleStamina();

        if (canInteract)
        {
            HandleInteractionCheck();
            HandleInteractionInput();
        }
    }

    // ***** PLAYER MOVEMENT *****

    public void IsOnDialogue(bool isOnDialogue)
    {
        if (isOnDialogue)
        {
            Cursor.lockState = CursorLockMode.None;
            this.isOnDialogue = true;
        }
        else
        {
            Cursor.lockState = CursorLockMode.Locked;
            this.isOnDialogue = false;
        }
    }
    
    private void MovementInput()
    {
        CurrentInput = new Vector2((isCrouching ? crouchSpeed : IsSprinting ? SprintSpeed : WalkSpeed) * Input.GetAxis("Vertical"), (isCrouching ? crouchSpeed : IsSprinting ? SprintSpeed : WalkSpeed) * Input.GetAxis("Horizontal"));
        
        float moveDirectionY = MoveDir.y;
        MoveDir = (transform.TransformDirection(Vector3.forward) * CurrentInput.x) + (transform.TransformDirection(Vector3.right) * CurrentInput.y);
        MoveDir = MoveDir.normalized * (isCrouching ? crouchSpeed : IsSprinting ? SprintSpeed : WalkSpeed); //normalize
        MoveDir.y = moveDirectionY;
    }
    
    public bool CanMove
    {
        get => canMove;
        set => canMove = value;
    }

    public void ResetMoveDir()
    {
        MoveDir = Vector3.zero;
    }
    

    // ***** PLAYER LOOK *****

    private void MouseLook()
    {
        if (!canLook) return;
        rotationX -= Input.GetAxis("Mouse Y") * LookSpeedY;
        rotationX = Mathf.Clamp(rotationX, -UpperLookLimit, LowerLookLimit);
        playerCamera.transform.localRotation = Quaternion.Euler(rotationX, 0, 0);
        transform.rotation *= Quaternion.Euler(0, Input.GetAxis("Mouse X") * LookSpeedX, 0);
    }

    public void ForceLookAtObject(Transform objectPosition)
    {
        playerCamera.transform.DOLookAt(objectPosition.position, 2).onComplete= () =>
        {
            rotationX = playerCamera.transform.localEulerAngles.x;
        };
    }
    
    

    private void CursorHandler()
    {
        if (Input.GetKey(KeyCode.LeftAlt))
        {
            canLook = false;
            Cursor.lockState = CursorLockMode.None;
        }
        else
        {
            canLook = true;
            Cursor.lockState = CursorLockMode.Locked;
        }
    }

    // ***** Jumping *****

    private void HandleJump()
    {
        if (ShouldJump)
            MoveDir.y = jumpForce;
    }

    // ***** Crouch *****

    private void HandleCrouch()
    {
        if (ShouldCrouch)
            StartCoroutine(CrouchStand());
    }

    public void ToggleCrouchStand()
    {
        StartCoroutine(CrouchStand());
    }

    // ***** PLAYER HEADBOB *****

    private void HandleHeadBob()
    {
        if (!CharacCtrl.isGrounded) return;

        if (Mathf.Abs(MoveDir.x) > 0.1f || Mathf.Abs(MoveDir.z) > 0.1f)
        {
            timer += Time.deltaTime * (isCrouching ? crouchBobSpeed : IsSprinting ? SprintBobSpeed : walkBobSpeed);
            playerCamera.transform.localPosition = new Vector3(
                playerCamera.transform.localPosition.x,
                defaultYPos + Mathf.Sin(timer)* (isCrouching ? crouchBobAmount : IsSprinting ? SprintBobAmount : walkBobAmount),
                playerCamera.transform.localPosition.z);
        }
    }

    // ***** PLAYER STAMINA *****

    private void HandleStamina()
    {
        if(IsSprinting && CurrentInput != Vector2.zero)
        {
            if(regeneratingStamina != null)
            {
                StopCoroutine(regeneratingStamina);
                regeneratingStamina = null;
            }

            currentStamina -= staminaDrain * Time.deltaTime;

            if (currentStamina < 0)
                currentStamina = 0;
            if (currentStamina <= 0)
                canSprint = false;
        }
        if(!IsSprinting && currentStamina < maxStamina && regeneratingStamina == null)
        {
            regeneratingStamina = StartCoroutine(RegenStamina());
        }
    }
    
    // ***** PLAYER FOOTSTEP SOUNDS *****

    private void HandleFootstep()
    {
        if (!CharacCtrl.isGrounded) return;
        if (CurrentInput == Vector2.zero) return;

        footstepTimer -= Time.deltaTime;

        if(footstepTimer <= 0)
        {
            if(Physics.Raycast(playerCamera.transform.position, Vector3.down, out RaycastHit hit, 3))
            {
                switch (hit.collider.tag)
                {
                    case "Footstep/WOOD":
                        footstepAudioSource.PlayOneShot(woodClips[Random.Range(0, woodClips.Length - 1)]);
                        break;
                    case "Footstep/STONE":
                        footstepAudioSource.PlayOneShot(stoneClips[Random.Range(0, stoneClips.Length - 1)]);
                        break;
                    case "Footstep/TILE":
                        footstepAudioSource.PlayOneShot(tileClips[Random.Range(0, tileClips.Length - 1)]);
                        break;
                    default:
                        break;
                }
            }

            footstepTimer = GetCurrentOffset;
        }
    }

    private void HandleInteractionCheck()
    {
        //modified this kasi minsan nagdodouble yung naka Onfocus
        
        previousInteractable = currentInteractable;
        
        if(Physics.Raycast(playerCamera.ViewportPointToRay(interactionRayPoint),out RaycastHit hit, interactionDistance))
        {
            if(hit.collider.gameObject.layer == 6 && (currentInteractable == null || hit.collider.gameObject.GetInstanceID() != currentInteractable.GetInstanceID()))
            {
                hit.collider.TryGetComponent(out currentInteractable);

                if (currentInteractable)
                    currentInteractable.OnFocus();
            }
        }
        else
        {
            currentInteractable = null;
        }
        
        if (previousInteractable != currentInteractable)
        {
            if (previousInteractable != null)
            {
                previousInteractable.OnLoseFocus();
            }
        }
    }

    private void HandleInteractionInput()
    {
        if (Input.GetKeyDown(interactKey) && currentInteractable != null && Physics.Raycast(playerCamera.ViewportPointToRay(interactionRayPoint), out RaycastHit hit, interactionDistance, interactionLayer))
        {
            currentInteractable.OnInteract();
        }
    }


    private void ApplyMovement()
    {
        if (!CharacCtrl.isGrounded)
            MoveDir.y -= gravity * Time.deltaTime;

        CharacCtrl.Move(MoveDir * Time.deltaTime);
    }

    // ***** Coroutines *****

    private IEnumerator CrouchStand()
    {

        if (isCrouching && Physics.Raycast(playerCamera.transform.position, Vector3.up, 1f))
            yield break;

        inCrouchAnim = true;

        float timeElapsed = 0;
        float targetHeight = isCrouching ? standHeight : crouchHeight;
        float currentHeight = CharacCtrl.height;
        Vector3 targetCenter = isCrouching ? standingCenter : crouchingCenter;
        Vector3 currentCenter = CharacCtrl.center;

        while(timeElapsed < timeToCrouch)
        {
            CharacCtrl.height = Mathf.Lerp(currentHeight, targetHeight, timeElapsed / timeToCrouch);
            CharacCtrl.center = Vector3.Lerp(currentCenter, targetCenter, timeElapsed / timeToCrouch);
            timeElapsed += Time.deltaTime;
            yield return null;
        }

        CharacCtrl.height = targetHeight;
        CharacCtrl.center = targetCenter;

        isCrouching = !isCrouching;

        inCrouchAnim = false;
    }

    private IEnumerator RegenStamina()
    {
        yield return new WaitForSeconds(timeBeforeStart);
        WaitForSeconds timeToWait = new WaitForSeconds(staminaRegenTime);

        while(currentStamina < maxStamina)
        {
            if (currentStamina > 0)
                canSprint = true;

            currentStamina += staminaRegenValue;

            if (currentStamina > maxStamina)
                currentStamina = maxStamina;

            yield return timeToWait;
        }

        regeneratingStamina = null;
    }
}
