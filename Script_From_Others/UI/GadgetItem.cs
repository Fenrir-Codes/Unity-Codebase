using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GadgetItem : MonoBehaviour {

	public Image ItemIcon;

	public void ItemSetup(Sprite Image)
	{
		ItemIcon.sprite = Image;
	}
}
