using System.Collections;
using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

/// <summary>
/// Class made for online multiplayer testing purposes, not being used in the actual gameplay
/// </summary>
public class LobbyListController : MonoBehaviour
{
    public string players;
    public Text playersText;
    public GameObject canvas;

    public void AddPlayer(string _newPlayer)
    {
        players += "- " + _newPlayer + "\n"; 
    }
    public string GetPlayers()
    {
        return players;
    }
    public void SetPlayersText()
    {
        playersText.text = players;
    }
    public void SetPlayers(string _text)
    {
        players = _text;
    }
    public void HideCanvas()
    {
        canvas.SetActive(false);
    }
}
