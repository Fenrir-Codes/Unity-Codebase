using UnityEngine;

public class SelectionController : MonoBehaviour
{

    public int activeWeapon = 0;
    public Transform[] weapons;

    // Start is called before the first frame update
    void Start()
    {
        if (weapons[0] != null && weapons[0].gameObject.tag == "Pistol")
        {
            weapons[0].gameObject.SetActive(true);
        }
        else
        {
            weapons[0].gameObject.SetActive(false);
            Debug.Log("No weapon in Array weapons[0] position, or weapon tag is not equal to Pistol");
        }

        //selectWeapon();
    }

    // Update is called once per frame
    void Update()
    {

    }

    void selectWeapon()
    {

        int i = 0;
        foreach (Transform weapon in weapons)
        {
            if (i == activeWeapon)
            {
                weapon.gameObject.SetActive(true);
            }
            else
            {
                weapon.gameObject.SetActive(false);
            }
            i++;
        }
    }
}
