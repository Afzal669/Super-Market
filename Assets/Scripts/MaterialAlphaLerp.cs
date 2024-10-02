using UnityEngine;
using System.Collections;

public class MaterialAlphaLerp : MonoBehaviour
{
    public Material material;  // Assign your material in the inspector
    private float emissionIntensity = -0.3f;  // Start emission intensity
    private bool increasing = true;  // Track if we are increasing or decreasing
    public float minEmissionIntensity = -0.3f;  // Minimum emission intensity
    public float maxEmissionIntensity = 0.2f;   // Maximum emission intensity
    public float emissionSpeed = 0.5f;  // Speed of the glow effect

    private Color whiteColor = Color.white;  // Reference white color

    private void Start()
    {
        // Ensure the material has an emission property enabled
        material.EnableKeyword("_EMISSION");

        // Set the base emission color to white
        material.SetColor("_EmissionColor", whiteColor * minEmissionIntensity);

    }
    private void OnEnable()
    {
        StartCoroutine(ChangeEmissionCoroutine());
    }
    private IEnumerator ChangeEmissionCoroutine()
    {
        // Loop indefinitely
        while (true)
        {
            // Calculate emission intensity based on the current value
            if (increasing)
            {
                emissionIntensity += Time.deltaTime * emissionSpeed;  // Increase intensity
                if (emissionIntensity >= maxEmissionIntensity)  // When it reaches the max, reverse direction
                {
                    emissionIntensity = maxEmissionIntensity;
                    increasing = false;
                }
            }
            else
            {
                emissionIntensity -= Time.deltaTime * emissionSpeed;  // Decrease intensity
                if (emissionIntensity <= minEmissionIntensity)  // When it reaches the min, reverse direction
                {
                    emissionIntensity = minEmissionIntensity;
                    increasing = true;
                }
            }

            // Set the emission color using the updated intensity, keeping the color white
            material.SetColor("_EmissionColor", whiteColor * Mathf.Clamp(emissionIntensity, minEmissionIntensity, maxEmissionIntensity));

            // Wait until the next frame
            yield return null;
        }
    }
    private void OnDisable()
    {
        StopCoroutine(ChangeEmissionCoroutine());
    }
}
