using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class LoadingBar : MonoBehaviour
{
    public Image targetImage;   // Assign your Image component in the Inspector.
    public float fillDuration = 2f;  // Time it takes to fill the image (2 seconds in this case).

    private void OnEnable()
    {
        // Reset the fillAmount to 0 when the object is enabled
        targetImage.fillAmount = 0f;
        StartCoroutine(FillImage());
    }

    private IEnumerator FillImage()
    {
        float elapsedTime = 0f;

        // Gradually increase the fillAmount from 0 to 1 over the duration
        while (elapsedTime < fillDuration)
        {
            elapsedTime += Time.deltaTime;
            targetImage.fillAmount = Mathf.Clamp01(elapsedTime / fillDuration);
            yield return null;  // Wait until the next frame
        }

        // Ensure the image is fully filled after the duration
        targetImage.fillAmount = 1f;
    }
}
