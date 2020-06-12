using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BuildingCaller : MonoBehaviour
{
    public Transform building;
    public Transform roadblock;
    public Transform sidewalk;
    public Transform sidewalkCorner;
    public Transform crosswalk;
    public Transform laneDivider;
    public Transform carGen;
    public Transform bot;
    Vector2[] gridPoints;
    int[] gridValue;
    List<int> clearedGrid;
    List<int> alreadyBuilt;
    int[] checkTemp;
    public List<Vector3> turnPoints;
    public float gridWidth;
    public float gridHeight;
    public float gridScale;
    Vector2 lastPoint;
    int genNew;
    bool genNewTrig;
    int GNT;
    int offlim1;
    int offlim2;
    float sign;
    float carx;
    float cary;

    void Awake()
    {
    	gridWidth = 5000f;
        gridHeight = 30000f;
        gridScale = 10f;
        alreadyBuilt = new List<int>();
        checkTemp = new int[24];
        clearedGrid = new List<int>();
        turnPoints = new List<Vector3>();
        GNT = 0;
        generateGrid(gridWidth, gridHeight);
        clearPath(gridWidth / 2f, 0f, 4);
        buildingGeneration();
    }
    

    void Start()
    {
        
    }

    
    void Update()
    {
        genNew = GameObject.Find("PlayerEmpty").GetComponent<OverallController>().turning;
        if(genNew != 0)
        {
            if(genNewTrig == false)
            {
                GNT++;
                genNewTrig = true;
                if(GNT == 2)
                {
                    clearPath(lastPoint.x, lastPoint.y, 2);
                    buildingGeneration();
                    GNT = 0;
                }
            }
        } else
        {
            genNewTrig = false;
        }
    }

    void generateGrid(float gridWidth, float gridHeight)
    {
    	gridPoints = new Vector2[(int)gridWidth * (int)gridHeight];
    	gridValue = new int[(int)gridWidth * (int)gridHeight];
    	int gridC = 0;
    	for(int y = 0; y < gridHeight; y++)
    	{
    		for(int x = 0; x < gridWidth; x++)
    		{
    			gridPoints[gridC] = new Vector2(x, y);
    			gridValue[gridC] = 1;
    			gridC++;
    		}
    	}
    }

    void clearPath(float originx, float originy, int gens)
    {
    	int listNum = (int)originy * (int)gridWidth + (int)originx;
    	Vector2 origin = gridPoints[listNum];
    	int generations = gens;
    	float turn = 0f;
        
        if(clearedGrid.Count == 0)
        {
            clearedGrid.Add(listNum);
            GenerateSidewalk(new Vector2(listItemToCoordinate(listNum).x, listItemToCoordinate(listNum).y), turn + Mathf.PI / 2f);
            GenerateSidewalk(new Vector2(listItemToCoordinate(listNum).x, listItemToCoordinate(listNum).y), turn - Mathf.PI / 2f);
            GenerateLaneDivider(new Vector2(listItemToCoordinate(listNum).x, listItemToCoordinate(listNum).y), turn);
        } else
        {
            clearedGrid.Clear();
        }
    	for (int g = 0; g < generations; g++)
    	{
    		int dist = UnityEngine.Random.Range(5, 8);
    		Vector2 dir = new Vector2(Mathf.Sin(turn), Mathf.Cos(turn));
    		gridValue[listNum] = 0;
            bool carOpt = true;
            float signH = 0;
    		for (int n = 0; n < dist; n++)
    		{
    			if(n != 0)
    			{
    				GenerateSidewalk(new Vector2(listItemToCoordinate(listNum).x, listItemToCoordinate(listNum).y), turn + Mathf.PI / 2f);
    				GenerateSidewalk(new Vector2(listItemToCoordinate(listNum).x, listItemToCoordinate(listNum).y), turn - Mathf.PI / 2f);
                    GenerateLaneDivider(new Vector2(listItemToCoordinate(listNum).x, listItemToCoordinate(listNum).y), turn);
    			}
    			listNum += (int) dir.x + (int) dir.y * (int) gridWidth;
                if(n == 2)
                {
                    sign = (float)(Random.Range(0, 2) * 2 - 1);
                    signH = sign;
                    carx = - sign * dir.y * 4.25f;
                    cary = sign * dir.x * 4.25f;
                    GenerateBot(new Vector3(listItemToCoordinate(listNum).x + carx, sign,listItemToCoordinate(listNum).y + cary), (turn - Mathf.PI / 2f) + sign * Mathf.PI / 2f);
                }
                if(n == 3 && carOpt)
                {
                    carOpt = false;
                    sign = (float)(Random.Range(0, 2) * 2 - 1);
                    carx = sign * dir.y * 1.75f;
                    cary = - sign * dir.x * 1.75f;
                    GenerateCar(new Vector3(listItemToCoordinate(listNum).x + carx, sign,listItemToCoordinate(listNum).y + cary), (turn - Mathf.PI / 2f) + sign * Mathf.PI / 2f);
                    sign = - signH;
                    carx = - sign * dir.y * 4.25f;
                    cary = sign * dir.x * 4.25f;
                    GenerateBot(new Vector3(listItemToCoordinate(listNum).x + carx, sign,listItemToCoordinate(listNum).y + cary), (turn - Mathf.PI / 2f) + sign * Mathf.PI / 2f);
                }
    			gridValue[listNum] = 0;
    			clearedGrid.Add(listNum);
    		}
            GenerateSidewalkCorner(new Vector2(listItemToCoordinate(listNum).x, listItemToCoordinate(listNum).y), turn);
    		GenerateCrosswalk(new Vector2(listItemToCoordinate(listNum).x, listItemToCoordinate(listNum).y), turn);
    		float turnTemp = turn;
    		turn += (float)(UnityEngine.Random.Range(0, 2) * 2 - 1) * Mathf.PI / 2;
	    	if(turn == Mathf.PI || turn == -Mathf.PI)
	    	{
	    		turn = 0f;
	    	}
	    	turnPoints.Add(new Vector3((float)listNum, turnTemp, turn));

	    	dir = new Vector2(Mathf.Sin(turn), Mathf.Cos(turn));
	    	Vector2 rbP = listItemToCoordinate(listNum);
	    	GenerateRoadblock(new Vector2(rbP.x - dir.x * 5.7f + dir.y * 3f, rbP.y - dir.y * 5.7f + dir.x * 3f), turn * 180f / Mathf.PI);
	    	GenerateRoadblock(new Vector2(rbP.x - dir.x * 5.7f, rbP.y - dir.y * 5.7f), turn * 180f / Mathf.PI);
	    	GenerateRoadblock(new Vector2(rbP.x - dir.x * 5.7f - dir.y * 3f, rbP.y - dir.y * 5.7f - dir.x * 3f), turn * 180f / Mathf.PI);
	    	for(int que = 0; que < 3; que++)
	    	{
	    		listNum += (int) -dir.x + (int) -dir.y * (int) gridWidth;
				gridValue[listNum] = 0;
				clearedGrid.Add(listNum);
				GenerateSidewalk(new Vector2(listItemToCoordinate(listNum).x, listItemToCoordinate(listNum).y), turn + Mathf.PI / 2f);
    			GenerateSidewalk(new Vector2(listItemToCoordinate(listNum).x, listItemToCoordinate(listNum).y), turn - Mathf.PI / 2f);
                GenerateLaneDivider(new Vector2(listItemToCoordinate(listNum).x, listItemToCoordinate(listNum).y), turn);
			}
			GenerateSidewalk(new Vector2(listItemToCoordinate(listNum).x, listItemToCoordinate(listNum).y), turn);
			for(int pue = 0; pue < 3; pue++)
			{
				listNum += (int) dir.x + (int) dir.y * (int) gridWidth;
			}
    	}
        offlim1 = listNum + (int) gridWidth;
        offlim2 = listNum + 2 * (int) gridWidth;
        lastPoint = new Vector2((float) listNum % gridWidth, Mathf.Floor((float) listNum / gridWidth));
    }

    void buildingGeneration()
    {
    	for(int b = 0; b < clearedGrid.Count; b++)
    	{
    		if(clearedGrid[b] != 0 && gridValue[clearedGrid[b]] == 0)
    		{
    			checkTemp[0] = clearedGrid[b] + (int) gridWidth * 2 - 2;
    			checkTemp[1] = clearedGrid[b] + (int) gridWidth * 2 - 1;
    			checkTemp[2] = clearedGrid[b] + (int) gridWidth * 2;
    			checkTemp[3] = clearedGrid[b] + (int) gridWidth * 2 + 1;
    			checkTemp[4] = clearedGrid[b] + (int) gridWidth * 2 + 2;
    			checkTemp[5] = clearedGrid[b] + (int) gridWidth - 2;
    			checkTemp[6] = clearedGrid[b] + (int) gridWidth - 1;
    			checkTemp[7] = clearedGrid[b] + (int) gridWidth;
    			checkTemp[8] = clearedGrid[b] + (int) gridWidth + 1;
    			checkTemp[9] = clearedGrid[b] + (int) gridWidth + 2;
    			checkTemp[10] = clearedGrid[b] - 2;
    			checkTemp[11] = clearedGrid[b] - 1;
    			checkTemp[12] = clearedGrid[b] + 1;
    			checkTemp[13] = clearedGrid[b] + 2;
    			checkTemp[14] = clearedGrid[b] - (int) gridWidth - 2;
    			checkTemp[15] = clearedGrid[b] - (int) gridWidth - 1;
    			checkTemp[16] = clearedGrid[b] - (int) gridWidth;
    			checkTemp[17] = clearedGrid[b] - (int) gridWidth + 1;
    			checkTemp[18] = clearedGrid[b] - (int) gridWidth + 2;
    			checkTemp[19] = clearedGrid[b] - (int) gridWidth * 2 - 2;
    			checkTemp[20] = clearedGrid[b] - (int) gridWidth * 2 - 1;
    			checkTemp[21] = clearedGrid[b] - (int) gridWidth * 2;
    			checkTemp[22] = clearedGrid[b] - (int) gridWidth * 2 + 1;
    			checkTemp[23] = clearedGrid[b] - (int) gridWidth * 2 + 2;
    			for(int c = 0; c < 24; c++)
    			{
    				if(!alreadyBuilt.Contains(checkTemp[c]))
    				{
    					if(checkTemp[c] > 0)
    					{
    						if(gridValue[checkTemp[c]] == 1)
	    					{
                                if(offlim1 != checkTemp[c] && offlim2 != checkTemp[c])
                                {
                                    alreadyBuilt.Add(checkTemp[c]);
                                    float xtem = (float) checkTemp[c] % gridWidth;
                                    float ytem = Mathf.Floor((float) checkTemp[c] / gridWidth);
                                    float xpos = (xtem - gridWidth / 2f) * gridScale;
                                    float ypos = ytem * gridScale;
                                    GenerateBuilding(new Vector2(xpos, ypos));
                                }
	    					}
    					}
    				}
    			}
    		}
    	}
    }

    Vector2 listItemToCoordinate(int point)
    {
    	float xtemy = (float) point % gridWidth;
		float ytemy = Mathf.Floor((float) point / gridWidth);
		float xposy = (xtemy - gridWidth / 2f) * gridScale;
		float yposy = ytemy * gridScale + 5f;
		return new Vector2(xposy, yposy);
    }

    void GenerateBuilding(Vector2 pos)
    {
    	Instantiate(building, new Vector3(pos.x, UnityEngine.Random.Range(-16f, -5f), pos.y), Quaternion.identity);
    }

    void GenerateRoadblock(Vector2 pos, float orientation)
    {
    	Instantiate(roadblock, new Vector3(pos.x, 0f, pos.y), Quaternion.Euler(0f, orientation, 0f));
    }

    void GenerateSidewalk(Vector2 pos, float orientation)
    {
    	Instantiate(sidewalk, new Vector3(pos.x, 0.01f, pos.y), Quaternion.Euler(0f, orientation * 180f / Mathf.PI, 0f));
    }
    
    void GenerateSidewalkCorner(Vector2 pos, float orientation)
    {
    	Instantiate(sidewalkCorner, new Vector3(pos.x, 0.01f, pos.y), Quaternion.Euler(0f, orientation * 180f / Mathf.PI, 0f));
    }

    void GenerateCrosswalk(Vector2 pos, float orientation)
    {
        Instantiate(crosswalk, new Vector3(pos.x, 0.01f, pos.y), Quaternion.Euler(0f, orientation * 180f / Mathf.PI, 0f));
    }

    void GenerateLaneDivider(Vector2 pos, float orientation)
    {
        Instantiate(laneDivider, new Vector3(pos.x, 0.01f, pos.y), Quaternion.Euler(0f, orientation * 180f / Mathf.PI, 0f));
    }

    void GenerateCar(Vector3 pos, float orientation)
    {
        Instantiate(carGen, new Vector3(pos.x, pos.y, pos.z), Quaternion.Euler(0f, orientation * 180f / Mathf.PI, 0f));
    }

    void GenerateBot(Vector3 pos, float orientation)
    {
        Instantiate(bot, new Vector3(pos.x, pos.y, pos.z), Quaternion.Euler(0f, orientation * 180f / Mathf.PI, 0f));
    }
}
