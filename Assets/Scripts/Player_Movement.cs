

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player_Movement : MonoBehaviour
{
    //character speed
    public float speed;
    float previousSpeed;
    public float runSpeed;
    public float gravity = -9.8f;

    private CharacterController _charCont;
    private Rigidbody rb;



    // Use this for initialization
    void Start()
    {
        _charCont = GetComponent<CharacterController>();
        previousSpeed = speed;
        //rb = GetComponent<Rigidbody>();
       
    }

    // Update is called once per frame
    void Update()
    {
        float deltaX = Input.GetAxis("Horizontal");
        float deltaZ = Input.GetAxis("Vertical");
        Vector3 movement = new Vector3(deltaX, 0, deltaZ);
        movement = Vector3.ClampMagnitude(movement, speed); //Limits the max speed of the player


        movement.y = gravity;

        movement *= Time.deltaTime;     //Ensures the speed the player moves does not change based on frame rate


        movement = transform.TransformDirection(movement);

        if (Input.GetKey(KeyCode.LeftShift))
        {
            previousSpeed = speed;
            speed += runSpeed;
        }

        //apply movement
        _charCont.Move(movement * speed);

        speed = previousSpeed;

    }
}
