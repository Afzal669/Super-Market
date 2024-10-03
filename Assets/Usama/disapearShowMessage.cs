using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class disapearShowMessage : MonoBehaviour
{
    private void OnEnable()
    {
        Invoke("ShowMessage",2f);
    }
    public void ShowMessage()
    {
        gameObject.SetActive(false);
    }

}
