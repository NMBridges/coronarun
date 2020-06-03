using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BotControllerScript : MonoBehaviour
{
	Rigidbody nRigid;
    BoxCollider boxCollider;
    Collider[] allColliders;
    Transform pTransform;
    Animator anim;
    float speed;
    float sign;
    List<Vector3> turnTiles;
    List<Vector2> turnTilesLocation;
    List<Vector3> tp;
    List<int> completedTurns;
    int turnTileDir;
    float gWidth;
    float gHeight;
    float gScale;
    float rad;
    Vector2 rcen;
    float turnCounter;
    float startDeg;
    float orient1;
    float orient2;
    float turnState;
    bool destroyTrigger;
    bool botIsMoving;

    // FIX SPEED BEING DIFFERENT IN GAME
    
    void Start()
    {
    	destroyTrigger = false;
    	botIsMoving = true;
    	nRigid = GetComponent<Rigidbody>();
    	boxCollider = GetComponent<BoxCollider>();
    	allColliders = GetComponentsInChildren<Collider>();
    	anim = GetComponentInChildren<Animator>();
    	doRagdoll(false, Vector3.zero);
    	sign = transform.position.y;
    	gScale = GameObject.Find("Environment").GetComponent<BuildingCaller>().gridScale;
        gWidth = GameObject.Find("Environment").GetComponent<BuildingCaller>().gridWidth;
        gHeight = GameObject.Find("Environment").GetComponent<BuildingCaller>().gridHeight;
        turnTilesLocation = new List<Vector2>();
        tp = new List<Vector3>();
        completedTurns = new List<int>();
        turnState = 0;
        turnTiles = GameObject.Find("Environment").GetComponent<BuildingCaller>().turnPoints;
        createTurnTilePoints();
    	speed = 5f;
    	transform.position = new Vector3(transform.position.x, 0f, transform.position.z);
    }

    
    void FixedUpdate()
    {
    	transform.position = new Vector3(transform.position.x, 0f, transform.position.z);
    	if(botIsMoving)
    	{
    		anim.enabled = true;
    		anim.GetBoneTransform(HumanBodyBones.Hips).transform.position = anim.transform.position;
    	}
    	if(turnTiles != tp)
        {
        	ResetTP();
        	createTurnTilePoints();
    	}
    	if(OnTurnTile(new Vector2(transform.position.x, transform.position.z)))
    	{
    		if(!completedTurns.Contains(turnTileDir))
    		{
    			if(turnState == 0)
    			{
    				turnCounter = 0f;
    				turnState = 1;
    				rcen = listItemToCoordinate((int)turnTiles[turnTileDir].x);
    				if(sign == 1f)
    				{
    					orient1 = (turnTiles[turnTileDir].y) * 180f / Mathf.PI;
    					orient2 = (turnTiles[turnTileDir].z) * 180f / Mathf.PI;
    				} else
    				{
    					orient1 = (turnTiles[turnTileDir].z + Mathf.PI) * 180f / Mathf.PI;
    					orient2 = (turnTiles[turnTileDir].y + Mathf.PI) * 180f / Mathf.PI;
    				}
    			}
    			if(turnState == 1)
    			{
	    			if(turnTiles[turnTileDir].y == 0f)
		        	{
		        		float yIn = transform.position.z - turnTilesLocation[turnTileDir].y + 5f;
		        		float xFromLeft = (transform.position.x - turnTilesLocation[turnTileDir].x) * -Mathf.Sign(turnTiles[turnTileDir].z) * 0.95f + 5f;
		        		if(sign * yIn > sign * xFromLeft)
		        		{
		        			turnState = 2;
		        		}
		    		} else
		    		{
		    			if(turnTiles[turnTileDir].y < 0f)
		    			{
		    				float yIn = turnTilesLocation[turnTileDir].x - transform.position.x + 5f;
		        			float xFromLeft = (turnTilesLocation[turnTileDir].y - transform.position.z) + 5f;
		        			if(sign * yIn > sign * xFromLeft)
			        		{
			        			turnState = 2;
			        		}
						} else
						{
							float yIn = transform.position.x - turnTilesLocation[turnTileDir].x + 5f;
							float xFromLeft = (turnTilesLocation[turnTileDir].y - transform.position.z) + 5f;
							if(sign * yIn > sign * xFromLeft)
							{
								turnState = 2;
							}
						}
		    		}
    			}
    			if(turnState == 2)
    			{
    				nRigid.velocity = Vector3.zero;
    				turnCounter += 5f * Time.fixedDeltaTime;
    				if(turnCounter >= 1f)
    				{
    					turnCounter = 1f;
    					turnState = 0;
    					completedTurns.Add(turnTileDir);
    				}
    				transform.rotation = Quaternion.Euler(0f, Mathf.SmoothStep(orient1, orient2, turnCounter), 0f);
    			}
    			movement(0f);
    		} else
    		{
    			movement(1f);
	    	}
		} else
		{
			movement(1f);
		}
    }

    void movement(float factor)
    {
		pTransform = GameObject.Find("PlayerEmpty").GetComponent<Transform>();
    	float fDist = Mathf.Sqrt((transform.position.x - pTransform.position.x) * (transform.position.x - pTransform.position.x) + (transform.position.z - pTransform.position.z) * (transform.position.z - pTransform.position.z));
		if(fDist < 60f)
		{
			if(botIsMoving)
			{
				nRigid.AddForce(transform.forward * speed * 30f * factor);
			}
		}
		if(fDist < 56f)
		{
	        if(nRigid.velocity.magnitude > 0.4f && factor > 0f)
	        {	
	        	nRigid.velocity = nRigid.velocity.normalized * 0.4f;
	        }
	        if(fDist < 20f)
	        {
	        	transform.localScale = new Vector3(1f, 1f, 1f);
				destroyTrigger = true;
        	} else
        	{
				transform.localScale = new Vector3(1f, Mathf.SmoothStep(0f, 1f, (Mathf.InverseLerp(56f, 20f, fDist))), 1f);
        	}
        } else
        {
        	transform.localScale = new Vector3(0f, 0f, 0f);
			if(destroyTrigger)
			{
				Destroy(gameObject);
			}
        }
    }

    void doRagdoll(bool isRagdoll, Vector3 force)
    {
    	foreach(Collider u in allColliders)
    	{
			u.enabled = isRagdoll;
			u.gameObject.GetComponent<Rigidbody>().useGravity = isRagdoll;
			if(u.gameObject != gameObject)
			{
				u.gameObject.GetComponent<Rigidbody>().AddForce(force);
			}
    	}
    	boxCollider.enabled = !isRagdoll;
    	nRigid.useGravity = false;
    	anim.enabled = !isRagdoll;
    }

    Vector2 listItemToCoordinate(int point)
    {
    	float xtemy = (float) point % gWidth;
		float ytemy = Mathf.Floor((float) point / gWidth);
		float xposy = (xtemy - gWidth / 2f) * gScale;
		float yposy = ytemy * gScale + 5f;
		return new Vector2(xposy, yposy);
    }

    void ResetTP()
    {
        tp = new List<Vector3>();
        foreach(Vector3 v in turnTiles)
        {
            tp.Add(v);
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

    bool OnTurnTile(Vector2 pos)
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

    private void OnCollisionEnter(Collision col)
    {
    	if(col.gameObject.tag == "Player")
        {
            nRigid.velocity = Vector3.zero;
            Vector3 velooo = col.gameObject.GetComponent<Rigidbody>().velocity;
            doRagdoll(true, 200f * (Vector3.up + 4f * col.contacts[0].normal + 2f * velooo));
            botIsMoving = false;
        }
    }
}
