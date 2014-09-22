using UnityEngine;
using System.Collections;

public class hatScript : MonoBehaviour 
{
	public GameObject sceneController;

	private bool hatStopped;
	private bool dataSent;
	
	mainGame controllerScript;

	private BoxCollider hatCollider;

	// Use this for initialization
	void Start () 
	{
		sceneController = GameObject.FindGameObjectWithTag("Scene Controller");
		controllerScript = sceneController.GetComponent<mainGame>();

		hatCollider = this.gameObject.GetComponent<BoxCollider>();
	}
	
	// Update is called once per frame
	void Update () 
	{
	}

	void OnTriggerEnter(Collider other)
	{
		if(other.tag == "Head" || other.tag == "Hat")
		{
			if(!dataSent)
			{
				Debug.Log ("Score++");
				controllerScript.score++;
				controllerScript.MoveCamera(hatCollider.size.y/3.0f);

				controllerScript.timeBetweenWaves = 0.8f;

				this.gameObject.rigidbody.isKinematic = true;
				this.gameObject.transform.parent = GameObject.FindGameObjectWithTag("Head").transform; 

				dataSent = true;
			}
		}

		if(other.tag == "HatDestroyer")
		{
			if(!dataSent)
			{
				Debug.Log("Health--");
				controllerScript.health--;
				this.gameObject.SetActive(false);
				dataSent = true;
			}
		}
	}
}
