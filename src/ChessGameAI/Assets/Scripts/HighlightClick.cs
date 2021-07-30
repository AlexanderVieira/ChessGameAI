using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HighlightClick : MonoBehaviour
{
    public Tile Tile;        
    void OnMouseDown(){
        
        Board._instance.TileClicked(this, null);
    }
}
