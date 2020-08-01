using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

[Serializable]
public class getGachInfoClass
{
    /// <summary>
    /// card information class
    /// </summary>
    [Serializable]
    public class cardDescClass
    {
        public int id; // card id
        public string name; // card name
        public int offence; // atteck
        public int defence; // defence
        public string text; // prfeab text
    }
    //ガード情報
    public List<cardDescClass> response;
    // 通信で得取するステータス
    public int status_code;
}
