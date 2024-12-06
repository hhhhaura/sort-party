using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class CloseRB : MonoBehaviour
{
    public List<Transform> SeedParents = new List<Transform>();
    // Start is called before the first frame update
    void Start()
    {
        foreach (var Parents in SeedParents)
        {
            foreach(var childCollider in Parents.GetComponentsInChildren<Collider>())
            {
				childCollider.enabled = false;

			}
        }
    }
}
