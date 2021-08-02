using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Knight : Piece
{
    private void Awake(){

        Movement = new KnightMovement();
    }
}
