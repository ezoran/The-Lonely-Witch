using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMove : MonoBehaviour
{

    private Animator anim;
    protected Vector3 moveDirection = Vector3.zero;
    public GameObject camera;

    private Vector3 camera_forward;

    public float speed = 15.0f; //unit velocity
    public float turnSpeed = 100.0f; //turn velocity

    public float jumpSpeed = 50.0f; //jump height
    public float verticalVelocity;
    public static float gravity = 20.0f; //gravity modifier

    public float controllerDrag = 0.005f; //account for controller error

    bool canMove;

    public float WalkMax = 0.03f;
    public float yWalkMax = 0.03f;

    RaycastHit wallHit;
    LayerMask mask;
    public float rayDist = 2.0f;


    float previousVertVel;

    void Start()
    {
        anim = GetComponent<Animator>();


        canMove = false;

        mask = LayerMask.GetMask("Ground");
        previousVertVel = -jumpSpeed;

    }

    void Update()
    {
        CharacterController controller = GetComponent<CharacterController>();
        // Debug.Log(HoldValues.isParallel);
        //  Debug.Log(controller.isGrounded);
        if (HoldValues.horizontalMovement == true) //if not climbing
        {
            if (controller.isGrounded) //check if character is grounded and player isn't attempting to move
            {

                RunAnimation();


                float x = Input.GetAxis("Joy LX");
                float y = Input.GetAxis("Joy LY");

                camera_forward = Vector3.Scale(camera.transform.forward, new Vector3(1, 0, 1).normalized);

                moveDirection = y * camera_forward + x * camera.transform.right;

                moveDirection *= speed;

                if (canMove)
                {
                    //the following three lines deal with character rotation
                    Vector3 newdir = Vector3.RotateTowards(transform.forward, moveDirection, 360 * Time.deltaTime, 0.0f);
                    //  Debug.Log("newDir: " + newdir);
                    Quaternion newRotation = Quaternion.LookRotation(newdir);
                    transform.rotation = Quaternion.Slerp(transform.rotation, newRotation, 0.1f);
                }

                //Debug.Log("Vert velocity " + verticalVelocity);

                if (Input.GetButtonDown("A Button"))
                {
                    //play jump animation

                    JumpAnimation();

                    verticalVelocity = jumpSpeed;
                    Debug.Log("Jumping");
                }
                else
                {
                    anim.SetBool("isJumping", false);

                    verticalVelocity = 0;
                    // Debug.Log("Grounded");
                }

                previousVertVel = verticalVelocity;
            }
            else if (!controller.isGrounded)
            {
                anim.SetFloat("playerHeight", verticalVelocity);
            }



            //The following sets character movement, accounting for controller drag
            if (Input.GetAxis("Joy LY") < -controllerDrag || Input.GetAxis("Joy LY") > controllerDrag || Input.GetAxis("Joy LX") < -controllerDrag || Input.GetAxis("Joy LX") > controllerDrag)
            {
                controller.Move(moveDirection * Time.deltaTime);
                canMove = true;

            }
            else
            {
                canMove = false;
            }

            verticalVelocity -= gravity * Time.deltaTime;
            ;

            Vector3 jumpVector = new Vector3(0, verticalVelocity, 0);
            controller.Move(jumpVector * Time.deltaTime);

        }
    }

    //Handles running animations
    void RunAnimation()
    {

        if (Input.GetAxis("Joy LY") < -controllerDrag || Input.GetAxis("Joy LY") > controllerDrag || Input.GetAxis("Joy LX") < -controllerDrag || Input.GetAxis("Joy LX") > controllerDrag)
        {
            anim.SetBool("isRunning", true);
            anim.SetFloat("InputX", Input.GetAxis("Joy LX"));
            anim.SetFloat("InputY", Input.GetAxis("Joy LY"));


        }
        else
        {
            anim.SetBool("isRunning", false);
        }
    }

    //Handles jumping animation
    void JumpAnimation()
    {

        // Debug.Log("Jumping");
        if (canMove)
        {
            anim.SetBool("isJumping", true);
            anim.SetFloat("playerHeight", verticalVelocity);
            //     Debug.Log("Running Jump");
            //   Debug.Log("VerticalVelocity " + verticalVelocity);
        }
        if (!canMove)
        {
            anim.SetBool("isJumping", true);
            //   Debug.Log("Standing Jump");
            //    Debug.Log("VerticalVelocity " + verticalVelocity);
            anim.SetFloat("playerHeight", verticalVelocity);


        }
    }

}
