using Test.LavaProject.Farm.DI;
using Test.LavaProject.Farm.Mechanica_Spawner.Cells.Plants;
using Test.LavaProject.Farm.Mechanica_UI;
using UnityEngine;
using Zenject;

namespace Test.LavaProject.Farm.Mechanica_Spawner.Planting
{
    public class PlantingSystem : MonoBehaviour
    {
        private SettingGame _settings;
        private UIPresentor _presentor;

        [SerializeField] private GameObject _prefabCarrotBed;
        [SerializeField] private GameObject _prefabGrassBed;
        [SerializeField] private GameObject _prefabTreeBed;

        [SerializeField] private Transform _parrentPlace;

        private Plant[] _plants;

        [Inject]
        public void Construct(SettingGame settingGame)
        {
            _settings = settingGame;
            _settings.PlantingSystem = this;
        }

        private void OnEnable()
        {
            _presentor = _settings.UIPresentor;
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
            tile.SetDataPlant(plant, time, experience, _presentor, selectedCell);
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