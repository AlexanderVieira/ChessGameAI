using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HighlightClick : MonoBehaviour
{    
    void OnMouseDown(){
        
        Board._instance.TileClicked(this, null);
    }
}
