using UnityEngine;
using System.Collections;

public class hatScript : MonoBehaviour 
{
	public GameObject sceneController;

	private bool hatStopped;
	private bool dataSent;

	private float delay;
//	endless_prototype controllerScript;
	mainGame controllerScript;


	// Use this for initialization
	void Start () 
	{
		sceneController = GameObject.FindGameObjectWithTag("Scene Controller");
		controllerScript = sceneController.GetComponent<mainGame>();
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
				if(this.gameObject.transform.position.y > controllerScript.mainCamera.transform.position.y - 4.0f)
				{
					Debug.Log ("Score++");
					controllerScript.score++;
					controllerScript.MoveCamera(this.gameObject.transform.localScale.y);

					this.gameObject.rigidbody.isKinematic = true;
					this.gameObject.transform.parent = GameObject.FindGameObjectWithTag("Head").transform; 

					dataSent = true;
				}
				else
				{
					Debug.Log("Health--");
					controllerScript.health--;
					this.gameObject.SetActive(false);
					dataSent = true;
				}
			}
		}
		if(other.tag == "HatDestroyer")
		{
			Debug.Log("Health--");
			controllerScript.health--;
			this.gameObject.SetActive(false);
			dataSent = true;
		}
	}
}
