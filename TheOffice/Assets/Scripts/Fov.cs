using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Fov : MonoBehaviour
{
    [SerializeField] int fovDensity = 20;
    [SerializeField] float fovDistance = 10f, fovAngle = 360;
    [SerializeField] MeshFilter fovMeshFilter;
    
    Vector2[] rayDirections;
    Mesh fovMesh;

    private void Start()
    {
        fovMesh = new Mesh();
        fovMeshFilter.mesh = fovMesh;
    }

    void Update()
    {
        rayDirections = new Vector2[fovDensity + 1];
        GetFovDirections();
        MakeFov();
    }

    void GetFovDirections()
    {
        for (int i = 0; i <= fovDensity; i++)
        {
            var dirAngle = (i / (float)fovDensity) * (Mathf.PI * 2 * (fovAngle / 360));
            var dir = AngleToVector(dirAngle);
            rayDirections[i] = dir;
        }
    }

    void MakeFov()
    {
        Vector3[] fovVertices = new Vector3[fovDensity + 1 + 1];
        //Vector2[] uv = new Vector2[fovVertices.Length];
        int[] fovTris = new int[fovDensity * 3];

        fovVertices[0] = Vector3.zero;

        int vertIndex = 1;
        int triIndex = 0;
        for (var i = 0; i <= fovDensity; i++)
        {
            RaycastHit2D hit = Physics2D.Raycast(transform.position, rayDirections[i], fovDistance);
            if(hit.collider != null)
            {
                fovVertices[vertIndex] = transform.InverseTransformPoint(hit.point);
            } else
            {
                fovVertices[vertIndex] = ((Vector3)rayDirections[i] * fovDistance);
            }

            if(i > 0)
            {
                fovTris[triIndex + 0] = 0;
                fovTris[triIndex + 1] = vertIndex - 1;
                fovTris[triIndex + 2] = vertIndex;
                triIndex += 3;
            }
            vertIndex++;
        }

        fovMesh.vertices = fovVertices;
        //fovMesh.uv = uv;
        fovMesh.triangles = fovTris;
    }

    private Vector2 AngleToVector(float angle)
    {
        return new Vector2(Mathf.Cos(angle), Mathf.Sin(angle));
    }
}
