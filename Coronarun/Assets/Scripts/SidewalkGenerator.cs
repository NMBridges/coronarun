using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SidewalkGenerator : MonoBehaviour
{
	public Material SidewalkBlank;
    Transform pTransform;
    Mesh mesh;
    Vector3[] vertices;
    int[] triangles;
    Color[] colors;
    Color[] colorOptions;
    
    void Start()
    {
    	mesh = new Mesh();
    	pTransform = GameObject.Find("PlayerEmpty").GetComponent<Transform>();
    	GetComponent<MeshFilter>().mesh = mesh;
        initArrays();
        generateMesh();
        updateTransparency();
        finalizeMesh();
        
    }

    void initArrays()
    {
    	
    	vertices = new Vector3[0];
	    triangles = new int[0];
	    colors = new Color[0];
	    colorOptions = new Color[]
	    {
	    	new Color(0.6f, 0.6f, 0.6f, 1f),
	    	new Color(0.8f, 0.8f, 0.8f, 1f)
	    };
    }

    
    void Update()
    {
    	pTransform = GameObject.Find("PlayerEmpty").GetComponent<Transform>();
    	updateTransparency();
        float distance = Mathf.Sqrt((transform.position.x - pTransform.position.x) * (transform.position.x - pTransform.position.x) + (transform.position.z - pTransform.position.z) * (transform.position.z - pTransform.position.z));
        if(pTransform.position.z > transform.position.z + 40f && distance > 50f)
        {
            Destroy(gameObject);
        }
    }

    void updateTransparency()
    {
    	float distance = Mathf.Sqrt((transform.position.x - pTransform.position.x) * (transform.position.x - pTransform.position.x) + (transform.position.z - pTransform.position.z) * (transform.position.z - pTransform.position.z));
    	
        float a = 0f;
    	if(distance < 56f && distance > 15f)
    	{
    		a = Mathf.SmoothStep(0f, 1f, Mathf.InverseLerp(56f, 15f, distance));
    	} else if (distance <= 15f)
    	{
    		a = 1f;
    	}

        gameObject.GetComponent<MeshRenderer>().material.color = new Color(colorOptions[0].r, colorOptions[0].g, colorOptions[0].b, a);
    }

    void generateMesh()
    {
    	vertices = new Vector3[4]
    	{
    		new Vector3(-5f, 0.01f, -3.5f),
    		new Vector3(5f, 0.01f, -3.5f),
    		new Vector3(5f, 0.01f, -5f),
    		new Vector3(-5f, 0.01f, -5f)
		};

		triangles = new int[6]
		{
			0, 1, 2,
			0, 2, 3
		};

		colors = new Color[4];
		for(int i = 0; i < vertices.Length; i++)
		{
			colors[i] = colorOptions[0];
		}

    }

    void finalizeMesh()
    {
        mesh.Clear();
        float distance = Mathf.Sqrt((transform.position.x - pTransform.position.x) * (transform.position.x - pTransform.position.x) + (transform.position.z - pTransform.position.z) * (transform.position.z - pTransform.position.z));
        if(true)
        {
            mesh.vertices = vertices;
            mesh.triangles = triangles;
            mesh.colors = colors;
            mesh.RecalculateNormals();
            mesh.RecalculateTangents();
            mesh.RecalculateBounds();
            mesh.Optimize();
        }

    }
}
