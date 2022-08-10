using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class ForDirection : MonoBehaviour
{
    
    
    public bool CheckDirection(Vector2 vec, PointerEventData eventData)
    {
        Vector2 delta = eventData.delta.normalized;
        //print("Vector2.Dot(delta, vec)ดย" + Vector2.Dot(delta, vec));
        if (Vector2.Dot(delta, vec) >= 0)
        {
            //ArrowChangeCheck(eventData);
            return true;
        }
        return false;
        //print($"Vec {vec} | delta | {delta}");
    }
   
}
