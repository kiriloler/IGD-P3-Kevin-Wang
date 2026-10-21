using System.Collections.Generic;
using System;
using UnityEngine;

public class TriggerEventHook : MonoBehaviour
{
    public Action<Collider2D> OnTriggerEnter;
    public Action<Collider2D> OnTriggerStay;
    public Action<Collider2D> OnTriggerExit;
    public List<string> TagList = new List<string>();

    private void OnTriggerEnter2D(Collider2D collision)
    {
        foreach(string  tag in TagList)
        {
            if (collision.CompareTag(tag))
            {
                OnTriggerEnter?.Invoke(collision);
                return;
            }
        }
    }
    private void OnTriggerExit2D(Collider2D collision)
    {
        foreach(string  tag in TagList)
        {
            if (collision.CompareTag(tag))
            {
                OnTriggerExit?.Invoke(collision);
                return;
            }
        }
    }
    private void OnTriggerStay2D(Collider2D collision)
    {
        foreach(string tag in TagList)
        {
            if (collision.CompareTag(tag))
            {
                OnTriggerStay?.Invoke(collision);
                return;
            }
        }
    }
}
