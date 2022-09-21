using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/*
beep sound - http://www.freesound.org/people/unfa/sounds/154894/

*/
public class GrenadeNew : MonoBehaviour {

    public float explodeAfter;
    public Transform explosion;
    public AudioClip soundHit;
    private float modifier;

	void Start()
	{
		StartCoroutine(DestroyNow());
	}

	public IEnumerator DestroyNow()
	{

		yield return new WaitForSeconds(explodeAfter);
		Instantiate(explosion, transform.position, transform.rotation);
		Destroy(gameObject);
	}

	public void FixedUpdate()
	{

		modifier = GetComponent<Rigidbody>().velocity.magnitude / 100;
		if (modifier > 0.3)
		{
			(GetComponent<Collider>() as SphereCollider).radius = .08f + modifier / 2f;
		}
		else
		{
			(GetComponent<Collider>() as SphereCollider).radius = .08f;
		}
	}

	public IEnumerator ApplyDamage(int damage)
	{
		yield return new WaitForSeconds(.1f);
		Instantiate(explosion, transform.position, transform.rotation);
		Destroy(gameObject);
	}

	public void OnCollisionEnter()
	{
		//if(modifier > .1)
		GetComponent<AudioSource>().PlayOneShot(soundHit, 0.3f);
	}
}
