using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraController : MonoBehaviour
{
    public string CameraSpeed = "1";
    
    void Update()
    {
        float result;
        if(!float.TryParse(CameraSpeed, out result))
        {
            Debug.LogError("CameraSpeed에는 숫자만 적어주세요");
        }
        if(Input.GetKey(KeyCode.W))
        {
            transform.position += new Vector3(0f, result, 0f);
        }
        if(Input.GetKey(KeyCode.A))
        {
            transform.position += new Vector3(-result, 0f, 0f);
        }
        if (Input.GetKey(KeyCode.S))
        {
            transform.position += new Vector3(0f, -result, 0f);
        }
        if (Input.GetKey(KeyCode.D))
        {
            transform.position += new Vector3(result, 0f, 0f);
        }
    }

    private void OnGUI()
    {
        CameraSpeed = GUI.TextField(new Rect(410, 10, 50, 20), CameraSpeed);
    }
}
