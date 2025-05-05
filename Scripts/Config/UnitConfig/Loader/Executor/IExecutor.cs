using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IExecutor
{
    public void SetData(params string[] data);
    public bool CheckFullData();
    public void Invoke();
}
