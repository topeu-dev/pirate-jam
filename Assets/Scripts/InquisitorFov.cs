using System;
using UnityEngine;

public class InquisitorFov : MonoBehaviour
{
    //1 step - render mesh +
    public MeshFilter meshFilter;

    private void Awake()
    {
        meshFilter = GetComponent<MeshFilter>();
    }

    private void Start()
    {
        drawMesh();
    }

    public void drawMesh()
    {
        Vector3[] triangles;
        Vector3[] vertices;

        // Создаём объект Mesh
        Mesh mesh = new Mesh();

        // Задаём вершины
        mesh.vertices = new Vector3[]
        {
            new Vector3(-1, -1, -1), new Vector3(1, -1, -1),
            new Vector3(1, 1, -1), new Vector3(-1, 1, -1),
            new Vector3(-1, -1, 1), new Vector3(1, -1, 1),
            new Vector3(1, 1, 1), new Vector3(-1, 1, 1),
        };

        mesh.triangles = new int[]
        {
            0, 2, 1, 0, 3, 2,
            4, 5, 6, 4, 6, 7,
            0, 1, 5, 0, 5, 4,
            2, 3, 7, 2, 7, 6,
            0, 4, 7, 0, 7, 3,
            1, 2, 6, 1, 6, 5
        };

        mesh.RecalculateNormals();
        meshFilter.mesh = mesh;
    }
}