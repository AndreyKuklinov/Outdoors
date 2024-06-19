using UnityEngine;

public class GameAudioManager : MonoBehaviour
{
    [SerializeField] AudioClip _buildingPlacedClip;
    [SerializeField] AudioClip _tileExploredClip;
    [SerializeField] GameBoard _gameBoard;
    [SerializeField] TileExplorer _tileExplorer;

    void Start()
    {
        _gameBoard.BuildingPlaced += (int x, int y, BuildingType buildingType) 
            => PlayBuildingPlacedClip(); 

        _tileExplorer.TileExplored += (TileType tileType) => PlayTileExploredClip();
    }

    void PlayBuildingPlacedClip()
    {
        SoundManager.Singleton.PlayClip(_buildingPlacedClip);
    }

    void PlayTileExploredClip()
    {
        SoundManager.Singleton.PlayClip(_tileExploredClip);
    }
}