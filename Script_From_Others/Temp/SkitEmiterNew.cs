using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SkitEmiterNew : MonoBehaviour {
	public SoundController skid;
	//var emiterFL : ParticleEmitter;
	//var emiterFR : ParticleEmitter;
	public ParticleSystem[] emiter;
	public void Start()
	{
		//emiterFL.gameObject.SetActive(true);
		//emiterFR.gameObject.SetActive(true);
		for (int i = 0; i < emiter.Length; i++){
			emiter[i].gameObject.SetActive(true);
		}
		//emiterRL.gameObject.SetActive(true);
		//emiterRL.gameObject.SetActive(true);
	}

	public void Update()
	{
		if (skid.emit == true)
		{
			//emiterFL.emit = true;
			//emiterFR.emit = true;
			for (int i = 0; i < emiter.Length; i++)
			{
				emiter[i].Emit(1);
				emiter[i].Play();
			}
			//emiterRL.Play();
			//emiterRR.Play();
		}
		else
		{
			//emiterFL.emit = false;
			//emiterFR.emit = false; 
			for (int i = 0; i < emiter.Length; i++)
			{
				//emiter[i].Stop();
				emiter[i].Stop(true, ParticleSystemStopBehavior.StopEmitting);
			}
			//emiterRL.Stop();
			//emiterRR.Stop();
		}
	}
}
