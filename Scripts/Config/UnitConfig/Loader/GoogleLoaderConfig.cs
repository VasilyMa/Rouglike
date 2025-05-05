using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

[CreateAssetMenu(fileName = "GoogleLoaderConfig", menuName = "GoogleLoaderConfig")]
public class GoogleLoaderConfig : Config
{
   public List<SheetId> sheetIds;
    public void DownloadCSV()
    {
        new CSVLoader().StartDownload(this);
    }

    public override IEnumerator Init()
    {
        yield return null;
    }

    [Serializable]
    public class SheetId
    {
        public string _sheetId = "1xNVAIR2p374CpoJEQgUghtP4gwrUp3OMXV5YQ0OkxIc";
        public int _gid;// ����������� ������� �����
        public Parser parser;
        public Executor executer;
        public IParser GetParser()
        {
            if (parser == Parser.StringParser) return new StringParser(GetExecuter());
            return null;
        }
        public IExecutor GetExecuter()
        {
            if (executer == Executor.Enemy) return new LoadedEnemyExecutor();
            if (executer == Executor.Ability) return new LoadedAbilitiesExecutor();
            return null;
        }
    }
}
public enum Parser
    {
        StringParser
    }
public enum Executor
    {
        Enemy,Ability
    }