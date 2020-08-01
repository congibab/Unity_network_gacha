using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


public class ItemNumberControl : MonoBehaviour
{
    // 表示のためのscript
    [SerializeField]
    private ItemNumberView _itemNumberView = null;

    // 現在のtextの上からの番号
    private int _posIndex = -1;
    public int posIndex { get { return _posIndex; } }

    // 選択された場合に呼び出されるcallback
    private Action<int> _callback = null;

    /// <summary>
    /// card number を表示するprefab reset
    /// </summary>
    /// <param name="positionIndex"> text number</param>
    /// <param name="cardIndex"> saved card number</param>
    /// <param name="callback"> 選択されたときに呼び出されるcall back</param>
    public void Init(int positionIndex, int cardIndex, Action<int> callback)
    {
        _callback = callback;
        _posIndex = positionIndex;
        _itemNumberView.SetMessage(cardIndex);
    }

    /// <summary>
    ///　ドラッグ開始時に呼び出されるが、今回はタップとして使用する
    /// </summary>
    public void OnClickText()
    {
        if(null != _callback)
        {
            _callback(_posIndex);
        }
    }

    /// <summary>
    /// chageable page color
    /// </summary>
    /// <param name="selected"> select flag</param>
    public void SetBaseColor(bool selected)
    {
        if (selected)
            GetComponent<Image>().color = new Color(0.8f, 0.8f, 1.0f);
        else
            GetComponent<Image>().color = Color.white;

    }
}
