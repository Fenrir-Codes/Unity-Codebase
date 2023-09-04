using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class ItemInteraction : MonoBehaviour
{
    [SerializeField] private InputController iController;
    Ray ray;
    RaycastHit hit;
    public bool HitUsable = false;
    [SerializeField] private float Distance = 2.0f;
    [SerializeField] private Camera mainCam;
    [SerializeField] private GameObject InteractionUI;

    // Update is called once per frame
    void Update()
    {
        RaycastObjects();
        EnableUI();
    }

    private void RaycastObjects()
    {

        Vector3 screenMiddle = new Vector3(Screen.width / 2, Screen.height / 2, 0);
        ray = mainCam.ScreenPointToRay(screenMiddle);
        //Debug.DrawRay(Camera.main.transform.position, forward, Color.green);

        if (Physics.Raycast(ray, out hit, Distance))
        {
            if (hit.collider.CompareTag("Usable"))
            {
                HitUsable = true;
                hit.transform.TryGetComponent(out Item item);
                if (item != null && Input.GetKey(KeyCode.E) && iController.isEmptyHands)
                {
                    item.Use();
                }
                else if (item != null && Input.GetKey(KeyCode.E) && !iController.isEmptyHands && !iController.isShooting && !iController.isReloading)
                {
                    item.Use();
                }
            }
        }
        else
        {
            HitUsable = false;
        }
    }

    private void EnableUI()
    {
        if (!HitUsable)
        {
            InteractionUI.SetActive(false);
        }
        else
        {
            InteractionUI.SetActive(true);
        }
    }
}
