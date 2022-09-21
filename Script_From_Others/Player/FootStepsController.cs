using System.Collections;
using System.Collections.Generic;
using System;
using UnityEngine;
using Random = UnityEngine.Random;

/**
*  Script written by OMA [www.armedunity.com]
*  Rewritten on C# & adapted for latest version Unity3D by DeadZone [vk.com/id160454360] || [Discord: DeadZoneGarry#3474] || [Skype: vanya197799] || [steamcommunity.com/profiles/76561198121485860]
**/

public class FootStepsController : MonoBehaviour {

	public AudioClip[] concrete;
	public AudioClip[] wood;
	//public AudioClip[] metal;
	public AudioClip[] dirt;
	//public AudioClip[] sand;
	public AudioClip[] grass;
	//public AudioClip[] snow;
	//public AudioClip[] water;

	public float audioStepLengthCrouch = 0.7f;
	public float audioStepLengthWalk = 0.45f;
	public float audioStepLengthRun = 0.25f;
	public float minWalkSpeed = 2.5f;
	public float maxWalkSpeed = 8.0f;
	public float audioVolumeCrouch = 0.2f;
	public float audioVolumeWalk = 0.8f;
	public float audioVolumeRun = 1.0f;
	private bool step = true;
	public GameObject soundsGO; 
	public CharacterController controller;
	public PlayerController cod;
	private string curMat;

	//Terrain
	private int surfaceIndex = 0;
	private Terrain terrain;
	private TerrainData terrainData;
	private Vector3 terrainPos;
	private bool onTerrain = false;

	void Start () {
		//for terrain
		terrain = Terrain.activeTerrain;
		if (terrain == null) return;
		terrainData = terrain.terrainData;
		terrainPos = terrain.transform.position;
	}

	//when you get out of vehicle
	void OnEnable()
	{
		step = true;
	}

	void OnControllerColliderHit(ControllerColliderHit hit)
	{
		float speed = controller.velocity.magnitude;
		curMat = hit.gameObject.tag;
		if (cod.state == 2) return;
		if (hit.collider.name == "Terrain")
		{
			//get index from terrain texture
			surfaceIndex = GetMainTexture(transform.position);
			onTerrain = true;
		}
		else
		{
			onTerrain = false;
		}

		if (controller.isGrounded && step && hit.normal.y > 0.8)
		{

			if (!onTerrain && curMat == "Untagged" || !onTerrain && curMat == "Concrete" || onTerrain && surfaceIndex == 0)
			{
				if (speed < minWalkSpeed && speed > 0.5) StartCoroutine(CrouchOnConcrete());
				if (speed < maxWalkSpeed && speed > minWalkSpeed) StartCoroutine(WalkOnConcrete());
				if (speed > maxWalkSpeed) StartCoroutine(RunOnConcrete());
			}
			else if (!onTerrain && curMat == "Wood")
			{
				if (speed < minWalkSpeed && speed > 0.5) StartCoroutine(CrouchOnWood());
				if (speed < maxWalkSpeed && speed > minWalkSpeed) StartCoroutine(WalkOnWood());
				if (speed > maxWalkSpeed) StartCoroutine(RunOnWood());
			}
			else if (!onTerrain && curMat == "Dirt" || onTerrain && surfaceIndex == 2)
			{
				if (speed < minWalkSpeed && speed > 0.5) StartCoroutine(CrouchOnDirt());
				if (speed < maxWalkSpeed && speed > minWalkSpeed) StartCoroutine(WalkOnDirt());
				if (speed > maxWalkSpeed) StartCoroutine(RunOnDirt());
				//}else if(curMat == "Metal"){
				//	if( speed < minWalkSpeed && speed > 1.0) CrouchOnMetal();
				//	if( speed < maxWalkSpeed && speed > minWalkSpeed) WalkOnMetal();
				//	if( speed > maxWalkSpeed) RunOnMetal();
				//}else if(curMat == "Sand"){
				//	if( speed < minWalkSpeed && speed > 1.0) CrouchOnSand();
				//	if( speed < maxWalkSpeed && speed > minWalkSpeed) WalkOnSand();
				//	if( speed > maxWalkSpeed) RunOnSand();
			}
			else if (!onTerrain && curMat == "Grass" || onTerrain && surfaceIndex == 1)
			{
				if (speed < minWalkSpeed && speed > 0.5) StartCoroutine(CrouchOnGrass());
				if (speed < maxWalkSpeed && speed > minWalkSpeed) StartCoroutine(WalkOnGrass());
				if (speed > maxWalkSpeed) StartCoroutine(RunOnGrass());
				//}else if(curMat == "Snow"){
				//	if( speed < minWalkSpeed && speed > 1.0) CrouchOnSnow();
				//	if( speed < maxWalkSpeed && speed > minWalkSpeed) WalkOnSnow();
				//	if( speed > maxWalkSpeed) RunOnSnow();
				//}else if(curMat == "Water"){
				//	if( speed < maxWalkSpeed && speed > minWalkSpeed) WalkOnWater();
				//	if( speed > maxWalkSpeed) RunOnWater();	
			}
		}
	}

	public IEnumerator JumpLand()
	{
		if (onTerrain)
		{
			if (surfaceIndex == 0)
			{
				PlayClimbSounds(concrete[Random.Range(0, concrete.Length)], soundsGO.transform.position, 1.0f);
				yield return new WaitForSeconds(0.1f);
				PlayClimbSounds(concrete[Random.Range(0, concrete.Length)], soundsGO.transform.position, 1.0f);
			}
			else if (surfaceIndex == 1)
			{
				PlayClimbSounds(grass[Random.Range(0, grass.Length)], soundsGO.transform.position, 1.0f);
				yield return new WaitForSeconds(0.1f);
				PlayClimbSounds(grass[Random.Range(0, grass.Length)], soundsGO.transform.position, 1.0f);
			}
			else if (surfaceIndex == 2)
			{
				PlayClimbSounds(dirt[Random.Range(0, dirt.Length)], soundsGO.transform.position, 1.0f);
				yield return new WaitForSeconds(0.1f);
				PlayClimbSounds(dirt[Random.Range(0, dirt.Length)], soundsGO.transform.position, 1.0f);
			}
		}
		else
		{
			if (curMat == "Untagged" || curMat == "Concrete")
			{
				PlayClimbSounds(concrete[Random.Range(0, concrete.Length)], soundsGO.transform.position, 0.8f);
				yield return new WaitForSeconds(0.1f);
				PlayClimbSounds(concrete[Random.Range(0, concrete.Length)], soundsGO.transform.position, 0.8f);
			}
			else if (curMat == "Wood")
			{
				PlayClimbSounds(wood[Random.Range(0, wood.Length)], soundsGO.transform.position, 1.0f);
				yield return new WaitForSeconds(0.1f);
				PlayClimbSounds(wood[Random.Range(0, wood.Length)], soundsGO.transform.position, 1.0f);
			}
			else if (curMat == "Dirt")
			{
				PlayClimbSounds(dirt[Random.Range(0, dirt.Length)], soundsGO.transform.position, 1.0f);
				yield return new WaitForSeconds(0.1f);
				PlayClimbSounds(dirt[Random.Range(0, dirt.Length)], soundsGO.transform.position, 1.0f);
			}
			else if (curMat == "Grass")
			{
				PlayClimbSounds(grass[Random.Range(0, grass.Length)], soundsGO.transform.position, 1.0f);
				yield return new WaitForSeconds(0.1f);
				PlayClimbSounds(grass[Random.Range(0, grass.Length)], soundsGO.transform.position, 1.0f);
			}
		}
	}


	float[] GetTextureMix(Vector3 worldPos)
	{

		int mapX = Convert.ToInt32(((worldPos.x - terrainPos.x) / terrainData.size.x) * terrainData.alphamapWidth );
		int mapZ = Convert.ToInt32(((worldPos.z - terrainPos.z) / terrainData.size.z) * terrainData.alphamapHeight );
		float[,,] splatmapData = terrainData.GetAlphamaps(mapX, mapZ, 1, 1 );
		float[] cellMix = new float[splatmapData.GetUpperBound(2) + 1];
	 
		for (int n = 0; n < cellMix.Length; n ++ ){
			cellMix[n] = splatmapData[0, 0, n];
		}
	
		return cellMix;
	}
 
 
	int GetMainTexture(Vector3 worldPos) 
	{
		float[] mix = GetTextureMix(worldPos );
		float maxMix = 0;
		int maxIndex = 0;

		for (int n = 0; n < mix.Length; n++ ){
			if (mix[n] > maxMix){
				maxIndex = n;
				maxMix = mix[n];
			}
		}

		return maxIndex;
	}


	/////////////////////////////////// CONCRETE ////////////////////////////////////////
	public IEnumerator CrouchOnConcrete()
	{
		step = false;
		PlayClimbSounds(concrete[Random.Range(0, concrete.Length)], soundsGO.transform.position, .1f);
		//soundsGO.GetComponent<AudioSource>().clip = concrete[Random.Range(0, concrete.length)];
		//soundsGO.GetComponent<AudioSource>().volume = 0.2;
		//soundsGO.GetComponent<AudioSource>().Play();
		yield return new WaitForSeconds(audioStepLengthCrouch);
		step = true;
	}



	public IEnumerator WalkOnConcrete()
	{
		step = false;
		PlayClimbSounds(concrete[Random.Range(0, concrete.Length)], soundsGO.transform.position, .4f);

		//soundsGO.GetComponent<AudioSource>().clip = concrete[Random.Range(0, concrete.length)];
		//soundsGO.GetComponent<AudioSource>().volume = audioVolumeWalk;
		//soundsGO.GetComponent<AudioSource>().Play();
		yield return new WaitForSeconds(audioStepLengthWalk);
		step = true;
	}

	public IEnumerator RunOnConcrete()
	{
		step = false;
		PlayClimbSounds(concrete[Random.Range(0, concrete.Length)], soundsGO.transform.position, .6f);
		//soundsGO.GetComponent<AudioSource>().clip = concrete[Random.Range(0, concrete.length)];
		//soundsGO.GetComponent<AudioSource>().volume = audioVolumeRun;
		//soundsGO.GetComponent<AudioSource>().Play();
		yield return new WaitForSeconds(audioStepLengthRun);
		step = true;
	}


	////////////////////////////////// WOOD /////////////////////////////////////////////
	public IEnumerator CrouchOnWood()
	{
		step = false;
		soundsGO.GetComponent<AudioSource>().clip = wood[Random.Range(0, wood.Length)];
		soundsGO.GetComponent<AudioSource>().volume = audioVolumeCrouch;
		soundsGO.GetComponent<AudioSource >().Play();
		yield return new WaitForSeconds(audioStepLengthCrouch);
		step = true;
	}

	public IEnumerator WalkOnWood()
	{
		step = false;
		PlayClimbSounds(wood[Random.Range(0, wood.Length)], soundsGO.transform.position, .3f);
		//soundsGO.GetComponent<AudioSource>().clip = wood[Random.Range(0, wood.length)];
		//soundsGO.GetComponent<AudioSource>().volume = audioVolumeWalk;
		//soundsGO.GetComponent<AudioSource>().Play();
		yield return new WaitForSeconds(audioStepLengthWalk);
		step = true;
	}

	public IEnumerator RunOnWood()
	{
		step = false;
		PlayClimbSounds(wood[Random.Range(0, wood.Length)], soundsGO.transform.position, .6f);
		//soundsGO.GetComponent<AudioSource>().clip = wood[Random.Range(0, wood.length)];
		//soundsGO.GetComponent<AudioSource>().volume = audioVolumeRun;
		//soundsGO.GetComponent<AudioSource>().Play();
		yield return new WaitForSeconds(audioStepLengthRun);
		step = true;
	}

	/////////////////////////////////// DIRT //////////////////////////////////////////////
	public IEnumerator CrouchOnDirt()
	{
		step = false;
		soundsGO.GetComponent<AudioSource>().clip = dirt[Random.Range(0, dirt.Length)];
		soundsGO.GetComponent<AudioSource>().volume = audioVolumeCrouch;
		soundsGO.GetComponent<AudioSource>().Play();
		yield return new WaitForSeconds(audioStepLengthCrouch);
		step = true;
	}

	public IEnumerator WalkOnDirt()
	{
		step = false;
		PlayClimbSounds(dirt[Random.Range(0, dirt.Length)], soundsGO.transform.position, .4f);

		//soundsGO.GetComponent<AudioSource>().clip = dirt[Random.Range(0, dirt.length)];
		//soundsGO.GetComponent<AudioSource>().volume = audioVolumeWalk;
		//soundsGO.GetComponent<AudioSource>().Play();
		yield return new WaitForSeconds(audioStepLengthWalk);
		step = true;
	}

	public IEnumerator RunOnDirt()
	{
		step = false;
		PlayClimbSounds(dirt[Random.Range(0, dirt.Length)], soundsGO.transform.position, .8f);
		//soundsGO.GetComponent<AudioSource>().clip = dirt[Random.Range(0, dirt.length)];
		//soundsGO.GetComponent<AudioSource>().volume = audioVolumeRun;
		//soundsGO.GetComponent<AudioSource>().Play();
		yield return new WaitForSeconds(audioStepLengthRun);
		step = true;
	}

	/*
	////////////////////////////////// METAL ///////////////////////////////////////////////
	void CrouchOnMetal(){
		step = false;
		soundsGO.GetComponent<AudioSource>().clip = metal[Random.Range(0, metal.length)];
		soundsGO.GetComponent<AudioSource>().volume = audioVolumeCrouch;
		soundsGO.GetComponent<AudioSource>().Play();
		yield return new WaitForSeconds (audioStepLengthCrouch);
		step = true;
	}

	void WalkOnMetal() {	
		step = false;
		PlayClimbSounds(metal[Random.Range(0, metal.length)], soundsGO.transform.position, .3);
		//soundsGO.GetComponent<AudioSource>().clip = metal[Random.Range(0, metal.length)];
		//soundsGO.GetComponent<AudioSource>().volume = audioVolumeWalk;
		//soundsGO.GetComponent<AudioSource>().Play();
		yield return new WaitForSeconds (audioStepLengthWalk);
		step = true;
	}

	void RunOnMetal() {
		step = false;
		PlayClimbSounds(metal[Random.Range(0, metal.length)], soundsGO.transform.position, .7);
		//soundsGO.GetComponent<AudioSource>().clip = metal[Random.Range(0, metal.length)];
		//soundsGO.GetComponent<AudioSource>().volume = audioVolumeRun;
		//soundsGO.GetComponent<AudioSource>().Play();
		yield return new WaitForSeconds (audioStepLengthRun);
		step = true;
	}


	////////////////////////////////// SAND ///////////////////////////////////////////////
	void CrouchOnSand(){
		step = false;
		soundsGO.GetComponent<AudioSource>().clip = sand[Random.Range(0, sand.length)];
		soundsGO.GetComponent<AudioSource>().volume = audioVolumeCrouch;
		soundsGO.GetComponent<AudioSource>().Play();
		yield return new WaitForSeconds (audioStepLengthCrouch);
		step = true;
	}

	void WalkOnSand() {	
		step = false;
		soundsGO.GetComponent<AudioSource>().clip = sand[Random.Range(0, sand.length)];
		soundsGO.GetComponent<AudioSource>().volume = audioVolumeWalk;
		soundsGO.GetComponent<AudioSource>().Play();
		yield return new WaitForSeconds (audioStepLengthWalk);
		step = true;
	}

	void RunOnSand() {
		step = false;
		soundsGO.GetComponent<AudioSource>().clip = sand[Random.Range(0, sand.length)];
		soundsGO.GetComponent<AudioSource>().volume = audioVolumeRun;
		soundsGO.GetComponent<AudioSource>().Play();
		yield return new WaitForSeconds (audioStepLengthRun);
		step = true;
	}
	*/
	////////////////////////////////// GRASS ///////////////////////////////////////////////
	public IEnumerator CrouchOnGrass()
	{
		step = false;
		soundsGO.GetComponent<AudioSource>().clip = grass[Random.Range(0, grass.Length)];
		soundsGO.GetComponent<AudioSource>().volume = audioVolumeCrouch;
		soundsGO.GetComponent<AudioSource>().Play();
		yield return new WaitForSeconds(audioStepLengthCrouch);
		step = true;
	}

	public IEnumerator WalkOnGrass()
	{
		step = false;
		PlayClimbSounds(grass[Random.Range(0, grass.Length)], soundsGO.transform.position, .3f);
		//soundsGO.GetComponent<AudioSource>().clip = grass[Random.Range(0, grass.length)];
		//soundsGO.GetComponent<AudioSource>().volume = audioVolumeWalk;
		//soundsGO.GetComponent<AudioSource>().Play();
		yield return new WaitForSeconds(audioStepLengthWalk);
		step = true;
	}

	public IEnumerator RunOnGrass()
	{
		step = false;
		PlayClimbSounds(grass[Random.Range(0, grass.Length)], soundsGO.transform.position, .6f);
		//soundsGO.GetComponent<AudioSource>().clip = grass[Random.Range(0, grass.length)];
		//soundsGO.GetComponent<AudioSource>().volume = audioVolumeRun;
		//soundsGO.GetComponent<AudioSource>().Play();
		yield return new WaitForSeconds(audioStepLengthRun);
		step = true;
	}
	/*
	////////////////////////////////// SNOW ///////////////////////////////////////////////
	void CrouchOnSnow(){
		step = false;
		soundsGO.GetComponent<AudioSource>().clip = snow[Random.Range(0, snow.length)];
		soundsGO.GetComponent<AudioSource>().volume = audioVolumeCrouch;
		soundsGO.GetComponent<AudioSource>().Play();
		yield return new WaitForSeconds (audioStepLengthCrouch);
		step = true;
	}

	void WalkOnSnow() {	
		step = false;
		soundsGO.GetComponent<AudioSource>().clip = snow[Random.Range(0, snow.length)];
		soundsGO.GetComponent<AudioSource>().volume = audioVolumeWalk;
		soundsGO.GetComponent<AudioSource>().Play();
		yield return new WaitForSeconds (audioStepLengthWalk);
		step = true;
	}

	void RunOnSnow() {
		step = false;
		soundsGO.GetComponent<AudioSource>().clip = snow[Random.Range(0, snow.length)];
		soundsGO.GetComponent<AudioSource>().volume = audioVolumeRun;
		soundsGO.GetComponent<AudioSource>().Play();
		yield return new WaitForSeconds (audioStepLengthRun);
		step = true;
	}

	////////////////////////////////// SNOW ///////////////////////////////////////////////

	void WalkOnWater() {	
		step = false;
		soundsGO.GetComponent<AudioSource>().clip = water[Random.Range(0, water.length)];
		soundsGO.GetComponent<AudioSource>().volume = audioVolumeWalk;
		soundsGO.GetComponent<AudioSource>().Play();
		yield return new WaitForSeconds (audioStepLengthWalk);
		step = true;
	}

	void RunOnWater() {
		step = false;
		soundsGO.GetComponent<AudioSource>().clip = water[Random.Range(0, water.length)];
		soundsGO.GetComponent<AudioSource>().volume = audioVolumeRun;
		soundsGO.GetComponent<AudioSource>().Play();
		yield return new WaitForSeconds (audioStepLengthRun);
		step = true;
	}
	*/
	//@script RequireComponent(AudioSource)


	AudioSource PlayClimbSounds(AudioClip clip, Vector3 position, float volume)
	{
		var go = new GameObject("One shot audio");
		go.transform.position = position;
		AudioSource source = go.AddComponent<AudioSource>();
		source.clip = clip;
		source.volume = volume;
		//source.pitch = Random.Range(0.9,1.1);
		source.Play();
		Destroy(go, clip.length);
		return source;
	}
}
