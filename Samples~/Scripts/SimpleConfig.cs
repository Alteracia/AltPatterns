using UnityEngine;

[CreateAssetMenu(fileName = "SimpleConfig", menuName = "ScriptableObjects/SimpleConfig", order = 0)]
public class SimpleConfig : ScriptableObject
{
   private float _privateFloat = 5;
   [SerializeField] private string stringField;
   public int publicIntField;
   public ScriptableObject reference;
}
