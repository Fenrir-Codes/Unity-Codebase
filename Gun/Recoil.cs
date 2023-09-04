
using UnityEngine;


public class Recoil : MonoBehaviour
{
    [SerializeField] InputController inputcontroller;
    Gun Gun;
    private Vector3 currentRotation;
    private Vector3 targetRotation;

    [SerializeField] private float snappiness;
    [SerializeField] private float returnSpeed;

    bool isAiming;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (!inputcontroller.isEmptyHands)
        {
            Gun = GetComponentInChildren<Gun>();
        }

        isAiming = inputcontroller.isAiming;

        targetRotation = Vector3.Lerp(targetRotation,Vector3.zero, returnSpeed * Time.deltaTime);
        currentRotation = Vector3.Slerp(currentRotation, targetRotation, snappiness * Time.deltaTime);
        transform.localRotation = Quaternion.Euler(currentRotation);


    }

    public void RecoilFire()
    {
        if (isAiming)
        {
            targetRotation += new Vector3(Gun.aimrecoilX, Random.Range(-Gun.aimrecoilY, Gun.aimrecoilY), Random.Range(-Gun.aimrecoilZ, Gun.aimrecoilZ));

        }
        else
        {
            targetRotation += new Vector3(Gun.recoilX, Random.Range(-Gun.recoilY, Gun.recoilY), Random.Range(-Gun.recoilZ, Gun.recoilZ));
        }
    }
}
