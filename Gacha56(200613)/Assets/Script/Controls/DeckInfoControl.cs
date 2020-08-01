using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class DeckInfoControl : MonoBehaviour
{
    // page return button
    [SerializeField]
    private Button _prevButton = null;

    // page next button
    [SerializeField]
    private Button _nextButton = null;

    // page number view
    [SerializeField]
    private Text _pageText = null;

    // information text
    [SerializeField]
    private Text _infoText = null;

    private int _pageMax = 1;
    public int pageMax
    {
        get { return _pageMax; }
        set { _pageMax = value; SetButtonStatus(); }
    }

    //　現在のpage
    private int _page = 0;
    public int page { get { return _page; } }

    //　page切り替え通知flag
    private bool _isPageChange = false;
    public bool IspageChange
    {
        get
        {
            var stat = _isPageChange;
            _isPageChange = false;
            return stat;
        }
    }

    void Start()
    {
        SetButtonStatus();
    }

    /// <summary>
    /// input Prev Button
    /// </summary>
    public void OnPrevButton()
    {
        _page--;
        _isPageChange = true;
        SetButtonStatus();
    }

    /// <summary>
    /// input next button 
    /// </summary>
    public void OnNextButton()
    {
        _page++;
        _isPageChange = true;
        SetButtonStatus();
    }


    /// <summary>
    /// Button_config 
    /// </summary>
    private void SetButtonStatus()
    {
        //return button pon, off
        _prevButton.interactable = 0 != _page;
        //進button pon, off
        _nextButton.interactable = _pageMax - 1 != _page;
        //現在のpage number view
        _pageText.text = _page.ToString();
        // 情報が存在する場合は表示
        if (null != NetworkData.deckLists)
        {
            // page information view
            _infoText.text = GetInfoMessage(_page);
        }
    }

    /// <summary>
    /// 指定したpageの情報を得取
    /// </summary>
    /// <param name="index">page number</param>
    /// <returns></returns>
    private string GetInfoMessage(int index)
    {
        //dateを短い表記で得取できるように変数を用意する
        var deckInfo = NetworkData.deckLists.response.decks[index];
        var msg = "";
        msg = "ID = " + deckInfo.id.ToString() + Environment.NewLine;
        msg += "名前 ＝" + deckInfo.name + Environment.NewLine;
        msg += "説明 ＝ " + deckInfo.detail + Environment.NewLine;
        msg += "引けるか？ ＝ " + deckInfo.can_loot;
        return msg;
    }
}
