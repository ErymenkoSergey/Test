using UnityEngine;
using UnityEngine.UI;
using Zenject;
using TMPro;
using Test.LavaProject.Farm.Mechanica.MainProcess.Game;
using Test.LavaProject.Farm.DI;

namespace Test.LavaProject.Farm.UI
{
    public class UIManager : MonoBehaviour
    {
        private SettingGame _settings;
        private GameProcess _gameProcess;

        [SerializeField] private GameObject _choicePanel;

        [SerializeField] private TextMeshProUGUI _textExperience;
        [SerializeField] private TextMeshProUGUI _textCarrots;

        [SerializeField] private Button _closeChoicePanelButton;

        [SerializeField] private Button _choiceGrassButton;
        [SerializeField] private Button _choiceCarrotsButton;
        [SerializeField] private Button _choiceTreeButton;

        [SerializeField] private RawImage _grassImage;
        [SerializeField] private RawImage _carrotsImage;
        [SerializeField] private RawImage _treeImage;

        [Inject]
        public void Construct(SettingGame settingGame)
        {
            _settings = settingGame;
            _settings.UIManager = this;
        }

        private void OnEnable()
        {
            _closeChoicePanelButton.onClick.AddListener(CloseChoicePanel);

            _choiceGrassButton.onClick.AddListener(() => ChoicePlant(PlantType.Grass));
            _choiceCarrotsButton.onClick.AddListener(() => ChoicePlant(PlantType.Carrot));
            _choiceTreeButton.onClick.AddListener(() => ChoicePlant(PlantType.Tree));

            _gameProcess = _settings.GameProcess;
        }

        private void Start()
        {
            SetImageIcons();
        }

        private void OnDisable()
        {
            _closeChoicePanelButton.onClick.RemoveListener(CloseChoicePanel);

            _choiceGrassButton.onClick.RemoveListener(() => ChoicePlant(PlantType.Grass));
            _choiceCarrotsButton.onClick.RemoveListener(() => ChoicePlant(PlantType.Carrot));
            _choiceTreeButton.onClick.RemoveListener(() => ChoicePlant(PlantType.Tree));
        }

        public void SetTextExperience(string countExperience)
        {
            _textExperience.text = $"Experience: {countExperience}";
        }

        public void SetTextCarrots(string countCarrots)
        {
            _textCarrots.text = $"Carrots: {countCarrots}";
        }

        private void ChoicePlant(PlantType type)
        {
            OpenChoicePanel(false);
            _gameProcess.StartPlanting(type);
        }

        public void OpenChoicePanel(bool isOpen)
        {
            _choicePanel.SetActive(isOpen);
        }

        private void CloseChoicePanel()
        {
            OpenChoicePanel(false);
            _gameProcess.CancelPlanting();
        }

        private void SetImageIcons()
        {
            GetImageUI imageUI;
            imageUI = _gameProcess.GetIconPlant;

            _grassImage.texture = imageUI(PlantType.Grass);
            _carrotsImage.texture = imageUI(PlantType.Carrot);
            _treeImage.texture = imageUI(PlantType.Tree);
        }
    }
}