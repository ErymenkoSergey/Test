using System;
using UnityEngine;

namespace Test.LavaProject.Farm.Data
{
    [CreateAssetMenu(fileName = "GameSetting", menuName = "ScriptableObject/GameSetting")]
    public class GameConfiguration : ScriptableObject
    {
        [Header("Game Field")]
        [SerializeField] private Vector2 _cellSize;
        public Vector2 CellSize => _cellSize;

        [Header("Plants")]
        [SerializeField] private Plant[] _plants;
        public Plant[] Plants => _plants;
    }
}

[Serializable]
public struct Plant
{
    public PlantType PlantType;
    public float GrowthTime;
    public int Experience;
    public Texture2D UIIcon;
}

public enum PlantType
{
    None = 0,
    Grass = 1,
    Carrot = 2,
    Tree = 3
}

public enum TypeScoreText
{
    None = 0,
    Experience = 1,
    Carrots = 2
}

public enum GrowthStatus
{
    None = 0,
    Idle = 1,
    Growth = 2,
    Ready = 3
}

public enum Harvest
{
    None = 0,
    PlantCrop = 1,
    PickUpHarvest = 2
}