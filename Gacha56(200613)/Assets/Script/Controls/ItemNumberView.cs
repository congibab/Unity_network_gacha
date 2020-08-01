using UnityEngine;
using UnityEngine.UI;

public class ItemNumberView : MonoBehaviour
{

    /// <summary>
    /// card number view
    /// </summary>
    /// <param name="cardIndex"></param>
    public void SetMessage(int cardIndex)
    {
        GetComponent<Text>().text = cardIndex.ToString();
    }
}
