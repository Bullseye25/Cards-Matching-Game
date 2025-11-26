using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class CardManager : MonoBehaviour
{
    [SerializeField] private int cardID;
    [SerializeField] private Image card;

    private bool isFound = false;

    public bool IsFound { get => isFound; set => isFound = value; }
    public int CardID { get => cardID; set => cardID = value; }
    public Image Card { get => card; set => card = value; }

    IEnumerator Start()
    {
        yield return new WaitForSeconds(1.5f); // Show the cards before begining
        
        //Then proceed 
    }
}