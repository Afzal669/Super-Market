using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityStandardAssets.Characters.FirstPerson;

public class Player : MonoBehaviour
{
    [Header("RayCast")]
    public GameObject FPS_Camera;
    public GameObject Current_Interated_Obj;
    private RaycastHit _hitInfo;
    private float rayDistance = 20f;

    [Header("Info")]
    public CharacterController characterController;
    public FirstPersonController FirstPersonController;
    public GameObject FPS_Btns;
    public GameObject FPS_Player;
    public GameObject whiteSport;
    public GameObject CF2Rig;
    public GameObject CF2Panal;
    public float PlayerAmount = 5000;

    [Header("CashMachine Controls")]
    public bool isOnCashMachine = false;
    public bool isOnATMMachine = false;
    private Cash_Machine cash_Machine;

    [Header("Computer")]
    public bool isOnComputer = false;

    [Header("UI Bttons")]
    public GameObject ClickBtn;
    public GameObject ExitBtn;
    public GameObject PlaceBtn;
    public GameObject RotateBtn;

    [Header("Object Placement")]
    public bool isRotatingObject;
    public bool canRotate;
    private float moveWheelRotation, objectRotateSPeed = 0.2f;
    public GameObject SpawnObject;
    public GameObject TempPlaceObject;
    public Material green;
    public Material red;
    public Material OriginalMat;
    public Transform HandBoxPosition;



    public static Player instance;

    private void Awake()
    {
        instance = this;
    }

    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        RayCastFunction();

        if (canRotate)
        {
            RotateObject();
        }
    }

    private void RayCastFunction()
    {
        if (FPS_Camera.activeSelf && Physics.Raycast(FPS_Camera.transform.position, FPS_Camera.transform.forward, out _hitInfo, rayDistance))
        {
            Debug.DrawRay(FPS_Camera.transform.position, FPS_Camera.transform.forward * _hitInfo.distance, Color.red);

            if (SpawnObject)
            {
                SpawnObject.transform.position = _hitInfo.point;
                //SpawnObject.transform.rotation = Quaternion.FromToRotation(Vector3.up, _hitInfo.normal);

                if (SpawnObject.GetComponent<ColiisionDecision>().isColliding)
                {
                    SpawnObject.GetComponent<MeshRenderer>().material = red;
                    PlaceBtn.SetActive(false);
                }
                else
                {
                    SpawnObject.GetComponent<MeshRenderer>().material = green;
                    PlaceBtn.SetActive(true);
                }
            }

            if (_hitInfo.transform)
            {
                Current_Interated_Obj = _hitInfo.collider.gameObject;
                if (Current_Interated_Obj.CompareTag("Click") || Current_Interated_Obj.CompareTag("Pick"))
                {
                    ClickBtn.SetActive(true);

                }
                else
                {
                    ClickBtn.SetActive(false);
                }

            }
        }
    }

    public void OnClickBtn()
    {
        if (Current_Interated_Obj.CompareTag("Click"))
        {
            if (Current_Interated_Obj.name == "Cash_Machine")
            {
                cash_Machine = Current_Interated_Obj.GetComponent<Cash_Machine>();
                FPS_Player.transform.parent = cash_Machine.positionOnChair.transform;
                cash_Machine.IntractCollider.enabled = false;
                FPS_Player.transform.localPosition = Vector3.zero;
                //FPS_Player.transform.localEulerAngles = Vector3.zero;
                characterController.enabled = false;
                FPS_Btns.SetActive(false);
                ExitBtn.SetActive(true);
                isOnCashMachine = true;
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
            }
            //sanaullah Code
            else if (Current_Interated_Obj.GetComponent<item>())
            {
                string objectname = Current_Interated_Obj.GetComponent<item>().Name;
                if (objectname == "Box")
                {
                    Current_Interated_Obj.transform.SetParent(HandBoxPosition);
                    Current_Interated_Obj.transform.localPosition = Vector3.zero;
                    Current_Interated_Obj.transform.localRotation = Quaternion.Euler(0, 90, 0); ;

                }
                if (objectname == "ShelfBox")
                {
                    ShelfPlacement shelf = Current_Interated_Obj.GetComponent<ShelfPlacement>();
                    if (!shelf)
                        return;

                    if (HandBoxPosition.childCount > 0)
                    {
                        if (shelf.currentProduct)
                        {
                            if (shelf.currentProduct.GetComponent<item>().Name != HandBoxPosition.GetChild(0).GetComponent<BoxAddRemove>().Product.GetComponent<item>().Name)
                                return;
                            if (!shelf.hasAvailableSlot())
                            {
                                return;
                            }
                            GameObject o = HandBoxPosition.GetChild(0).GetComponent<BoxAddRemove>().RemoveProduct();
                            if (o)
                            {
                                Current_Interated_Obj.GetComponent<ShelfPlacement>().AddProduct(o);
                            }
                        }
                        else
                        {
                            GameObject o = HandBoxPosition.GetChild(0).GetComponent<BoxAddRemove>().RemoveProduct();
                            if (o)
                            {
                                Current_Interated_Obj.GetComponent<ShelfPlacement>().AddProduct(o);
                            }
                        }
                    }

                }
                if (objectname == "DustBean")
                {
                    if (HandBoxPosition.childCount > 0)
                    {
                        Destroy(HandBoxPosition.GetChild(0).GetComponent<Box>().gameObject);

                    }

                }

            }

        }
        else if (Current_Interated_Obj.CompareTag("Pick"))
        {
            if (Current_Interated_Obj.name == "Box")
            {
                GameObject box = Current_Interated_Obj.gameObject;
                TempPlaceObject = Current_Interated_Obj.GetComponent<Box>().item;
                print("Temp is = " + TempPlaceObject);
                SpawnObject = Instantiate(TempPlaceObject);
                SpawnObject.SetActive(true);
                OriginalMat = SpawnObject.GetComponent<MeshRenderer>().material;
                box.SetActive(false);
                SpawnObject.layer = 2;
                SpawnObject.GetComponent<MeshRenderer>().material = red;
                SpawnObject.AddComponent<ColiisionDecision>();
                SpawnObject.GetComponent<FurnishItem>().offSetCollider.enabled = true;
                PlaceBtn.SetActive(true);
                RotateBtn.SetActive(true);
            }



        }
    }

    public void onClickPlaceBtn()
    {
        PlaceBtn.SetActive(false);
        RotateBtn.SetActive(false);
        SpawnObject.GetComponent<MeshRenderer>().material = OriginalMat;
        SpawnObject.layer = 0;
        Destroy(SpawnObject.GetComponent<ColiisionDecision>());
        Destroy(SpawnObject.GetComponent<Rigidbody>());
        /*TempPlaceObject.SetActive(true);
        TempPlaceObject.transform.position = SpawnObject.transform.position;
        TempPlaceObject.transform.rotation = SpawnObject.transform.rotation;
        Destroy(SpawnObject);*/
        SpawnObject = null;
        TempPlaceObject = null;
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
            moveWheelRotation = (moveWheelRotation + Time.deltaTime);
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
}
