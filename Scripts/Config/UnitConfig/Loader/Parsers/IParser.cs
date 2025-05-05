using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IParser
{
    public void ParseData(string data);
}
public abstract class ParserData : IParser
{
    protected IExecutor _executor;
    public ParserData(IExecutor executor)
    {
        _executor = executor;
    }
    public abstract void ParseData(string data);
}
