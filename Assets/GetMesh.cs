using UnityEngine;
using System.Collections;
using UnityEditor;

public class GetMesh : MonoBehaviour {
    private MeshFilter filter;
    // Use this for initialization
    void Start() {
        filter = GetComponent<MeshFilter>();
        for(int i = 0; i < filter.sharedMesh.vertices.Length; i++) {
            var vertex = filter.sharedMesh.vertices[i];
            Debug.Log(vertex.x.ToString() + ", " + vertex.y.ToString() + ", " + vertex.z.ToString());
        }

        var m = new Mesh();
        Vector3[] vertices = new Vector3[6];
        // left-handed basis!!11, clockwise face
        var nearZ = 0.5f;
        var farZ = -0.5f;
        vertices[0] = new Vector3(0, 0.5f, nearZ);             // 3
        vertices[1] = new Vector3(0.4330127f, -0.25f, nearZ); // 4
        vertices[2] = new Vector3(-0.4330127f, -0.25f, nearZ);  // 5

        for(int i = 3; i <= 5; i++) {
            vertices[i] = new Vector3(vertices[i - 3].x, vertices[i - 3].y, farZ);
        }

        int[] triangles = new int[3 * 2];

        int triangleIndex = 0;
        triangles[triangleIndex] = 0;
        triangles[triangleIndex + 1] = 1;
        triangles[triangleIndex + 2] = 2;

        triangleIndex += 3;
        triangles[triangleIndex] = 0;
        triangles[triangleIndex + 1] = 2;
        triangles[triangleIndex + 2] = 5;

        //triangleIndex++;
        //triangles[triangleIndex] = 3;
        //triangles[triangleIndex + 1] = 4;
        //triangles[triangleIndex + 2] = 0;

        m.vertices = vertices;
        m.triangles = triangles;
        m.RecalculateNormals();

        AssetDatabase.CreateAsset(m, "Assets/Editor/Mesh.asset");
        AssetDatabase.SaveAssets();
    }

    // Update is called once per frame
    void Update() {

    }
}
