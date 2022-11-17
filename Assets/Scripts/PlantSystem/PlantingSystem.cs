using Test.LavaProject.Farm.Mechanica_Spawner.Cells.Plants;
using UnityEngine;

namespace Test.LavaProject.Farm.Mechanica_Spawner.Planting
{
    public class PlantingSystem : MonoBehaviour, IPlantingSystem
    {
        [SerializeField] private GameObject _presentor;
        private IUIPresentor _IUIPresentor;

        [SerializeField] private GameObject _prefabCarrotBed;
        [SerializeField] private GameObject _prefabGrassBed;
        [SerializeField] private GameObject _prefabTreeBed;

        [SerializeField] private Transform _parrentPlace;

        private Plant[] _plants;

        private void Start()
        {
            if (_presentor.TryGetComponent(out IUIPresentor presentor))
                _IUIPresentor = presentor;
        }

        public void SetData(Plant[] plant)
        {
            _plants = plant;
        }

        public void CreateNewPlant(PlantType plant, Transform positionPalnt, int selectedCell)
        {
            PlantTile tileCarrots = Instantiate(GetCurrentPrefab(plant), positionPalnt).GetComponent<PlantTile>();
            SetPlaceAndTime(tileCarrots, plant, selectedCell);
        }

        private GameObject GetCurrentPrefab(PlantType plant)
        {
            if (plant == PlantType.Carrot)
                return _prefabCarrotBed;
            if (plant == PlantType.Grass)
                return _prefabGrassBed;
            if (plant == PlantType.Tree)
                return _prefabTreeBed;
            else
                return null;
        }

        private void SetPlaceAndTime(PlantTile tile, PlantType plant, int selectedCell)
        {
            GetExperience(plant, out int experience, out float time);
            tile.SetDataPlant(plant, time, experience, _IUIPresentor, selectedCell);
            tile.transform.SetParent(_parrentPlace);
        }

        private void GetExperience(PlantType plant, out int Experience, out float time)
        {
            var _Experience = 0;
            var _time = 0f;

            foreach (var pla in _plants)
            {
                if (pla.PlantType == plant)
                {
                    _Experience = pla.Experience;
                    _time = pla.GrowthTime;
                }
            }

            Experience = _Experience;
            time = _time;
        }
    }
}