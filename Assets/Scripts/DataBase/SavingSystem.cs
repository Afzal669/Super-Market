using System.Collections.Generic;
using UnityEngine;

public class SavingSystem : MonoBehaviour
{
    [SerializeField] private GenrateBox boxSaving;
    public List<GameObject> IntanSavingList = new List<GameObject>();
    private const string instantiatedPlayerPrefsKey = "InstantiatedGameObjectsData";
    public List<GameObject> prefabList;
    public InstantiatedGameObjectListsClass instantiatedGameObjectListsClassObject;
    public static int shelfInstance=0;
    public static SavingSystem Instance;
    public CustomerManager CUSTOMERITEMS;
    public GameObject AlreadPlaceShuffle;
    private void Awake()
    {
        if (Instance == null)
            Instance = this;
        PlayerPrefs.GetInt("ShelfPlament_Id", 0);
    }

    private void Start()
    {
        if (PlayerPrefs.GetInt("AlreadyPlace", 0) == 0)
        {
            IntanSavingList.Add(AlreadPlaceShuffle);
            SaveGameObjectsData(IntanSavingList);
            PlayerPrefs.SetInt("AlreadyPlace", 1);
        }
    }


    public void SaveGameObjectsData(List<GameObject> instantiatedObjects)
    {
        PlayerPrefs.SetFloat("Money", Player.instance.PlayerAmount);
        instantiatedGameObjectListsClassObject.InstantiatedObjectDataList = new List<InstantiatedGameObjectData>();
        foreach (GameObject obj in instantiatedObjects)
        {
            if (obj != null)
            {
                InstantiatedGameObjectData data = new InstantiatedGameObjectData
                {
                    ObjectName = obj.GetComponent<item>().Name,
                    position = obj.transform.position,
                    rotation = obj.transform.rotation,
           
                };
                if (obj.GetComponent<ShelfParent>())
                {
                    data.Chidl1 = obj.transform.GetChild(0).GetComponent<ShelfPlacement>().productzsizeX;
                    data.GrossPrice5 = obj.transform.GetChild(0).GetComponent<ShelfPlacement>().GrossPrice;
                    data.Chidl2 = obj.transform.GetChild(1).GetComponent<ShelfPlacement>().productzsizeX;
                    data.GrossPrice4 = obj.transform.GetChild(1).GetComponent<ShelfPlacement>().GrossPrice;
                    data.Chidl3 = obj.transform.GetChild(2).GetComponent<ShelfPlacement>().productzsizeX;
                    data.GrossPrice3 = obj.transform.GetChild(2).GetComponent<ShelfPlacement>().GrossPrice;
                    data.Chidl4 = obj.transform.GetChild(3).GetComponent<ShelfPlacement>().productzsizeX;
                    data.GrossPrice2 = obj.transform.GetChild(3).GetComponent<ShelfPlacement>().GrossPrice;

                    if (obj.transform.GetChild(4).GetComponent<ShelfPlacement>())
                    {
                        data.Chidl5 = obj.transform.GetChild(4).GetComponent<ShelfPlacement>().productzsizeX;
                        data.GrossPrice1 = obj.transform.GetChild(4).GetComponent<ShelfPlacement>().GrossPrice;
                        print("Save" + data.GrossPrice1);
                    }
                }

                instantiatedGameObjectListsClassObject.InstantiatedObjectDataList.Add(data);
            }
        }

        string instantiatedObjJson = JsonUtility.ToJson(instantiatedGameObjectListsClassObject, true);
        PlayerPrefs.SetString(instantiatedPlayerPrefsKey, instantiatedObjJson);
        PlayerPrefs.Save();
    }

    public void LoadGameObjectsData(List<GameObject> prefabList)
    {
        Player.instance.PlayerAmount = PlayerPrefs.GetFloat("Money",5000);
        Player.instance.PlayerMoneyText.text ="$"+ Player.instance.PlayerAmount.ToString("F2");
        string instantiatedObjInfo = PlayerPrefs.GetString(instantiatedPlayerPrefsKey);
        if (!string.IsNullOrEmpty(instantiatedObjInfo))
        {
            instantiatedGameObjectListsClassObject.InstantiatedObjectDataList = new List<InstantiatedGameObjectData>();
            InstantiatedGameObjectListsClass loadedData = JsonUtility.FromJson<InstantiatedGameObjectListsClass>(instantiatedObjInfo);
         
            foreach (InstantiatedGameObjectData data in loadedData.InstantiatedObjectDataList)
            {
               
                GameObject prefab = prefabList.Find(p => p.name== data.ObjectName);
                if (prefab != null)
                {
                    GameObject instantiatedObject = Instantiate(prefab, data.position, data.rotation);
                    if(instantiatedObject.GetComponent<FurnishItem>())
                    instantiatedObject.name = instantiatedObject.GetComponent<FurnishItem>().name;
                    if (instantiatedObject.GetComponent<ShelfParent>())
                    {
                        foreach(GameObject val in instantiatedObject.GetComponent<FurnishItem>().childs)
                        {
                            val.GetComponent<BoxCollider>().enabled = true;
                        }

                        instantiatedObject.transform.GetChild(0).GetComponent<ShelfPlacement>().productzsizeX = data.Chidl1;
                        instantiatedObject.transform.GetChild(0).GetComponent<ShelfPlacement>().GrossPrice = data.GrossPrice1;
                        instantiatedObject.transform.GetChild(1).GetComponent<ShelfPlacement>().productzsizeX = data.Chidl2;
                        instantiatedObject.transform.GetChild(1).GetComponent<ShelfPlacement>().GrossPrice = data.GrossPrice2;
                        instantiatedObject.transform.GetChild(2).GetComponent<ShelfPlacement>().productzsizeX = data.Chidl3;
                        instantiatedObject.transform.GetChild(2).GetComponent<ShelfPlacement>().GrossPrice = data.GrossPrice3;
                        instantiatedObject.transform.GetChild(3).GetComponent<ShelfPlacement>().productzsizeX = data.Chidl4;
                        instantiatedObject.transform.GetChild(3).GetComponent<ShelfPlacement>().GrossPrice = data.GrossPrice4;
                        if (instantiatedObject.transform.GetChild(4).GetComponent<ShelfPlacement>())
                        {
                             instantiatedObject.transform.GetChild(4).GetComponent<ShelfPlacement>().productzsizeX = data.Chidl5;
                             instantiatedObject.transform.GetChild(4).GetComponent<ShelfPlacement>().GrossPrice = data.GrossPrice5;
                            //print("Load"+instantiatedObject.transform.GetChild(4).GetComponent<ShelfPlacement>().GrossPrice);
                        }
                        instantiatedObject.GetComponent<ShelfParent>().LoadShelf();

                    }else if (instantiatedObject.GetComponent<item>().Name!= "CashCounter")
                    {
                        CUSTOMERITEMS.shelvesItem.Add(instantiatedObject.transform);
                    }
                   

                    IntanSavingList.Add(instantiatedObject);
                }
            }
        }
    }

    /*public void OnSaveButtonClick()
    {
        boxSaving.SaveBoxState();
        SaveGameObjectsData(IntanSavingList);
    }*/
    private void OnApplicationQuit()
    {
        boxSaving.SaveBoxState();
        SaveGameObjectsData(IntanSavingList);
    }
    public void OnReloadButtonClick()
    {
        boxSaving.LoadBoxState();
        LoadGameObjectsData(prefabList);
    }
    public void OnExitButtonClick()
    {
        // If we are running in the Unity Editor, just stop playing
#if UNITY_EDITOR
        UnityEditor.EditorApplication.isPlaying = false;
#else
        // If running in a build, quit the application
        Application.Quit();
         boxSaving.LoadBoxState();
        LoadGameObjectsData(prefabList);
#endif
    }
}

[System.Serializable]
public class InstantiatedGameObjectData
{
    public string ObjectName;
    public Vector3 position;
    public Quaternion rotation;
    public float Chidl1;
    public float Chidl2;
    public float Chidl3;
    public float Chidl4;
    public float Chidl5;
    public float GrossPrice1;
    public float GrossPrice2;
    public float GrossPrice3;
    public float GrossPrice4;
    public float GrossPrice5;
}

[System.Serializable]
public class InstantiatedGameObjectListsClass
{
    public List<InstantiatedGameObjectData> InstantiatedObjectDataList = new List<InstantiatedGameObjectData>();
}
