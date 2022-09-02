using UnityEngine;

public class PlayerMouseController : MonoBehaviour
{
    // This script should to be attached to main camera, then link the player body to it as reference.
    // (Main camera should be on soldier head in the rig)

    [Header("Mouse settings (Main Camera)")]
    [Tooltip("Mouse sensitivity")]
    [SerializeField] private float mouseSensitivity = 100f;
    [Tooltip("Transform player")]
    [SerializeField] private Transform playerBody;  // Firstpersonplayer
    private float xRotation = 0f;
    private Transform target;

    // Start is called before the first frame update
    void Start()
    {
        Cursor.lockState = CursorLockMode.Locked;
        this.target = GameObject.FindWithTag("MainCamera").transform;
    }

    // Update is called once per frame
    void Update()
    {
        mouseLook();
    }

    private void mouseLook()
    {
        float mouseX = Input.GetAxis("Mouse X") * mouseSensitivity * Time.deltaTime;
        float mouseY = Input.GetAxis("Mouse Y") * mouseSensitivity * Time.deltaTime;

        xRotation -= mouseY;
        xRotation = Mathf.Clamp(xRotation, -85f, 90f);

        transform.localRotation = Quaternion.Euler(xRotation, 0f, 0f);
        playerBody.Rotate(Vector3.up * mouseX);


    }
}
