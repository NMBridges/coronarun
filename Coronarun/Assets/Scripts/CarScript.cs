using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CarScript : MonoBehaviour
{
    Transform pTransform;
    List<Vector3> turnTiles;
    List<Vector2> turnTilesLocation;
    List<Vector3> tp;
    List<int> completedTurns;
    List<Color> colorOptions;
    Rigidbody cRigid;
    int turnTileDir;
    float gWidth;
    float gHeight;
    float gScale;
    int turnState;
    float rad;
    Vector2 rcen;
    float turnCounter;
    float sign;
    float startDeg;
    float orient1;
    float orient2;
    bool destroyTrigger;

    void Start()
    {
    	destroyTrigger = false;
    	sign = transform.position.y;
    	gScale = GameObject.Find("Environment").GetComponent<BuildingCaller>().gridScale;
        gWidth = GameObject.Find("Environment").GetComponent<BuildingCaller>().gridWidth;
        gHeight = GameObject.Find("Environment").GetComponent<BuildingCaller>().gridHeight;
        transform.position = new Vector3(transform.position.x, 0.01f, transform.position.z);
        cRigid = GetComponent<Rigidbody>();
        pTransform = GameObject.Find("PlayerEmpty").GetComponent<Transform>();
        turnTilesLocation = new List<Vector2>();
        tp = new List<Vector3>();
        completedTurns = new List<int>();
        turnState = 0;
        turnTiles = GameObject.Find("Environment").GetComponent<BuildingCaller>().turnPoints;
        createTurnTilePoints();
        createColors();
        GameObject prefabMain = transform.GetChild(0).gameObject;
        int numOfChild = prefabMain.transform.childCount;
        for(int ch = 0; ch < numOfChild; ch++)
        {
        	GameObject carBody = prefabMain.transform.GetChild(ch).gameObject;
        	if(carBody.name == "car")
        	{
        		int pp = Random.Range(0, colorOptions.Count);
        		carBody.GetComponent<Renderer>().material.color = new Color(colorOptions[pp].r, colorOptions[pp].g, colorOptions[pp].b, 1f);
        	}
    	}
        
    }

    void Update()
    {
        pTransform = GameObject.Find("PlayerEmpty").GetComponent<Transform>();
        float fDist = Mathf.Sqrt((transform.position.x - pTransform.position.x) * (transform.position.x - pTransform.position.x) + (transform.position.z - pTransform.position.z) * (transform.position.z - pTransform.position.z));
        if(fDist < 56f)
        {
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

    
    void FixedUpdate()
    {
    	transform.position = new Vector3(transform.position.x, 0f, transform.position.z);
        if(turnTiles != tp)
        {
        	ResetTP();
        	createTurnTilePoints();
    	}
    	if(OnTurnTile(new Vector2(transform.position.x, transform.position.z)))
    	{
    		if(!completedTurns.Contains(turnTileDir))
    		{
    			cRigid.velocity = Vector3.zero;
	    		if(turnState == 0)
	    		{
	    			Vector2 TTC = new Vector2(turnTilesLocation[turnTileDir].x, turnTilesLocation[turnTileDir].y);
	    			startDeg = 0f;
	    			orient1 = turnTiles[turnTileDir].y * 180f / Mathf.PI;
	    			orient2 = turnTiles[turnTileDir].z * 180f / Mathf.PI;
	    			if(sign == -1f)
	    			{
	    				startDeg = Mathf.PI / 2f;
	    				orient1 = (turnTiles[turnTileDir].z + Mathf.PI) * 180f / Mathf.PI;
	    				orient2 = (turnTiles[turnTileDir].y + Mathf.PI) * 180f / Mathf.PI;
	    			}
	    			if(turnTiles[turnTileDir].z < 0f)
	    			{
	    				rad = 5f + sign * 1.75f;
	    				rcen = new Vector2(TTC.x - 5f, TTC.y - 5f);
	    			} else if(turnTiles[turnTileDir].z > 0f)
	    			{
	    				rad = 5f - sign * 1.75f;
	    				rcen = new Vector2(TTC.x + 5f, TTC.y - 5f);
	    			} else
	    			{
	    				rad = 5f + Mathf.Sign(turnTiles[turnTileDir].y) * sign * 1.75f;
	    				rcen = new Vector2(TTC.x - Mathf.Sign(turnTiles[turnTileDir].y) * 5f, TTC.y + 5f);
	    			}
	    			turnCounter = 0f;
	    			turnState = 1;
	    		}
	    		if(turnState == 1)
	    		{
	    			turnCounter += sign * Mathf.PI / 2f * Time.fixedDeltaTime;
	    			if(turnCounter >= Mathf.PI / 2f || turnCounter <= - Mathf.PI / 2f)
	    			{
	    				turnCounter = Mathf.Clamp(turnCounter, - Mathf.PI / 2f, Mathf.PI / 2f);
	    				turnState = 2;
	    				completedTurns.Add(turnTileDir);
	    			}
	    			if(turnTiles[turnTileDir].z == 0f)
	    			{
	    				transform.position = new Vector3(rcen.x + rad * Mathf.Sign(turnTiles[turnTileDir].y) * Mathf.Sin(startDeg + turnCounter), 0f, rcen.y - rad * Mathf.Cos(startDeg + turnCounter));
	    				transform.rotation = Quaternion.Euler(0f, Mathf.SmoothStep(orient1, orient2, Mathf.Abs(turnCounter) / (Mathf.PI / 2f)), 0f);
    				} else
    				{
	    				transform.position = new Vector3(rcen.x - rad * Mathf.Sign(turnTiles[turnTileDir].z) * Mathf.Cos(startDeg + turnCounter), 0f, rcen.y + rad * Mathf.Sin(startDeg + turnCounter));
	    				transform.rotation = Quaternion.Euler(0f, Mathf.SmoothStep(orient1, orient2, Mathf.Abs(turnCounter) / (Mathf.PI / 2f)), 0f);
	    			}
	    		}
	    		if(turnState == 2)
	    		{
	    			transform.position = transform.position + transform.rotation * new Vector3(0f, 0f, 1f) * 10f * Time.deltaTime;
	    			turnState = 0;
	    			turnCounter = 0f;
	    		}
    		}
    	} else
    	{
    		cRigid.AddForce(transform.forward * 3000f);
    		if(cRigid.velocity.magnitude > 10f)
    		{
    			cRigid.velocity = cRigid.velocity.normalized * 10f;
    		}
    	}
    }

    void createColors()
    {
    	colorOptions = new List<Color>();
    	colorOptions.Add(new Color(0.5f, 0.2f, 1f, 1f));
    	colorOptions.Add(new Color(0.5f, 0.2f, 0f, 1f));
    	colorOptions.Add(new Color(0.1f, 0.75f, 0.95f, 1f));
    	colorOptions.Add(new Color(0.16f, 0.014f, 1f, 1f));
    	colorOptions.Add(new Color(0.014f, 1f, 0.31f, 1f));
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
}
