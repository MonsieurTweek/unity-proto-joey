using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "New SkinData", menuName = "Skin Data", order = 51)]
public class SkinData : ScriptableObject
{

    [SerializeField]
    private string skinName;
    [SerializeField]
    private Sprite preview;
    [SerializeField]
    private PlayerSkinController model;
    [SerializeField]
    private int cost;

    public string SkinName { get => skinName; set => skinName = value; }
    public Sprite Preview { get => preview; set => preview = value; }
    public PlayerSkinController Model { get => model; set => model = value; }
    public int Cost { get => cost; set => cost = value; }
}
