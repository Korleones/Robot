using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class follow : MonoBehaviour
{
    public Transform npc;
    public RectTransform rect;
    private Vector3 screenposition;
    // Start is called before the first frame update
    void Start()
    {
        screenposition = Camera.main.WorldToScreenPoint(npc.position);
        rect.position = screenposition;
    }

    // Update is called once per frame
    void Update()
    {
        screenposition = Camera.main.WorldToScreenPoint(npc.position);
        rect.position = screenposition;
    }
}
