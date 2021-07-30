using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Piece : MonoBehaviour
{
    public Tile tile;
    public Movement Movement;
    
    void OnMouseDown(){
        //Debug.Log("Clicou em " + transform.name);
        Board._instance.TileClicked(this, transform.parent.GetComponent<Player>());
    }
}
