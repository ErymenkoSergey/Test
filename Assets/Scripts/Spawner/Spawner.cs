using System.Collections.Generic;
using Test.LavaProject.Farm.DI;
using Test.LavaProject.Farm.Mechanica.AI;
using Test.LavaProject.Farm.Mechanica.MainProcess.Game;
using Test.LavaProject.Farm.Mechanica_Spawner.Cells;
using UnityEngine;
using UnityEngine.AI;
using Zenject;

namespace Test.LavaProject.Farm.Mechanica_Spawner
{
    public class Spawner : MonoBehaviour
    {
        private SettingGame _settingGame;
        private GameProcess _gameProcess;

        [SerializeField] private NavMeshSurface _navMeshSurface;
        [SerializeField] private GameObject _farmerPrefab;
        [SerializeField] private Transform _startPointFarmer;

        [SerializeField] private GameObject _cellPrefab;
        [SerializeField] private Transform _fieldPoint;

        [SerializeField] private Transform _parrentPlace;

        private int _rowsCount;
        private int _columnsCount;
        private const float _DISTANCE_BETWEEN_BLOCKS = 4;

        private Dictionary<int, CellPrefab> _cells = new Dictionary<int, CellPrefab>();
        private Farmer _farmer;
        private int _currentSelectedCell;

        [Inject]
        public void Construct(SettingGame settingGame)
        {
            _settingGame = settingGame;
            _settingGame.Spawner = this;
        }

        private void OnEnable()
        {
            _gameProcess = _settingGame.GameProcess;
        }

        public void SetGameConfiguration(Vector2 vector)
        {
            _rowsCount = (int)vector.x > 10 ? 10 : (int)vector.x; //my limit, becouse game field - small =(
            _columnsCount = (int)vector.y > 10 ? 10 : (int)vector.y;

            CreateField();
            CreateFarmer();
        }

        public void CreateFarmer()
        {
            Instantiate(_farmerPrefab, _startPointFarmer).GetComponent<Farmer>().SetInfo(this);
        }

        public void RegisterFarmer(Farmer farmer)
        {
            _farmer = farmer;
            _gameProcess.SetTransformFarmer(_farmer.GetFarmerPosition());
        }

        private void CreateField()
        {
            var index = 0;

            for (int x = 0; x < _rowsCount; x++)
            {
                for (int y = 0; y < _columnsCount; y++)
                {
                    var pos = GetSpawnPosition(x, y, _DISTANCE_BETWEEN_BLOCKS);
                    CellPrefab cellPrefab = Instantiate(_cellPrefab, pos, Quaternion.identity).GetComponent<CellPrefab>();
                    SetPlaceAndData(cellPrefab, index);
                    index++;
                }
            }

            CreateNavMesh();
        }

        private void SetPlaceAndData(CellPrefab cell, int index)
        {
            cell.SetInfoCell(this, index);
            cell.transform.SetParent(_parrentPlace);
        }

        private void CreateNavMesh()
        {
            _navMeshSurface.BuildNavMesh();
        }

        private Vector3 GetSpawnPosition(int x, int y, float distance)
        {
            return _fieldPoint.position + Vector3.forward * y * distance + Vector3.right * x * distance;
        }

        public void RegisterCells(int index, CellPrefab cell)
        {
            _cells.Add(index, cell);
        }

        public void StartPlanting(int index, out Transform point)
        {
            _currentSelectedCell = index;
            point = _cells[index].GetTransform();
            _farmer.StartMoved(true, point, Harvest.PlantCrop);
        }

        public void StartDisembarkation()
        {
            _gameProcess.StartDisembarkation(_currentSelectedCell, out PlantType type);
            _cells[_currentSelectedCell].ChangeStatus(GrowthStatus.Growth);
            _cells[_currentSelectedCell].SetPlantType(type);
        }

        public Transform GetTransformFarmer()
        {
            return _farmer.transform;
        }

        public void SetStatusReady(int index)
        {
            _cells[index].ChangeStatus(GrowthStatus.Ready);
        }

        public void CheckStatusReady(int index)
        {
            _currentSelectedCell = index;
            var point = _cells[index].GetTransform();
            _farmer.StartMoved(true, point, Harvest.PickUpHarvest);
        }

        public void StartHarvesting()
        {
            _cells[_currentSelectedCell].ChangeStatus(GrowthStatus.Idle);
            _gameProcess.PickUpHarvest(_currentSelectedCell);
        }
    }
}