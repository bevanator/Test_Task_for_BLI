using UnityEngine;

public enum CameraStyle
{
    freeRoam,
    topDown
}
public class CamControl : MonoBehaviour
{
    [Header("FreeRoam Camera Settings")]
    public float lookSpeedH = 1f;
    public float lookSpeedV = 1f;
    public float zoomSpeed = 2f;
    public float dragSpeed = 6f;
    public float panSpeed = 20f;
    private Vector3 lastFreeCameraPosition;
    private Vector3 lastFreeCameraRotation;


    private float yaw = 0f;
    private float pitch = 0f;

    [Header("General Settings")]
    public bool isCamTopDown = false;
    public CameraStyle cameraStyle;

    [Header("TopDown Camera Settings")]
    public Vector3 topDownCameraPosition;
    public Vector3 topDownCameraRotation;

    void Start()
    {
        lastFreeCameraPosition = new Vector3(transform.position.x, transform.position.y, transform.position.z);
    }

    void Update()
    {
        //Look around with Right Mouse
        if (Input.GetMouseButton(1))
        {
            yaw += lookSpeedH * Input.GetAxis("Mouse X");
            pitch -= lookSpeedV * Input.GetAxis("Mouse Y");

            transform.eulerAngles = new Vector3(pitch, yaw, 0f);
        }

        //Movement with W,A,S,D

        if (Input.GetKey("w"))
        {
            transform.position += transform.forward * (Time.deltaTime * panSpeed);
        }

        if (Input.GetKey("d"))
        {
            transform.position += transform.right * (Time.deltaTime * panSpeed);
        }

        if (Input.GetKey("a"))
        {
            transform.position -= transform.right * (Time.deltaTime * panSpeed);
        }

        if (Input.GetKey("s"))
        {
            transform.position -= transform.forward * (Time.deltaTime * panSpeed);
        }

        //Zoom in and out with Mouse Wheel
        transform.Translate(0, 0, Input.GetAxis("Mouse ScrollWheel") * zoomSpeed, Space.Self);

    }

    public void Interact()
    {

        isCamTopDown = false;
        cameraStyle = CameraStyle.freeRoam;
        ChangeCameraPos(isCamTopDown);

    }

    public void TopDown()
    {

        isCamTopDown = true;
        cameraStyle = CameraStyle.topDown;
        ChangeCameraPos(isCamTopDown);

    }

    public void ChangeCameraPos(bool goTop)
    {
        if (goTop)
        {
            lastFreeCameraPosition = new Vector3(transform.position.x, transform.position.y, transform.position.z);
            lastFreeCameraRotation = new Vector3(transform.rotation.eulerAngles.x, transform.rotation.eulerAngles.y, transform.rotation.eulerAngles.z);
            transform.position = topDownCameraPosition;
            transform.eulerAngles = topDownCameraRotation;

            lookSpeedH = 0f;
            lookSpeedV = 0f;
            zoomSpeed = 2f;
            dragSpeed = 0f;
            panSpeed = 20f;
        }
        else
        {
            transform.position = lastFreeCameraPosition;
            transform.eulerAngles = lastFreeCameraRotation;
            lookSpeedH = 1f;
            lookSpeedV = 1f;
            zoomSpeed = 2f;
            dragSpeed = 6f;
            panSpeed = 20f;
        }
    }
}