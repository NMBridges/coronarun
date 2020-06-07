using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class OutlineScript : MonoBehaviour
{
    public MeshFilter[] meshSources;
    public SkinnedMeshRenderer[] SMRs;
    private static readonly Vector3 zeroVec = Vector3.zero;

    void Start()
    {
        updateBorder();
    }

    void Update ()
    {
        //updateBorder();
    }

    private struct VertInfo
    {
        public Vector3 vert;
        public int origIndex;
        public Vector3 normal;
        public Vector3 averagedNormal;
    }

    void updateBorder()
    {
    	foreach (MeshFilter meshSource in meshSources)
    	{
    		Mesh MESH = meshSource.mesh;
    		if(meshSource.name == "Body")
            {
            	SkinnedMeshRenderer skin = SMRs[0];
            	skin.sharedMesh = (Mesh) Instantiate(skin.sharedMesh);
            	MESH = skin.sharedMesh;
            	UnityEngine.Debug.Log("gamer");
            }
            Vector3[] verts = MESH.vertices;
            Vector3[] normals = MESH.normals;
            VertInfo[] vertInfo = new VertInfo[verts.Length];
            for (int i = 0; i < verts.Length; i++)
            {
                vertInfo[i] = new VertInfo()
                {
                    vert = verts[i],
                    origIndex = i,
                    normal = normals[i]
                };
            }
            var groups = vertInfo.GroupBy(x => x.vert);
            VertInfo[] processedVertInfo = new VertInfo[vertInfo.Length];
            int index = 0;
            foreach (IGrouping<Vector3, VertInfo> group in groups)
            {
                Vector3 avgNormal = zeroVec;
                foreach (VertInfo item in group)
                {
                    avgNormal += item.normal;
                }
                avgNormal = avgNormal / group.Count();
                foreach (VertInfo item in group)
                {
                    processedVertInfo[index] = new VertInfo()
                    {
                        vert = item.vert,
                        origIndex = item.origIndex,
                        normal = item.normal,
                        averagedNormal = avgNormal.normalized
                    };
                    index++;
                }
            }
            Color[] colors = new Color[verts.Length];
            for (int i = 0; i < processedVertInfo.Length; i++)
            {
                VertInfo info = processedVertInfo[i];

                int origIndex = info.origIndex;
                Vector3 normal = info.averagedNormal;
                //normal = Quaternion.Euler(0f, -45f, 0f) * normal;
                Vector4 normColor = new Vector4((normal.x + 1f) / 2f, (normal.y + 1f) / 2f, (normal.z + 1f) / 2f, 1f);
                colors[origIndex] = normColor;
            }
            if(meshSource.name == "Body")
            {
            	MESH.colors = colors;
            } else
            {
            	meshSource.mesh.colors = colors;
            }
        }
    }
}
