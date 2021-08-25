using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HighlightClick : MonoBehaviour
{
    public Tile Tile;
    public AvailableMove AvailableMove;        
    void OnMouseDown(){
        
        InputController.Instance.TileClicked(this, null);
    }
}
