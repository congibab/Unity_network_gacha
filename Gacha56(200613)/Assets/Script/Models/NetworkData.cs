using System.Collections;
using System.Collections.Generic;
using UnityEngine;

static public class NetworkData
{
    //　基本んいなる URL
    static public string NetworkBaseURL { get; } = "http://www.gjmj.net/loot_box";
    // UUIDを保存
    static public string uuid = "";
    static public string uuidKey = "uuid";
    //登録したユーザー名
    static public string userName = "";
    static public string userNameKey = "username";
    // 保存する Session ID
    static public string sessionId = "";
    // request Header 文字列
    static public string requestHeader = "";
    //ガチャデッキ情報リスト
    static public gachaDeckListClass deckLists = null;
    //user card data
    static public UserDataClass userData = new UserDataClass();
    //サブコマンドリクエスト用の enum
    public enum SubCommandType
    {
        UUID = 0,
        REGISTER,
        SESSION_ID,
        GACHA_DECK_INFO_LIST,
        GACHA
    }
    //　サブコンどリスト
    static public string[] SubCommand { get; } =
    {
        "/uuid",
        "/register",
        "/session/get",
        "/loot_box/list",
        "/loot_box/draw/"
    };
    // UUID　のエスト用URL
    static public string uuidURL { get; } = NetworkBaseURL + SubCommand[(int)SubCommandType.UUID];
    static public string registerURL { get; } = NetworkBaseURL + SubCommand[(int)SubCommandType.REGISTER];
    static public string sessionIdURL { get; } = NetworkBaseURL + SubCommand[(int)SubCommandType.SESSION_ID];
    static public string gachaDeckInfoListURL { get; } = NetworkBaseURL + SubCommand[(int)SubCommandType.GACHA_DECK_INFO_LIST];
    static public string getGachaURL { get; } = NetworkBaseURL + SubCommand[(int)SubCommandType.GACHA];
}
