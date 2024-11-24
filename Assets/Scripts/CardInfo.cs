using UnityEngine;

[CreateAssetMenu(fileName = "New Card", menuName = "Cards")]
public class FormStats : ScriptableObject
{
    //Hosts all of the following information for each form type: 
    public string cardName;
    public float damageOutput;
    public float hitChance;
    public string abilityDescription; 
    public Sprite sprite; 

}
