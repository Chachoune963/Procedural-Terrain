using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TerrainGenerator : MonoBehaviour
{
    Mesh currentMesh;
    public MeshFilter meshFilter;

    public int resolution = 2;
    // Scale modifies how elevated each point is. I don't recomment putting it to high as at some point the terrain will just fly off into the distance.
    [Range(0, 256)]
    public float scale;
    public float meshSize;
    // Idea explained by Sebastian Laggue
    // Basically, how farther apart the points are taken from one another indirectly affects the difference beetween the values; And thus, the steepness of the terrain
    public float roughness;

    // These help randomise the terrain by picking different points to pick as the "centre" of our perlin noise sample
    public float xOffset;
    public float yOffset;

    private void OnValidate()
    {
        currentMesh = new Mesh();
        meshFilter.mesh = currentMesh;
        Vector3[] vertices = new Vector3[resolution*resolution];
        // This wastes a TON of space for now and I don't know how to fix it
        int[] triangles = new int[resolution * resolution * 6];
        int i = 0;
        for (int x = 0; x < resolution; x++)
        {
            for (int z = 0; z < resolution; z++)
            {
                Vector3 vertice = new Vector3(((float)x /resolution) * meshSize, scale * Mathf.PerlinNoise(((float)x/resolution) * roughness + xOffset, ((float)z/resolution) * roughness + yOffset), ((float)z/resolution) * meshSize);
                vertices[i] = vertice;

                if (x != resolution - 1 && z != resolution - 1)
                {
                    // Triangle 1: Takes the indexes of the 3 verticles of the triangle
                    // Their "coordinates" (Indexes) are i (Top left vertice), i+resolution+1 (Bottom right) and i+resolution (Bottom left)
                    // Note that the vertices HAVE to be indicated in a clockwise order or the triangle won't render correctly
                    triangles[i * 6] = i;
                    triangles[i * 6 + 1] = i + resolution + 1;
                    triangles[i * 6 + 2] = i + resolution;
                    // Triangle 2
                    triangles[i * 6 + 3] = i;
                    triangles[i * 6 + 4] = i + 1;
                    triangles[i * 6 + 5] = i + resolution + 1;
                }
                i++;
            }
        }
        currentMesh.Clear();
        currentMesh.vertices = vertices;
        currentMesh.triangles = triangles;
        currentMesh.RecalculateNormals();
    }
}
