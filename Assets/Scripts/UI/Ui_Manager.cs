using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using UnityEngine;

public class Ui_Manager : MonoBehaviour
{
    [Header("Computer Panal")]
    public GameObject ComputerPanal;
    public Text TotalPrice;

    public static Ui_Manager instance;
    private void Awake()
    {
        instance = this;
    }
}
