using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class playerup : MonoBehaviour
{
    public float speed = 10f;
    public float rotationSpeed = 45f;
    public Animator animController;
    private CharacterController controller;
    
    

    public bl_Joystick floatingJoystick;
    void Awake()
    {
        controller = GetComponent<CharacterController>();
        floatingJoystick = GameObject.FindGameObjectWithTag("Joystick").GetComponent<bl_Joystick>();
    }

    private void Update()
    {

        float horizontalInput = floatingJoystick.Horizontal;
        float verticalInput = floatingJoystick.Vertical;

        Vector3 movementDirection = new Vector3(horizontalInput, 0, verticalInput);
        movementDirection.Normalize();
        if (floatingJoystick.Vertical <= -0.35 || floatingJoystick.Vertical >= 0.35 || floatingJoystick.Horizontal <= -0.35 || floatingJoystick.Horizontal >= 0.35)
        { 
            controller.SimpleMove(movementDirection * speed);
        
        if (movementDirection != Vector3.zero )
        {
                Quaternion toRotation = Quaternion.LookRotation(movementDirection, Vector3.up);
                transform.rotation = Quaternion.RotateTowards(transform.rotation, toRotation, rotationSpeed * Time.deltaTime);
                //transform.rotation = Quaternion.LookRotation(movementDirection);
                if (!animController)
                return;

            animController.SetBool("run", true);
            animController.SetBool("Idle", false);

        }
        }
        else
        {
            if (!animController)
                return;

            animController.SetBool("run", false);
            animController.SetBool("Idle", true);
        }
        
    }
}
