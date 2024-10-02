using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Video;

public class StartScene : MonoBehaviour
{
  //  public GameObject Fps;
    public GameObject cf2;
    public GameObject scenceCanvas;
    public Button button;
    public VideoPlayer videoplayer;
    public AudioSource Music;

     public void PlayOnPlayBtn()
    {

        if (PlayerPrefs.GetInt("PlayScene", 0) == 0)
        {
            PlayerPrefs.SetInt("PlayScene", 1);

           // Fps.SetActive(false);
            cf2.SetActive(false);
            scenceCanvas.SetActive(true);
            videoplayer.gameObject.SetActive(true);
            videoplayer.Play();
            button.onClick.AddListener(OnClickSkipButton);

        }
        else
        {
            Music.Play();
            this.enabled = false;
        }
    }

    private void Update()
    {
       
        if (videoplayer.time > 23)
        {

            button.gameObject.SetActive(true);

        }

        if (videoplayer.time > 61.5)
        {
            OnClickSkipButton();
        }
    }

   
     void OnClickSkipButton()
    {
     //   Fps.SetActive(true);
        cf2.SetActive(true);
        scenceCanvas.SetActive(false);
        Music.Play();
        videoplayer.gameObject.SetActive(false);
    }

   
}