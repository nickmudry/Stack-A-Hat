using UnityEngine;
using System.Collections;

/// <summary>
/// Outdated prototype script.
/// 
/// Not sure if I delete locally or do something in GitHub. 
/// Will just let it take space until I find out.
/// </summary>

public class endless_prototype : MonoBehaviour 
{
	public GameObject mouseCursorObject;

	public GameObject [] balloonObject; 
	public GameObject [] placeholderHat;
	public GameObject poppedHatObject;

	public GameObject headObject;

	public GameObject gameOverObject;

	public int score;
	public int health;
	public int spawnCounter;

	public bool balloonTouched;
	public bool [] balloonPopped;
	public bool balloonSpawned;
	public bool [] balloonDir;

	public float balloonSpeed;
	public float balloonLift;
	public float timeUntilNextBalloon;

	private Vector3 startHeadPos;
	private bool balloonBool;
	private bool headSwayDir;
	private bool gameOver;
	private float gameOverTimer;

	void Start ()
	{
		gameOver = false;
		gameOverObject.SetActive(false);
		score = 0;
		health = 3;
		balloonPopped = new bool[3];
		balloonDir = new bool[3];

		balloonSpeed = 2.0f;
		balloonLift = 0.5f;

		for(int i = 0; i < balloonObject.Length; i++)
		{
			RespawnBalloon(i);
		}
		startHeadPos = headObject.transform.position;

		if(Random.Range (0, 1.0f) > 0.5f)
		{
			headSwayDir = true;
		}
	}
	
	void Update () 
	{
		if(health <= 0)
		{
			gameOver = true;
		}
		if(score > 1)
		{
			balloonSpeed = score;
			SwayHead();
		}

		headObject.transform.position = new Vector3(headObject.transform.position.x,
		                                            startHeadPos.y- score*0.42f,
		                                            headObject.transform.position.z);
		                                           
		mouseCursorObject.transform.position = Camera.main.ScreenToWorldPoint( new Vector3(Input.mousePosition.x, Input.mousePosition.y, 10.0f));  
	
		if(balloonSpawned)
		{
			if(!balloonBool)
			{
				// Determining the balloon direction based on initial position
				if(balloonObject[spawnCounter].transform.position.x < spawnCounter)
				{
					balloonDir[spawnCounter] = true;
				}
				else
				{
					balloonDir[spawnCounter] = false;
				}
				balloonBool = true;
			}

			if(balloonDir[spawnCounter])
			{
				balloonObject[spawnCounter].transform.position = new Vector3(balloonObject[spawnCounter].transform.position.x + balloonSpeed*Time.deltaTime,
				                                                             balloonObject[spawnCounter].transform.position.y + balloonLift*Time.deltaTime,
				                                                             balloonObject[spawnCounter].transform.position.z);
			}
			else
			{
				balloonObject[spawnCounter].transform.position = new Vector3(balloonObject[spawnCounter].transform.position.x - balloonSpeed*Time.deltaTime,
				                                                             balloonObject[spawnCounter].transform.position.y + balloonLift*Time.deltaTime,
				                                                             balloonObject[spawnCounter].transform.position.z);
			}

			// "Unspawning" the current balloon once it's past the screen
			if(balloonDir[spawnCounter] && balloonObject[spawnCounter].transform.position.x > 4.0f || balloonObject[spawnCounter].transform.position.y > 11.0f)
			{
				health--; 
				timeUntilNextBalloon = Random.Range (0.4f, 1.5f);
				balloonSpawned = false;
			}
			if(!balloonDir[spawnCounter] && balloonObject[spawnCounter].transform.position.x < -4.0f || balloonObject[spawnCounter].transform.position.y > 11.0f)
			{
				health--;
				timeUntilNextBalloon = Random.Range (0.4f, 1.5f);
				balloonSpawned = false;
			}

			if(Input.GetMouseButtonDown(0) && balloonTouched)
			{
				PopBalloon(spawnCounter);
			}
		}
		else
		{
			RespawnBalloon(spawnCounter);
			
			spawnCounter++;
			spawnCounter %= 3;

			if(timeUntilNextBalloon <= 0.0f)
			{
				balloonSpawned = true;
				balloonBool = false;
			}
			else
			{
				timeUntilNextBalloon -= Time.deltaTime;
			}
		}

		if(gameOver)
		{
			GameOverScreen();
		}
	}
		
	void GameOverScreen()
	{
		gameOverObject.SetActive(true);
		gameOverTimer += Time.deltaTime;

		if(Input.anyKey && gameOverTimer >= .2f)
		{
			Application.LoadLevel (Application.loadedLevel);
		}
	}

	void PopBalloon(int _num)
	{
		Instantiate(poppedHatObject, placeholderHat[_num].transform.position, placeholderHat[_num].transform.rotation);
		timeUntilNextBalloon = Random.Range (1.5f, 2.4f);
		balloonSpawned = false;
	}

	void SwayHead()
	{
		if(headObject.transform.position.x <= -3.1f * (float)score/30.0f)
		{
			headSwayDir = true;
		}
		if(headObject.transform.position.x >= 3.1f * (float)score/30.0f)
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


	void OnTriggerStay(Collider other)
	{
		if(other.tag == "Balloon")
		{
			balloonTouched = true;
		}
	}
	void OnTriggerExit(Collider other)
	{
		if(other.tag == "Balloon")
		{
			balloonTouched = false;
		}
	}

	void RespawnBalloon(int _num)
	{
		if(Random.Range(0.0f,1.0f) > 0.5f)
		{
			balloonObject[_num].transform.position = new Vector3(Random.Range (-8.0f, -4.5f), 
			                                                             Random.Range (0.5f, 4.0f),
			                                                             0);
		}
		else
		{
			balloonObject[_num].transform.position = new Vector3(Random.Range (4.5f, 8.0f), 
			                                                             Random.Range (0.5f, 4.0f),
			                                                             0);
		}
		balloonLift = Random.Range (0.5f, 1.0f);
	}

	void OnGUI()
	{
		if(!gameOver)
		{
			GUI.TextField( new Rect(0, 20, 75, 20), "Health: " + health);
		}
		GUI.TextField( new Rect(0, 0, 75, 20), "Score: " + score);
	}
}
