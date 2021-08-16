using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Piece : MonoBehaviour
{
    public Tile tile;
    public Movement Movement;
    public bool WasMoved;
    void OnMouseDown(){
        //Debug.Log("Clicou em " + transform.name);
        InputController.Instance.TileClicked(this, transform.parent.GetComponent<Player>());
    }
}
