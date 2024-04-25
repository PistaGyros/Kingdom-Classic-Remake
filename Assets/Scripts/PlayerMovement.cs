using System.Collections;
using System.Collections.Generic;
using Cinemachine;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerMovement : MonoBehaviour
{
    Vector2 moveInput;
    Rigidbody2D myRigidbody;
    Animator myAnimator;
    [SerializeField] float playerSpeed = 5f;
    [SerializeField] private GameObject cinemachineCamera;
    private CinemachineVirtualCamera virtualCamera;
    private float cameraDelay = 5;
    private bool hasChangedPositionOfCamera;
    private float lerpTime = 0;
    private float leftCameraDirection = 0.75f;
    private float rightCameraDirection = 0.25f;
    private float cameraDirection;
    private bool playerHorizontalSpeed;
    
    void Start()
    {
        myRigidbody = GetComponent<Rigidbody2D>();
        myAnimator = GetComponent<Animator>();
        virtualCamera = cinemachineCamera.GetComponent<CinemachineVirtualCamera>();
        cameraDirection = rightCameraDirection;
    }

    
    void FixedUpdate()
    {
        myAnimator.SetBool("IsWalking", false);
        Run();
        FlipSprite();
        if (!playerHorizontalSpeed)
        {
            cameraDelay -= Time.fixedDeltaTime;
            if (cameraDelay <= 0)
            {
                ShiftCamera();
            }
            else
            {
                ShiftCameraBack();
            }
        }
    }

    void OnMove(InputValue value)
    {
        moveInput = value.Get<Vector2>();
        if (moveInput.x > 0)
            cameraDirection = rightCameraDirection;
        else if (moveInput.x < 0)
            cameraDirection = leftCameraDirection;
    }

    void Run()
    {
        Vector2 playerVelocity = new Vector2(moveInput.x, 0f);
        myRigidbody.velocity = playerVelocity * playerSpeed;
    }

    void FlipSprite()
    {
        playerHorizontalSpeed = Mathf.Abs(myRigidbody.velocity.x) > Mathf.Epsilon;
        if (playerHorizontalSpeed)
        {
            transform.localScale = new Vector2(Mathf.Sign(myRigidbody.velocity.x), 1f);
            myAnimator.SetBool("IsWalking", true);
            cameraDelay = 5f;
            lerpTime = 0;
        }
    }

    private void ShiftCamera()
    {
        if (lerpTime <= 3f)
        {
            lerpTime += Time.fixedDeltaTime;
            virtualCamera.GetCinemachineComponent<CinemachineFramingTransposer>().m_ScreenX =
                Mathf.Lerp(0.5f, cameraDirection, lerpTime);           
        }
    }

    private void ShiftCameraBack()
    {
        if (lerpTime >= 0)
        {
            lerpTime -= Time.fixedDeltaTime;
            virtualCamera.GetCinemachineComponent<CinemachineFramingTransposer>().m_ScreenX =
                Mathf.Lerp(0.5f, cameraDirection, lerpTime);
        }
    }
}
