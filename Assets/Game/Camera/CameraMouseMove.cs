using System.Collections.Generic;
using UnityEngine;

public class CameraMouseMove : MonoBehaviour {
    public float moveSpeed = 0.5f;
    public float xMinLimit = -30;
    public float xMaxLimit = 30;
    public float zMinLimit = -15;
    public float zMaxLimit = 15;
    public float wheelSpeed = 2f;

    private Vector3 _originalPos;

    [System.Runtime.InteropServices.DllImport("user32.dll")] //引入dll
    public static extern int SetCursorPos(int x, int y);

    // Use this for initialization
    void Start() {
        //if (GetComponent<Rigidbody>()) {
        //    GetComponent<Rigidbody>().freezeRotation = true;
        //}
        _originalPos = transform.localPosition;
    }

    // Update is called once per frame
    void Update() {
        Vector3 mousePosition = Input.mousePosition;
        Vector3 offset = transform.localPosition;
        if (mousePosition.x < 0) {
            offset.x -= moveSpeed;
        }

        if (mousePosition.x > Screen.width) {
            offset.x += moveSpeed;
        }

        if (mousePosition.y < 0) {
            offset.z -= moveSpeed;
        }

        if (mousePosition.y > Screen.height) {
            offset.z += moveSpeed;
        }

        offset.x = Mathf.Clamp(offset.x, _originalPos.x + xMinLimit, _originalPos.x + xMaxLimit);
        offset.z = Mathf.Clamp(offset.z, _originalPos.z + zMinLimit, _originalPos.z + zMaxLimit);

        float dis = offset.magnitude;
        dis -= Input.GetAxis("Mouse ScrollWheel") * wheelSpeed;

        if (dis < 10 || dis > 40) {
            return;
        }
        offset = offset.normalized * dis;

        transform.localPosition = offset;
    }
}
