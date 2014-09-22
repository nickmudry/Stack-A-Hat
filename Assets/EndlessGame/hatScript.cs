using UnityEngine;
using System.Collections;

/// <summary>
/// Hat script.
/// 
/// Hats.
/// </summary>

public class hatScript : MonoBehaviour 
{
	public GameObject sceneController;
	mainGame controllerScript;

	private bool hatStopped;	//once on, stop doing stuff to the hat
	private BoxCollider hatCollider;	//this is for future-proofing for when we add more hats with different colliders

	void Awake () 
	{
		sceneController = GameObject.FindGameObjectWithTag("Scene Controller");
		controllerScript = sceneController.GetComponent<mainGame>();

		hatCollider = this.gameObject.GetComponent<BoxCollider>();
	}

	void OnTriggerEnter(Collider other)
	{
		if(other.tag == "Head" || other.tag == "Hat")
		{
			if(!hatStopped)
			{
				Debug.Log ("Score++");
				controllerScript.score++;

				//I'll have a way later on to move the camera only when it reaches the uppermost hat
				//I'll wait for when we test more and decide if we only allow stacking on the topmost hat or allow stacking on other hats visible on screen
				controllerScript.MoveCamera(hatCollider.size.y/3.0f);

				controllerScript.timeBetweenWaves = 0.8f;

				this.gameObject.rigidbody.isKinematic = true;	//Stop hat from moving 
				this.gameObject.transform.parent = GameObject.FindGameObjectWithTag("Head").transform;	//Parent it to the head

				hatStopped = true;
			}
		}

		if(other.tag == "HatDestroyer")
		{
			if(!hatStopped)
			{
				Debug.Log("Health--");
				controllerScript.health--;
				this.gameObject.SetActive(false);
				hatStopped = true;
			}
		}
	}
}
