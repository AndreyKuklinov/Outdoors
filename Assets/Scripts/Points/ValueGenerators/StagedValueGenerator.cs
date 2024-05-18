using UnityEngine;

public abstract class StagedValueGenerator<T> : MonoBehaviour
 {
     public abstract T GetValueAtStage(int stageNumber);
 }