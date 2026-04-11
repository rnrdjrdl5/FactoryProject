using UnityEngine;

public class PlayerView : MonoBehaviour
{
    public PixemRuntimeCharacter Character => character;
    
    [SerializeField] PixemRuntimeCharacter character;
}
