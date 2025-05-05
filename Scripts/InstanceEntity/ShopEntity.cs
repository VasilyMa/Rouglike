using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShopEntity : SourceEntity
{
    private static ShopEntity _instance;
    public static ShopEntity Instance
    {
        get
        {
            if (_instance == null)
                _instance = new ShopEntity();
            return _instance;
        }
    }

    public override SourceEntity Init()
    {

        return _instance;
    }

}
