using UnityEngine;

public class GameAudioManager : MonoBehaviour
{
    [SerializeField] AudioClip _buildingPlacedClip;
    [SerializeField] GameBoard _gameBoard;

    void Start()
    {
        _gameBoard.BuildingPlaced += (int x, int y, BuildingType buildingType) 
            => PlayBuildingPlacedClip(); 
    }

    void PlayBuildingPlacedClip()
    {
        SoundManager.Singleton.PlayClip(_buildingPlacedClip);
    }
}