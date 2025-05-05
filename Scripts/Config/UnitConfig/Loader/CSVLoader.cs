using System;
using System.Collections;
using UnityEngine;
using UnityEngine.Networking;

public class CSVLoader
{
    private string _templateURLRequest = "https://docs.google.com/spreadsheets/d/SHEETID/export?format=csv&gid=GID";

    public void StartDownload(GoogleLoaderConfig loaderConfig)
    {
        foreach (var sheet in loaderConfig.sheetIds)
        {
            if (string.IsNullOrEmpty(sheet._sheetId) || sheet._gid == 0)
            {
                
                return;
            }

            string addressURL = _templateURLRequest
                .Replace("SHEETID", sheet._sheetId)
                .Replace("GID", sheet._gid.ToString()); // �������� gid ����� ��� ������� URL

#if UNITY_EDITOR
            EditorCoroutine.Start(DownloadRawCSV(addressURL, sheet.GetParser().ParseData));
#endif
        }
    }

    private IEnumerator DownloadRawCSV(string url, Action<string> callback)
    {
        
        using (UnityWebRequest request = UnityWebRequest.Get(url))
        {
            request.SendWebRequest();
            while (!request.isDone)
            {
                yield return null;
            }
            

            if (request.result == UnityWebRequest.Result.ConnectionError ||
                request.result == UnityWebRequest.Result.ProtocolError ||
                request.result == UnityWebRequest.Result.DataProcessingError)
            {
                
                callback(null);
            }
            else
            {
                
                string csvContent = request.downloadHandler.text;
                if (string.IsNullOrEmpty(csvContent))
                {
                    
                }
                else
                {
                    
                    callback(csvContent);
                }
            }
        }
    }
}