using UnityEngine;

public class SphereDistributor : MonoBehaviour
{
    public GameObject spherePrefab; // Assign your sphere prefab in the Inspector
    public int numberOfSpheres = 100; // Total number of spheres to distribute
    public float radius = 5f; // Distance from the center to each sphere

    void Start()
    {
        DistributeSpheres();
    }

    void DistributeSpheres()
    {
        float offset = 2f / numberOfSpheres;
        float increment = Mathf.PI * (3f - Mathf.Sqrt(5f)); // Golden angle in radians

        for (int i = 0; i < numberOfSpheres; i++)
        {
            float y = i * offset - 1 + (offset / 2);
            float r = Mathf.Sqrt(1 - y * y);
            float phi = i * increment;

            float x = Mathf.Cos(phi) * r;
            float z = Mathf.Sin(phi) * r;

            Vector3 position = new Vector3(x, y, z) * radius;
            Instantiate(spherePrefab, position, Quaternion.identity, transform);
        }
    }
}
