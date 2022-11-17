using UnityEngine;

public interface IPlantingSystem
{
    void SetData(Plant[] plants);
    void CreateNewPlant(PlantType type, Transform pos, int cell);
}
