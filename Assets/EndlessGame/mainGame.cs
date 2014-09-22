using UnityEngine;
using System.Collections;

public class mainGame : MonoBehaviour 
{
	public GameObject mainCamera;

	public GameObject mouseCursorObject;

	//Here are balloons and everything associated with them
	public GameObject [] balloonObject; 
	public float [] balloonSpeed;
	public float [] balloonLift;

	public bool [] balloonTouched;

	public int waveNumber;
	public bool waveSpawned;
	public float timeBetweenWaves;
	
	public GameObject [] placeholderHat;
	public GameObject poppedHatObject;
	public GameObject headObject;

	public int score;
	public int health;

	private bool headSwayDir;
	private bool gameOver;
	    
	// Use this for initialization
	void Start () 
	{
		gameOver = false;

		score = 0;
		health = 3;

		waveSpawned = false;
		waveNumber = 0;
		timeBetweenWaves = 0.8f;   

		if(Random.Range (0, 1.0f) > 0.5f)
		{
			headSwayDir = true;
        }        

		balloonSpeed = new float[balloonObject.Length];
		balloonLift = new float[balloonObject.Length]; 
		balloonTouched = new bool[balloonObject.Length];

		for(int i = 0; i<balloonObject.Length; i++)
		{
			balloonObject[i].SetActive(false);
			balloonSpeed[i] = 0.0f;
			balloonLift[i] = 0.0f;
		}
		Debug.Log("Working");
    }
	
	// Update is called once per frame
	void Update () 
	{	
		if(health <= 0)
		{
			gameOver = true;
		}
		if(score > 1)
		{
            SwayHead();
        }
		if(!waveSpawned)
		{
			timeBetweenWaves -= Time.deltaTime;
		}
		if(timeBetweenWaves <= 0.0f)
		{
			SpawnHat();
		}

		mouseCursorObject.transform.position = Camera.main.ScreenToWorldPoint(new Vector3(Input.mousePosition.x, Input.mousePosition.y, 10.0f));

		for(int i=0; i<balloonObject.Length; i++)
		{
			if(balloonObject[i])
			{
				MoveBalloon(i);
			}
			if(Input.GetMouseButtonDown(0) && balloonTouched[i])
			{
				PopBalloon(i);
			}
		}
    }

	void MoveBalloon(int _num)
	{
		balloonObject[_num].transform.position = new Vector3(balloonObject[_num].transform.position.x + balloonSpeed[_num]*Time.deltaTime,
		                                                     balloonObject[_num].transform.position.y + balloonLift[_num]*Time.deltaTime,
		                                                     balloonObject[_num].transform.position.z);
	}

	void SpawnHat()
	{
		Debug.Log ("HAT Inbound");
		waveSpawned = true;

		balloonObject[0].SetActive(true);

		if(Random.Range (0.0f, 2.0f) <= 1.0f)
	 	{
			balloonObject[0].transform.position = new Vector3(Random.Range (-8.0f, -5.1f), 
                                     							Random.Range (0.5f, 3.0f) + mainCamera.transform.position.y - 1.0f,
			                                                     0);

			balloonSpeed[0] = Random.Range (0.8f, 1.2f) * score*0.25f + 2.0f;
		}
		else
		{
			balloonObject[0].transform.position = new Vector3(Random.Range (5.1f, 8.0f), 
		                                                	  Random.Range (0.5f, 3.0f) + mainCamera.transform.position.y - 1.0f,
                                     							0);

			balloonSpeed[0] = Random.Range (0.8f, 1.2f) * -score*0.25f - 2.0f;
		}

		balloonLift[0] = Random.Range (0, .5f)*score*0.5f;
		timeBetweenWaves = 0.8f;  
	}

	void SwayHead()
	{
		if(headObject.transform.position.x <= -3.2f * (float)score/30.0f)
		{
			headSwayDir = true;
		}
		if(headObject.transform.position.x >= 3.2f * (float)score/30.0f)
		{
			headSwayDir = false;
		}
		
		if(headSwayDir)
		{
			headObject.transform.position = new Vector3(headObject.transform.position.x + Time.deltaTime*score*.1f,
			                                            headObject.transform.position.y,
			                                            headObject.transform.position.z);
		}
		else
		{
			headObject.transform.position = new Vector3(headObject.transform.position.x - Time.deltaTime*score*.1f,
			                                            headObject.transform.position.y,
                                                        headObject.transform.position.z);
        }
    }

	public void MoveCamera(float _hatHeight)
	{
		iTween.MoveTo(mainCamera, new Vector3(mainCamera.transform.position.x,
		                                          mainCamera.transform.position.y + _hatHeight,
		                                          mainCamera.transform.position.z), 1.0f);

		waveNumber++;
	}

	void PopBalloon(int _num)
	{
		Instantiate(poppedHatObject, placeholderHat[_num].transform.position, placeholderHat[_num].transform.rotation);

		balloonTouched[_num] = false;
		balloonObject[_num].SetActive(false);
		timeBetweenWaves = 0.8f;
		waveSpawned = false;

		balloonObject[_num].GetComponent<balloonScript>().inView = false;
	}

	void OnTriggerEnter(Collider other)
	{
		if(other.tag == "Balloon0")
		{
			balloonTouched[0] = true;
		}
		if(other.tag == "Balloon1")
		{
			balloonTouched[1] = true;
		}
		if(other.tag == "Balloon2")
		{
			balloonTouched[2] = true;
		}

	}
	void OnTriggerExit(Collider other)
	{
		if(other.tag == "Balloon0")
		{
			balloonTouched[0] = false;
		}
		if(other.tag == "Balloon1")
		{
			balloonTouched[1] = false;
		}		
		if(other.tag == "Balloon2")
		{
			balloonTouched[2] = false;
		}
	}

	void OnGUI()
	{
		if(!gameOver)
		{
			GUI.TextField( new Rect(0, 40, 75, 20), "Health: " + health);
		}
		GUI.TextField( new Rect(0, 0, 75, 20), "Hats: " + score);
		GUI.TextField( new Rect(0, 20, 75, 20), "Height: " + (mainCamera.transform.position.y * 0.8f - 0.8f) + "ft.");
	}

}
