using UnityEngine;
using System.Collections;

public class mainGame : MonoBehaviour 
{
	public GameObject mainCamera;

	//Here are balloons and everything associated with them
	public GameObject [] balloonObject; 
	public float [] balloonSpeed;
	public float [] balloonLift;

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

	private Vector3 camStartPos;
    
	// Use this for initialization
	void Start () 
	{
		gameOver = false;

		score = 0;
		health = 3;

		waveNumber = 0;
		timeBetweenWaves = 0.8f;   

		camStartPos = mainCamera.transform.position;

		if(Random.Range (0, 1.0f) > 0.5f)
		{
			headSwayDir = true;
        }        
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

    }

	void SpawnHat()
	{

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
		iTween.MoveUpdate(mainCamera, new Vector3(mainCamera.transform.position.x,
		                                          mainCamera.transform.position.y + _hatHeight,
		                                          mainCamera.transform.position.z), 0.6f);
	}
}
