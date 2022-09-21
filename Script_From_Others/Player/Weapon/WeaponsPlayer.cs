using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using ScreenLocker;

/**
*  Script written by OMA [www.armedunity.com]
*  Rewritten on C# & adapted for latest version Unity3D by DeadZone [vk.com/id160454360] || [Discord: DeadZoneGarry#3474] || [Skype: vanya197799] || [steamcommunity.com/profiles/76561198121485860]
**/

[Serializable]
public class WeaponListNew
{
	public string weaponName = "";
	public GameObject weaponGO;
	public ControllerWeapon wepController;
	public ScriptWeapon weapon;                     //Weapon GO from Hierarchy window (Drag and drop to this field)
	public Rigidbody weaponToDrop;              //Prefab from Project (Drag and drop in this field)

	public float wepSwitchTime = 0.5f;            //We are giving time to play animation before take out next weapon.
	public bool showAmmo;
	[HideInInspector]
	public bool selected;                   //becomes true, when you hold your weapon in hands
	//[HideInInspector]
	public bool inUse = false;              //becomes true, when weapon is equpped (in slot 1 or 2)
}

[ExecuteInEditMode]
public class WeaponsPlayer : MonoBehaviour {

	//[HideInInspector]
	public WeaponListNew[] weaponsInUse;// = new WeaponListNew[3];

	public WeaponListNew[] weaponList;
	public GameObject[] special;
	public int sentrygunUnits = 0;

	[HideInInspector]
	public bool canSwitch = true;
	[HideInInspector]
	public int i = 0;
	[HideInInspector]
	public int weaponToSelect;
	[HideInInspector]
	public int currentWeapon;
	[HideInInspector]
	public bool switching = false;
	[HideInInspector]
	public int bullets;
	[HideInInspector]
	public int mags;
	//public GUISkin mySkin;
	//public GUISkin mySkin2;

	public GameObject SentryGUN;
	public Transform spawnPos;

	public Transform dropPosition;
	[HideInInspector]
	public bool specialSelected = false;
	public LayerMask layerMaskWeapon;
	private RaycastHit hit;
	public ManagerScore scoreManager;
	public PlayerController codcontroller;
	internal bool showInfo;
	internal bool pickupGUI;
	internal bool buyWepGUI;
	internal string textWepName  = "";
	internal bool wepInUseGUI;
	internal int wepPrice;
	internal int ammoPrice;
	internal bool enoughMoney;
	internal int ammoPricePickup;
	internal bool ammoPickup;
	public Texture sentryGunTexture;
	public AudioClip errorSound;

	private float myFloat1 = 115.0f;
	private float myFloat2 = 155.0f;


	public bool gadgetSelected;
	public ExplosivesDrop gadgetController;

	[Header("Current Weapon ID Slot")]
	public int selectWepSlot1 = 0; //Use First ID Weapon slot from WeaponList
	public int selectWepSlot2 = 0; //Use Second ID Weapon slot from WeaponList

	internal HUDManager HUD_MGR;

	public void Start()//	DeactivateAllWeapons();
	{
		//special[0].gameObject.SetActiveRecursively(false);
		//for (var i : int = 0; i < weaponsInUse.length; i++){
		//	weaponsInUse[i] = weaponList[i];
		//	weaponsInUse[i].inUse = true; 
		//}
		HUD_MGR = FindObjectOfType<HUDManager>();
		weaponsInUse[0] = weaponList[selectWepSlot1];
		weaponsInUse[0].inUse = true;
		weaponsInUse[1] = weaponList[selectWepSlot2];
		weaponsInUse[1].inUse = true;
		//Gadgets
		weaponsInUse[2] = weaponList[0];
		weaponsInUse[2].inUse = true;
		weaponToSelect = 0;
		currentWeapon = weaponToSelect;
		StartCoroutine(DeselectWeapon(1));
	}

	public void Update()//}
	{
		if (!ScreenLock.lockCursor) return;
		if (!switching)
		{
			if (canSwitch)
			{
				if (!specialSelected)
				{
					if ((Input.GetKeyDown("1") && (weaponsInUse.Length >= 1)) && (weaponToSelect != 0))
					{
						StartCoroutine(DeselectWeapon(1));
						weaponToSelect = 0;
					}
					else
					{
						if ((Input.GetKeyDown("2") && (weaponsInUse.Length >= 2)) && (weaponToSelect != 1))
						{
							StartCoroutine(DeselectWeapon(1));
							weaponToSelect = 1;
						}
						else
						{
							if ((Input.GetKeyDown("3") && (weaponsInUse.Length >= 3)) && (weaponToSelect != 2))
							{
								StartCoroutine(DeselectWeapon(2));
								weaponToSelect = 2;
							}
						}
					}
					if (Input.GetAxis("Mouse ScrollWheel") > 0)
					{
						weaponToSelect++;
						if (weaponToSelect > (weaponsInUse.Length - 1))
						{
							weaponToSelect = 0;
						}
						if (weaponToSelect == 2)
						{
							StartCoroutine(DeselectWeapon(2));
						}
						else
						{
							StartCoroutine(DeselectWeapon(1));
						}
					}
					if (Input.GetAxis("Mouse ScrollWheel") < 0)
					{
						weaponToSelect--;
						if (weaponToSelect < 0)
						{
							weaponToSelect = weaponsInUse.Length - 1;
						}
						if (weaponToSelect == 2)
						{
							StartCoroutine(DeselectWeapon(2));
						}
						else
						{
							StartCoroutine(DeselectWeapon(1));
						}
					}
				}
				if (Input.GetKeyDown("4") && (special.Length >= 1))
				{
					if (sentrygunUnits >= 1)
					{
						specialSelected = true;
						StartCoroutine(DeselectWeapon(3));
						sentrygunUnits = sentrygunUnits - 1;
					}
				}
				if ((Input.GetKeyDown(KeyCode.F) && !(weaponsInUse[currentWeapon] == weaponList[0])) && (currentWeapon != 2)) //with weaponList[0] we checking if we don't have "No Weapon"  
				{
					if (codcontroller.controller.velocity.magnitude < 5)
					{
						DropWeapon(currentWeapon, 2); // 2 = drop with force
						weaponsInUse[currentWeapon] = weaponList[0];
						SelectWeapon(currentWeapon, 1);
					}
				}

				if (currentWeapon == 2)
				{
					showInfo = false;
					pickupGUI = false;
					buyWepGUI = false;
					return;
				}
				//Pick up and buy weapons
				Vector3 position = transform.parent.position;
				Vector3 direction = transform.TransformDirection(Vector3.forward);
				if (Physics.Raycast(position, direction, out hit, 2, layerMaskWeapon.value))
				{
					showInfo = true;
					if (hit.transform.CompareTag("BuyWeapons"))
					{
						InfoBuyWeapon getInfo = hit.transform.GetComponent<InfoBuyWeapon>();
						//for GUI
						buyWepGUI = true;
						textWepName = getInfo.weaponName;
						if ((getInfo.special == false) && (weaponList[getInfo.weaponIndex].inUse == true))
						{
							wepInUseGUI = true;
							ammoPrice = getInfo.ammoPrice;
							if (scoreManager.currentScore >= getInfo.ammoPrice)
							{
								enoughMoney = true;
							}
							else
							{
								enoughMoney = false;
							}
						}
						else
						{
							wepInUseGUI = false;
							wepPrice = getInfo.weaponPrice;
							if (scoreManager.currentScore >= getInfo.weaponPrice)
							{
								enoughMoney = true;
							}
							else
							{
								enoughMoney = false;
							}
						}
						if (Input.GetKeyDown("e"))
						{
							if (getInfo.special == false)
							{
								//Buy Weapons
								if (((scoreManager.currentScore >= getInfo.weaponPrice) && (scoreManager.updatingScore == false)) && (weaponList[getInfo.weaponIndex].inUse == false))
								{
									DropWeapon(currentWeapon, 1);
									weaponsInUse[currentWeapon] = weaponList[getInfo.weaponIndex];
									weaponList[getInfo.weaponIndex].inUse = true;
									SelectWeapon(weaponToSelect, 1);
									scoreManager.DecreaseScore(getInfo.weaponPrice);
								}
								else
								{
									//Buy AMMO
									if ((((scoreManager.currentScore >= getInfo.ammoPrice) && (scoreManager.updatingScore == false)) && (weaponList[getInfo.weaponIndex].inUse == true)) && (weaponsInUse[currentWeapon].selected == true))
									{
										if ((weaponsInUse[currentWeapon].weapon.numberOfClips < 300) && (weaponsInUse[currentWeapon].weaponName == getInfo.weaponName))
										{
											weaponsInUse[currentWeapon].weapon.numberOfClips = weaponsInUse[currentWeapon].weapon.numberOfClips + getInfo.ammoAmount;
											scoreManager.DecreaseScore(getInfo.ammoPrice);
										}
										else
										{
											GetComponent<AudioSource>().PlayOneShot(errorSound, 0.1f);
										}
									}
									else
									{
										GetComponent<AudioSource>().PlayOneShot(errorSound, 0.1f);
									}
								}
							}
							else
							{
								if ((scoreManager.currentScore >= getInfo.weaponPrice) && (scoreManager.updatingScore == false))
								{
									scoreManager.DecreaseScore(getInfo.weaponPrice);
									sentrygunUnits = sentrygunUnits + 1;
								}
								else
								{
									GetComponent<AudioSource>().PlayOneShot(errorSound, 0.1f);
								}
							}
						}
					}
					else
					{
						InfoWeapon pickUp = hit.collider.transform.GetComponent<InfoWeapon>();
						//for GUI
						pickupGUI = true;
						textWepName = pickUp.weaponName;
						if (weaponList[pickUp.weaponIndex].inUse == true)
						{
							wepInUseGUI = true;
						}
						else
						{
							wepInUseGUI = false;
						}
						if (Input.GetKeyDown("e") && (weaponList[pickUp.weaponIndex].inUse == false))
						{
							DropWeapon(currentWeapon, 2);
							//					
							//						if(weaponList[pickUp.weaponIndex].weaponGO.activeInHierarchy == false){
							//							weaponList[pickUp.weaponIndex].weaponGO.SetActive(true);
							//						}
							weaponsInUse[currentWeapon] = weaponList[pickUp.weaponIndex];
							weaponList[pickUp.weaponIndex].inUse = true;
							SelectWeapon(weaponToSelect, 1);
							Destroy(hit.collider.transform.root.gameObject);
						}
					}
				}
				else
				{
					showInfo = false;
					pickupGUI = false;
					buyWepGUI = false;
				}
			}
		}
	}

	//Only for Parkour we need to deselect this way
	public void QuickDeselectWeapon()
	{
		switching = true;
		int i = 0;
		while (i < weaponsInUse.Length)
		{
			// Deactivate all weapons
			if (weaponsInUse[i].weaponName != "C4")
			{
				StartCoroutine(weaponsInUse[i].weapon.DeselectWeapon());
				weaponsInUse[i].wepController.enabled = false;
				weaponsInUse[i].selected = false;
			}
			i++;
		}
	}

	//abc-  1= weapons 2=gadgets 3=specials
	public IEnumerator DeselectWeapon(int abc)
	{
		switching = true;
		int i = 0;
		while (i < weaponsInUse.Length)
		{
			// Deactivate all weapons
			if (weaponsInUse[i].weaponName != "C4")
			{
				StartCoroutine(weaponsInUse[i].weapon.DeselectWeapon());
				weaponsInUse[i].wepController.enabled = false;
				weaponsInUse[i].selected = false;
			}
			i++;
		}
		StartCoroutine(gadgetController.Deactivate());
		yield return new WaitForSeconds(weaponsInUse[currentWeapon].wepSwitchTime);
		if (abc == 1)
		{
			SelectWeapon(weaponToSelect, 1);

			if(currentWeapon == 0)
				HUD_MGR.Slots.Play(HUD_MGR.AnimSlot1);
			else if (currentWeapon == 1)
				HUD_MGR.Slots.Play(HUD_MGR.AnimSlot2);
		}
		else
		{
			if (abc == 2)
			{
				SelectWeapon(weaponToSelect, 2);
			}
			else
			{
				if (abc == 3)
				{
					SelectSpecial(0);
				}
			}
		}
	}

	//abc - 1= weapons 2=gadgets 3=specials
	public void SelectWeapon(int i, int abc)
	{
		//Activate weapon
		if (abc == 1)
		{
			weaponsInUse[i].weapon.SelectWeapon();
			weaponsInUse[i].wepController.enabled = true;
			weaponsInUse[i].selected = true;
		}
		else
		{
			if (abc == 2)
			{
				gadgetController.ActivateGadget();
			}
		}
		currentWeapon = i;
	}

	public void SelectSpecial(int i)
	{
		//Activate Special weapon
		special[i].gameObject.SetActive(true);
	}

	public void DropWeapon(int index, int useForce)
	{
		switching = true;
		int i = 0;
		while (i < weaponsInUse.Length)
		{
			weaponsInUse[i].weapon.QuickDeselectWeapon();
			weaponsInUse[i].wepController.enabled = false;
			weaponsInUse[i].selected = false;
			i++;
		}
		if (weaponsInUse[index].weaponToDrop != null)
		{
			Rigidbody drop = Instantiate(weaponsInUse[index].weaponToDrop, dropPosition.transform.position, dropPosition.transform.rotation);
			weaponsInUse[index].inUse = false;
			if (useForce == 2)
			{
				//if (Physics.Raycast (transform.parent.position, transform.TransformDirection (Vector3.forward), hit, 2, layerMaskWeapon.value)){
				drop.AddRelativeForce(0, 1000, 1500);
				drop.AddRelativeTorque(50, -200, 0);
			}
			else
			{
				drop.AddRelativeForce(0, 0, 0);
				drop.AddRelativeTorque(-10, -100, 0);
			}
		}
	}

	/*
function DeactivateAllWeapons(){
	for (var i : int = 0; i < weaponList.length; i++){
		if(weaponList[i].inUse == false)
		weaponList[i].weaponGO.SetActive(false);
	}	
}
*/
	/*public void OnGUI()
	{
		GUI.skin = mySkin;
		GUI.skin = mySkin2;
		GUIStyle style1 = mySkin.customStyles[0];
		//GUIStyle style2 = mySkin.customStyles[1];
		GUIStyle style3 = mySkin.customStyles[2];
		GUIStyle style4 = mySkin2.customStyles[3];
		GUIStyle style5 = mySkin2.customStyles[4];
		//GUIStyle cstyle2 = mySkin2.customStyles[1];
		GUIStyle style6 = mySkin2.customStyles[2];
		//if(weaponsInUse[currentWeapon].showAmmo == true){
		//GUI.Label (Rect(Screen.width - 150, Screen.height - 115,100,60),"", cstyle2);
		//GUI.Label (Rect(Screen.width - 170, Screen.height - 115,100,60),"" + weaponsInUse[currentWeapon].weaponName, style4);
		if (weaponsInUse[currentWeapon].showAmmo == true)
		{
			GUI.Label(new Rect(Screen.width - 35, Screen.height - 60, 200, 80), " /" + mags, style3);
			GUI.Label(new Rect(Screen.width - 105, Screen.height - 60, 200, 80), "" + bullets, style1);
		}
		else
		{
			GUI.Label(new Rect(Screen.width - 35, Screen.height - 60, 200, 80), " __  / __", style3);
		}
		float minimum = 115f;
		float maximum = 155f;
		float moveSpeed = 50;
		if (currentWeapon == 0)
		{
			if (myFloat1 > minimum)
			{
				myFloat1 = myFloat1 - (Time.deltaTime * moveSpeed);
			}
			if (myFloat2 < maximum)
			{
				myFloat2 = myFloat2 + (Time.deltaTime * moveSpeed);
			}
			GUI.Label(new Rect(Screen.width - 80, Screen.height - myFloat1, 200, 60), "" + weaponsInUse[0].weaponName, style6);
			GUI.Label(new Rect(Screen.width - 80, Screen.height - myFloat2, 200, 60), "" + weaponsInUse[1].weaponName, style6);
		}
		else
		{
			if (myFloat1 < maximum)
			{
				myFloat1 = myFloat1 + (Time.deltaTime * moveSpeed);
			}
			if (myFloat2 > minimum)
			{
				myFloat2 = myFloat2 - (Time.deltaTime * moveSpeed);
			}
			GUI.Label(new Rect(Screen.width - 80, Screen.height - myFloat1, 200, 60), "" + weaponsInUse[0].weaponName, style6);
			GUI.Label(new Rect(Screen.width - 80, Screen.height - myFloat2, 200, 60), "" + weaponsInUse[1].weaponName, style6);
		}
		if (sentrygunUnits > 0)
		{
			GUI.DrawTexture(new Rect(Screen.width - 105, Screen.height - 220, 100, 70), sentryGunTexture);
			GUI.Label(new Rect(Screen.width - 140, Screen.height - 200, 100, 70), sentrygunUnits.ToString(), style4);
		}
		if (showInfo)
		{
			if (pickupGUI)
			{
				if (wepInUseGUI)
				{
					GUI.Label(new Rect((Screen.width / 2) - 400, 10, 800, 100), textWepName + " is already equipped", style5);
				}
				else
				{
					GUI.Label(new Rect((Screen.width / 2) - 400, 10, 800, 100), "" + textWepName, style5);
				}
			}
			else
			{
				if (buyWepGUI)
				{
					if (wepInUseGUI)
					{
						if (enoughMoney)
						{
							GUI.Label(new Rect((Screen.width / 2) - 400, 10, 800, 100), ((textWepName + " is already equipped") + " \n Ammo Price = ") + ammoPrice, style5);
						}
						else
						{
							GUI.Label(new Rect((Screen.width / 2) - 400, 10, 800, 100), (textWepName + " is already equipped") + " \n Not enough money for AMMO", style5);
						}
					}
					else
					{
						//Ammo
						//if(wepInUseGUI){
						//if(){
						//GUI.Label(Rect(Screen.width/2 - 400,10,800,100), textWepName + "Press key  [ E ]  to buy >" + " \n Ammo" + ammoPickup, style5);
						//}
						//}
						//Ammo
						if (enoughMoney)
						{
							GUI.Label(new Rect((Screen.width / 2) - 400, 10, 800, 100), (("Press key  [ E ]  to buy > " + textWepName) + " \n  Price: ") + wepPrice, style5);
						}
						else
						{
							GUI.Label(new Rect((Screen.width / 2) - 400, 10, 800, 100), (((textWepName + " \n Not enough money  ") + " [ Price = ") + wepPrice) + " ] ", style5);
						}
					}
				}
			}
		}
	}*/
}
