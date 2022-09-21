using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InfoWeapon : MonoBehaviour {
	//This script must be attached to weapon, which you can pick up from the ground.
	public int weaponIndex;  //tells which weapon to select. We take this index from PlayerWeapons script (WeaponList - count from top starting from zero)
	public string weaponName = ""; 
}
