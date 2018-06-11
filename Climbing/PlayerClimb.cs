using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerClimb : MonoBehaviour {

    private Animator anim;

    public float climbingSpeed;
    public Vector3 distanceToTop = new Vector3(0, 1, 1);
    float controllerDrag = 0.007f;

    private float saveClimbingSpeed;

    float vert;
    float hori;

    // Use this for initialization
    void Start()
    {
        anim = GetComponent<Animator>();
        saveClimbingSpeed = climbingSpeed;

    }

    
    // Update is called once per frame
    void Update()
    {
        // Debug.Log("isClimbing: " + anim.GetBool("isClimbing"));

        //check if climb check has been set
        //Debug.Log("climbingSpeed: " + climbingSpeed);

        ClimbingAnimationHandler();

        if (HoldValues.climbMovement == true)
        {
            HoldValues.horizontalMovement = false; //turn normal movement off


            MovePlayer();

            if(HoldValues.topEdgeHit)
            {
                ClimbToTop();
            }
        }
        else
        {
            anim.SetBool("isClimbing", false);

        }
    }

    void MovePlayer()
    {
        vert = Mathf.Round(10 * (Input.GetAxisRaw("Joy LY")));
        hori = Mathf.Round(10 * (-Input.GetAxisRaw("Joy LX")));

       
        if (Input.GetAxis("Joy LY") < -controllerDrag || Input.GetAxis("Joy LY") > controllerDrag || Input.GetAxis("Joy LX") < -controllerDrag || Input.GetAxis("Joy LX") > controllerDrag)
        {
            if (vert == 0)
            {
              //  Debug.Log("Climbing horizontally");
                transform.Translate(Vector3.left * (hori / 10) * climbingSpeed * Time.deltaTime);
            }
            else if (hori == 0)
            {
              //  Debug.Log("Climbing vertically");
                transform.Translate(Vector3.up * (vert / 10) * climbingSpeed * Time.deltaTime);
            }
        }
      

        //Debug.Log("Vertical " + vert);
       Debug.Log("Horizontal " + hori);

       

      
    }
    


    void ClimbToTop()
    {
        //code to move player to top of object once top edge has been hit
        Debug.Log("Climb to edge");
        
        transform.Translate(distanceToTop);
        HoldValues.topEdgeHit = false;
    }

    void ClimbingAnimationHandler()
    {
        anim.SetBool("isClimbing", true);
        anim.SetFloat("InputX", Input.GetAxis("Joy LX"));
        anim.SetFloat("InputY", Input.GetAxis("Joy LY"));
    }

}
