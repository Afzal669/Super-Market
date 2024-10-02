using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class InAppFunctions : MonoBehaviour
{
    public static InAppFunctions instance;

    public float maxAdTime = 300;
    public float adTimeCalculator;
    public Text adverTisingText;

    public GameObject InAppFailPanal;
    public GameObject InAppSuccessPanal;

    private void Awake()
    {
        instance = this;
       /// PlayerPrefs.SetInt("RemoveAds", 1);
       
    }
    private void Start()
    {
     GoogleAdMobController.adMobController.ShowBannerAd();

    }
    private void Update()
    {
        #region Ads
        adTimeCalculator += Time.deltaTime;
        if (adTimeCalculator >= maxAdTime)
        {
            adTimeCalculator = 0;
            StartCoroutine(ShowAd());
        }

        #endregion
    }
    public void OnClickPurchaseBtn(float amount)
    {
        Player.instance.UpdatePlayerMoney(amount);
    }
    public void OnRemoveAd()
    {

    }
    public void ShowInterstitialAds()
    {
        GoogleAdMobController.adMobController.ShowInterstitialAd();

    }
    public void RewardAd()
    {
        GoogleAdMobController.adMobController.ShowRewardedAd();

    }
    public void OnRemoveAdSuccess()
    {
        // call remove ads from admob controller
        GoogleAdMobController.adMobController.OnRemoveAds();
    }

    IEnumerator ShowAd()
    {
        WaitForSeconds oneSecond = new WaitForSeconds(1);
        int _counter = 5;
        while (_counter > 0)
        {
            adverTisingText.text = "Advertising in " + _counter;
            _counter--;
            yield return oneSecond;
        }
        adverTisingText.text = "";

        GoogleAdMobController.adMobController.ShowInterstitialAd();
    }

    public void OnInAppSuccess()
    {
        //InAppUI.instance.ShowImagesAndPrice();
        InAppSuccessPanal.SetActive(true);
        
    }

    public void OnInAppFail()
    {
        InAppFailPanal.SetActive(true);
    }
}
