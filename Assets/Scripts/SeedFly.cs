using UnityEngine;
using System.Collections.Generic;

public class SeedFly : MonoBehaviour
{
    public GameObject targetContainer; // Parent GameObject containing child objects
    public Vector3 forceDirection = Vector3.up; // Base force direction
    public float forceIntensity = 10f; // Base force intensity
    public float randomDirectionAngle = 15f; // Max random angle deviation in degrees
    public float randomIntensityFactor = 0.2f; // Max random intensity deviation as a fraction of base intensity
    public float fadeDuration = 2f; // Duration over which the object fades out
    public float destroyDelay = 2f; // Delay before the object is destroyed after fading out
    public bool trigger = false; // Control variable to apply force

    private bool hasAppliedForce = false;

    void Update()
    {
        if (trigger && !hasAppliedForce)
        {
            ApplyForceToChildren();
            hasAppliedForce = true;
        }
        else if (!trigger && hasAppliedForce)
        {
            hasAppliedForce = false;
        }
    }

    void ApplyForceToChildren()
    {
        if (targetContainer == null)
        {
            Debug.LogWarning("Target container is not assigned.");
            return;
        }

        // Collect all child transforms into a list
        List<Transform> children = new List<Transform>();
        foreach (Transform child in targetContainer.transform)
        {
            children.Add(child);
        }

        // Apply force to each child
        foreach (Transform child in children)
        {
            Rigidbody rb = child.GetComponent<Rigidbody>();
            if (rb != null)
            {
                // Detach the child from the parent
                child.SetParent(null, true);

                // Calculate random direction
                Vector3 randomDirection = GetRandomDirection(forceDirection, randomDirectionAngle);

                // Calculate random intensity
                float randomIntensity = forceIntensity * (1 + Random.Range(-randomIntensityFactor, randomIntensityFactor));

                // Apply the force
                Vector3 forceVector = randomDirection.normalized * randomIntensity;
                rb.AddForce(forceVector, ForceMode.Impulse);

                // Start the fade-out and destruction process
                StartCoroutine(FadeOutAndDestroy(child.gameObject));
            }
            else
            {
                Debug.LogWarning($"Child '{child.name}' does not have a Rigidbody component.");
            }
        }
    }

    Vector3 GetRandomDirection(Vector3 baseDirection, float maxAngle)
    {
        // Convert maxAngle to radians
        float maxAngleRad = maxAngle * Mathf.Deg2Rad;

        // Generate random angles within the specified range
        float randomYaw = Random.Range(-maxAngleRad, maxAngleRad);
        float randomPitch = Random.Range(-maxAngleRad, maxAngleRad);

        // Create rotation quaternion from random angles
        Quaternion randomRotation = Quaternion.Euler(randomPitch * Mathf.Rad2Deg, randomYaw * Mathf.Rad2Deg, 0);

        // Apply random rotation to the base direction
        return randomRotation * baseDirection;
    }

    System.Collections.IEnumerator FadeOutAndDestroy(GameObject obj)
    {
        Renderer renderer = obj.GetComponent<Renderer>();
        if (renderer == null)
        {
            Debug.LogWarning($"GameObject '{obj.name}' does not have a Renderer component.");
            yield break;
        }

        Material material = renderer.material;

        // Switch to Transparent render mode
        material.SetFloat("_Mode", 3); // Set render mode to Transparent
        material.SetInt("_SrcBlend", (int)UnityEngine.Rendering.BlendMode.SrcAlpha);
        material.SetInt("_DstBlend", (int)UnityEngine.Rendering.BlendMode.OneMinusSrcAlpha);
        material.SetInt("_ZWrite", 0);
        material.DisableKeyword("_ALPHATEST_ON");
        material.EnableKeyword("_ALPHABLEND_ON");
        material.DisableKeyword("_ALPHAPREMULTIPLY_ON");
        material.renderQueue = 3000;

        Color initialColor = material.color;
        Color initialEmission = material.GetColor("_EmissionColor"); // Get initial emission color
        float elapsedTime = 0f;

        while (elapsedTime < fadeDuration)
        {
            elapsedTime += Time.deltaTime;

            // Lerp transparency
            float alpha = Mathf.Lerp(initialColor.a, 0f, elapsedTime / fadeDuration);
            material.color = new Color(initialColor.r, initialColor.g, initialColor.b, alpha);

            // Dim emission
            Color currentEmission = Color.Lerp(initialEmission, Color.black, elapsedTime / fadeDuration);
            material.SetColor("_EmissionColor", currentEmission);

            yield return null;
        }

        // Ensure transparency and emission are fully off
        material.color = new Color(initialColor.r, initialColor.g, initialColor.b, 0f);
        material.SetColor("_EmissionColor", Color.black);

        // Wait before destroying the object
        yield return new WaitForSeconds(destroyDelay);

        Destroy(obj);
    }
}
