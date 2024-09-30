using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BoxOnOff : MonoBehaviour
{
    public BoxCollider boxcol;
   
    public void OnEneld()
    {
        boxcol.enabled = true;
    }
    public void OffAttack()
    {
        boxcol.enabled = false;
    }
   
}
