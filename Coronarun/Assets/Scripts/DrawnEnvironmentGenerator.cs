using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DrawnEnvironmentGenerator : MonoBehaviour
{
    Transform pTransform;
    Mesh mesh;
    Vector3[] vertices;
    int[] triangles;
    Color[] colors;
    //Vector2[] uvs;
    Vector3[] normals;
    Color[] colorOptions;
    float buildingWidth;
    float buildingDepth;
    float spawnDistance;
    float height;
    float xoff;
    float zoff;
    Vector3 pVelo;

    void Start()
    {
        pTransform = GameObject.Find("PlayerEmpty").GetComponent<Transform>();
        height = - transform.position.y;
        xoff = transform.position.x;
        zoff = transform.position.z;
        mesh = new Mesh();
        buildingWidth = 10f;
        buildingDepth = 10f;
        transform.position = new Vector3(0f, 0f, 0f);
        GetComponent<MeshFilter>().mesh = mesh;
        initializeArrays();
        GenerateBuilding();
        PrepareMesh();
        GetComponent<MeshCollider>().sharedMesh = mesh;
        
    }

    void Update()
    {
        pTransform = GameObject.Find("PlayerEmpty").GetComponent<Transform>();
        morphBuilding();
        PrepareMesh();
        GetComponent<MeshCollider>().sharedMesh = mesh;
    }

    void initializeArrays()
    {
    	vertices = new Vector3[0];
	    triangles = new int[0];
	    colors = new Color[0];
        //uvs = new Vector2[0];
        normals = new Vector3[0];
	    colorOptions = new Color[6]
	    {
	    	new Color(1f, 0.2f, 0f, 1f),
	    	new Color(0.8f, 0.8f, 0f, 1f),
            new Color(0f, 0.8f, 1f, 1f),
            new Color(0f, 0.8f, 0.3f, 1f),
            new Color(0.8f, 0.3f, 0.3f, 1f),
            new Color(0.8f, 0.2f, 0.8f, 1f)
            //new Color(0.5f, 0.5f, 0.5f, 1f),
            //new Color(0.4f, 0.45f, 0.4f, 1f),
	    	//new Color(0.6f, 0.6f, 0.7f, 1f)
	    };
    }

    void GenerateBuilding()
    {
    	
    	float xcenter = xoff;
    	spawnDistance = zoff;

    	vertices = new Vector3[20]
    	{
    		
    		// the first of each triplet is for the front or back faces
    		// the second of each triplet is for the left or right faces
    		// the third of each triplet is for the top face

    		new Vector3(xcenter - buildingWidth / 2f, 0f, spawnDistance), // front bottom left                         [0]
    		new Vector3(xcenter - buildingWidth / 2f, 0f, spawnDistance), //                                           [1]
    		new Vector3(xcenter - buildingWidth / 2f, height / 10f, spawnDistance), // front top left                  [2]
    		new Vector3(xcenter - buildingWidth / 2f, height / 10f, spawnDistance), //                                 [3]
    		new Vector3(xcenter - buildingWidth / 2f, height / 10f, spawnDistance), //                                 [4]
    		new Vector3(xcenter + buildingWidth / 2f, height / 10f, spawnDistance), // front top right                 [5]
    		new Vector3(xcenter + buildingWidth / 2f, height / 10f, spawnDistance), //                                 [6]
    		new Vector3(xcenter + buildingWidth / 2f, height / 10f, spawnDistance), //                                 [7]
    		new Vector3(xcenter + buildingWidth / 2f, 0f, spawnDistance), // front bottom right                        [8]
    		new Vector3(xcenter + buildingWidth / 2f, 0f, spawnDistance), //                                           [9]
    		new Vector3(xcenter - buildingWidth / 2f, height / 10f, spawnDistance + buildingDepth), // back top left   [10]
    		new Vector3(xcenter - buildingWidth / 2f, height / 10f, spawnDistance + buildingDepth), //                 [11]
    		new Vector3(xcenter - buildingWidth / 2f, height / 10f, spawnDistance + buildingDepth), //                 [12]
    		new Vector3(xcenter + buildingWidth / 2f, height / 10f, spawnDistance + buildingDepth), // back top right  [13]
    		new Vector3(xcenter + buildingWidth / 2f, height / 10f, spawnDistance + buildingDepth), //                 [14]
    		new Vector3(xcenter + buildingWidth / 2f, height / 10f, spawnDistance + buildingDepth), //                 [15]
    		new Vector3(xcenter - buildingWidth / 2f, 0f, spawnDistance + buildingDepth), // back bottom left          [16]
    		new Vector3(xcenter - buildingWidth / 2f, 0f, spawnDistance + buildingDepth), //                           [17]
    		new Vector3(xcenter + buildingWidth / 2f, 0f, spawnDistance + buildingDepth), // back bottom right         [18]
    		new Vector3(xcenter + buildingWidth / 2f, 0f, spawnDistance + buildingDepth) //                           [19]
    	
    	};

    	// color creation

    	colors = new Color[20];
    	Color selectedColor = colorOptions[Random.Range(0, colorOptions.Length)];
    	for (int i = 0; i < 20; i++)
    	{
    		colors[i] = selectedColor;
    	}

        // triangle creation

        /*uvs = new Vector2[20]
        {

            new Vector2(0f, 0f), // front bottom left                         [0]
            new Vector2(0f, 0f), //                                           [1]
            new Vector2(0f, 0f), // front top left                  [2]
            new Vector2(0f, 0f), //                                 [3]
            new Vector2(0f, 0f), //                                 [4]
            new Vector2(0f, 0f), // front top right                 [5]
            new Vector2(0f, 0f), //                                 [6]
            new Vector2(0f, 0f), //                                 [7]
            new Vector2(0f, 0f), // front bottom right                        [8]
            new Vector2(0f, 0f), //                                           [9]
            new Vector2(0f, 0f), // back top left   [10]
            new Vector2(0f, 0f), //                 [11]
            new Vector2(0f, 0f), //                 [12]
            new Vector2(0f, 0f), // back top right  [13]
            new Vector2(0f, 0f), //                 [14]
            new Vector2(0f, 0f), //                 [15]
            new Vector2(0f, 0f), // back bottom left          [16]
            new Vector2(0f, 0f), //                           [17]
            new Vector2(0f, 0f), // back bottom right         [18]
            new Vector2(0f, 0f) //                           [19]

        };*/

        triangles = new int[30]
    	{
    		0, 2, 5,    // front        (top left tri)
    		0, 5, 8,    // front        (bottom right tri)
    		18, 13, 10, // back         (top left tri)
    		18, 10, 16, // back         (bottom right tri)
    		17, 11, 3,  // left side    (top left tri)
    		17, 3, 1,   // left side    (bottom right tri)
    		9, 6, 14,  // right side   (top left tri)
    		9, 14, 19, // right side   (bottom right tri)
    		4, 12, 15,  // top          (top left tri)
    		4, 15, 7    // top          (bottom right tri)
    	};


    }

    void morphBuilding()
    {
    	for(int i = 0; i < vertices.Length; i++)
    	{
    		if(vertices[i].y != 0f)
    		{
    			float tempDistance = Mathf.Sqrt((vertices[i].z - pTransform.position.z) * (vertices[i].z - pTransform.position.z) + (vertices[i].x - pTransform.position.x) * (vertices[i].x - pTransform.position.x));
    			if(tempDistance >= 15f && tempDistance < 50f)
    			{
					vertices[i].y = Mathf.SmoothStep(0.01f, height, (Mathf.InverseLerp(50f, 15f, tempDistance)));
    			} else if(tempDistance < 50f)
    			{
    				vertices[i].y = height;
    			} else
    			{
    				vertices[i].y = 0.01f;
    			}
    		}
    	}

    }


    void PrepareMesh()
    {
    	mesh.Clear();
    	float fDist = Mathf.Sqrt((vertices[0].x - pTransform.position.x) * (vertices[0].x - pTransform.position.x) + (vertices[0].z - pTransform.position.z) * (vertices[0].z - pTransform.position.z));
		if(fDist < 56f)
		{
			mesh.vertices = vertices;
			mesh.triangles = triangles;
			mesh.colors = colors;
            mesh.RecalculateBounds();
            mesh.RecalculateNormals();
		    mesh.RecalculateTangents();
		    mesh.Optimize();
	    }
        if(fDist > 50f && pTransform.position.z > vertices[0].z + 50f)
        {
            Destroy(gameObject);
        }
        
    }
}
