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
            Destroy(transform.parent.gameObject, 1f);
            Debug.Log("Beggars camp has been destroyed");
        }
    }
}
