using System;

[Serializable]
public class UuidDataClass
{
    /// <summary>
    /// UUID保存Class
    /// </summary>
    [Serializable]
    public class UUIDClass
    {
        public string uuid;
    }

    // response json 解析Data
    public UUIDClass response;
    //受信状態ステータス
    public int status_code;
    
 }
