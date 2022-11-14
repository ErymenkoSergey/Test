using Test.LavaProject.Farm.DI;
using Test.LavaProject.Farm.Mechanica.MainProcess.Game;
using Test.LavaProject.Farm.UiInterface;
using UnityEngine;
using UnityEngine.InputSystem;
using Zenject;

namespace Test.LavaProject.Farm.Mechanica_Input
{
    public class TouchCheck : MonoBehaviour
    {
        private SettingGame _settingGame;
        private GameProcess _gameProcess;
        private Camera _camera;

        [SerializeField] private InputActionAsset _inputActions;
        private InputActionMap _playerActionMap;
        private InputAction _movement;

        private bool _isChoiseMade;

        [Inject]
        public void Construct(SettingGame settingGame)
        {
            _settingGame = settingGame;
        }

        private void Awake()
        {
            _playerActionMap = _inputActions.FindActionMap("Player");
            _movement = _playerActionMap.FindAction("Touches");
            _movement.started += HandleMovementAction;
            _movement.Enable();
            _playerActionMap.Enable();
            _inputActions.Enable();

            _gameProcess = _settingGame.GameProcess;
            _camera = _settingGame.CameraControl.GetCamera();

            _gameProcess.SetTouchCheck(this);
        }

        private void HandleMovementAction(InputAction.CallbackContext Context)
        {
            ProcessInput();
        }

        private void ProcessInput()
        {
            if (_isChoiseMade)
                return;

            RaycastHit ray;

            Ray rayCast = _camera.ScreenPointToRay(Mouse.current.position.ReadValue());

            if (Physics.Raycast(rayCast, out ray))
            {
                Transform ClickObject = ray.transform;

                if (ClickObject.TryGetComponent(out IClickable click))
                {
                    click.Click(out int id, out GrowthStatus component, out PlantType type);

                    if (type == PlantType.Tree)
                        return;

                    if (component == GrowthStatus.Idle || component == GrowthStatus.None)
                        SetSelectedCell(id);
                    if (component == GrowthStatus.Ready)
                        Harvesting(id);
                    if (component == GrowthStatus.Growth)
                        return;
                }
            }
        }

        private void SetSelectedCell(int index)
        {
            _gameProcess.SetSelectedCell(index);
            SetChecngeChoiseStatus(true);
        }

        private void Harvesting(int index)
        {
            _gameProcess.Harvesting(index);
            SetChecngeChoiseStatus(true);
        }

        public void SetChecngeChoiseStatus(bool isMade)
        {
            _isChoiseMade = isMade;
        }
    }
}