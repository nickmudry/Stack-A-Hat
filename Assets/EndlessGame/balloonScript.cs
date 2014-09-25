using UnityEngine;
using System.Collections;

/// <summary>
/// Balloon script.
/// This handles collisions for the balloon as it enters and exit the camera's view.
/// </summary>

public class balloonScript : MonoBehaviour 
{
	public GameObject sceneController;
	mainGame controllerScript;

    //audio variables
    public GameObject SFX_balRocket;

	[System.NonSerialized]
	public bool inView;	//public for mainGame.cs to access. This is primarily for left and right bounds to check if it's passed through one already

	void Start () 
	{	
		sceneController = GameObject.FindGameObjectWithTag("Scene Controller");
		controllerScript = sceneController.GetComponent<mainGame>();
	}

	/* I wanted to switch exit and enter really badly since it makes sense for it to set inView true as it enters and decrement score
	 * as it leaves, but that means it enters and exits and does both at the same time.
	 * I have another way to improve this, but I'll wait until we get it running on Android before I try implementing. 
	 */
	void OnTriggerExit(Collider other)
	{
		if(other.tag == "SideBound")
		{
			if(!inView)
			{
				inView = true;
			}
		}
		if(other.tag == "HatDestroyer")
		{
			Debug.Log("Health--");
			controllerScript.health--;
			controllerScript.waveSpawned = false;
			this.gameObject.SetActive(false);
			inView = false;
		}
	}

	void OnTriggerEnter(Collider other)
	{
		if(other.tag == "SideBound")
		{
			if(inView)
			{
                SFX_balRocket.audio.Play(); //plays the fizzle from missing a hat
				Debug.Log("Health--");
				controllerScript.health--;
				controllerScript.waveSpawned = false;
				this.gameObject.SetActive(false);
				inView = false;
			}
		}
	}
}
