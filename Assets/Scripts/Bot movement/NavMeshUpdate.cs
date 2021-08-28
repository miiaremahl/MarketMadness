using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

/*
 * Class for handling navmesh changes
 * 
 * Miia Remahl 
 * mrema003@gold.ac.uk
 * Last edited: 8.1.2021
 * 
 * Refs:
 * 1.Unity NavMesh tutorial: https://learn.unity.com/tutorial/unity-navmesh#5c7f8528edbc2a002053b497 , how to update navmesh
 * 
 */

public class NavMeshUpdate : MonoBehaviour
{
    //ref to navmesh
    public NavMeshSurface surface;

    //Builds the navmesh
    public void BakeNavmesh()
    {
        //builds the navmesh
        surface.BuildNavMesh();
    }

}
