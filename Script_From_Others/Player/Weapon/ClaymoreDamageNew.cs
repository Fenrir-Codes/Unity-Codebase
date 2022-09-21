using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ClaymoreDamageNew : MonoBehaviour {

	public ClaymoreMineNew claymoremine;

	public void ApplyDamage()
	{
		StartCoroutine(claymoremine.Explosion());
	}

}
