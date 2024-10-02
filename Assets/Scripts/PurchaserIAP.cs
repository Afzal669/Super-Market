using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Purchasing;
using UnityEngine.Events;

// Deriving the Purchaser class from IStoreListener enables it to receive messages from Unity Purchasing.
using UnityEngine.Purchasing.Security;


public class PurchaserIAP : MonoBehaviour, IStoreListener
{
    public static PurchaserIAP purcahser;


    [System.Serializable]
    public class CustomIAP
    {
        public string _IAPID;
        public string _IAPName;
        public ProductType ProductType;
        public UnityEvent OnIAPSuccesEvent;

    }






    private static IStoreController m_StoreController;          // The Unity Purchasing system.
    private static IExtensionProvider m_StoreExtensionProvider; // The store-specific Purchasing subsystems.

    // Product identifiers for all products capable of being purchased: 
    // "convenience" general identifiers for use with Purchasing, and their store-specific identifier 
    // counterparts for use with and outside of Unity Purchasing. Define store-specific identifiers 
    // also on each platform's publisher dashboard (iTunes Connect, Google Play Developer Console, etc.)

    // General product identifiers for the consumable, non-consumable, and subscription products.
    // Use these handles in the code to reference which product to purchase. Also use these values 
    // when defining the Product Identifiers on the store. Except, for illustration purposes, the 
    // kProductIDSubscription - it has custom Apple and Google identifiers. We declare their store-
    // specific mapping to Unity Purchasing's AddProduct, below.
    //public static string removeAdsID = "com.gf.squadgame.removeads";

    public bool _isDontDestroy;
    public List<CustomIAP> customIAPs;
    public GameObject pleaseWaitePanel;
    public UnityEvent OnPurchaseFailedEvent;
    public UnityEvent OnPurchaseSuccesEvent;



    //public string _1000Coins_C;
    //public string _3000Coins_C;
    //public string _AstonCar_;
    //public string _PoliceCar_;
    //public string _GrassEnvironment_;



    //public string _5000Coins_C;




    // Apple App Store-specific product identifier for the subscription product.
    //	private static string kProductNameAppleConsumable = "";
    //		private static string kProductNameAppleSubscription =  "com.unity3d.subscription.new";

    // Google Play Store-specific product identifier subscription product.
    		private static string kProductNameGooglePlayConsumable =  "com.unity3d.subscription.original";
    //private static string kProductNameGooglePlayNonConsumable = "com.unity3d.subscription.original";
    //		private static string kProductNameGooglePlaySubscription =  "com.unity3d.subscription.original"; 

    void Awake()
    {
        // If we haven't set up the Unity Purchasing reference
        if (_isDontDestroy)
        {
            if (purcahser == null)
            {
                purcahser = this;
                DontDestroyOnLoad(gameObject);
            }
            else
            {
                Destroy(gameObject);
            }
        }
        else
            purcahser = this;
    
        //if (IsInitialized())
        //{
        //    return;
        //}
        //if (m_StoreController == null)
        {
            // Begin to configure our connection to Purchasing
            InitializePurchasing();
        }
        //		if (m_StoreController.products.WithID(kProductIDNonConsumable).hasReceipt) {
        //			iap.IAPBought ();
        //		 }


        
    }





    public void InitializePurchasing()
    {
        var module = StandardPurchasingModule.Instance();
        // Create a builder, first passing in a suite of Unity provided stores.


        var builder = ConfigurationBuilder.Instance(module);

        // Add a product to sell / restore by way of its identifier, associating the general identifier
        // with its store-specific identifiers.
        //builder.AddProduct(removeAdsID, ProductType.NonConsumable);


        for(int i=0; i<customIAPs.Count; i++)
        {
            builder.AddProduct(customIAPs[i]._IAPID, customIAPs[i].ProductType);
        }

        //builder.AddProduct(_1000Coins_C, ProductType.Consumable);
        //builder.AddProduct(_3000Coins_C, ProductType.Consumable);
        //builder.AddProduct(_AstonCar_, ProductType.NonConsumable);
        //builder.AddProduct(_PoliceCar_, ProductType.NonConsumable);
        //builder.AddProduct(_GrassEnvironment_, ProductType.NonConsumable);
        //builder.AddProduct(_5000Coins_C, ProductType.Consumable);
        // Continue adding the non-consumable product.
        //builder.AddProduct(kProductIDNonConsumable, ProductType.NonConsumable, new IDs() {
        //	{ kProductNameAppleNonConsumable, AppleAppStore.Name },
        //	{ kProductNameGooglePlayNonConsumable, GooglePlay.Name}
        //});
        // And finish adding the subscription product. Notice this uses store-specific IDs, illustrating
        // if the Product ID was configured differently between Apple and Google stores. Also note that
        // one uses the general kProductIDSubscription handle inside the game - the store-specific IDs 
        // must only be referenced here. 

        // Kick off the remainder of the set-up with an asynchrounous call, passing the configuration 
        // and this class' instance. Expect a response either in OnInitialized or OnInitializeFailed.
        UnityPurchasing.Initialize(this, builder);
    }


    private bool IsInitialized()
    {
        // Only say we are initialized if both the Purchasing references are set.
        return m_StoreController != null && m_StoreExtensionProvider != null;
    }


    //public void BuyProduct()
    //{
    //    // Buy the consumable product using its general identifier. Expect a response either 
    //    // through ProcessPurchase or OnPurchaseFailed asynchronously.

    //    BuyProductID(removeAdsID);
    //}





    public void HideWaitePanel()
    {
        purcahser.pleaseWaitePanel.SetActive(false);
    }

    public void BuyProduct(int _index)
    {
        purcahser.pleaseWaitePanel.SetActive(false);
        BuyProductID(customIAPs[_index]._IAPID);
    }


    public void BuyConsumableProducts(int index)
    {
        //if (Menu.Instance)
        //{
        //    Menu.Instance.pleaseWaitePanel.SetActive(true);
        //}
        //switch (index)
        //{
        //    case 0:
        //        BuyProductID(_1000Coins_C);
        //        break;
        //    case 1:
        //        BuyProductID(_3000Coins_C);
        //        break;
        //    case 2:
        //        BuyProductID(_AstonCar_);
        //        break;
        //    case 3:
        //        BuyProductID(_PoliceCar_);
        //        break;
        //    case 4:
        //        BuyProductID(_GrassEnvironment_);
        //        break;
        //}
    }

    void BuyProductID(string productId)
    {
        // If Purchasing has been initialized ...
        if (IsInitialized())
        {
            // ... look up the Product reference with the general product identifier and the Purchasing 
            // system's products collection.
            Product product = m_StoreController.products.WithID(productId);

            // If the look up found a product for this device's store and that product is ready to be sold ... 
            if (product != null && product.availableToPurchase)
            {
                Debug.Log(string.Format("Purchasing product asychronously: '{0}'", product.definition.id));
                // ... buy the product. Expect a response either through ProcessPurchase or OnPurchaseFailed 
                // asynchronously.
                m_StoreController.InitiatePurchase(product);
            }
            // Otherwise ...
            else
            {
                OnPurchaseFailed();

                // ... report the product look-up failure situation  
                Debug.Log("BuyProductID: FAIL. Not purchasing product, either is not found or is not available for purchase");
            }
        }
        // Otherwise ...
        else
        {
            // ... report the fact Purchasing has not succeeded initializing yet. Consider waiting longer or 
            // retrying initiailization.
            OnPurchaseFailed();
            Debug.Log("BuyProductID FAIL. Not initialized.");
        }
    }
    void OnPurchaseFailed()
    {
        //if (Menu.Instance)
        //{
        //    Menu.Instance.onPurchaseFailed();
        //}
        HideWaitePanel();
        if (OnPurchaseFailedEvent != null)
            OnPurchaseFailedEvent.Invoke();
        else
            Debug.Log("No Event For Purchase Failed");
    }

    // Restore purchases previously made by this customer. Some platforms automatically restore purchases, like Google. 
    // Apple currently requires explicit purchase restoration for IAP, conditionally displaying a password prompt.
    public void RestorePurchases()
    {
        // If Purchasing has not yet been set up ...
        if (!IsInitialized())
        {
            // ... report the situation and stop restoring. Consider either waiting longer, or retrying initialization.
            OnPurchaseFailed();

            Debug.Log("RestorePurchases FAIL. Not initialized.");
            return;
        }

        // If we are running on an Apple device ... 
        if (Application.platform == RuntimePlatform.IPhonePlayer ||
            Application.platform == RuntimePlatform.OSXPlayer)
        {
            // ... begin restoring purchases
            Debug.Log("RestorePurchases started ...");

            // Fetch the Apple store-specific subsystem.
            var apple = m_StoreExtensionProvider.GetExtension<IAppleExtensions>();
            // Begin the asynchronous process of restoring purchases. Expect a confirmation response in 
            // the Action<bool> below, and ProcessPurchase if there are previously purchased products to restore.
            apple.RestoreTransactions((result) =>
            {
                // The first phase of restoration. If no more responses are received on ProcessPurchase then 
                // no purchases are available to be restored.
                Debug.Log("RestorePurchases continuing: " + result + ". If no further messages, no purchases available to restore.");
            });
        }
        // Otherwise ...
        else
        {
            OnPurchaseFailed();

            // We are not running on an Apple device. No work is necessary to restore purchases.
            Debug.Log("RestorePurchases FAIL. Not supported on this platform. Current = " + Application.platform);
        }
    }


    //  
    // --- IStoreListener
    //

    public void OnInitialized(IStoreController controller, IExtensionProvider extensions)
    {
        // Purchasing has succeeded initializing. Collect our Purchasing references.
        Debug.Log("OnInitialized: PASS");
        // Overall Purchasing system, configured with products for this application.
        m_StoreController = controller;
        // Store specific subsystem, for accessing device-specific store features.
        m_StoreExtensionProvider = extensions;

    }
    public string GetLocalizedText(string _productID)
    {
        Product product = m_StoreController.products.WithID(_productID);
        if (product != null)
        {
            return product.metadata.localizedPriceString;
        }
        else
        {
            return null;
        }
    }

    public void OnInitializeFailed(InitializationFailureReason error)
    {
        // Purchasing set-up has not succeeded. Check error for reason. Consider sharing this reason with the user.
        Debug.Log("OnInitializeFailed InitializationFailureReason:" + error);
    }

    public PurchaseProcessingResult ProcessPurchase(PurchaseEventArgs args)
    {
        //UIManager.instance.OnRemoveAdsSuccess();
        bool _Process = false;

        for (int i = 0; i < customIAPs.Count; i++)
        {
            if (String.Equals(args.purchasedProduct.definition.id, customIAPs[i]._IAPID, StringComparison.Ordinal))
            {
                HideWaitePanel();

                if (customIAPs[i].OnIAPSuccesEvent != null)
                    customIAPs[i].OnIAPSuccesEvent.Invoke();
                else
                    Debug.LogError("No Succes Event Define for Product" + customIAPs[i]._IAPID);
                if (OnPurchaseSuccesEvent != null)
                    OnPurchaseSuccesEvent.Invoke();
                _Process = true;
                break;
            }
        }

        if(!_Process)
        {
            // Purchase Failed
            OnPurchaseFailed();
        }

        #region OLD_CODE

        //A consumable product has been purchased by this user.
        //if (String.Equals(args.purchasedProduct.definition.id, _1000Coins_C, StringComparison.Ordinal))
        //{
        //    Debug.Log(string.Format("ProcessPurchase: PASS. Product: '{0}'", args.purchasedProduct.definition.id));
        //    // The consumable item has been successfully purchased, add 100 coins to the player's in-game score.
        //    if (Menu.Instance)
        //    {
        //        Menu.Instance.addCash(0);
        //        Menu.Instance.onPurchaseSuccess();
        //    }
           
        //}
        //// Or ... a non-consumable product has been purchased by this user.
        //else if (String.Equals(args.purchasedProduct.definition.id, _3000Coins_C, StringComparison.Ordinal))
        //{
        //    Debug.Log(string.Format("ProcessPurchase: PASS. Product: '{0}'", args.purchasedProduct.definition.id));
        //    // TODO: The non-consumable item has been successfully purchased, grant this item to the player.
        //    if (Menu.Instance)
        //    {
        //        Menu.Instance.addCash(1);
        //        Menu.Instance.onPurchaseSuccess();
        //    }
        //}
        //else if (String.Equals(args.purchasedProduct.definition.id, _AstonCar_, StringComparison.Ordinal))
        //{
        //    Debug.Log(string.Format("ProcessPurchase: PASS. Product: '{0}'", args.purchasedProduct.definition.id));
        //    // TODO: The non-consumable item has been successfully purchased, grant this item to the player.
        //    if (Menu.Instance)
        //    {

        //        Menu.Instance.AstonCarIAP();
        //        Menu.Instance.onPurchaseSuccess();
        //    }
        //}
        //else if (String.Equals(args.purchasedProduct.definition.id,_PoliceCar_, StringComparison.Ordinal))
        //{
        //    Debug.Log(string.Format("ProcessPurchase: PASS. Product: '{0}'", args.purchasedProduct.definition.id));
        //    // TODO: The non-consumable item has been successfully purchased, grant this item to the player.
        //    if (Menu.Instance)
        //    {

        //        Menu.Instance.PoliceCarIAP();
        //        Menu.Instance.onPurchaseSuccess();
        //    }
        //}
        //else if (String.Equals(args.purchasedProduct.definition.id, _GrassEnvironment_, StringComparison.Ordinal))
        //{
        //    Debug.Log(string.Format("ProcessPurchase: PASS. Product: '{0}'", args.purchasedProduct.definition.id));
        //    // TODO: The non-consumable item has been successfully purchased, grant this item to the player.
        //    if (Menu.Instance)
        //    {
        //        Menu.Instance.GreenEnvironmentIAP();
        //        Menu.Instance.onPurchaseSuccess();
        //    }
        //}
        //// Or ... a subscription product has been purchased by this user.
        ////else if (String.Equals(args.purchasedProduct.definition.id, _5000Coins_C , StringComparison.Ordinal))
        ////{
        ////    Debug.Log(string.Format("ProcessPurchase: PASS. Product: '{0}'", args.purchasedProduct.definition.id));
        ////    Menu.Instance.addCash(2);
        ////    Menu.Instance.onPurchaseSuccess();
        ////    // TODO: The subscription item has been successfully purchased, grant this to the player.
        ////}
        //// Or ... an unknown product has been purchased by this user. Fill in additional products here....
        //else
        //{
        //    Debug.Log(string.Format("ProcessPurchase: FAIL. Unrecognized product: '{0}'", args.purchasedProduct.definition.id));
        //    OnPurchaseFailed();
        //}
        #endregion
        // Return a flag indicating whether this product has completely been received, or if the application needs 
        // to be reminded of this purchase at next app launch. Use PurchaseProcessingResult.Pending when still 
        // saving purchased products to the cloud, and when that save is delayed. 
        return PurchaseProcessingResult.Complete;
    }
    public void OnPurchaseFailed(Product product, PurchaseFailureReason failureReason)
    {
        // A product purchase attempt did not succeed. Check failureReason for more detail. Consider sharing 
        // this reason with the user to guide their troubleshooting actions.
        Debug.Log(string.Format("OnPurchaseFailed: FAIL. Product: '{0}', PurchaseFailureReason: {1}", product.definition.storeSpecificId, failureReason));
        OnPurchaseFailed();

    }

    public void OnInitializeFailed(InitializationFailureReason error, string message)
    {
        throw new NotImplementedException();
    }
}