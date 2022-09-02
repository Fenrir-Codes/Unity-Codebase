using UnityEngine;

public class WeaponPickUp : MonoBehaviour
{
    GunController gun;
    private GameObject weapon;
    [Header("Pick up settings")]
    [SerializeField] private Transform EquipPosition; // Equip position for weapons
    [SerializeField] private Transform parentingArm; // the weapon parented to this arm
    [SerializeField] private GameObject armsToHide; // hiding arms if no weapon in hands
    [SerializeField] private GameObject noClipCamera; // camera to avoid clipping into objects 
    [SerializeField] private GameObject currentWeapon; // the current weapon we have in the arms

    [Header("Heads Up Displays for ammo")]
    public GameObject HUD_M4Carabine;
    public GameObject HUD_AKM;
    public GameObject HUD_M16A1;

    private Vector3 sizeOfWeapon = new Vector3(1.5f, 1.5f, 1.5f);

    //booleans
    public float Distance = 5f; // distance from you can pick up a weapon
    private bool canPickUp = false; // canpickup tells if you can pick up a thing
    public bool isEmptyHands = true; // checking if hands (EquipPosition) is empty or not

    //private KeyCode FireButton = KeyCode.Mouse0;
    private KeyCode EquipButton = KeyCode.E;

    #region Start
    private void Start()
    {
        armsToHide.SetActive(false); //hide arms at the start (no weapons)
        noClipCamera.SetActive(false);
        isEmptyHands = true; // set emyphands true ,because we have no weapons at start
    }
    #endregion

    #region Update
    void Update()
    {
        CheckCanIGrab(); // checking if we can grab a thing

        //if can grab true
        if (canPickUp)
        {
            if (Input.GetKeyDown(EquipButton))
            {
                if (currentWeapon != null) // if weapon slot is populated
                {
                    Drop(); // we can drop it
                }
                PickUp();
            }
        }

        //Check is weapon slot is empty or not (if not you can drop weapon with Q or automatically if another weapon grabbed)
        if (currentWeapon != null)
        {
            isEmptyHands = false;
            if (Input.GetKeyDown(KeyCode.Q))
            {
                Drop();
            }
        }

        //if hands are  not empty (weapon attached to container)
        if (isEmptyHands == false)
        {
            setLayer(currentWeapon, 7);
            // if it can find a gun script on the transform you can control the gun
            if (gun != null && gun.canShoot == true)
            {
                gun.Shoot();
            }
            if (gun != null && Input.GetKeyDown(KeyCode.R))
            {
                if (gun.CurrentMagazine != gun.MaxMagazineSize && gun.AmmoReserve > 0)
                {
                    StartCoroutine(gun.Reload());
                }
            }
        }

    }
    #endregion

    #region Setting the layer of the weapons and children, for avoid clipping trough walls
    void setLayer(GameObject obj, int newLayer)
    {
        if (obj == null)
        {
            return;
        }

        obj.layer = newLayer;

        foreach (Transform child in obj.transform)
        {
            if (null == child)
            {
                continue;
            }
            setLayer(child.gameObject, newLayer);
        }
    }
    #endregion

    #region Check if you can grab
    private void CheckCanIGrab()
    {
        RaycastHit hit; // casting ray out to distance
        if (Physics.Raycast(Camera.main.transform.position, Camera.main.transform.forward, out hit, Distance))
        {
            if (hit.transform.tag == "canPickUp") // if it hits a tag with canPickUp
            {
                // Debug.Log("I can grab this weapon!");
                canPickUp = true;
                weapon = hit.transform.gameObject;
            }
            else
            {
                canPickUp = false;
            }
        }
    }
    #endregion

    #region Pick up script
    private void PickUp()
    {
        //if we picking up things
        isEmptyHands = false; // our hands have weapon
        armsToHide.SetActive(true); // showing hands
        noClipCamera.SetActive(true); // turn on noclip camera

        currentWeapon = weapon; // gameobject weapon is our current weapon
        currentWeapon.transform.SetParent(EquipPosition); // setting the parent of the transfor of the weapon
        currentWeapon.transform.localPosition = Vector3.zero; // position relative to the parent
        currentWeapon.transform.localRotation = Quaternion.Euler(Vector3.zero); // rotation relative to parent
        currentWeapon.transform.localScale = Vector3.one; // scaling the weapon transform to 1 on all axes
        currentWeapon.GetComponent<Rigidbody>().isKinematic = true; // iskinematic otherwise it is bugging
        EquipPosition.SetParent(parentingArm); // our equip positions parent

        gun = GetComponentInChildren<GunController>();  // on pick up a gun, call the script on the gun

        //getting name of the transfom we have in our hands then show the belonging HUD
        if (gun.transform.name == "AKM")
        {
            HUD_AKM.SetActive(true);
            HUD_M4Carabine.SetActive(false);
            HUD_M16A1.SetActive(false);
        }
        if (gun.transform.name == "M4Carabine")
        {
            HUD_M4Carabine.SetActive(true);
            HUD_AKM.SetActive(false);
            HUD_M16A1.SetActive(false);
        }
        if (gun.transform.name == "M16A1")
        {
            HUD_M16A1.SetActive(true);
            HUD_AKM.SetActive(false);
            HUD_M4Carabine.SetActive(false);
        }

    }
    #endregion

    #region Drop Script
    private void Drop()
    {
        //if we drop a thing
        isEmptyHands = true; // hands are empty
        armsToHide.SetActive(false); // arms hided
        noClipCamera.SetActive(false); // noclip camera is not activ now

        currentWeapon.transform.SetParent(null); // no weapon in our hads
        currentWeapon.transform.localScale = sizeOfWeapon; // sizeing the transform back to 1.5 on drop
        setLayer(currentWeapon, 0);  // setting the layer to default, otherwise we will see the weapon trough walls
        currentWeapon.GetComponent<Rigidbody>().isKinematic = false;
        currentWeapon = null; // no curr. weapon
        weapon = null; // no weapon
        gun = null; // no gun

        //hud is not showing
        HUD_AKM.SetActive(false); // disable akm hud
        HUD_M4Carabine.SetActive(false); // disable m4 hud

    }
    #endregion

}


