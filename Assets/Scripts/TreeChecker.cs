using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TreeChecker : MonoBehaviour
{
    private void OnTriggerExit2D(Collider2D collider2D)
    {
        if (collider2D.CompareTag("MarkedTree"))
        {
            GetComponentInParent<BeggarsCamps>().DestroyCamp();
            Debug.Log("Beggars camp has been destroyed");
        }
    }
}
