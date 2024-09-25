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
    public GameObject CF2Panal;

    [Header("CashMachine Controls")]
    public bool isOnCashMachine = false;
    public bool isOnATMMachine = false;
    private Cash_Machine cash_Machine;

    [Header("UI Bttons")]
    public GameObject ClickBtn;
    public GameObject ExitBtn;

    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        RayCastFunction();
    }

    private void RayCastFunction()
    {
        if (FPS_Camera.activeSelf && Physics.Raycast(FPS_Camera.transform.position, FPS_Camera.transform.forward, out _hitInfo, rayDistance))
        {
            Debug.DrawRay(FPS_Camera.transform.position, FPS_Camera.transform.forward * _hitInfo.distance, Color.red);

            if (_hitInfo.transform)
            {
                Current_Interated_Obj = _hitInfo.collider.gameObject;
                if(Current_Interated_Obj.CompareTag("Click"))
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
            }else if (Current_Interated_Obj.name == "ATM")
            {
                CF2Panal.SetActive(false);
                ClickBtn.SetActive(false);
                cash_Machine.ATM_Camera.enabled = true;
                whiteSport.SetActive(false);
                isOnATMMachine = true;
            }
        }
    }

    public void OnClickExitBtn()
    {
        if (isOnATMMachine)
        {
            isOnATMMachine = false;
            whiteSport.SetActive(true);
            cash_Machine.ATM_Camera.enabled = false;
            CF2Panal.SetActive(true);
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
}
