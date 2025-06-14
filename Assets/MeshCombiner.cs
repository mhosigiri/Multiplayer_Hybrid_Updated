using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/* Combine mesh filters in children into one mesh on this object */

/*
[RequireComponent(typeof(MeshFilter))]
[RequireComponent(typeof(MeshRenderer))]*/
public class MeshCombiner : MonoBehaviour
{
    private MeshRenderer[] Renderers;

    void Start()
    {
        Renderers = GetComponentsInChildren<MeshRenderer>();
    }

    public void RenderersOn(bool on)
    {
        foreach (MeshRenderer r in Renderers)
        {
            r.enabled = on;
        }
    }
    /*
    void Start()
    {
        MeshFilter[] meshFilters = GetComponentsInChildren<MeshFilter>();
        CombineInstance[] combine = new CombineInstance[meshFilters.Length];

        int i = 0;
        while (i < meshFilters.Length)
        {
            combine[i].mesh = meshFilters[i].sharedMesh;
            combine[i].transform = meshFilters[i].transform.localToWorldMatrix;
            meshFilters[i].gameObject.SetActive(false);

            i++;
        }

        Mesh mesh = new Mesh();
        mesh.CombineMeshes(combine);
        transform.GetComponent<MeshFilter>().sharedMesh = mesh;
    }
    */
}
