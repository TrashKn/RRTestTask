using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

public class FileDownloader
{    
    public static IEnumerator DownloadImage(string MediaUrl, Action<Texture2D> callback)
    {
        UnityWebRequest request = UnityWebRequestTexture.GetTexture(MediaUrl);
        yield return request.SendWebRequest();
        if (request.result != UnityWebRequest.Result.Success)
            Debug.Log(request.error);
        else
            callback.Invoke(((DownloadHandlerTexture)request.downloadHandler).texture);
            
    }
}
