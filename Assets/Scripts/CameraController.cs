using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraController : MonoBehaviour
{

    Camera mainCamera;
    

    public GameObject tableGameObject;
    // Start is called before the first frame update
    void Start()
    {
        mainCamera = GetComponent<Camera>();
    }

    // Update is called once per frame
    void Update()
    {
        //transform.LookAt(mainCamera.ScreenToWorldPoint(new Vector3(Input.mousePosition.x, Input.mousePosition.y, mainCamera.nearClipPlane)), Vector3.up);
        //Camera.main.transform.LookAt(Input.mousePosition)
       // transform.localRotation = Quaternion.Euler(new Vector4(-1f * (mouseY * 180f), mouseX * 360f, transform.localRotation.z));
       // float mouseX = (Input.mousePosition.x / Screen.width) - 0.5f;
        //float mouseY = (Input.mousePosition.y / Screen.height) - 0.5f;
        //transform.localRotation = Quaternion.Slerp(transform.localRotation, Quaternion.Euler(new Vector4(-1f * (mouseY * 180f), mouseX * 360f, transform.localRotation.z)), 0.15f);
    }
}
