using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;


public class GachaListCongtrol : MonoBehaviour
{
    // ガチャlistを貼り付けるコンテナー
    [SerializeField]
    private GameObject _gachaCardContainer = null;

    // card text prefab
    [SerializeField]
    private GameObject _cardListTextPrefab = null;

    // 選択中のカード番号を表示するtext
    [SerializeField]
    private Text _selectCardNumberText = null;

    // 現在選択中のindex
    private int _selectIndex = -1;
    public int selectIndex { get{ return _selectIndex; } }

    // get card number
    public int cardNumber
    {
        get
        {
            if (0 > _selectIndex) return -1;
            var userCards = NetworkData.userData.userData.takeCards;
            return userCards[_selectIndex];
        }
    }
    private List<GameObject> _cardLists = new List<GameObject>();

    /// <summary>
    /// card data list を scroll viewに表示
    /// 　list dataを先頭からの番号とdata set で出力したいときに
    /// 　   ここで使った方法をとるころがある
    /// </summary>
    public void SetUserGachaList()
    {
        // user card data
        //var userCards = NetworkData.userData.userData.takeCards;
        var userCards = NetworkData.userData.userData.nowGetCards;
        // dataを一時的に加工すて順番に処理する
        foreach (var dataSet in userCards.Select((cardData, index) => new { index, cardData }))
        {
            // スクロールリストのコンテナにカードテキストが生成される
            var gobj = Instantiate(_cardListTextPrefab, _gachaCardContainer.transform);
            //　data reset
            gobj.GetComponent<ItemNumberControl>().Init(dataSet.index, 
                dataSet.cardData, GetCardCallback);
            //生成したobjecttを保存しておく
            _cardLists.Add(gobj);
        }
    }

    /// <summary>
    /// objectの解放
    /// </summary>
    public void ReleasePrefab()
    {
        foreach(var p in _cardLists)
        {
            Destroy(p);
        }
        _cardLists.Clear();
    }

    private void GetCardCallback(int cardPosIndex)
    {
        if (0 <= _selectIndex)
            _cardLists[_selectIndex].GetComponent<ItemNumberControl>().SetBaseColor(false);
        _selectIndex = cardPosIndex;
        _cardLists[_selectIndex].GetComponent<ItemNumberControl>().SetBaseColor(false);
        _selectCardNumberText.text = cardNumber.ToString();
    }
}
