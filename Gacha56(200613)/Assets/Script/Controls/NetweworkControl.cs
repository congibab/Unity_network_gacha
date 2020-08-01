using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

public class NetweworkControl : MonoBehaviour
{
    // callBack
    private Action<String> _callback = null;

    //ユーザー名
    private string _userName = "";

    // Start is called before the first frame update
    void Start()
    {
        //スタート時に保存していたデータを取得
        //データが存在しないときは空の文字列を入力
        NetworkData.uuid = PlayerPrefs.GetString(NetworkData.uuidKey);
        NetworkData.userName = PlayerPrefs.GetString(NetworkData.userNameKey);
    }

    #region UUID 取得

    /// <summary>
    /// UUID 取得ボタンを押したらここを通す
    /// </summary>
    public void OnGetUuid(Action<string> callback = null)
    {
        if(!string.IsNullOrEmpty(NetworkData.uuid))
        {
            if(null != callback)
                    callback("UUIDは登録されています");
            return;
        }

        if (!string.IsNullOrEmpty(NetworkData.userName))
        {
            if (null != callback)
            {
                callback("すでにユーザー名登録されています");
            }
        }

        _callback = callback;



        // Http Get コルーチン呼び出し
        StartCoroutine(HttpGet(NetworkData.uuidURL,
            (jsonText) =>
            {
                var uuidData = JsonUtility.FromJson<UuidDataClass>(jsonText);
                // Json をクラスに格納する
                NetworkData.uuid = uuidData.response.uuid;
                //　PlayerPrefsにuuidを登録
                PlayerPrefs.SetString(NetworkData.uuidKey, NetworkData.uuid);
                //　callback が保存するならコールバックを実行する
                if (null != _callback)
                {
                    //　UUIDを返す
                    _callback(NetworkData.uuid);
                    _callback = null;
                }
                //　ステータスコードをDebug出力
                Debug.Log("status_code = " + uuidData.status_code);
            }
            ));
    }

    #endregion

    #region HTTPコルーチン

    /// <summary>
    /// HTTP Get Command
    /// </summary>
    /// <param name="url">呼び出し</param>
    /// <param name="callback">得取した　JSON　テキストを拾うコールバック</param>
    /// <returns></returns>
    private IEnumerator HttpGet(string url, Action<string> callback)
    {
        using (var req = UnityWebRequest.Get(url))
        {   
            // session id が得取できていればHeaderとして追加する
            if(!string.IsNullOrEmpty(NetworkData.requestHeader))
            {
                req.SetRequestHeader("Authorization", NetworkData.requestHeader);
            }
            // HTTP　で URL の adressに　GET リクエストする
            yield return req.SendWebRequest();
            // エラーならばえらーログを出力する
            if (req.isNetworkError || req.isHttpError)
            {
                Debug.LogError(req.error);
            }

            else
            {
                if (null != callback)
                {
                    callback(req.downloadHandler.text);
                }
            }
        }
    }

    private IEnumerator HttpGetBinary(string url, Action<byte[]> callback)
    {
        using (var req = UnityWebRequest.Get(url))
        {
            // session id が得取できていればHeaderとして追加する
            if (!string.IsNullOrEmpty(NetworkData.requestHeader))
            {
                req.SetRequestHeader("Authorization", NetworkData.requestHeader);
            }
            // HTTP　で URL の adressに　GET リクエストする
            yield return req.SendWebRequest();
            // エラーならばえらーログを出力する
            if (req.isNetworkError || req.isHttpError)
            {
                Debug.LogError(req.error);
            }

            else
            {
                if (null != callback)
                {
                    callback(req.downloadHandler.data);
                }
            }
        }
    }


    /// <summary>
    /// HttpPost Command
    /// </summary>
    /// <param name="url">呼び出し</param>
    /// <param name="formData">追加で受信するデータ</param>
    /// <param name="callback">得取した　JSON　テキストを拾うコールバック</param>
    /// <returns></returns>
    private IEnumerator HttpPost(string url, List<IMultipartFormSection> formData, Action<string> callback)
    {
        //　Http でUrl　に　post リクエストする。
        using (var req = UnityWebRequest.Post(url, formData))
        {
            // session id が得取できていればHeaderとして追加する
            if (!string.IsNullOrEmpty(NetworkData.requestHeader))
            {
                req.SetRequestHeader("Authorization", NetworkData.requestHeader);
            }
            //エラーならばエラーログを出力する
            yield return req.SendWebRequest();

            if (req.isNetworkError || req.isHttpError)
            {
                Debug.LogError(req.error);
            }

            else
            {
                //コールバックが存在するならば
                if (null != callback)
                {
                    //jSON テキストを返す
                    callback(req.downloadHandler.text);
                }
            }
        }
    }
    #endregion

    #region ガチャデッキ情報リスト
    
    public void GetGachaDeckList(Action<string> callback = null)
    {
        if(string.IsNullOrEmpty(NetworkData.sessionId))
        {
            if(null != callback)
            {
                callback("session id is empty.");
            }
            return;
        }
        _callback = callback;
        StartCoroutine(HttpGet(NetworkData.gachaDeckInfoListURL,
            (jsonText) =>
            {
                NetworkData.deckLists = JsonUtility.FromJson<gachaDeckListClass>(jsonText);
                if (null != _callback)
                {
                    _callback(NetworkData.deckLists.status_code.ToString());
                    _callback = null;
                }
            }));
    }

    #endregion

    #region ガチャを引く

    public void GetGacha(int gachaIndex, Action<string> callback = null)
    {
        // text listが得取されいなければ何もしない
        if(null == NetworkData.deckLists)
        {
            if(null != callback)
            {
                callback("card list is not loaded.");
            }
            return;
        }
        _callback = callback;
        //送信添付deta list 準備
        List<IMultipartFormSection> formData = new List<IMultipartFormSection>();
        var gachaUrl = NetworkData.getGachaURL + gachaIndex.ToString();
        // HTTP post Coroutine 
        StartCoroutine(HttpPost(gachaUrl,
            formData,
            (jsonText) =>
            {
                // Json をクラスに格納
                var gachaClass = JsonUtility.FromJson<getGachaDataClass>(jsonText);
                //配列の配列を追加
                NetworkData.userData.userData.takeCards.AddRange(gachaClass.response.card_ids);
                NetworkData.userData.userData.nowGetCards.Clear();
                NetworkData.userData.userData.nowGetCards.AddRange(gachaClass.response.card_ids);
                // callback が保存するならコールバックを実行する
                if (null != _callback)
                {
                    _callback(gachaClass.status_code.ToString());
                    _callback = null;
                }
                //status code
                Debug.Log("status_code = " + gachaClass.status_code);
            }));
    }

    #endregion

    #region 名前登録

    public void SetRegistName(string name, Action<string> callback)
    {
        if (string.IsNullOrEmpty(NetworkData.uuid))
        {
            if (null != callback)
                callback("UUID が設定されていません");
            return;
        }
        if(!string.IsNullOrEmpty(NetworkData.userName))
        {
            if(null != callback)
            {
                callback("すでにuser名前は登録されています");
                return;
            }
        }
        // コールバック関数を一時的保存
        _callback = callback;
        // ユーザー名を一時的保存
        _userName = name;
        List<IMultipartFormSection> formData = new List<IMultipartFormSection>();
        // 送信添付データリストの準備
        formData.Add(new MultipartFormDataSection("uuid", NetworkData.uuid));
        // UUID dataをリストに登録
        formData.Add(new MultipartFormDataSection("name", name));
        // user nameをリストに登録


        StartCoroutine(HttpPost(NetworkData.registerURL,
            formData,
            (jsonText) =>
            {
                // Json をクラスに格納する
                var registName = JsonUtility.FromJson<RegistNameClass>(jsonText);
                //PlayerPrefs にuser name を　保存
                NetworkData.userName = _userName;

                PlayerPrefs.SetString(NetworkData.userNameKey, NetworkData.userName);
                //　callback が存在するならコールバックを実行する
                if (null != _callback)
                {
                    //　ステータスコードを返す
                    _callback(registName.status_code.ToString());
                    _callback = null;
                }
                //　ステータスコードDebug
                Debug.Log("status_code = " + registName.status_code);
            }));
    }

    #endregion

    #region SessionID得取
    
    public void GetSessionID(Action<string> callback = null)
    {
        //UUID が設定されていなければメッセージを出して終了
        if(string.IsNullOrEmpty(NetworkData.uuid))
        {
            if(null != callback)
            {
                callback("uuid is empty");
            }
            return;
        }
        _callback = callback;
        // 通信添付データリストの準備
        List<IMultipartFormSection> formData = new List<IMultipartFormSection>();
        //  UUID データをリストに登録
        formData.Add(new MultipartFormDataSection("uuid", NetworkData.uuid));
        // HTTP post Coroutine
        StartCoroutine(HttpPost(NetworkData.sessionIdURL,
            formData,
            (jsonText) =>
            {
                // json をclassに格納
                var sessionID = JsonUtility.FromJson<sessionIDClass>(jsonText); ;
                NetworkData.sessionId = sessionID.response.session_id;
                // requestHeaderの生成
                NetworkData.requestHeader = "Bearer " + NetworkData.sessionId;
                // callback が存在するならcallbackを実行する
                if (null != _callback)
                {
                    _callback(NetworkData.sessionId);
                    _callback = null;
                }
                //statusをDebugLogに出力
                Debug.Log("status_code = " + sessionID.status_code);
            }));
    }
    #endregion
}