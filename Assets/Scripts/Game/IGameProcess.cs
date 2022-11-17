using Test.LavaProject.Farm.Mechanica.Score;
using Test.LavaProject.Farm.Mechanica_Input;
using UnityEngine;

public interface IGameProcess
{
    void SetTransformFarmer(Transform farmer);
    void StartDisembarkation(int cell, out PlantType type);
    void PickUpHarvest(int currentCell);
    void SetTouchCheck(TouchCheck touch);
    void SetSelectedCell(int index);
    void Harvesting(int cell);
    void StartPlanting(PlantType type);
    void CancelPlanting();
    Texture2D GetIconPlant(PlantType plant);
    void SetScoreSystem(ScoreSystem system);
}
