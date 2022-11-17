using Test.LavaProject.Farm.UiInterface;
using UnityEngine;
using UnityEngine.InputSystem;

namespace Test.LavaProject.Farm.Mechanica_Input
{
    public class TouchCheck : MonoBehaviour
    {
        [SerializeField] private GameObject _gameProcess;
        private IGameProcess _iGameProcess;

        [SerializeField] private GameObject _cameraControl;
        private ICameraControl _iCameraControl;
        private Camera _camera;

        [SerializeField] private InputActionAsset _inputActions;
        private InputActionMap _playerActionMap;
        private InputAction _movement;

        private bool _isChoiseMade;

        private void Awake()
        {
            SetLinks();
        }

        private void SetLinks()
        {
            _playerActionMap = _inputActions.FindActionMap("Player");
            _movement = _playerActionMap.FindAction("Touches");
            _movement.started += HandleMovementAction;
            _movement.Enable();
            _playerActionMap.Enable();
            _inputActions.Enable();

            SetCamera();

            _iGameProcess = SetGameProcess();
            _iGameProcess.SetTouchCheck(this);
        }

        private void SetCamera()
        {
            if (_cameraControl.TryGetComponent(out ICameraControl control))
                _iCameraControl = control;

            _camera = _iCameraControl.GetCamera();
        }

        private IGameProcess SetGameProcess()
        {
            if (_gameProcess.TryGetComponent(out IGameProcess gameProcess))  
                return gameProcess;
            else
                return null;
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
            _iGameProcess.SetSelectedCell(index);
            SetChecngeChoiseStatus(true);
        }

        private void Harvesting(int index)
        {
            _iGameProcess.Harvesting(index);
            SetChecngeChoiseStatus(true);
        }

        public void SetChecngeChoiseStatus(bool isMade)
        {
            _isChoiseMade = isMade;
        }
    }
}