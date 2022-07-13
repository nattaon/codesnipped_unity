using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraController : MonoBehaviour
{
    private float x;// = 0f;
    private float y;// = 0f;
    private float m_Vertical;
    private float m_Horizontal;
    private float distance;

    float move_speed = 1f;

    float xSpeed = 250f;
    float ySpeed = 120f;

    //private float xMinLi = -90f;//-30f;
    //private float xMaxLi = 90f;

    //private float yMinLi = -90f;//-30f;
    //private float yMaxLi = 90f;//60f;

    void Start()
    {
        Cursor.lockState = CursorLockMode.Locked;
        //Cursor.visible = false;
        x = transform.localEulerAngles.y;
        y = transform.localEulerAngles.x;

        m_Horizontal = transform.position.x;
        m_Vertical = transform.position.y;
    }
    void LateUpdate()
    {
        x += Input.GetAxis("Mouse X") * xSpeed * 0.02f;
        y -= Input.GetAxis("Mouse Y") * ySpeed * 0.02f;
        m_Vertical = Input.GetAxis("Vertical") * move_speed;
        m_Horizontal = Input.GetAxis("Horizontal") * move_speed *-1f;
        //Debug.Log("cam x,y = " + x + "," + y);

        //x = ClampAngle(x, xMinLi, xMaxLi);
        //y = ClampAngle(y, yMinLi, yMaxLi);

        if (Input.GetAxis("Mouse ScrollWheel") > 0)
        {
            distance = -1f;
        }
        else if (Input.GetAxis("Mouse ScrollWheel") < 0)
        {
            distance = 1f;
        }
        else
        {
            distance = 0f;
        }

        Quaternion rotation = Quaternion.Euler(y, x, 0);
        Vector3 look = transform.TransformDirection(Vector3.forward*distance);
        Vector3 Y_move = Vector3.up * m_Vertical;
        Vector3 X_move = Vector3.right * m_Horizontal;


        transform.position = transform.position + look + X_move + Y_move;
        transform.rotation = Quaternion.Euler(y, x, 0);
    }
    static float ClampAngle(float angle, float min, float max)
    {

        if (angle < -360)
            angle += 360;
        if (angle > 360)
            angle -= 360;
        return Mathf.Clamp(angle, min, max);
    }
}
