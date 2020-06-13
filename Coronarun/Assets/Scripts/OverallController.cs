﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class OverallController : MonoBehaviour
{
    
    public Vector3 playerVelo;
    public Transform skid;
    float playerDir;
    float speed;
    float x;
    List<Vector3> turnTiles;
    List<Vector2> turnTilesLocation;
    float gScale;
    float gWidth;
    float gHeight;
    int turnTileDir;
    public int turning;
    float turnCount;
    Rigidbody pRigid;
    BoxCollider boxCollider;
    Collider[] allColliders;
    Animator anim;
    List<Vector3> tp;
    public bool playerIsMoving;
    bool skidding;

    void Awake()
    {
        playerIsMoving = true;
    	pRigid = GetComponent<Rigidbody>();
    	boxCollider = GetComponent<BoxCollider>();
    	allColliders = GetComponentsInChildren<Collider>();
    	anim = GetComponentInChildren<Animator>();
    	doRagdoll(false, Vector3.zero);
        turnTiles = GameObject.Find("Environment").GetComponent<BuildingCaller>().turnPoints;
        ResetTP();
    }

    void Start()
    {
        speed = 10f;
        playerDir = 0f;
        skidding = false;
        playerVelo = new Vector3(speed * Mathf.Sin(playerDir), 0f, speed * Mathf.Cos(playerDir));
        turnTilesLocation = new List<Vector2>();
        turnTiles = GameObject.Find("Environment").GetComponent<BuildingCaller>().turnPoints;
        ResetTP();
        gScale = GameObject.Find("Environment").GetComponent<BuildingCaller>().gridScale;
        gWidth = GameObject.Find("Environment").GetComponent<BuildingCaller>().gridWidth;
        gHeight = GameObject.Find("Environment").GetComponent<BuildingCaller>().gridHeight;
        createTurnTilePoints();
        turning = 0;
        turnTileDir = 0;
    }

    // Update is called once per frame
    void Update()
    {
        turnScript();
    }

    void FixedUpdate()
    {
        input();
        movement();
    }

    void input()
    {
    	x = Input.GetAxis("Horizontal") * 5f;
        if(Input.GetButtonDown("Jump"))
        {
        	playerIsMoving = false;
            pRigid.velocity = Vector3.zero;
            GameObject.Find("Slider").GetComponent<Slider>().value = Mathf.Floor(Time.time * 50f);
            GameObject.Find("Slider").GetComponent<Slider>().maxValue = Mathf.Floor(Time.time * 50f);
        }
    }

    void movement()
    {
        if(playerIsMoving)
        {
            playerVelo = new Vector3(x, 0f, 0f);
            GameObject.Find("CoronaRunPlayer").GetComponent<Transform>().localRotation = Quaternion.Euler(0f, x * 5f, 0f);
            pRigid.AddForce((transform.forward * speed + transform.rotation * playerVelo) * 30f);
            pRigid.velocity = new Vector3(pRigid.velocity.x * 0.7f, pRigid.velocity.y, pRigid.velocity.z * 0.7f);
        } else
        {
            pRigid.velocity = new Vector3(pRigid.velocity.x * 0.9f, pRigid.velocity.y, pRigid.velocity.z * 0.9f);
            if(transform.position.y < 0f)
            {
                transform.position = new Vector3(transform.position.x, 0f, transform.position.z);
                pRigid.velocity = Vector3.zero;
            }
            GameObject.Find("Slider").GetComponent<Slider>().maxValue = Mathf.Floor(Time.time * 50f);
            GameObject.Find("Slider").GetComponent<Slider>().value -= 1;
        }
        pRigid.AddForce(Vector3.down * 20f);
    }

    void turnScript()
    {
        if(turnTiles != tp)
        {
            createTurnTilePoints();
            ResetTP();
        }
        if(ifOnTurnTile(new Vector2(transform.position.x, transform.position.z)))
        {
        	if(turnTiles[turnTileDir].y == 0f)
        	{
        		float yIn = transform.position.z - turnTilesLocation[turnTileDir].y + 5f;
        		float xFromLeft = (transform.position.x - turnTilesLocation[turnTileDir].x) * 0.9f * turnTiles[turnTileDir].z / Mathf.PI * -2f + 5f;
        		if(yIn > xFromLeft)
        		{
        			if(!skidding)
                    {
                        GenSkid();
                    }
                    turnPlayer(5f);
        		}
    		} else
    		{
    			if(turnTiles[turnTileDir].y == -Mathf.PI / 2f)
    			{
    				float yIn = turnTilesLocation[turnTileDir].x - transform.position.x + 5f;
        			float xFromLeft = (turnTilesLocation[turnTileDir].y - transform.position.z) * 0.9f + 5f;
        			if(yIn > xFromLeft)
	        		{
                        if(!skidding)
                        {
                            GenSkid();
                        }
	        			turnPlayer(5f + Mathf.Abs((turnTilesLocation[turnTileDir].y - transform.position.z) * 1.6f));
	        		}
				} else
				{
					float yIn = transform.position.x - turnTilesLocation[turnTileDir].x + 5f;
					float xFromLeft = (turnTilesLocation[turnTileDir].y - transform.position.z) * 0.9f + 5f;
					if(yIn > xFromLeft)
					{
                        if(!skidding)
                        {
                            GenSkid();
                        }
						turnPlayer(5f + Mathf.Abs((transform.position.z - turnTilesLocation[turnTileDir].y) * 1.6f));
					}

				}
    		}
        } else
        {
        	if(turnCount != 0f)
        	{
        		transform.rotation = Quaternion.Euler(0f, Mathf.SmoothStep(turnTiles[turnTileDir].y * 180f / Mathf.PI, turnTiles[turnTileDir].z * 180f / Mathf.PI, 1f), 0f);
        	}
        	turning = 0;
        	turnCount = 0f;
            skidding = false;
        }
        GetComponentInChildren<CameraFollow>().pDir = transform.localRotation.eulerAngles.y;
    }

    void doRagdoll(bool isRagdoll, Vector3 force)
    {
    	foreach(Collider c in allColliders)
    	{
			c.enabled = isRagdoll;
			c.gameObject.GetComponent<Rigidbody>().useGravity = isRagdoll;
			if(c.gameObject != gameObject)
			{
				c.gameObject.GetComponent<Rigidbody>().AddForce(force);
			}
    	}
    	boxCollider.enabled = !isRagdoll;
    	pRigid.useGravity = !isRagdoll;
    	anim.enabled = !isRagdoll;
    }

    void ResetTP()
    {
        tp = new List<Vector3>();
        foreach(Vector3 v in turnTiles)
        {
            tp.Add(v);
        }
    }

    void turnPlayer(float speed)
    {
		transform.rotation = Quaternion.Euler(0f, Mathf.SmoothStep(turnTiles[turnTileDir].y * 180f / Mathf.PI, turnTiles[turnTileDir].z * 180f / Mathf.PI, turnCount), 0f);
		if(turning == 1)
		{
			turnCount += speed * Time.deltaTime;
			if (turnCount >= 1f)
			{
				turnCount = 1f;
				transform.rotation = Quaternion.Euler(0f, Mathf.SmoothStep(turnTiles[turnTileDir].y * 180f / Mathf.PI, turnTiles[turnTileDir].z * 180f / Mathf.PI, turnCount), 0f);
				turning = 2;
                skidding = true;
			}
		} else if (turning == 0)
		{
			turning = 1;
			turnCount = 0f;
		}
    }

    private void OnCollisionEnter(Collision col)
    {
    	if(col.gameObject.tag == "carsbruh")
        {
            pRigid.velocity = Vector3.zero;
            Vector3 velooo = col.gameObject.GetComponent<Rigidbody>().velocity;
            doRagdoll(true, 200f * (Vector3.up + 4f * col.contacts[0].normal + 2f * velooo));
            playerIsMoving = false;
        }
        if(col.gameObject.tag == "botbruh")
        {
            //pRigid.velocity = Vector3.zero;
            Vector3 velooo = col.gameObject.GetComponent<Rigidbody>().velocity;
            doRagdoll(true, 200f * (Vector3.up + 2f * (pRigid.velocity + velooo) + 4f * col.contacts[0].normal));
            playerIsMoving = false;
        }
    }

    private void OnCollisionExit(Collision colll)
    {
    	if(colll.gameObject.tag != "ground")
    	{
    		
		}
    }
    
    void createTurnTilePoints()
    {
        turnTilesLocation = new List<Vector2>();
    	for(int i = 0; i < turnTiles.Count; i++)
    	{
    		float xtem = turnTiles[i].x % gWidth;
			float ytem = Mathf.Floor(turnTiles[i].x / gWidth);
			float xpos = (xtem - gWidth / 2f) * gScale;
			float ypos = ytem * gScale + 5f;
			turnTilesLocation.Add(new Vector2(xpos, ypos));
    	}
    }

    bool ifOnTurnTile(Vector2 pos)
    {
    	for(int p = 0; p < turnTilesLocation.Count; p++)
    	{
    		if(turnTilesLocation[p].x - 5f <= pos.x && turnTilesLocation[p].x + 5f >= pos.x)
    		{
    			if(turnTilesLocation[p].y - 5f <= pos.y && turnTilesLocation[p].y + 5f >= pos.y)
	    		{
	    			turnTileDir = p;
	    			return true;
	    		}
    		}
    	}
    	return false;
    }

    void GenSkid()
    {
        Transform temp = Instantiate(skid, transform.position, transform.rotation);
        Destroy(temp.gameObject, 2);
    }

    void OnGUI()
    {

        GUI.Label(new Rect(100, 100, 100, 100), " " + 1f / Time.deltaTime);
    }
}
