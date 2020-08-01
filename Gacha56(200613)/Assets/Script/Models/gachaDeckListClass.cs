using System;
using System.Collections.Generic;

[Serializable]
public class gachaDeckListClass
{
    /// <summary>
    /// ガチャタイプ情報
    /// </summary>
    [Serializable]
    public class DeckInfoClass
    {
        public int id; // デッキID
        public string name; // デッキ名
        public string detail;　// デッキの種類
        public bool can_loot;　// 現在の手持ち（リソース）でこのガチャが引けるかどうか（bool）
    }
    /// <summary>
    /// ガチャタイプリスト
    /// </summary>
    [Serializable]
    public class DeckClass
    {
        public List<DeckInfoClass> decks;
    }
    //ガチャタイプ
    public DeckClass response;
    //受信で取得するStause
    public int status_code;
}
