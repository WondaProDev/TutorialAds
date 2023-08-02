using System.Collections;
using System.Collections.Generic;
using Unity.Services.Core;
using Unity.Services.Core.Environments;
using UnityEngine;
using UnityEngine.Purchasing;
using UnityEngine.Purchasing.Extension;

public class IAPManager : MonoBehaviour, IDetailedStoreListener
{
    public IStoreController storeController;
    public  IExtensionProvider extensionProvider;

    private void Awake() {
      Initialize();
      DontDestroyOnLoad(this);
     }
     public async void Initialize()
    {
          InitializationOptions options = new InitializationOptions()
#if UINITY_EDITOR || DEVELOPMENT_BUILD
        .SetEnvironmentName("test");
#else
      .SetEnvironmentName("production");
#endif
        await UnityServices.InitializeAsync(options);
        ResourceRequest operation = Resources.LoadAsync<TextAsset>("IAPProductCatalog");
        operation.completed += HandleIAPProductCatalogLoaded;
    }

    void HandleIAPProductCatalogLoaded(AsyncOperation operation)
    {
        ResourceRequest request = operation as ResourceRequest;
        ProductCatalog catalog = JsonUtility.FromJson<ProductCatalog>((request.asset as TextAsset).text);
#if UNITY_ANDROID
        ConfigurationBuilder builder = ConfigurationBuilder.Instance(
            StandardPurchasingModule.Instance(AppStore.GooglePlay)
        );
#elif UNITY_IOS
        ConfigurationBuilder builder = ConfigurationBuilder.Instance(
            StandardPurchasingModule.Instance(AppStore.AppleAppStore)
        );
#else
        ConfigurationBuilder builder = ConfigurationBuilder.Instance(
            StandardPurchasingModule.Instance(AppStore.NotSpecified)
        );
#endif
        foreach (ProductCatalogItem item in catalog.allProducts)
        {
            builder.AddProduct(item.id, item.type);
        }
        UnityPurchasing.Initialize(this, builder);
    }

    void IStoreListener.OnInitialized(IStoreController controller, IExtensionProvider extensions)
    {
        storeController = controller;
        extensionProvider = extensions;
        Debug.Log("Inapppurchase Initialize successfully");
    }

    void IStoreListener.OnInitializeFailed(InitializationFailureReason error)
    {
        Debug.LogError($"Error initializing IAP because of {error}." + 
                        $"\r\nShow a message to the player on the error.");
    }

    void IStoreListener.OnInitializeFailed(InitializationFailureReason error, string message)
    {
         Debug.LogError($"Error initializing IAP because of {error}." + 
                        $"\r\nShow a message to the player on the error.");
    }

    void IDetailedStoreListener.OnPurchaseFailed(Product product, PurchaseFailureDescription failureDescription)
    {
        Debug.LogError($"Error Purchase IAP because of {failureDescription.reason}." + 
                        $"\r\nShow a message to the player on the error.");
    }

    void IStoreListener.OnPurchaseFailed(Product product, PurchaseFailureReason failureReason)
    {
        Debug.LogError($"22 Error Purchase IAP because of {failureReason}." + 
                        $"\r\nShow a message to the player on the error.");
    }

    PurchaseProcessingResult IStoreListener.ProcessPurchase(PurchaseEventArgs purchaseEvent)
    {
        return PurchaseProcessingResult.Complete;
    }

    public void BuyItem(string id){
        storeController.InitiatePurchase(id);

    }

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
