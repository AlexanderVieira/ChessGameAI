using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Piece : MonoBehaviour
{
    public Tile tile;

    void Start(){
        Board._instance.AddPiece(transform.parent.name, this);
    }
    void OnMouseDown(){
        Debug.Log("Clicou em "+transform.name);
    }
}
