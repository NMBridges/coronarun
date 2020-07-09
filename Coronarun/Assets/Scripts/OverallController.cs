using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class OverallController : MonoBehaviour
{
    
    public Vector3 playerVelo;
    public Transform skid;
    public Transform blood;
    public Transform deathScoreParticles;
    public Transform covidEffect;
    public Transform starsEffect;
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
    bool resetJump;
    int score;
    float startTime;
    float cola;
    float colaGoal;
    float lastColaGoal;
    Color colorGreen;
    Color colorRed;
    Color colorWhite;
    Color colorGoal;
    Color colorTransparent;
    string[] coronaStatements;
    string[] carStatements;
    string[] fireHydrantStatements;
    string[] botStatements;
    string[] trashCanStatements;
    float currPer;
    float breathTime;
    int breathState;
    float lastBreathTime;
    float timeAtBreath;


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
        resetJump = true;
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
        score = 0;
        startTime = Time.time;
        cola = 0f;
        colaGoal = 0f;
        lastColaGoal = 0f;
        colorGreen = new Color(0.164f, 0.274f, 0.156f, 1f);
        colorRed = new Color(0.4f, 0.05f, 0.05f, 0.75f);
        colorWhite = new Color(1f, 1f, 1f, 0.6f);
        colorGoal = colorGreen;
        colorTransparent = new Color(0.05f, 0.2f, 0.05f, 0.1f);
        GameObject.Find("CoronaOverlay").GetComponent<RawImage>().material.SetColor("_Color", Color.Lerp(colorTransparent, colorGoal, cola));
        GameObject deathText = transform.GetChild(2).GetChild(0).GetChild(7).gameObject;
        deathText.SetActive(false);
        statements();
        currPer = 0f;
        breathTime = 2f;
        breathState = 0;
    }

    void Update()
    {
        input();
        turnScript();
        updateColor();
    }

    void FixedUpdate()
    {
        movement();
        if(playerIsMoving)
        {
            score = (int)Mathf.Floor((Time.time - startTime) * 30f);
            GameObject.Find("ScoreText").GetComponent<Text>().text = " " + score;
            colaGoal = Mathf.Clamp(colaGoal - 0.01f, 0f, 1f);
            if(pRigid.velocity.magnitude < 1f)
            {
                transform.Translate((float)(Random.Range(0, 2) * 2 - 1) * new Vector3(0.2f, 0.2f, 0f));
            }
        }
        cola += (colaGoal - cola) * 0.08f;
        GameObject.Find("CoronaOverlay").GetComponent<RawImage>().material.SetColor("_Color", Color.Lerp(colorTransparent, colorGoal, cola));
    }

    void input()
    {
    	x = Input.GetAxis("Horizontal") * 5f;
        if(Input.GetButtonDown("Jump") && playerIsMoving && resetJump)
        {
            pRigid.AddForce(Vector3.up * 400f);
            resetJump = false;
        }
        if(Input.GetKey("left shift"))
        {
            if(playerIsMoving)
            {
                if(breathState == 0)
                {
                    lastBreathTime = breathTime;
                    timeAtBreath = Time.time;
                    breathState = 1;
                    lastColaGoal = colaGoal;
                    colorGoal = colorWhite;
                }
                if(breathState == 1)
                {
                    breathTime = lastBreathTime - (Time.time - timeAtBreath);
                    colaGoal = Mathf.Clamp(colaGoal + 6f * Time.deltaTime, 0f, 1f);
                    if(breathTime <= 0)
                    {
                        breathTime = 0;
                        breathState = 2;
                        
                    }
                }
            }
        }
        if(breathState == 1)
        {
            if(!Input.GetKey("left shift"))
            {
                breathState = 0;
            }
        }
        GameObject.Find("BreathTime").GetComponent<RawImage>().material.SetFloat("_Percentage", breathTime / 2f);
    }

    void movement()
    {
        if(coronaLevel >= 8 && playerIsMoving)
        {
            GenCovid(transform.GetChild(0).GetChild(1).GetChild(0).GetChild(2).GetChild(0).GetChild(0).GetChild(2).GetChild(0).position, transform.GetChild(0).GetChild(1).GetChild(0).GetChild(2).GetChild(0).GetChild(0).GetChild(2).GetChild(0).forward);
            doRagdoll(true, 50200f * pRigid.velocity);
            GameObject deathText = transform.GetChild(2).GetChild(0).GetChild(7).gameObject;
            deathText.SetActive(true);
            deathText.GetComponent<TMPro.TextMeshProUGUI>().text = coronaStatements[Random.Range(0, coronaStatements.Length)];
            colaGoal = 1f;
            pRigid.velocity = Vector3.zero;
            playerIsMoving = false;
            StartCoroutine(goBackToMainMenu());
            timeManager.SlowMotion();
            StartCoroutine(GenScoreFX());
        }
        if(playerIsMoving)
        {
            playerVelo = new Vector3(x, 0f, 0f);
            GameObject.Find("CoronaRunPlayer").GetComponent<Transform>().localRotation = Quaternion.Euler(0f, x * 5f, 0f);
            pRigid.AddForce((transform.forward * speed + transform.rotation * playerVelo) * 30f);
            pRigid.velocity = new Vector3(pRigid.velocity.x * 0.7f, pRigid.velocity.y, pRigid.velocity.z * 0.7f);
        } else
        {
            if(transform.position.y < 0f)
            {
                transform.position = new Vector3(transform.position.x, 0f, transform.position.z);
                pRigid.velocity = Vector3.zero;
            }
        }
        pRigid.AddForce(Vector3.down * 20f / Time.timeScale);
    }

    void statements()
    {
        coronaStatements = new string[]
        {
            "you caught  'rona... better luck next time",
            "maintain social distancing!",
            "that's what you get for not wearing a mask",
            "death by coronavirus",
            "6 feet astray keeps the reaper away",
            "coughin to the coffin",
            "wasted on  'rona",
            "achoo"
        };
        carStatements = new string[]
        {
            "cars can hurt",
            "big car little man",
            "physics - albert einstein",
            "bloody hell!",
            "hahahaahah",
            "splat",
            "watch where you're going bro",
            "not cool man you dented their car",
            "got life insurance?",
            "ouch!",
            "you lost the battle of conservation of momentum",
            "face meet car"
        };
        botStatements = new string[]
        {
            "he who runs into others is bad - gandhi",
            "oops",
            "shoulda seen the other guy!",
            "this isn't WWE!",
            "chill dude he's just walking down the street",
            "get big bro",
            "smacked"
        };
        trashCanStatements = new string[]
        {
            "you're the real trash here... lol",
            "if garbage can you can too...",
            "garbage!",
            "thrashed by trash",
            "bin there",
            "crash",
            "filthy",
            "try jumping over it"
        };
        fireHydrantStatements = new string[]
        {
            "fire hydrants aren't meant to move...",
            "now that's a stubbed toe",
            "yowzers",
            "try jumping over it",
            "get  'em next time",
            "at least  'rona didn't get ya",
            "that's gotta hurt!"
        };
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
        float percentage = ((float) coronaLevel) / 8f;
        percentage = Mathf.Clamp(percentage, 0f, 1f);
        currPer += (percentage - currPer) * Time.deltaTime;
        Color tempColor = Color.Lerp(healthyColor, coronaColor, currPer);
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
        resetJump = true;
    	if(col.gameObject.tag == "carsbruh" && playerIsMoving)
        {
            GameObject deathText = transform.GetChild(2).GetChild(0).GetChild(7).gameObject;
            deathText.SetActive(true);
            deathText.GetComponent<TMPro.TextMeshProUGUI>().text = carStatements[Random.Range(0, carStatements.Length)];
            if(GameObject.Find("LoadingScreenColorObject") != null)
            {
                if(GameObject.Find("LoadingScreenColorObject").GetComponent<LoadingScreenColor>().bloodval == 0)
                {
                    GenBlood(col.contacts[0].point, col.contacts[0].normal);
                } else if(GameObject.Find("LoadingScreenColorObject").GetComponent<LoadingScreenColor>().bloodval == 1)
                {
                    GenCovid(transform.GetChild(0).GetChild(1).GetChild(0).GetChild(2).GetChild(0).GetChild(0).GetChild(2).GetChild(0).position, transform.GetChild(0).GetChild(1).GetChild(0).GetChild(2).GetChild(0).GetChild(0).GetChild(2).GetChild(0).forward);
                } else if(GameObject.Find("LoadingScreenColorObject").GetComponent<LoadingScreenColor>().bloodval == 2)
                {
                    GenStars(transform.GetChild(0).GetChild(1).GetChild(0).GetChild(2).GetChild(0).GetChild(0).GetChild(2).GetChild(0).position, transform.GetChild(0).GetChild(1).GetChild(0).GetChild(2).GetChild(0).GetChild(0).GetChild(2).GetChild(0).forward);
                }
            } else
            {
                GenBlood(col.contacts[0].point, col.contacts[0].normal);
            }
            colaGoal = 1f;
            colorGoal = colorRed;
            pRigid.velocity = Vector3.zero;
            Vector3 velooo = col.gameObject.GetComponent<Rigidbody>().velocity;
            doRagdoll(true, 200f * (Vector3.up + 4f * col.contacts[0].normal + 2f * velooo));
            playerIsMoving = false;
            FindObjectOfType<AudioManager>().Play("CarThud");
            StartCoroutine(goBackToMainMenu());
            timeManager.SlowMotion();
            StartCoroutine(GenScoreFX());
        }
        if(col.gameObject.tag == "botbruh" && playerIsMoving)
        {
            GameObject deathText = transform.GetChild(2).GetChild(0).GetChild(7).gameObject;
            deathText.SetActive(true);
            deathText.GetComponent<TMPro.TextMeshProUGUI>().text = botStatements[Random.Range(0, botStatements.Length)];
            colaGoal = 1f;
            colorGoal = colorRed;
            pRigid.velocity = Vector3.zero;
            Vector3 velooo = col.gameObject.GetComponent<Rigidbody>().velocity;
            doRagdoll(true, 200f * (Vector3.up + 2f * (pRigid.velocity + velooo) + 4f * col.contacts[0].normal));
            playerIsMoving = false;
            FindObjectOfType<AudioManager>().Play("BotThud");
            StartCoroutine(goBackToMainMenu());
            timeManager.SlowMotion();
            StartCoroutine(GenScoreFX());
        }
        if(col.gameObject.tag == "firehydrantbruh" && playerIsMoving)
        {
            GameObject deathText = transform.GetChild(2).GetChild(0).GetChild(7).gameObject;
            deathText.SetActive(true);
            deathText.GetComponent<TMPro.TextMeshProUGUI>().text = fireHydrantStatements[Random.Range(0, fireHydrantStatements.Length)];
            colaGoal = 1f;
            colorGoal = colorRed;
            pRigid.velocity = Vector3.zero;
            doRagdoll(true, 200f * (Vector3.up + 2f * pRigid.velocity + 4f * col.contacts[0].normal));
            playerIsMoving = false;
            FindObjectOfType<AudioManager>().Play("BotThud");
            StartCoroutine(goBackToMainMenu());
            timeManager.SlowMotion();
            StartCoroutine(GenScoreFX());
        }
        if(col.gameObject.tag == "garbagecanbruh" && playerIsMoving)
        {
            GameObject deathText = transform.GetChild(2).GetChild(0).GetChild(7).gameObject;
            deathText.SetActive(true);
            deathText.GetComponent<TMPro.TextMeshProUGUI>().text = trashCanStatements[Random.Range(0, trashCanStatements.Length)];
            colaGoal = 1f;
            colorGoal = colorRed;
            pRigid.velocity = Vector3.zero;
            doRagdoll(true, 200f * (Vector3.up + 2f * pRigid.velocity + 4f * col.contacts[0].normal));
            playerIsMoving = false;
            FindObjectOfType<AudioManager>().Play("BotThud");
            StartCoroutine(goBackToMainMenu());
            timeManager.SlowMotion();
            StartCoroutine(GenScoreFX());
        }
    }

    /*private void OnCollisionExit(Collision colll)
    {
    	if(colll.gameObject.tag != "ground")
    	{
    		
		}
    }*/

    private void OnParticleCollision(GameObject col)
    {
        if(col.tag == "coughbruh" && breathState != 1)
        {
            coronaLevel += 1;
            colorGoal = colorGreen;
            colaGoal = Mathf.Clamp(colaGoal + 0.6f, 0f, 1f);
        }
    }

    IEnumerator goBackToMainMenu()
    {
        yield return new WaitForSeconds(2);
        GameObject.Find("EventSystem").GetComponent<TransitionManager>().lastScore = score;
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

    void GenCovid(Vector3 pos, Vector3 rot)
    {
        Transform parent = transform.GetChild(0).GetChild(1).GetChild(0).GetComponent<Transform>();
        Transform temp = Instantiate(covidEffect, pos, Quaternion.LookRotation(rot, Vector3.up), parent);
    }

    void GenStars(Vector3 pos, Vector3 rot)
    {
        Transform parent = transform.GetChild(0).GetChild(1).GetChild(0).GetComponent<Transform>();
        Transform temp = Instantiate(starsEffect, pos, Quaternion.LookRotation(rot, Vector3.up), parent);
    }

    private IEnumerator GenScoreFX()
    {
        Transform temp = Instantiate(deathScoreParticles, GameObject.Find("PreProcessedCanvas").GetComponent<Transform>());
        yield return new WaitForSeconds(3f);
        Destroy(temp.gameObject);
    }

    void OnGUI()
    {
        GUI.Label(new Rect(2, 2, 100, 100), " " + Mathf.Ceil(1f / Time.deltaTime));
    }
}
