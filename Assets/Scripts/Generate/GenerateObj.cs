using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GenerateObj : MonoBehaviour
{
    public GameObject GenerateObjPrefab;
    List<GameObject> objs = new List<GameObject>();
    public BoxCollider GenerateArea;
    float GenerateTime = 1f;

	// Update is called once per frame
	void Update()
    {
		GenerateTime -= Time.deltaTime;
		if (GenerateTime < 0f)
		{
			GenerateOneObj();
			GenerateTime = 1f;
		}

	}

    public void GenerateOneObj()
    {
        GameObject newObj = Instantiate(GenerateObjPrefab, GenerateArea.transform);
        objs.Add(newObj);
        newObj.transform.localPosition = RandomPointInBounds(GenerateArea.bounds);
    }

	public static Vector3 RandomPointInBounds(Bounds bounds)
	{
		return new Vector3(
			Random.Range(bounds.min.x, bounds.max.x),
			Random.Range(bounds.min.y, bounds.max.y),
			Random.Range(bounds.min.z, bounds.max.z)
		);
	}
}
