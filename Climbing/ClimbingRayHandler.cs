using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ClimbingRayHandler : MonoBehaviour {

    /*
     * This class is used to check for climbable surfaces, sending out a ray in the forward direction
     * to look for surfaces that are on the "Climbable" Layer.
     * 
     * Attached to an empty object parented to the player object
     */
    public GameObject player;
    private Text actionText;




    Vector3 fwd;
    LayerMask mask;
    LayerMask inverse;

    RaycastHit wallHit;

    public Transform leftEdgeRay;
    public Transform rightEdgeRay;
    public Transform topEdgeRay;

    public float rayDistance = 10.0f;

    void Start()
    {
       
        mask = LayerMask.GetMask("Climbable");
        inverse = LayerMask.GetMask("ClimbableInverse");

        actionText = GameObject.FindGameObjectWithTag("ClimbText").GetComponent<UnityEngine.UI.Text>();

    }

    void mainRayDirectionSet()
    {
        Vector3 mainRayForward = transform.TransformDirection(Vector3.forward);
        Vector3 mainRayBackward = transform.TransformDirection(Vector3.back);
        Vector3 mainRayLeft = transform.TransformDirection(Vector3.left);
        Vector3 mainRayRight = transform.TransformDirection(Vector3.right);

        if ((Physics.Raycast(transform.position, mainRayForward, out wallHit, rayDistance, mask)))
        {
            //forward ray
            fwd = mainRayForward;
          //  Debug.Log("Forward Side Hit");
        }
        else if((Physics.Raycast(transform.position, mainRayBackward, out wallHit, rayDistance, mask)))
        {
            fwd = mainRayBackward;
         //   Debug.Log("Backward Side Hit");

        }
        else if ((Physics.Raycast(transform.position, mainRayLeft, out wallHit, rayDistance, mask)))
        {
            fwd = mainRayLeft;
         //   Debug.Log("Left Side Hit");

        }
        else if ((Physics.Raycast(transform.position, mainRayRight, out wallHit, rayDistance, mask)))
        {
            fwd = mainRayRight;
         //   Debug.Log("Right Side Hit");

        }

    }

    // Update is called once per frame
    void FixedUpdate () {

        mainRayDirectionSet();

        CharacterController controller = this.transform.parent.GetComponent<CharacterController>();

        //  Debug.Log("Forward " + fwd);



        // Debug.DrawRay(transform.position, fwd * rayDistance, Color.red);
        Debug.DrawRay(topEdgeRay.position, fwd * rayDistance, Color.green);

        if ((Physics.Raycast(transform.position, fwd, out wallHit, rayDistance, mask)))
        {
            actionText.text = "Press RB To Climb";
        }
        else
        {
            actionText.text = "";
        }


        if ((Physics.Raycast(transform.position, fwd, out wallHit,  rayDistance, mask)) && Input.GetButton("RB Button") && controller.isGrounded) //main climb ray checker
        {
            actionText.text = "";

            HoldValues.climbMovement = true; //ensure that horizontal plane movement is turned off
            HoldValues.currentClimbingObject = wallHit.collider.transform; //set static variable to current climbing object

            //The following if-statement rotates the player to face each desired side of the object
            if(transform.parent.rotation != HoldValues.currentClimbingObject.rotation && !HoldValues.isParallel)
            {
                Vector3 roundedFWD = new Vector3((Mathf.Round(fwd.x)), Mathf.Round(fwd.y), Mathf.Round(fwd.z));
                Debug.Log("Forward Rounded " + roundedFWD);
                transform.parent.rotation = Quaternion.LookRotation(roundedFWD, Vector3.up);
                HoldValues.isParallel = true;
            }
    
            if (!Physics.Raycast(topEdgeRay.position, fwd, rayDistance, mask) && !Physics.Raycast(topEdgeRay.position, fwd, rayDistance, inverse)) //checks to see if player has hit top edge
            {
                HoldValues.topEdgeHit = true;
            }
         

        }
        else
        {
            HoldValues.climbMovement = false;
            HoldValues.horizontalMovement = true;
            HoldValues.currentClimbingObject = null;
            HoldValues.isParallel = false;

        }
    }


}
