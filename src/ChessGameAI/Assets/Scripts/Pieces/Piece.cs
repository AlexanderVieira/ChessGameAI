using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Piece : MonoBehaviour
{
    public Tile tile;
    public Movement Movement;
    public bool WasMoved;
    public bool MaxKingdom;
    virtual protected void Start(){

        if (transform.parent.name == "GoldenPieces")
        {
            MaxKingdom = true;
        }
    }
    void OnMouseDown(){
        //Debug.Log("Clicou em " + transform.name);
        InputController.Instance.TileClicked(this, transform.parent.GetComponent<Player>());
    }
}
