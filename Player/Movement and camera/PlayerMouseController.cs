using UnityEngine;
using UnityEngine.Rendering.HighDefinition;

public class PlayerMouseController : MonoBehaviour
{
    // This script should to be attached to main camera, then link the player body to it as reference.
    // (Main camera should be on soldier head in the rig)
    [Header("Mouse settings (Main Camera)")]
    [Tooltip("Mouse sensitivity")]
    [SerializeField] private float mouseSensitivity = 200f;
    [Tooltip("Transform player")]
    [SerializeField] private Transform playerBody;  // Firstpersonplayer
    [SerializeField] private float lookUpLimit = -85f;
    [SerializeField] private float lookDownLimit = 85f;
    private float xRotation;

    public float mouseX;
    public float mouseY;

    // Start is called before the first frame update
    void Start()
    {
        Cursor.lockState = CursorLockMode.Locked;
    }

    // Update is called once per frame
    private void Update()
    {
        set_Sensivity(mouseSensitivity);
        mouseLook();
    }

    #region look around
    private void mouseLook()
    {
        mouseX = Input.GetAxis("Mouse X") * mouseSensitivity * Time.deltaTime;
        mouseY = Input.GetAxis("Mouse Y") * mouseSensitivity * Time.deltaTime;

        xRotation -= mouseY;
        xRotation = Mathf.Clamp(xRotation, lookUpLimit, lookDownLimit);
        transform.localRotation = Quaternion.Euler(xRotation, 0f, 0f);
        playerBody.Rotate(Vector3.up * mouseX);  

    }
    #endregion

    #region set the sensitivity
    public void set_Sensivity(float sense)
    {
       mouseSensitivity = sense;
    }
    #endregion
}
