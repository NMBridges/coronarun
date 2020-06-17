using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class OverallController : MonoBehaviour
{
    
    public Vector3 playerVelo;
    public Transform skid;
    public Transform blood;
    TimeRemap timeManager;
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
    public int coronaLevel;
    Material bodyMat;
    Color healthyColor;
    Color coronaColor;

    void Awake()
    {
    	pRigid = GetComponent<Rigidbody>();
    	boxCollider = GetComponent<BoxCollider>();
    	allColliders = GetComponentsInChildren<Collider>();
    	anim = GetComponentInChildren<Animator>();
    	doRagdoll(false, Vector3.zero);
    }

    void Start()
    {
        speed = 10f;
        playerDir = 0f;
        playerVelo = new Vector3(speed * Mathf.Sin(playerDir), 0f, speed * Mathf.Cos(playerDir));
        playerIsMoving = true;
        skidding = false;
        coronaLevel = 0;
        bodyMat = gameObject.GetComponent<Transform>().GetChild(0).GetChild(0).GetComponent<Renderer>().material;
        healthyColor = new Color(1f, 0f, 0f, 1f);
        coronaColor = new Color(0f, 0.4f, 0f, 1f);
        turnTilesLocation = new List<Vector2>();
        turnTiles = GameObject.Find("Environment").GetComponent<BuildingCaller>().turnPoints;
        ResetTP();
        gScale = GameObject.Find("Environment").GetComponent<BuildingCaller>().gridScale;
        gWidth = GameObject.Find("Environment").GetComponent<BuildingCaller>().gridWidth;
        gHeight = GameObject.Find("Environment").GetComponent<BuildingCaller>().gridHeight;
        createTurnTilePoints();
        turning = 0;
        turnTileDir = 0;
        timeManager = GameObject.Find("EventSystem").GetComponent<TimeRemap>();
        FindObjectOfType<AudioManager>().Play("Song");
    }

    // Update is called once per frame
    void Update()
    {
        turnScript();
        updateColor();
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

    void updateColor()
    {
        float percentage = ((float) coronaLevel) / 15f;
        percentage = Mathf.Clamp(percentage, 0f, 1f);
        Color tempColor = Color.Lerp(healthyColor, coronaColor, percentage);
        bodyMat.SetColor("AlbedoInstance", tempColor);
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
            if(playerIsMoving)
            {
                GenBlood(col.contacts[0].point, col.contacts[0].normal);
            }
            pRigid.velocity = Vector3.zero;
            Vector3 velooo = col.gameObject.GetComponent<Rigidbody>().velocity;
            doRagdoll(true, 200f * (Vector3.up + 4f * col.contacts[0].normal + 2f * velooo));
            playerIsMoving = false;
            timeManager.SlowMotion();
            FindObjectOfType<AudioManager>().Play("CarThud");
            StartCoroutine(goBackToMainMenu());
        }
        if(col.gameObject.tag == "botbruh")
        {
            //pRigid.velocity = Vector3.zero;
            Vector3 velooo = col.gameObject.GetComponent<Rigidbody>().velocity;
            doRagdoll(true, 200f * (Vector3.up + 2f * (pRigid.velocity + velooo) + 4f * col.contacts[0].normal));
            playerIsMoving = false;
            timeManager.SlowMotion();
            StartCoroutine(goBackToMainMenu());
        }
    }

    private void OnCollisionExit(Collision colll)
    {
    	if(colll.gameObject.tag != "ground")
    	{
    		
		}
    }

    private void OnParticleCollision(GameObject col)
    {
        if(col.tag == "coughbruh")
        {
            coronaLevel += 1;
        }
    }

    IEnumerator goBackToMainMenu()
    {
        yield return new WaitForSeconds(2);
        GameObject.Find("EventSystem").GetComponent<TransitionManager>().gameToMain();
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

    void GenBlood(Vector3 pos, Vector3 rot)
    {
        Transform parent = transform.GetChild(0).GetChild(1).GetChild(0).GetComponent<Transform>();
        Transform temp = Instantiate(blood, pos, Quaternion.LookRotation(rot, Vector3.up), parent);
    }

    void OnGUI()
    {

        GUI.Label(new Rect(100, 100, 100, 100), " " + 1f / Time.deltaTime);
        GUI.Label(new Rect(100, 150, 100, 100), " " + coronaLevel);
    }
}
