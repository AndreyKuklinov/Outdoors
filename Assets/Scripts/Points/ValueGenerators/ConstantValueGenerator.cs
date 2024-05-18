using UnityEngine;

public class ConstantValueGenerator : StagedValueGenerator<int>
 {
    [SerializeField] private int _value;

     public override int GetValueAtStage(int stageNumber)
     {
         return _value;
     }
 }