using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Piece : MonoBehaviour
{
    
    [HideInInspector]
    public Movement Movement;    
    public Tile tile;
    public bool WasMoved;
    public bool MaxKingdom;
    public List<Piece> Kingdom;
    virtual protected void Start(){

        if (transform.parent.name == "GoldenPieces")
        {
            Kingdom = Board.Instance.GoldenPieces;
            MaxKingdom = true;
        }else
        {
            Kingdom = Board.Instance.GreenPieces;
        }
    }
    public virtual AffectedPiece CreateAffected(){

        return new AffectedPiece();
    }
    void OnMouseDown(){
        
        InputController.Instance.TileClicked(this, transform.parent.GetComponent<Player>());
    }
}
