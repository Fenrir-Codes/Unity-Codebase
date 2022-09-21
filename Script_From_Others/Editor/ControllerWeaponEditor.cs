using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

[CustomEditor(typeof(ControllerWeapon))]
public class ControllerWeaponEditor : Editor {

	public override void OnInspectorGUI()
	{
		var _target = target as ControllerWeapon;
		bool allowSceneObjects = !EditorUtility.IsPersistent(_target);
		//EditorGUIUtility.LookLikeInspector ();
		//EditorGUIUtility.LookLikeControls();
		GUILayout.Label("Weapon run Position/Rotation", EditorStyles.boldLabel);

		EditorGUILayout.BeginVertical("Box");
		_target.moveTo = EditorGUILayout.Vector3Field("", _target.moveTo);
		if (GUILayout.Button(new GUIContent("Save Run Position"), "miniButton"))
		{
			_target.moveTo = _target.transform.localPosition;
		}

		_target.rotateTo = EditorGUILayout.Vector3Field("", _target.rotateTo);
		if (GUILayout.Button(new GUIContent("Save Run Rotation"), "miniButton"))
		{
			_target.rotateTo = _target.transform.localEulerAngles;
		}

		_target.movementSpeed = EditorGUILayout.FloatField("Smooth ", _target.movementSpeed);
		if (GUILayout.Button(new GUIContent("Reset Position/Rotation"), "miniButton"))
		{
			_target.transform.localPosition = Vector3.zero;
			_target.transform.localEulerAngles = Vector3.zero;
		}

		if (GUILayout.Button(new GUIContent("Set Position/Rotation"), "miniButton"))
		{
			_target.transform.localPosition = _target.moveTo;
			_target.transform.localEulerAngles = _target.rotateTo;
		}
		EditorGUILayout.EndVertical();

		GUILayout.Label("Weapon Sway Animations", EditorStyles.boldLabel);
		EditorGUILayout.BeginVertical("Box");

		_target.sway = EditorGUILayout.TextField("Sway anim name", _target.sway);
		_target.idle = EditorGUILayout.TextField("Idle anim name", _target.idle);
		_target.anim = (GameObject)EditorGUILayout.ObjectField("Animation GO ", _target.anim, typeof(GameObject), allowSceneObjects);
		_target.animSpeed = EditorGUILayout.FloatField("Anim. Speed ", _target.animSpeed);
		_target.withWeapon = EditorGUILayout.Toggle("With Weapon", _target.withWeapon);
		EditorGUILayout.EndVertical();


		GUILayout.Label("Attach Scripts", EditorStyles.boldLabel);
		EditorGUILayout.BeginVertical("Box");
		_target.codcontroller = (PlayerController)EditorGUILayout.ObjectField("CODcontroller ", _target.codcontroller, typeof(PlayerController), allowSceneObjects);
		_target.weaponScript = (ScriptWeapon)EditorGUILayout.ObjectField("WeaponScript ", _target.weaponScript, typeof(ScriptWeapon), allowSceneObjects);
		EditorGUILayout.EndVertical();
	}
}
