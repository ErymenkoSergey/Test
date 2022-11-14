using Test.LavaProject.Farm.UiInterface;
using UnityEngine;

namespace Test.LavaProject.Farm.Mechanica_Spawner.Cells
{
    public class CellPrefab : MonoBehaviour, IClickable
    {
        private Spawner _spawner;
        private int _indexCell;
        private GrowthStatus _currentStatus;
        private PlantType _currentType;

        public void SetInfoCell(Spawner spawner, int index)
        {
            _indexCell = index;
            _spawner = spawner;

            SetRegister();
            ChangeStatus(GrowthStatus.Idle);
        }

        private void SetRegister()
        {
            _spawner.RegisterCells(_indexCell, this);
        }

        public int GetIndexCell()
        {
            return _indexCell;
        }

        public void Click(out int index, out GrowthStatus currentStatus, out PlantType type)
        {
            currentStatus = _currentStatus;
            index = _indexCell;
            type = _currentType;
        }

        public Transform GetTransform()
        {
            return transform;
        }

        public void ChangeStatus(GrowthStatus newStatus)
        {
            _currentStatus = newStatus;
        }

        public void SetPlantType(PlantType plant)
        {
            _currentType = plant;
        }

        public GrowthStatus GetGrowthStatus()
        {
            return _currentStatus;
        }
    }
}