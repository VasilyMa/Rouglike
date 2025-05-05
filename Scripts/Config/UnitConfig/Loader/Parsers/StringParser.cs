using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using Unity.VisualScripting;
using UnityEngine;

public class StringParser : ParserData
{
    public StringParser(IExecutor executor) : base(executor) { }
    public override void ParseData(string data)
    {
        var strParse = data.Split("\n");
        for (int i = 1; i < strParse.Length; i++)
        {
            strParse[i] = strParse[i].Replace("\r", "");
            var rowParse = strParse[i].Split(",");
            rowParse = rowParse.Select(s => s.Replace('.', ',')).ToArray();
            _executor.SetData(rowParse);
            _executor.Invoke();
        }
        
        
    }
}
