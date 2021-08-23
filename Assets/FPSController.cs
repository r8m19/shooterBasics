using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FPSController : MonoBehaviour
{
    CharacterController characterController;
    [SerializeField] float speed = 1;
    [SerializeField] Transform groundCheck;
    [SerializeField] LayerMask groundMask;
    bool doCheckSphere = true;
    [SerializeField] float groundRadius = 0.5f;
    [SerializeField] float jumpHeight = 2;
    [SerializeField] [Range(0.0f,0.5f)] float smoothTime = 0.3f;

    Vector3 velocity;
    Vector2 currentDir = Vector2.zero;
    Vector2 currentDirVelocity = Vector2.zero;

    private void Awake() {
        characterController = GetComponent<CharacterController>();
    }

    void Update(){
        WASD();
        GravityCalculations();
    }

    void GravityCalculations(){
        bool isGrounded = Physics.CheckSphere(groundCheck.position,groundRadius,groundMask,QueryTriggerInteraction.Ignore) && doCheckSphere;
        if(/*velocity.y < 0 &&*/ isGrounded)
        {
            velocity.y = -2;
        }
        velocity.y += Physics.gravity.y * Time.deltaTime;

        if(Input.GetKeyDown(KeyCode.Space) && isGrounded)
        {
            Jump();
        }



        characterController.Move(velocity * Time.deltaTime);
    }

    void Jump(){
        velocity.y += Mathf.Sqrt(jumpHeight * -2f * Physics.gravity.y);
        StartCoroutine(disableGroundCheckFor(0.3f));        
    }

    IEnumerator disableGroundCheckFor(float seconds){
        doCheckSphere = false;
        yield return new WaitForSeconds(seconds);
        doCheckSphere = true;
    }

    void WASD(){
        Vector2 target = new Vector2(Input.GetAxis("Horizontal"),Input.GetAxis("Vertical"));
        target.Normalize();

        currentDir = Vector2.SmoothDamp(currentDir,target,ref currentDirVelocity,smoothTime);

        Vector3 movimiento = (transform.right * currentDir.x + transform.forward * currentDir.y) * speed;

        characterController.Move(movimiento * Time.deltaTime);
    }


    private void OnDrawGizmos() {
        Gizmos.DrawWireSphere(groundCheck.position,groundRadius);
    }
}
