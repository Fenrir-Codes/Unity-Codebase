using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class ConsoleInputSubmit : InputField
{

	public SubmitEvent onSubmit_ = new SubmitEvent();
	private HUDManager HUD;

	protected override void Start()
	{
		HUD = FindObjectOfType<HUDManager>();
	}

	public override void OnSubmit(BaseEventData eventData)
	{
		base.OnSubmit(eventData);
		//if (onSubmit_ != null)
		//	onSubmit_.Invoke(text);

		HUD.ChangeValueCommand();
	}
}
