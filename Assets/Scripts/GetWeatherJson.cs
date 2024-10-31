using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.UI;

public class SampleSceneController : MonoBehaviour
{
    [SerializeField] private Button button;

    // Start is called before the first frame update
    void Start()
    {
        button.onClick.AddListener(OnClickedButton);
    }

    private void OnClickedButton()
    {
        StartCoroutine(GetRequest("https://www.jma.go.jp/bosai/forecast/data/overview_forecast/130000.json"));
    }

    private IEnumerator GetRequest(string uri)
    {
        using (UnityWebRequest webRequest = UnityWebRequest.Get(uri))
        {
            // リクエストを送信
            yield return webRequest.SendWebRequest();

            if (webRequest.result != UnityWebRequest.Result.Success)
            {
                // エラー処理
                switch (webRequest.result)
                {
                    case UnityWebRequest.Result.ConnectionError:
                    case UnityWebRequest.Result.DataProcessingError:
                    case UnityWebRequest.Result.ProtocolError:
                    default:
                        Debug.LogError(webRequest.error);
                        break;
                }
                yield break;
            } else {
                // レスポンスの JSON データを処理
                var jsonResponse = JsonUtility.FromJson<WeatherData>(webRequest.downloadHandler.text);
                Debug.Log("配信元: " + jsonResponse.publishingOffice);
                Debug.Log("報告日時: " + jsonResponse.reportDatetime);
                Debug.Log("対象地域: " + jsonResponse.targetArea);
                Debug.Log("見出し: " + jsonResponse.headlineText);
                Debug.Log("本文: " + jsonResponse.text);
            }

            yield return null;
        }
    }
}

[System.Serializable]
public class WeatherData
{
    public string publishingOffice;
    public string reportDatetime;
    public string targetArea;
    public string headlineText;
    public string text;
}