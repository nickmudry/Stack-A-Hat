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

	//Here are balloons and everything associated with them
	public GameObject balloonObject; 
	
	//Wave variables
	public int waveNumber;
	public bool waveSpawned;
	public float timeBetweenWaves;
	public float spawnThreshold;

	public int currentBalloons;
	
	//Floodsicle
	public GameObject headObject;

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
	}
	
	void Update () 
	{	
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
				timeBetweenWaves += Time.deltaTime;	//lower counter while between waves

				if(timeBetweenWaves >= spawnThreshold)
				{
					SpawnHat();	
					timeBetweenWaves = 0.0f;
					waveSpawned = true;
				}
			}
		}
		else 
		{
			GameOverUpdate();
		}

		if(currentBalloons <= 0)
		{
			waveSpawned = false;
		}
    }
	
	void SpawnHat()
	{
		currentBalloons++;
		waveSpawned = true;

		if(Random.Range (0.0f, 1.0f) <= 0.5f)
		{
			Instantiate(balloonObject, 
		    	        new Vector3(Random.Range (-7.0f, -5.1f), Random.Range (1.5f, 3.0f) + mainCamera.transform.position.y - 1.0f, 0),
		        	    balloonObject.transform.rotation);
		}
		else
		{
			Instantiate(balloonObject, 
			            new Vector3(Random.Range (5.1f, 7.0f), Random.Range (1.5f, 3.0f) + mainCamera.transform.position.y - 1.0f, 0),
			            balloonObject.transform.rotation);
		}
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
