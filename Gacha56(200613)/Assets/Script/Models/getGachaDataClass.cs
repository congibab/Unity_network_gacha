using System;
using System.Collections.Generic;
public class getGachaDataClass
{
    /// <summary>
    /// 取得ガチャ card status_code
    /// </summary>
    [Serializable]
    public class GachData
    {
        //
        public List<int> card_ids;
    }
    //取得したcard data
    public GachData response;
    //通信で取得するstatus
    public int status_code;
}
