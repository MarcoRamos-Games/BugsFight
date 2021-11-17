using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GameMaster : MonoBehaviour
{
    public Unit selectedUnit;

    public int playerTurn = 1;

    public GameObject selectedUnitSquare;
    public Image playerIndicator;
    public Sprite player1Indicator, player2Indicator;

    public int player1Gold =100, player2Gold =100;

    public Text player1GoldText, player2GoldText;

    public ShopItem purchasedItem;
    private void Start()
    {
        GetGoldIncome(1);
    }

    public void UpdateGoldText()
    {
        player1GoldText.text = player1Gold.ToString();
        player2GoldText.text = player2Gold.ToString();
    }

    public void ResetTiles()
    {
        foreach(Tile tile in FindObjectsOfType<Tile>())
        {
            tile.Reset();
        }
    }

    void GetGoldIncome(int playerTurn)
    {
        foreach(Castle castle in FindObjectsOfType<Castle>())
        {
            if(castle.playerNumber == playerTurn)
            {
                if(playerTurn == 1)
                {
                    player1Gold += castle.goldPerTurn;
                }
                else
                {
                    player2Gold += castle.goldPerTurn;
                }              
                
            }
        }
        UpdateGoldText();
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space))
        {
            EndTurn();
        }

        if(selectedUnit != null)
        {
            selectedUnitSquare.SetActive(true);
            selectedUnitSquare.transform.position = new Vector2( selectedUnit.transform.position.x, selectedUnit.transform.position.y);
        }
        else
        {
            selectedUnitSquare.SetActive(false);
        }
    }

    private void EndTurn()
    {
        if(playerTurn == 1)
        {
            playerTurn = 2;
            playerIndicator.sprite = player2Indicator;
        }
        else if(playerTurn ==2)
        {
            playerTurn = 1;
            playerIndicator.sprite = player1Indicator;
        }

        GetGoldIncome(playerTurn);
        if(selectedUnit != null)
        {
            selectedUnit.selected = false;
            selectedUnit = null;
        }
        ResetTiles();
        foreach(Unit unit in FindObjectsOfType<Unit>())
        {
            unit.hasMoved = false;
            unit.weaponIcon.SetActive(false);
            unit.hasAttacked = false;
        }
        GetComponent<Shop>().CloseMenu();
    }
}
