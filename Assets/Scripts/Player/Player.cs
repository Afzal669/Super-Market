using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UnityStandardAssets.Characters.FirstPerson;

public class Player : MonoBehaviour
{
    [Header("RayCast")]
    public GameObject FPS_Camera;
    public GameObject Current_Interated_Obj;
    private RaycastHit _hitInfo;
    private float rayDistance = 5f;
    public bool RayCastControl=true;
    // Define the distance limit for the raycast
    public float TouchRayCastDistance = 10f;
    [Header("Info")]
    public CharacterController characterController;
    public FirstPersonController FirstPersonController;
    public GameObject FPS_Btns;
    public GameObject FPS_Player;
    public GameObject whiteSport;
    public GameObject CF2Rig;
    public GameObject CF2Panal;
    public float PlayerAmount;
    public DayTimeManager DayTime;
    public GameObject SurfaceCollider;
    [Header("CashMachine Controls")]
    public bool isOnCashMachine = false;
    public bool isOnATMMachine = false;
    private Cash_Machine cash_Machine;

    [Header("Computer")]
    public bool isOnComputer = false;
    //Usama
    private CashDrawerWorking CashController;
    public GameObject prefabItemDetailinBill;
    private List<item> PurchasedItem = new List<item>();
    private List<GameObject> PurchasedItemDetail = new List<GameObject>();
    private float TotalPayment = 0;
    private float DayTotalPayment = 0;
    private float DayStartPayment = 0;
    private CharcaterBehaviour CharacterAtCashMachine;
    public CustomerManager CUSTOMERITEMS;

    [Header("UI Bttons")]
    public GameObject ClickBtn;
    public GameObject ExitBtn;
    public GameObject PlaceBtn;
    public GameObject DropBtn;
    public GameObject MinusDolllarBtn;
    public GameObject RotateBtn;
    public GameObject ReplaceBtn;
    public Button ConfirmPayment;
    public GameObject OpenBoxButton;
    public GameObject EditPricePanal;
    public Text PlayerMoneyText;
    public Text ClickButtonText;
    public GameObject ItemPlaceWarning;
    public Text ItemPlaceWarningText;
    public Button TakeMoneybtn;
    [Header("Object Placement")]
    public bool isRotatingObject;
    public bool canRotate;
    private float moveWheelRotation, objectRotateSPeed = 100f;
    public GameObject SpawnObject;
    public GameObject TempPlaceObject;
    public Material green;
    public Material red;
    public Material OriginalMat;
    public Transform HandBoxPosition;
    [Header("Sound")]
    public AudioSource Music;
    public AudioSource Sound;
    public AudioClip Swap;
    public AudioClip BoxOpen;
    public AudioClip place;
    public AudioClip Money;
    //bool isBoxOpen;
    public bool canPlace = false;


    public static Player instance;

    private void Awake()
    {
        instance = this;
    }

    void Start()
    {
        TakeMoneybtn.onClick.AddListener(TakeMoneyFromUser);
        Application.targetFrameRate = 60;
        PlayerAmount = PlayerPrefs.GetFloat("Money", 200);
        PlayerPrefs.SetFloat("Money", 5000);
        PlayerMoneyText.text = "$" + PlayerAmount;
       
    }

    // Update is called once per frame
    void Update()
    {
        RayCastFunction();

        if (canRotate)
        {
            RotateObject();
        }

        if(HandBoxPosition.childCount>0)
        {
            if (isboxinhand)
                return;
            foreach(GameObject o in SavingSystem.Instance.IntanSavingList)
            {
                if (!o.CompareTag("Item")&&o.GetComponent<BoxCollider>())
                    o.GetComponent<BoxCollider>().enabled = false;
            }
            isboxinhand = true;
        }
        else
        {

            if (HandBoxPosition.childCount == 0 && isboxinhand)
            {
               
                foreach (GameObject o in SavingSystem.Instance.IntanSavingList)
                {
                    if (o.GetComponent<BoxCollider>())
                        o.GetComponent<BoxCollider>().enabled = true;
                }
                isboxinhand = false;
            }

       }
    }

    bool isboxinhand;

    private void RayCastFunction()
    {
        if(RayCastControl)
        {
            if (FPS_Camera.activeSelf && Physics.Raycast(FPS_Camera.transform.position, FPS_Camera.transform.forward, out _hitInfo, rayDistance))
            //,~LayerMask.GetMask("TriggerLayer"), QueryTriggerInteraction.Ignore))
            {
                Debug.DrawRay(FPS_Camera.transform.position, FPS_Camera.transform.forward * _hitInfo.distance, Color.red);

                if (SpawnObject)
                {
                    SpawnObject.transform.position = _hitInfo.point;
                    //SpawnObject.transform.rotation = Quaternion.FromToRotation(Vector3.up, _hitInfo.normal);

                    if (SpawnObject.GetComponent<FurnishItem>().Name == "CashCounter")
                    {
                        print("founds3");
                        if (SpawnObject.GetComponent<FurnishItem>().targetGreenMesh.GetComponent<ColiisionDecision>().isColliding)
                        {
                            canPlace = false;
                            PlaceBtn.SetActive(false);
                            SpawnObject.GetComponent<FurnishItem>().targetGreenMesh.GetComponent<MeshRenderer>().material = red;
                        }
                        else
                        {
                            canPlace = true;
                            PlaceBtn.SetActive(true);
                            print("founds2");
                            SpawnObject.GetComponent<FurnishItem>().targetGreenMesh.GetComponent<MeshRenderer>().material = green;
                        }



                    }
                    else
                    {
                        if (SpawnObject.GetComponent<ColiisionDecision>().isColliding)
                        {
                            canPlace = false;
                            SpawnObject.GetComponent<MeshRenderer>().material = red;
                            PlaceBtn.SetActive(false);
                        }
                        else
                        {
                            canPlace = true;
                            SpawnObject.GetComponent<MeshRenderer>().material = green;
                            PlaceBtn.SetActive(true);

                        }
                    }

                }

                if (_hitInfo.transform && RayCastControl)
                {
                    Current_Interated_Obj = _hitInfo.collider.gameObject;
                    if (Current_Interated_Obj.CompareTag("Click") || Current_Interated_Obj.CompareTag("Item")
                        || Current_Interated_Obj.CompareTag("Pick"))
                    {

                        if (Current_Interated_Obj.name == "Cash_Machine")
                        {
                            ClickButtonText.text = "Check In";
                        }
                        else if (Current_Interated_Obj.name == "Computer")
                        {
                            ClickButtonText.text = "Click";
                        }
                        else if (Current_Interated_Obj.name == "DustBean")
                        {
                            ClickButtonText.text = "Dispose";
                        }
                        else
                        {
                            ClickButtonText.text = "Click";
                        }
                        ClickBtn.SetActive(true);
                    }
                    else
                    {
                        OffButton();
                    }

                }
            }
        }
        else
        {
            OffButton();
            // Check if there is any touch on the screen
            if (Input.touchCount > 0)
            {
                // Get the first touch (can expand this to handle multiple touches if needed)
                Touch touch = Input.GetTouch(0);
                bool touchbool = true;
                // Check if the touch has begun or moved (you can choose other touch phases like Ended or Moved as per requirement)
                if (touch.phase == TouchPhase.Began)
                {
                    // Convert the touch position to a ray from the camera
                    Ray ray = Camera.main.ScreenPointToRay(touch.position);

                    // Variable to store information about what the ray hits
                    RaycastHit hit;

                    // Perform the raycast and check if it hits an object within the limited distance
                    if (Physics.Raycast(ray, out hit, rayDistance) && touchbool)
                    {
                        // The object that the ray hit
                        GameObject hitObject = hit.collider.gameObject;

                        
                        if (hitObject.gameObject.CompareTag("Item"))
                        {
                            GeneratringBilll(hitObject.gameObject.GetComponent<item>());
                            StartCoroutine(MoveInParabola(hitObject.gameObject.transform, cash_Machine.transform.GetChild(0).position, 1f, 0.4f));
                        }
                        else if (hitObject.name == "Dollar" && isOnCashMachine)
                        {
                            float dollar = hitObject.GetComponent<DollarPrice>().DollarValue;
                            CashController.UpdateGivingAmount(dollar);
                        }
                    }
                }
                if (touch.phase == TouchPhase.Ended)
                {
                    touchbool = false;
                }
            }
        }
    }
    public void OffButton()
    {
        ReplaceBtn.SetActive(false);
        ClickBtn.SetActive(false);
    }
    public void OnClickBtn()
    {
        if (Current_Interated_Obj.CompareTag("Click"))
        {
            if (Current_Interated_Obj.name == "Cash_Machine")
            {
                cash_Machine = Current_Interated_Obj.GetComponent<Cash_Machine>();
                CashController = Current_Interated_Obj.GetComponent<CashDrawerWorking>();
                FPS_Player.transform.parent = cash_Machine.positionOnChair.transform;
                cash_Machine.IntractCollider.enabled = false;
                FPS_Player.transform.localPosition = Vector3.zero;
                //FPS_Player.transform.localEulerAngles = Vector3.zero;
                characterController.enabled = false;
                FPS_Btns.SetActive(false);
                ExitBtn.SetActive(true);
                isOnCashMachine = true;
                RayCastControl = false;
            }
            else if (Current_Interated_Obj.name == "place")
            {
                EditPricePanal.SetActive(true);
                EditPrice EditPanel = EditPricePanal.GetComponent<EditPrice>();
                GameObject parent = Current_Interated_Obj.transform.parent.gameObject;
                //  GameObject child = Current_Interated_Obj.transform.GetChild(0).gameObject;

                GameObject currentProduct = parent.GetComponent<ShelfPlacement>().currentProduct;
                EditPanel.image.sprite = currentProduct.GetComponent<item>().sprite;
                EditPanel.Name.text = currentProduct.GetComponent<item>().Name;
                float UnitPrice = currentProduct.GetComponent<item>().Unit_Price;
                EditPanel.buyingPrice.text = UnitPrice.ToString();
                //editPanla.sellingPrice.text = (currentProduct.GetComponent<item>().Unit_Price * 12).ToString();
                EditPanel.sellingPrice.text = parent.GetComponent<ShelfPlacement>().GrossPrice.ToString();
                float per = (UnitPrice * 10) / 100;
                EditPanel.competitivePrice.text = "COMPETITIVE PRICE " + (UnitPrice + per).ToString();
                float SelllingPrice = parent.GetComponent<ShelfPlacement>().GrossPrice ;
                EditPanel.sellingProfit.text = (parent.GetComponent<ShelfPlacement>().GrossPrice - UnitPrice).ToString();
                if (SelllingPrice < UnitPrice)
                {
                    EditPanel.sellingProfit.color = Color.red;
                }
                else
                {
                    EditPanel.sellingProfit.color = Color.green;
                }
            }
            else if (Current_Interated_Obj.name == "ATM")
            {
                CF2Rig.SetActive(false);
                ClickBtn.SetActive(false);
                cash_Machine.ATM_Camera.enabled = true;
                whiteSport.SetActive(false);
                isOnATMMachine = true;
            }
            else if (Current_Interated_Obj.name == "Computer")
            {
                CF2Panal.SetActive(false);
                ClickBtn.SetActive(false);
                whiteSport.SetActive(false);
                Ui_Manager.instance.ComputerPanal.SetActive(true);
                isOnComputer = true;
                RayCastControl = false;

            }
            //sanaullah Code
            else if (Current_Interated_Obj.GetComponent<item>())
            {
                string objectname = Current_Interated_Obj.GetComponent<item>().Name;
                if (objectname == "Box")
                {
                    DropBtn.SetActive(true);
                    if (Current_Interated_Obj.GetComponent<Rigidbody>())
                    {
                        Destroy(Current_Interated_Obj.GetComponent<Rigidbody>());

                    }
                    Current_Interated_Obj.transform.SetParent(HandBoxPosition);
                    Current_Interated_Obj.transform.localPosition = Vector3.zero;
                    Current_Interated_Obj.transform.localRotation = Quaternion.Euler(0, 90, 0);
                    if (!Current_Interated_Obj.GetComponent<BoxAddRemove>().isOpenBox)
                        OpenBoxButton.SetActive(true);

                }
                if (objectname == "ShelfBox")
                {
                    ShelfPlacement shelf = Current_Interated_Obj.GetComponent<ShelfPlacement>();
                    if (!shelf)
                        return;

                    if (HandBoxPosition.childCount > 0)
                    {
                        if (!HandBoxPosition.GetChild(0).GetComponent<BoxAddRemove>().isOpenBox)
                            return;
                        BoxAddRemove boxScript = HandBoxPosition.GetChild(0).GetComponent<BoxAddRemove>();
                        if (!boxScript)
                            return;
                        if (shelf.currentProduct)
                        {
                            if (!boxScript.IscurrentProduct(shelf.currentProduct.GetComponent<item>().Name))
                            {
                                ItemPlaceWarningText.text = "This place is dedicated for " + shelf.currentProduct.GetComponent<item>().Name;
                                ItemPlaceWarning.SetActive(true);
                                return;
                            }
                            if (!shelf.hasAvailableSlot())
                            {
                                return;
                            }
                            GameObject o = HandBoxPosition.GetChild(0).GetComponent<BoxAddRemove>().RemoveProduct();
                            if (o)
                            {
                                CUSTOMERITEMS.shelvesItem.Add(o.transform);
                                Current_Interated_Obj.GetComponent<ShelfPlacement>().AddProduct(o);
                            }
                        }

                        else
                        {

                            if (!(shelf.GetComponent<item>().Display == boxScript.Product.GetComponent<item>().Display))
                            {
                                ItemPlaceWarningText.text = " Place this item in " + boxScript.Product.GetComponent<item>().Display;
                                ItemPlaceWarning.SetActive(true);
                                return;
                            }

                            if (shelf.currentProduct)
                                if (shelf.currentProduct.GetComponent<item>().Name != boxScript.Product.GetComponent<item>().Name)
                                {
                                    ItemPlaceWarningText.text = "This place is dedicated for " + shelf.currentProduct.GetComponent<item>().Name;
                                    ItemPlaceWarning.SetActive(true);
                                    return;
                                }

                            GameObject o = HandBoxPosition.GetChild(0).GetComponent<BoxAddRemove>().RemoveProduct();
                            if (o)
                            {
                                CUSTOMERITEMS.shelvesItem.Add(o.transform);
                                Current_Interated_Obj.GetComponent<ShelfPlacement>().AddProduct(o);

                                if (Current_Interated_Obj.GetComponent<ShelfPlacement>().currentProduct)
                                {
                                    Current_Interated_Obj.GetComponent<ShelfPlacement>().Tag.SetActive(true);
                                    Current_Interated_Obj.GetComponent<ShelfPlacement>().Tag.transform.GetChild(0).GetComponent<Tag>().price.text = "$" +
                                    Current_Interated_Obj.GetComponent<ShelfPlacement>().currentProduct.GetComponent<item>().Gross_Price.ToString();


                                    Current_Interated_Obj.GetComponent<ShelfPlacement>().Tag.transform.GetChild(0).GetComponent<Tag>().image.sprite =
                                        Current_Interated_Obj.GetComponent<ShelfPlacement>().currentProduct.GetComponent<item>().sprite;
                                }



                            }
                        }
                    }

                }
                if (objectname == "DustBean")
                {
                    if (HandBoxPosition.childCount > 0)
                    {
                        Destroy(HandBoxPosition.GetChild(0).gameObject);
                        DropBtn.SetActive(false);
                        OpenBoxButton.SetActive(false);

                    }

                }


            }
            //Usama.......
            else if (Current_Interated_Obj.name == "Money" && isOnCashMachine)
            {
                /* CashController.UpdateCashPanel(TotalPayment, ConfirmPayment);
                 //CashController.UpdateCashPanel(54.65f, ConfirmPayment);
                   PedesterienIKA IKaAnim= Current_Interated_Obj.GetComponentInParent<PedesterienIKA>();
                 if (IKaAnim)
                 {
                     IKaAnim.rightHandObj = null;
                     IKaAnim.ikActive = false;
                 }
                 Animator animplay = Current_Interated_Obj.GetComponentInParent<Animator>();
                 if(animplay)
                 {
                     animplay.Play("Idle_Bag");
                 }
                 MinusDolllarBtn.SetActive(true);
                 Current_Interated_Obj.SetActive(false);*/

            }
            else if (Current_Interated_Obj.name == "Shop")
            {
                if (!DayTime.open)
                {
                    DayTime.open = true;
                    Current_Interated_Obj.GetComponent<TextMeshPro>().color = Color.green;
                    Current_Interated_Obj.GetComponent<TextMeshPro>().text = "Open";
                }
                else
                {
                    DayTime.open = false;
                    Current_Interated_Obj.GetComponent<TextMeshPro>().color = Color.red;
                    Current_Interated_Obj.GetComponent<TextMeshPro>().text = "Close";
                }
            }
        }
        else if (Current_Interated_Obj.CompareTag("Pick"))
        {
            if (Current_Interated_Obj.name == "Box")
            {
                SurfaceCollider.SetActive(false);
                GameObject box = Current_Interated_Obj.gameObject;
                TempPlaceObject = Current_Interated_Obj.GetComponent<Box>().item;
                print("Temp is = " + TempPlaceObject);
                SpawnObject = Instantiate(TempPlaceObject);
                SpawnObject.SetActive(true);
                if(SpawnObject.GetComponent<FurnishItem>())
                {
                    SpawnObject.name = SpawnObject.GetComponent<FurnishItem>().Name;
                }
                
                OriginalMat = SpawnObject.GetComponent<MeshRenderer>().material;
                box.SetActive(false);
                SpawnObject.layer = 2;
                SpawnObject.GetComponent<MeshRenderer>().material = red;
                
                SpawnObject.AddComponent<ColiisionDecision>();
                SpawnObject.GetComponent<FurnishItem>().offSetCollider.enabled = true;
                PlaceBtn.SetActive(true);
                RotateBtn.SetActive(true);

                if(SpawnObject.GetComponent<FurnishItem>().Name == "CashCounter")
                {
                    print("founds");
                    OriginalMat = SpawnObject.GetComponent<FurnishItem>().targetGreenMesh.GetComponent<MeshRenderer>().material;
                    SpawnObject.GetComponent<FurnishItem>().targetGreenMesh.GetComponent<MeshRenderer>().material = red;
                    SpawnObject.GetComponent<FurnishItem>().targetGreenMesh.AddComponent<ColiisionDecision>();
                    SpawnObject.GetComponent<FurnishItem>().targetGreenMesh.layer = 2;
                    foreach (GameObject val in SpawnObject.GetComponent<FurnishItem>().childs)
                    {
                        if(val.GetComponent<Collider>())
                        {
                            val.GetComponent<Collider>().enabled = false;
                            val.layer = 2;
                        }
                    }

                }
                else
                {
                    print(SpawnObject.GetComponent<FurnishItem>().Name);
                }
            }
        }
            
    }


    /// <summary>
    ///          ///  Bill generate //Usama
    /// </summary>
    /// 

    public void OnClcikReplaceBtn()
    {
        if (Current_Interated_Obj.CompareTag("Pick"))
        {
            if (Current_Interated_Obj.name == "Shelf")
            {
                SpawnObject = Current_Interated_Obj;
                SpawnObject.SetActive(true);
                
                OriginalMat = SpawnObject.GetComponent<MeshRenderer>().material;
                SpawnObject.layer = 2;
                SpawnObject.GetComponent<MeshRenderer>().material = red;

                SpawnObject.AddComponent<ColiisionDecision>();
                SpawnObject.GetComponent<FurnishItem>().offSetCollider.enabled = true;
                PlaceBtn.SetActive(true);
                RotateBtn.SetActive(true);

                foreach(var val in SpawnObject.GetComponent<FurnishItem>().childs)
                {
                    val.GetComponent<Collider>().enabled = false;
                }

                if (SpawnObject.GetComponent<FurnishItem>().Name == "CashCounter")
                {
                    OriginalMat = SpawnObject.GetComponent<FurnishItem>().targetGreenMesh.GetComponent<MeshRenderer>().material;
                    SpawnObject.GetComponent<FurnishItem>().targetGreenMesh.GetComponent<MeshRenderer>().material = red;
                    SpawnObject.GetComponent<FurnishItem>().targetGreenMesh.AddComponent<ColiisionDecision>();
                    foreach (GameObject val in SpawnObject.GetComponent<FurnishItem>().childs)
                    {
                        if (val.GetComponent<Collider>())
                        {
                            val.layer = 2;
                        }
                    }

                }
                else
                {
                    print(SpawnObject.GetComponent<FurnishItem>().Name);
                }
            }
        }
    }
    public void GeneratringBilll(item DetailedObj)
    {
        int unit = 1;
        bool Exist = false;
        foreach (item items in PurchasedItem)
        {
            if (items == DetailedObj)
            {
                unit++;
            }
        }
        string ItemName = DetailedObj.Name;
        float ItemPrice = DetailedObj.Gross_Price;
        int Quantity = unit;
        float Totalprice = unit * ItemPrice;
        TotalPayment += ItemPrice;
        if (!PurchasedItem.Contains(DetailedObj))
        {
            PurchasedItem.Add(DetailedObj);
        }

        foreach (GameObject obj in PurchasedItemDetail)
        {
            string ObjName = obj.transform.GetChild(0).gameObject.GetComponent<TextMeshProUGUI>().text;
            if (ObjName == ItemName)
            {
                obj.transform.GetChild(1).gameObject.GetComponent<TextMeshProUGUI>().text = Quantity.ToString();
                obj.transform.GetChild(3).gameObject.GetComponent<TextMeshProUGUI>().text = Totalprice.ToString();
                Exist = true;
            }
        }
        if (!Exist)
        {
            GameObject DeatilPrefab = Instantiate(prefabItemDetailinBill);
            DeatilPrefab.transform.GetChild(0).gameObject.GetComponent<TextMeshProUGUI>().text = ItemName.ToString();
            DeatilPrefab.transform.GetChild(1).gameObject.GetComponent<TextMeshProUGUI>().text = Quantity.ToString();
            DeatilPrefab.transform.GetChild(2).gameObject.GetComponent<TextMeshProUGUI>().text = ItemPrice.ToString();
            DeatilPrefab.transform.GetChild(3).gameObject.GetComponent<TextMeshProUGUI>().text = Totalprice.ToString();
            DeatilPrefab.transform.parent = cash_Machine.DetailingPanel;
            DeatilPrefab.transform.localScale = new Vector3(1, 1, 1);
            DeatilPrefab.transform.localRotation = Quaternion.identity;
            DeatilPrefab.transform.localPosition = new Vector3(DeatilPrefab.transform.position.x, DeatilPrefab.transform.position.y, 0);
            PurchasedItemDetail.Add(DeatilPrefab);
        }
        cash_Machine.BillAmount.text = TotalPayment.ToString();
      
    }

    public void MinusDollorAmount()
    {
        CashController.UndoRGivingAmount();
    }
    public void OnClickOpenBoxBtn()
    {
        if (HandBoxPosition.childCount > 0)
        {
            if (BoxOpen != null)
            {
                Sound.clip = BoxOpen;
                Sound.Play();
            }
            HandBoxPosition.GetChild(0).GetComponent<BoxAddRemove>().boxopen();
            OpenBoxButton.SetActive(false);
        }
    }

    public void onClickPlaceBtn()
    {
        if(canPlace)
        {
            PlaceBtn.SetActive(false);
            RotateBtn.SetActive(false);
            SpawnObject.GetComponent<MeshRenderer>().material = OriginalMat;
            if (SpawnObject.GetComponent<FurnishItem>())
            {
                foreach (GameObject val in SpawnObject.GetComponent<FurnishItem>().childs)
                {
                    if(val.GetComponent<Collider>())
                        val.GetComponent<Collider>().enabled = true;
                }
            }
            if (SpawnObject.GetComponent<FurnishItem>().Name == "CashCounter")
            {
                SpawnObject.GetComponent<FurnishItem>().targetGreenMesh.GetComponent<MeshRenderer>().material = OriginalMat;
                SpawnObject.GetComponent<FurnishItem>().targetGreenMesh.layer = 0;
                foreach (GameObject val in SpawnObject.GetComponent<FurnishItem>().childs)
                {
                    if (val.GetComponent<Collider>())
                    {
                        val.GetComponent<Collider>().enabled = true;
                        val.layer = 0;
                    }
                }
            }
            if(SpawnObject.GetComponent<FurnishItem>().offSetCollider)
            {
                SpawnObject.GetComponent<FurnishItem>().offSetCollider.enabled = false;
            }
            if (SpawnObject.GetComponent<FurnishItem>().bigCollider)
            {
                SpawnObject.GetComponent<FurnishItem>().bigCollider.enabled = true;
            }
            if (SpawnObject.GetComponent<FurnishItem>().extraCol)
            {
                SpawnObject.GetComponent<FurnishItem>().extraCol.enabled = true;
            }
            SpawnObject.layer = 0;

            if (!SavingSystem.Instance.IntanSavingList.Contains(SpawnObject))
                SavingSystem.Instance.IntanSavingList.Add(SpawnObject);
            Sound.clip = place;
            Sound.Play();
            Destroy(SpawnObject.GetComponent<ColiisionDecision>());
            Destroy(SpawnObject.GetComponent<Rigidbody>());



            /*TempPlaceObject.SetActive(true);
            TempPlaceObject.transform.position = SpawnObject.transform.position;
            TempPlaceObject.transform.rotation = SpawnObject.transform.rotation;
            Destroy(SpawnObject);*/
            SpawnObject = null;
            TempPlaceObject = null;
            canPlace = false;
            SurfaceCollider.SetActive(true);
        }
        
    }

    public void OnClickExitBtn()
    {
        if (isOnComputer)
        {
            isOnComputer = false;
            CF2Panal.SetActive(true);
            ClickBtn.SetActive(true);
            whiteSport.SetActive(true);
            Ui_Manager.instance.ComputerPanal.SetActive(false);
        }
        else if (isOnATMMachine)
        {
            isOnATMMachine = false;
            whiteSport.SetActive(true);
            cash_Machine.ATM_Camera.enabled = false;
            CF2Rig.SetActive(true);
        }
        else if (isOnCashMachine && cash_Machine)
        {
            FPS_Player.transform.parent = cash_Machine.positionOffChair.transform;
            cash_Machine.IntractCollider.enabled = true;
            FPS_Player.transform.localPosition = Vector3.zero;
            //FPS_Player.transform.localEulerAngles = Vector3.zero;
            characterController.enabled = true;
            FPS_Btns.SetActive(true);
            ExitBtn.SetActive(false);
            isOnCashMachine = false;       
            FPS_Player.transform.parent = null;
            cash_Machine = null;
        }
        RayCastControl = true;
    }


    #region Object Rotation


    int position;
    public void RotateObjectBtnDown(int s)
    {

        position = s;
        {
            isRotatingObject = true;
            canRotate = true;
            StartCoroutine(AddingRotationToRotateObject());
        }
    }
    IEnumerator AddingRotationToRotateObject()
    {
        while (isRotatingObject)
        {
            //moveWheelRotation += ControlFreak2.CF2Input.mouseScrollDelta.y;
            moveWheelRotation = Time.deltaTime;//(moveWheelRotation + Time.deltaTime);
            if (moveWheelRotation > 50)
            {
                moveWheelRotation = 50;
            }
            yield return null;
        }
    }
    void RotateObject()
    {
        if (SpawnObject)
        {
            if (position == 0)
            {
                SpawnObject.transform.Rotate(Vector3.up, moveWheelRotation * objectRotateSPeed);

            }
            else if (position == 1)
            {
                SpawnObject.transform.Rotate(Vector3.left, moveWheelRotation * objectRotateSPeed);
            }
        }
    }
    public void RotateObjectBtnUp()
    {
        moveWheelRotation = 0;
        isRotatingObject = false;
        canRotate = false;
    }

    #endregion
    #region   Usama

    public void TakeMoneyFromUser()
    {
        CashController.UpdateCashPanel(TotalPayment, ConfirmPayment);
        //CashController.UpdateCashPanel(54.65f, ConfirmPayment);
        PedesterienIKA IKaAnim = CharacterAtCashMachine.GetComponentInParent<PedesterienIKA>();
        if (IKaAnim)
        {
            IKaAnim.rightHandObj = null;
            IKaAnim.ikActive = false;
        }
        Animator animplay = CharacterAtCashMachine.GetComponentInParent<Animator>();
        if (animplay)
        {
            animplay.Play("Idle_Bag");
        }
        CharacterAtCashMachine.gameObject.GetComponent<CharcaterBehaviour>().Money.gameObject.SetActive(false);
        MinusDolllarBtn.SetActive(true);
        Current_Interated_Obj.SetActive(false);
        TakeMoneybtn.gameObject.SetActive(true);
    }

    public void ConfirmPaymentFunc()
    {
        foreach (GameObject obj in PurchasedItemDetail)
        {
            Destroy(obj);
        }

        UpdatePlayerMoney(TotalPayment);
        DayTotalPayment += TotalPayment;
        TotalPayment = 0;
        PurchasedItemDetail.Clear();
        PurchasedItem.Clear();
        Sound.clip = Money;
        Sound.Play();
        if (CharacterAtCashMachine != null)
        {
            CharacterAtCashMachine.MoveBack();
        }
        if (CUSTOMERITEMS.InstantiatedObj.Contains(CharacterAtCashMachine.gameObject))
        {
            CUSTOMERITEMS.InstantiatedObj.Remove(CharacterAtCashMachine.gameObject);
        }
        MinusDolllarBtn.SetActive(false);
        cash_Machine.BillAmount.text = TotalPayment.ToString();

    }
    public void onClickDropBtn()
    {
        if(HandBoxPosition.childCount>0)
        {
           
            HandBoxPosition.GetChild(0).gameObject.AddComponent<AddRemoveRigidbody>();
            HandBoxPosition.GetChild(0).parent = null;
            DropBtn.SetActive(false);
            OpenBoxButton.SetActive(false);
        }
        
    }
    public void UpdatePlayerMoney(float amount)
    {
        PlayerAmount += amount;
        PlayerAmount = Mathf.Round(PlayerAmount * 100f) / 100f; ;
        PlayerMoneyText.text = "$" + PlayerAmount.ToString("F2");
        PlayerPrefs.SetFloat("Money", PlayerAmount);
        print("New Price is = " + PlayerAmount);
    }

    ///
    ///
    /// // Coroutine that moves a Transform in a parabolic arc
    public IEnumerator MoveInParabola(Transform objectToMove, Vector3 targetPosition, float duration, float arcHeight)
    {
        Vector3 startPosition = objectToMove.position; // Initial position
        float timeElapsed = 0f;
        Sound.clip = Swap;
        Sound.Play();
        while (timeElapsed < duration)
        {
            timeElapsed += Time.deltaTime;
            float t = timeElapsed / duration; // Normalize time to [0,1]

            // Linear interpolation between start and target positions
            Vector3 currentPosition = Vector3.Lerp(startPosition, targetPosition, t);

            // Adding the parabolic height to the Y-axis using a sine function for smoother motion
            float heightOffset = Mathf.Sin(t * Mathf.PI) * arcHeight;

            // Set the current position with the height offset
            currentPosition.y += heightOffset;

            // Apply the new position
            objectToMove.position = currentPosition;

            yield return null; // Wait for the next frame
        }

        // Ensure the object reaches the target position at the end
        objectToMove.position = targetPosition;
        // Destroy(objectToMove.gameObject);
        CashMachine Cashmachine = cash_Machine.gameObject.GetComponent<CashMachine>();
        if (Cashmachine)
        {
            if (Cashmachine.waitingCustomers.Count > 0)
            {
                GameObject HoldingObj = Cashmachine.waitingCustomers[0].gameObject;
                if (HoldingObj.GetComponent<CharcaterBehaviour>())
                {
                    print("Object Found");
                    CharacterAtCashMachine = HoldingObj.GetComponent<CharcaterBehaviour>();
                    if (HoldingObj.GetComponent<CharcaterBehaviour>().CharcaterHoldingItem.Contains(objectToMove.gameObject))
                    {
                        HoldingObj.GetComponent<CharcaterBehaviour>().CharcaterHoldingItem.Remove(objectToMove.gameObject);
                    } 
                    if (HoldingObj.GetComponent<CharcaterBehaviour>().CharcaterHoldingItem.Count==0)
                    {
                        TakeMoneybtn.gameObject.SetActive(false);
                    }
                   
                }
            }
        }
        Destroy(objectToMove.gameObject);
    }
    #endregion
}
