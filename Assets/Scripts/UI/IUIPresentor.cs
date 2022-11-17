using Test.LavaProject.Farm.Mechanica.Score;
using Test.LavaProject.Farm.Mechanica_Spawner.Cells.Plants;
using Test.LavaProject.Farm.Mechanica_UI;
using UnityEngine;

public interface IUIPresentor
{
    void SetScoreSystem(ScoreSystem system);
    void ReadinessCheck(int cell);
    void RegisterPlant(PlantTile tile);
    void FinishedFlowerBeds(int cell, int experience, PlantTile tile);
    void UnRegisterPlant(PlantTile tile, PlantType _currentPlantType, int _carrotCount);
}
