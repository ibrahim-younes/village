using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Movement : MonoBehaviour
{
    // CinemachineVirtualCamera virtualCamera;
    [SerializeField] Transform playerCamer = null;
    [SerializeField] float sensitivity = 3.5f;
    [SerializeField] float normalWalkSpeed = 6.0f;

    private float currentWalkSpeed;
    [SerializeField] float sprintSpeed = 5.0f;


    [SerializeField] float gravity = -13.0f;
    [SerializeField] float jumpHeight = 3f;   
    [SerializeField] bool lockCursor = true;

    float camerapitch = 0.0f;
    CharacterController controller = null;
    Vector3 velocity;
    [SerializeField] private int force; 

    void Start(){
        controller = GetComponent<CharacterController>();
        if (lockCursor){
            Cursor.visible = false;
        }

    }

    void Update()
    {
       UpdateMouseLook();
       UpdateMovement();
    }

    void UpdateMouseLook(){
        Vector2 currentMouseDelta = new Vector2 (Input.GetAxis("Mouse X"), Input.GetAxis("Mouse Y"));

        camerapitch -= currentMouseDelta.y * sensitivity;
        camerapitch = Mathf.Clamp(camerapitch, -90.0f,90.0f);

        playerCamer.localEulerAngles = Vector3.right * camerapitch;

        transform.Rotate(Vector3.up * currentMouseDelta.x * sensitivity);
    }

    void UpdateMovement(){
        float x = Input.GetAxis("Horizontal");
        float z = Input.GetAxis("Vertical");

        if (controller.isGrounded){
            velocity.y = 0.0f;
        }

        velocity.y += gravity * Time.deltaTime;

        if (controller.isGrounded && Input.GetButtonDown("Jump")){
            velocity.y = Mathf.Sqrt(jumpHeight * -2f * gravity);
        }

        if (Input.GetKey(KeyCode.LeftShift))
        {
            currentWalkSpeed = normalWalkSpeed + sprintSpeed;
        }
        else
        {
            currentWalkSpeed = normalWalkSpeed;
        }

        Vector3 move = (transform.right * x) + (transform.forward * z) + (Vector3.up * velocity.y);
        
        controller.Move(move * Time.deltaTime * currentWalkSpeed);
    }
    
    private void OnControllerColliderHit(ControllerColliderHit obj){
        Rigidbody rigidbody = obj.collider.attachedRigidbody;

        if (rigidbody != null){
            Vector3 forceDirection = obj.gameObject.transform.position - transform.position;
            forceDirection.y = 0;
            forceDirection.Normalize();

            rigidbody.AddForceAtPosition(forceDirection * force, transform.position, ForceMode.Impulse);
        } 
    }

    private void ChangeCamera(){
        if (Input.GetButton("y")){
            
        }
    }
}
