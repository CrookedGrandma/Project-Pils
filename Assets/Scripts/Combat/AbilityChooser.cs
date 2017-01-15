using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
using LitJson;

public class AbilityChooser : MonoBehaviour {

    public List<Ability> UsableAbilities = new List<Ability>();
    public List<Ability> AbilityDatabase = new List<Ability>();
    public JsonData Abilities;

    string weapon1;
    string weapon2;

    public static string Ability1Text;
    public static string Ability2Text;
    public static int selectedAbility;

    // Use this for initialization
    void Start () {
        Abilities = JsonMapper.ToObject(File.ReadAllText(Application.dataPath + "/StreamingAssets/Abilities.json"));
        ConstructAbilityDatabase();
        FindUsables();
        weapon1 = PersistentInventoryScript.instance.itemList[UsableAbilities[0].WeaponID, 0].ToString();
        weapon2 = PersistentInventoryScript.instance.itemList[UsableAbilities[1].WeaponID, 0].ToString();
        print("Weapon 1 is " + weapon1);
        FillText();
    }

    private void ConstructAbilityDatabase() {
        for (int i = 0; i < Abilities.Count; i++) {
            AbilityDatabase.Add(new Ability((int)Abilities[i]["id"], Abilities[i]["type"].ToString(),
                                            Abilities[i]["title"].ToString(), (int)Abilities[i]["weaponid"]));
        }
    }

    private void FindUsables() {
        int equippedWeapon = PersistentInventoryScript.instance.equipmentList[0, 0];
        int equippedRanged = PersistentInventoryScript.instance.equipmentList[1, 0];
        for (int i = 0; i < AbilityDatabase.Count; i++) {
            if (AbilityDatabase[i].WeaponID == equippedWeapon) {
                UsableAbilities[0] = AbilityDatabase[i];
            }
            if (AbilityDatabase[i].WeaponID == equippedRanged) {
                UsableAbilities[1] = AbilityDatabase[i];
            }
        }
    }

    private void FillText() {
        Ability1Text = UsableAbilities[0].Title + " <color=#bbbbbb>using your</color> <color=#cccccc>" + weapon1 + "</color>";
        Ability2Text = UsableAbilities[1].Title + " <color=#bbbbbb>using your</color> <color=#cccccc>" + weapon2 + "</color>";
    }

    public void SelectAbility(int a) {
        if (a == 1) {
            Ability1Text = "<color=red>" + UsableAbilities[0].Title + " <color=#ffbbbb>using your</color> <color=#ffcccc>" + weapon1 + "</color>";
            selectedAbility = 1;
        }
        if (a == 2) {
            Ability2Text = "<color=red>" + UsableAbilities[1].Title + " <color=#ffbbbb>using your</color> <color=#ffcccc>" + weapon2 + "</color>";
            selectedAbility = 2;
        }
    }
}