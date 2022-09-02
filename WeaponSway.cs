using UnityEngine;

public class WeaponSway : MonoBehaviour
{

    // This script should be on another transform like weapon holder to make it work
    //not working if the animation drives a trtansform
    //placed on Arms object

    //weapon swing
    [Header("Weapon swing settings")]
    [SerializeField]
    private float smooth = 6f;
    [SerializeField]
    private float multiplier = 3;


    // Update is called once per frame
    void Update()
    {
        swingWeapon();
    }

    #region Weapon swing effect
    void swingWeapon()
    {
        // get mouse input
        float mouseX = Input.GetAxisRaw("Mouse X") * multiplier;
        float mouseY = Input.GetAxisRaw("Mouse Y") * multiplier;

        // calculate target rotation
        Quaternion rotationX = Quaternion.AngleAxis(-mouseY, Vector3.right);
        Quaternion rotationY = Quaternion.AngleAxis(mouseX, Vector3.up);

        Quaternion targetRotation = rotationX * rotationY;

        // rotate 
        transform.localRotation = Quaternion.Slerp(transform.localRotation, targetRotation, smooth * Time.deltaTime);
    }
    #endregion
}
