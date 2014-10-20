using UnityEngine;
using System.Collections;

/// <summary>
/// Hat script.
/// 
/// Hats.
/// </summary>

public class hatScript : MonoBehaviour 
{
	mainGame controllerScript;

	public bool hatActive;	//once on, stop doing stuff to the hat
	private BoxCollider2D hatCollider;	//this is for future-proofing for when we add more hats with different colliders

	void Awake () 
	{
		controllerScript = GameObject.FindGameObjectWithTag("Scene Controller").GetComponent<mainGame>();

		hatCollider = this.gameObject.GetComponent<BoxCollider2D>();
	}

	void OnTriggerEnter2D(Collider2D other)
	{
		if(other.tag == "Head" && controllerScript.score <= 0 || other.tag == "Hat")
		{
			if(hatActive)
			{
				if(other.tag == "Hat")
				{
					other.tag = "Inactive Hat";
				}

				this.rigidbody2D.gravityScale = 0.0f;
                audio.Play();
				Debug.Log ("Score++");
				controllerScript.score++;

				//I'll have a way later on to move the camera only when it reaches the uppermost hat
				//I'll wait for when we test more and decide if we only allow stacking on the topmost hat or allow stacking on other hats visible on screen
				controllerScript.MoveCamera(hatCollider.size.y/5.0f);
				controllerScript.timeBetweenWaves = 0.0f;

				this.gameObject.rigidbody2D.isKinematic = true;	//Stop hat from moving 
				this.gameObject.transform.parent = GameObject.FindGameObjectWithTag("Head").transform;	//Parent it to the head

				hatActive = false;
			}
		}

//		if(other.tag == "HatDestroyer")
//		{
//			if(!hatStopped)
//			{
//				Debug.Log("Health--");
//				controllerScript.health--;
//				this.gameObject.SetActive(false);
//				hatStopped = true;
//			}
//		}
	}
}
