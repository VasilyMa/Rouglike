using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GenericTemporaryObject : MonoBehaviour
{
    public float duration;
    public void OnEnable()
    {
        Invoke("SetActive", duration);
    }
   public void SetActive()
    {
        gameObject.SetActive(false);
    }
}
