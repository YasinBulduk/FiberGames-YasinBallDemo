using System;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "new PainterColorTable", menuName = "Scriptable Objects/Colors/Create Painter Color Table")]
public class PainterColorTable : ScriptableObject
{
    [Tooltip("Empty inMaterial means. Whatever color comes, paints self color. outMaterial loses its importance.\nOtherwise will be dyed to the outMaterial if appropriate.")]
    public Material inMaterial;
    public Material selfMaterial;
    public Material outMaterial;
}
