using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ForArrow : MonoBehaviour
{
    protected int checknum;

    public int ArrowChange
    {
        get { return checknum; }
        //get => checknum;
        set { checknum = ++value; }
    }
}
