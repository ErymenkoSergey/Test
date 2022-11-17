using UnityEngine;

public interface ISpawn
{
    void SetGameConfiguration(Vector2 vector);
    void CheckStatusReady(int index);
    void StartPlanting(int cell, out Transform point);
    void SetStatusReady(int index);
}