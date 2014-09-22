using UnityEngine;
using System.Collections;

public class balloonScript : MonoBehaviour 
{
	public GameObject sceneController;
	public bool inView;

	private bool hatStopped;
	private bool dataSent;
	
	mainGame controllerScript;

	void Start () 
	{	
		sceneController = GameObject.FindGameObjectWithTag("Scene Controller");
		controllerScript = sceneController.GetComponent<mainGame>();
	}
	
	// Update is called once per frame
	void Update () 
	{
	}

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
				Debug.Log("Health--");
				controllerScript.health--;
				controllerScript.waveSpawned = false;
				this.gameObject.SetActive(false);
				inView = false;
			}
		}
	}
}
