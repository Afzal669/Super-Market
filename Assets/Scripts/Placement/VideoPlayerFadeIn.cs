using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using UnityEngine.Video;

public class VideoPlayerFadeIn : MonoBehaviour
{
    public VideoPlayer videoPlayer;
    public GameObject RawImage;
    public float fadeDuration = 1f; // Adjust the fade duration as needed

    private bool isFadingIn = false;

    private void Start()
    {

        StartCoroutine( WaitForVideoEnd());

      
    }

    private IEnumerator WaitForVideoEnd()
    {
        while (!videoPlayer.isPaused)
        {
            yield return null;
        }

        OnVideoEnded(videoPlayer);
    }
   

     void OnVideoEnded(VideoPlayer vp)
    {
        if (!isFadingIn)
        {
            isFadingIn = true;
            StartCoroutine(FadeOutCanvas());
        }
    }

    private IEnumerator FadeOutCanvas()
    {
        CanvasGroup canvasGroup = RawImage.GetComponent<CanvasGroup>();
        if (canvasGroup == null)
        {
            Debug.LogError("Canvas does not have a CanvasGroup component.");
            yield break;
        }

        canvasGroup.alpha = 1f; // Start with the canvas completely opaque

        float elapsedTime = 0f;
        while (elapsedTime < fadeDuration)
        {
            elapsedTime += Time.deltaTime;
            float alpha = 1f - (elapsedTime / fadeDuration);
            canvasGroup.alpha = alpha;
            yield return null;
        }

        canvasGroup.alpha = 0f; // Set the canvas to fully transparent
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1);

    }
}