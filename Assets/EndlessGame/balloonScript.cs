using UnityEngine;
using System.Collections;

/// <summary>
/// Balloon script.
/// This handles collisions for the balloon as it enters and exit the camera's view.
/// </summary>

public class balloonScript : MonoBehaviour 
{
	public GameObject attachedHat;
	public GameObject detachedHat;

	public float balloonSpeed;
	public float balloonLift;

	//audio variables
	mainGame controllerScript;

	void Awake()
	{
		controllerScript = GameObject.FindGameObjectWithTag("Scene Controller").GetComponent<mainGame>();
	}

	void Start () 
	{	
		if(this.transform.position.x > 0)
		{
			Debug.Log (this.transform.position);
			balloonSpeed = -Random.Range (0.8f, 1.2f) * controllerScript.score * 0.25f - 2.0f;
		}
		else
		{
			balloonSpeed = Random.Range (0.8f, 1.2f) * controllerScript.score * 0.25f + 2.0f;
		}
		balloonLift = Random.Range (0, .5f);
	}

	void Update()
	{
		this.transform.position = new Vector3(	this.transform.position.x + balloonSpeed*Time.deltaTime,
                            					this.transform.position.y + balloonLift*Time.deltaTime,
	                                     		this.transform.position.z );	
	}

	/* I wanted to switch exit and enter really badly since it makes sense for it to set inView true as it enters and decrement score
	 * as it leaves, but that means it enters and exits and does both at the same time.
	 * I have another way to improve this, but I'll wait until we get it running on Android before I try implementing. 
	 */
//	void OnTriggerExit(Collider other)
//	{
//		if(other.tag == "SideBound")
//		{
//			if(!inView)
//			{
//				inView = true;
//			}
//		}
//	}
//
//	void OnTriggerEnter(Collider other)
//	{
//		if(other.tag == "SideBound")
//		{
//			if(inView)
//			{
//              SFX_balRocket.audio.Play(); //plays the fizzle from missing a hat
//				Debug.Log("Health--");
//				controllerScript.health--;
//				controllerScript.waveSpawned = false;
//				this.gameObject.SetActive(false);
//				inView = false;
//			}
//		}
//	}

	void OnMouseDown()
	{
		this.gameObject.SetActive(false);
//		Instantiate(detachedHat, attachedHat.transform.position, detachedHat.transform.rotation);
		controllerScript.currentBalloons--;

		this.gameObject.transform.GetChild(1).gameObject.rigidbody2D.gravityScale = 1.0f;
		this.gameObject.transform.GetChild(1).gameObject.GetComponent<hatScript>().hatActive = true;
		this.gameObject.transform.GetChild(1).gameObject.tag = "Active Hat";
		this.gameObject.transform.GetChild(1).parent = null;
//		this.gameObject.tran
	}
}
