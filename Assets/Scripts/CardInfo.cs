using UnityEngine;

[CreateAssetMenu(fileName = "New Card", menuName = "Cards")]
public class FormStats : ScriptableObject
{
    //Hosts all of the following information for each form type: 
    public string cardName;
    public float damageOutput;
    public float hitPoints;
    public string abilityOne; 
    public string abilityTwo;
    public Sprite sprite; 

}
