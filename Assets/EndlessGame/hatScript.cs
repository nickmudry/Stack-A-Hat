using UnityEngine;
using System.Collections;

/// <summary>
/// Hat script.
/// 
/// Hats.
/// </summary>

public class hatScript : MonoBehaviour 
{

	public bool hatActive;	//once on, stop doing stuff to the hat
	private BoxCollider2D hatCollider;	//this is for future-proofing for when we add more hats with different colliders

	mainGame controllerScript;

	void Awake () 
	{
		controllerScript = GameObject.FindGameObjectWithTag("Scene Controller").GetComponent<mainGame>();

		hatCollider = this.gameObject.GetComponent<BoxCollider2D>();
	}

	void OnTriggerEnter2D(Collider2D other)
	{
		if(other.tag == "Head" && controllerScript.score <= 0 || other.tag == "Active Hat")
		{
			if(hatActive)
			{
				if(other.tag == "Active Hat")
				{
					other.tag = "Inactive Hat";
				}

				this.rigidbody2D.gravityScale = 0.0f;
                audio.Play();
				controllerScript.IncreaseScore(1);

				//I'll have a way later on to move the camera only when it reaches the uppermost hat
				//I'll wait for when we test more and decide if we only allow stacking on the topmost hat or allow stacking on other hats visible on screen
				controllerScript.MoveCamera(hatCollider.size.y/5.2f);
				controllerScript.timeBetweenWaves = 0.0f;

				this.gameObject.rigidbody2D.isKinematic = true;	//Stop hat from moving 
				this.gameObject.transform.parent = GameObject.FindGameObjectWithTag("Head").transform;	//Parent it to the head

				hatActive = false;
			}
		}
	}

	//OMG, this makes everything so much easier
		//OHHhhHHhohHHhhHHhhHhh...
	void OnBecameInvisible()	
	{
		if(this.transform.parent && this.transform.parent.tag == "Balloon")	//Prevent a null exception error (shouldn't set it inactive if it doesn't exist
		{
			//What happens if it's still attached to the balloon
			this.transform.parent.gameObject.SetActive(false);

			controllerScript.waveSpawned = false;
		} 	
		else if(this.tag != "Inactive Hat")
		{
			//What happens if it's no longer attached to the balloon
			this.gameObject.SetActive(false);
		}
	}
}
