using UnityEngine;
using Zenject;
using DG.Tweening;
using System.Collections;
using Test.LavaProject.Farm.DI;

namespace Test.LavaProject.Farm.Mechanica
{
    [RequireComponent(typeof(Camera))]
    public class CameraControl : MonoBehaviour
    {
        [SerializeField] private Camera _camera;

        private SettingGame _settings;

        private Transform _player;

        private Vector3 _defaultPosition;
        private Vector3 _currentPosition;
        private Vector3 _startPosition;

        private bool _isPlayerEnable;
        private float _timeFlyToGarden = 2f;

        private Vector3 _cameraApproachPosition = new Vector3(1.5f, -1, 1.5f);

        [Inject]
        public void Construct(SettingGame settingGame)
        {
            _settings = settingGame;
            _settings.CameraControl = this;
        }

        public void SetGameObject(Transform gameObject)
        {
            _player = gameObject;
            SetDefaultPosition();
        }

        private void SetDefaultPosition()
        {
            _defaultPosition = transform.position - _player.position;
            SetIsPlayerEnable(true);
        }

        private void Update()
        {
            if (!_isPlayerEnable)
                return;

            _currentPosition = transform.position = _player.position + _defaultPosition;
        }

        public Camera GetCamera()
        {
            return _camera;
        }

        public void CameraZoomCalculation(Vector3 posCam)
        {
            Vector3 endPos = posCam - _cameraApproachPosition;
            StartCoroutine(FlyToGarden(endPos));
        }

        private IEnumerator FlyToGarden(Vector3 GardenPoint)
        {
            SetIsPlayerEnable(false);
            transform.DOMove(GardenPoint, _timeFlyToGarden);
            yield return new WaitForSeconds(_timeFlyToGarden);
            SetIsPlayerEnable(true);
        }

        private void SetIsPlayerEnable(bool isEnable)
        {
            _isPlayerEnable = isEnable;
        }
    }
}