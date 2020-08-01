using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UserDataClass
{
    public class UserData
    {
        /// user 手持ちのcard
        public List<int> takeCards = new List<int>();
        public List<int> nowGetCards = new List<int>();
    }

    public UserData userData = new UserData();


    /// <summary>
    /// user data の保存
    /// </summary>
    public void SaveUserData()
    {

    }

    /// <summary>
    /// user data の読み込み
    /// </summary>
    public void LoadUserData()
    {

    }

}
