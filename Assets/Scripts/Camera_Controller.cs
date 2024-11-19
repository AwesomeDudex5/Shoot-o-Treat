using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Camera_Controller : MonoBehaviour
{
    Vector2 mousLook;
    Vector2 smoothV;
    public float sensitivity;
    public float smooth;

    GameObject character;

    void Start()
    {

        //hide and lock cursor to center or screen
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
        character = this.transform.parent.gameObject;
    }

    void Update()
    {
        //change in mouse position
        Vector2 mousedelta = new Vector2(Input.GetAxisRaw("Mouse X"), Input.GetAxisRaw("Mouse Y"));
       
        mousedelta = Vector2.Scale(mousedelta, new Vector2(sensitivity * smooth, sensitivity * smooth));
        //apply smooth and sensitivity
        smoothV.x = Mathf.Lerp(smoothV.x, mousedelta.x, 1f / smooth);
        smoothV.y = Mathf.Lerp(smoothV.y, mousedelta.y, 1f / smooth);
        mousLook += smoothV;

        //limits up and down view
        mousLook.y = Mathf.Clamp(mousLook.y, -90f, 90f);
        //rotate camera
        transform.localRotation = Quaternion.AngleAxis(-mousLook.y, Vector3.right);
        //limit player rotation to only left and right
        character.transform.localRotation = Quaternion.AngleAxis(mousLook.x, character.transform.up);
       // transform.GetChild(0).transform.Rotate(mouseD * Time.deltaTime * sensitivity);
    }

    public void changeMouseSpeed(float speed)
    {
        sensitivity = speed;
    }

    
}
