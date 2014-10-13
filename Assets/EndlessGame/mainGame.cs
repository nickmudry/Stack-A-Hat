using UnityEngine;
using System.Collections;

/// <summary>
/// Main game.
/// 
/// This script handles the majority of the main endless game
/// </summary>

public class mainGame : MonoBehaviour 
{
	public GameObject mainCamera;
	public GameObject gameOverObject;

	//Placeholder + testing purposes; controls using mouse
	public GameObject mouseCursorObject;

	//Here are balloons and everything associated with them
	public GameObject [] balloonObject; 
	public float [] balloonSpeed;	//horizontal movement
	public float [] balloonLift; 	//vertical movement
	public bool [] balloonTouched; 	//whether or not the balloon is currently being touched by finger/cursor

	//Stores hat attached to each balloon and object that falls after balloon is popped
	public GameObject [] placeholderHat;
	public GameObject poppedHatObject;

	//Wave variables
	public int waveNumber;
	public bool waveSpawned;
	public float timeBetweenWaves;
	
	//Floodsicle
	public GameObject headObject;

    //audio variables
    public GameObject SFX_balPop;

	//Number of hats and misses left
	public int score;
	public int health;

	private bool headSwayDir;	//Switches back and forth as head sways (determines direction)
	private bool gameOver;

	private float gameOverTimer;
	private Vector3 camStartPos;

	//Starting everything off
	void Start () 
	{
		ResetStartValues();
		Debug.Log("Main Game Start");
    }

	void ResetStartValues()
	{
		//Reset all the values
		Time.timeScale = 1.0f;

		gameOver = false;
		gameOverTimer = 0.0f;
		
		score = 0;
		health = 3;
		
		camStartPos = mainCamera.transform.position;
		
		waveSpawned = false;
		waveNumber = 0;
		timeBetweenWaves = 0.8f;   
		
		//Randomly pick direction head starts swaying at
		if(Random.Range (0, 1.0f) > 0.5f)
		{
			headSwayDir = true; 
		}        
		
		//Initialize the arrays of variables
		balloonSpeed = new float[balloonObject.Length];
		balloonLift = new float[balloonObject.Length]; 
		balloonTouched = new bool[balloonObject.Length];
		
		for(int i = 0; i<balloonObject.Length; i++)
		{
			balloonObject[i].SetActive(false);
			balloonSpeed[i] = 0.0f;
			balloonLift[i] = 0.0f;

			balloonSpeed[i] = 0.0f;
			balloonLift[i] = 0.0f;
			balloonTouched[i] = false;
		}
	}
	
	void Update () 
	{	
		//Main method of control
		mouseCursorObject.transform.position = Camera.main.ScreenToWorldPoint(new Vector3(Input.mousePosition.x, Input.mousePosition.y, 10.0f));

		if(!gameOver)
		{
			if(health <= 0)
			{
				gameOver = true;	//You dead
			}

			if(score >= 1)
			{
	            SwayHead();
	        }

			if(!waveSpawned)
			{
				timeBetweenWaves -= Time.deltaTime;	//lower counter while between waves
			}
			if(timeBetweenWaves <= 0.0f)
			{
				SpawnHat();	
			}

			//Check the three balloons
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
		else 
		{
			GameOverUpdate();
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
		waveSpawned = true;

		//Only one balloon for now; will have a way to pick between 1-3 balloons (increasing the chance the higher the score)
		balloonObject[0].SetActive(true);

		//Chance to spawn from the left side...
		if(Random.Range (0.0f, 2.0f) <= 1.0f)
	 	{
			balloonObject[0].transform.position = new Vector3(Random.Range (-7.0f, -5.1f), 
                                     							Random.Range (1.5f, 3.0f) + mainCamera.transform.position.y - 1.0f,
			                                                     0);

			balloonSpeed[0] = Random.Range (0.8f, 1.2f) * score*0.25f + 2.0f;
		}
		//...or right side
		else
		{
			balloonObject[0].transform.position = new Vector3(Random.Range (5.1f, 7.0f), 
		                                                	  Random.Range (1.5f, 3.0f) + mainCamera.transform.position.y - 1.0f,
                                     							0);

			balloonSpeed[0] = Random.Range (0.8f, 1.2f) * -score*0.25f - 2.0f;
		}

		//balloons goes up up
		balloonLift[0] = Random.Range (0, .5f)*score*0.5f;
		//Set time between waves so it doesn't keep spawning 
		timeBetweenWaves = 0.5f;  
	}

	void SwayHead()
	{
		//Cap swaying at 45 to keep from going past edges
		if(score < 45)
		{
			if(headObject.transform.position.x <= -3.2f * (float)score/30.0f)
			{
				headSwayDir = true;
			}
			if(headObject.transform.position.x >= 3.2f * (float)score/30.0f)
			{
				headSwayDir = false;
			}
		}
		else
		{
			if(headObject.transform.position.x <= -3.2f * 1.5f)
			{
				headSwayDir = true;
			}
			if(headObject.transform.position.x >= 3.2f * 1.5f)
			{
				headSwayDir = false;
			}
		}

		//Move left and right; will figure out a way in hat script to sway more organically later possibly
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

	//Function for hatScript to call each time hat is added on
	public void MoveCamera(float _hatHeight)
	{
		iTween.MoveTo(mainCamera, new Vector3(mainCamera.transform.position.x,
		                                          mainCamera.transform.position.y + _hatHeight,
		                                          mainCamera.transform.position.z), 1.0f);

		waveNumber++;
	}

	void PopBalloon(int _num)
	{
		balloonTouched[_num] = false;

		//deactive current balloon + make instance of hat by itself
		balloonObject[_num].SetActive(false);
		Instantiate(poppedHatObject, placeholderHat[_num].transform.position, placeholderHat[_num].transform.rotation);

        SFX_balPop.audio.Play(); // plays the pop noise on the balPop GameObject

		timeBetweenWaves = 0.8f;
		waveSpawned = false;

		balloonObject[_num].GetComponent<balloonScript>().inView = false;	//tell script attached to balloon the balloon is no longer "in view"
	}

	void GameOverUpdate()
	{
		//Move camera slowly to head
		iTween.MoveTo(mainCamera, camStartPos, (float)score/2.0f);

		gameOverTimer+= Time.deltaTime;

		//Fast-forward while holding down
		if(Input.GetMouseButtonDown(0))
		{
			Time.timeScale = 4.2f;
		}
		if(Input.GetMouseButtonUp(0))
	 	{
			Time.timeScale = 1.0f;
		}

		if(gameOverTimer >= score/2)
		{
			gameOverObject.SetActive(true);
		}

		//Only allows click to continue after scrolled all the way down
		if(gameOverTimer >= score/2 && Input.anyKey)
		{
			Application.LoadLevel(Application.loadedLevel);
		}
	}

	void OnTriggerEnter(Collider other)
	{
		//Only reason I'm using tags for this is so that it doesn't take up too much of a toll resource-wise
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

	//Temporary GUI
	void OnGUI()
	{
		if(!gameOver)
		{
			GUI.TextField( new Rect(0, 40, 75, 20), "Health: " + health);
		}
		GUI.TextField( new Rect(0, 0, 75, 20), "Hats: " + score);
		GUI.TextField( new Rect(0, 20, 75, 20), "Height: " + (mainCamera.transform.position.y * 0.8f - 0.8f) + "ft.");	//Will have a way to actually track height instead of just camera height
	}

}
