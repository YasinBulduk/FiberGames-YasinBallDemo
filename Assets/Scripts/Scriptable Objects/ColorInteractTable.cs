using System;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "new ColorInteractTable", menuName = "Scriptable Objects/Colors/Create Color Interact Table")]
public class ColorInteractTable : ScriptableObject
{
    [Tooltip("Index is important. Each trigger increases index until index reached length of array.")]
    public PainterColorTable[] interactTable;
}
