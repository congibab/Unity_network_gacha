﻿using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GameControlManager : MonoBehaviour
{
    // 情報テキスト
    [SerializeField]
    private Text _infomationText = null;

    [SerializeField]
    private InputField _nameInputField;

    // 通信制御 scrpit
    [SerializeField]
    private NetweworkControl _networkControl = null;

    // text information controller script
    [SerializeField]
    private DeckInfoControl _deckInfoControl = null;

    // 
    [SerializeField]
    private GachaListCongtrol _gachaListCongtrol = null;

    //ガチャボタンの有効、無効を設定するための取得
    [SerializeField]
    private Button _gachaButton = null;

    //card detail information
    [SerializeField]
    private Text _cardInofText = null;

    //gacha image
    [SerializeField]
    private Image _gachaImage = null;

    //初期化関数

    private void Update()
    {
        //page変更
        ObservPage();
    }

    #region セッションの得取(通信開始)
    public  void OnGetSessionId()
    {
        _networkControl.GetSessionID((str) => _infomationText.text = str);
    }

    #endregion


    /// <summary>
    /// UUID 取得
    /// </summary>
    public void OnGetUUID()
    {
        _networkControl.OnGetUuid((str) => _infomationText.text = str);
    }

    public void OnSetName()
    {
        _networkControl.SetRegistName(_nameInputField.text, (str) =>
        {
           _infomationText.text = str;
        });
    }

    public void OnResetUUID()
    {
        PlayerPrefs.SetString(NetworkData.uuidKey, "");
        PlayerPrefs.SetString(NetworkData.userNameKey, "");
        NetworkData.uuid = "";
        NetworkData.userName = "";
    }

    #region ガチャ関連

    private void ObservPage()
    {
        if(_deckInfoControl.IspageChange)
        {
            //get page information 
            var deckInfo = NetworkData.deckLists.response.decks[_deckInfoControl.page];
            //テキストが引けるがどうかのflogをそのままButtonの効無効に反映
            if(null != _gachaButton)
            {
                _gachaButton.interactable = deckInfo.can_loot;
            }
        }
    }
    #endregion

    public void OnDeckInfoList()
    {
        _networkControl.GetGachaDeckList((str) =>
        {
            _infomationText.text = str;
            // all page numbers is setting
            if(null != NetworkData.deckLists)
            {
                _deckInfoControl.pageMax = NetworkData.deckLists.response.decks.Count;
            }
        });
    }

    public void OnGetGachaCard()
    {
        if(null == NetworkData.deckLists)
        {
            _infomationText.text = "carrd list is not loaded.";
            return;
        }

        // 指定パージのガチャ用id得取
        var gachaIndex = NetworkData.deckLists.response.decks[_deckInfoControl.page].id;

        // ガチャを引く
        _networkControl.GetGacha(gachaIndex, (str) => 
        {
            _infomationText.text = str;
            _gachaListCongtrol.SetUserGachaList();
        });
    }

    /// <summary>
    /// カード情報を得取する
    /// </summary>
    public void OnCardInfo()
    {
        if (0 > _gachaListCongtrol.selectIndex)
            return;
        _networkControl.GetcardInfo(_gachaListCongtrol.cardNumber, (str) =>
        {
            // Json class
            var cardInfoClass = JsonUtility.FromJson<getGachInfoClass>(str);
            var res = cardInfoClass.response[0];
            string msg = "ID" + res.id.ToString() + Environment.NewLine;
            msg += "名前：" + res.name + Environment.NewLine;
            msg += "攻撃力" + res.offence.ToString() + Environment.NewLine;
            msg += "防御力" + res.defence.ToString() + Environment.NewLine;
            msg += "内容" + res.text;
            //カードの表示内容をテキストで表示
            _cardInofText.text = msg;
            
        });
    }

    /// <summary>
    /// 選択したカードのイメージを表示する
    /// </summary>
    public void OnGetImage()
    {
        if (0 > _gachaListCongtrol.selectIndex)
            return;
        _networkControl.GetGachaImage(_gachaListCongtrol.cardNumber, (str) =>
        {   //ok の文字を情報window表示
            _cardInofText.text = str;
            //texture 生成
            Texture2D texture = new Texture2D(1, 1);
            
            texture.LoadImage(NetworkData.imageData);
            //image view
            _gachaImage.sprite = Sprite.Create(texture, new Rect(0, 0, texture.width, texture.height), Vector2.zero);
            _gachaImage.gameObject.SetActive(true);
        });
    }

}
