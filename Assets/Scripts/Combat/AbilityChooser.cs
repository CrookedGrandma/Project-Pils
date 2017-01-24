using System;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
using UnityEngine.UI;
using LitJson;

public class AbilityChooser : MonoBehaviour {

    public Text A1T, A2T, AHT;
    public Text H1T, H2T, H3T, H4T, H5T, H6T, H7T;
    public HealthManager healthManager;

    private Ability[] UsableAbilities = new Ability[2];
    private List<Ability> AbilityDatabase = new List<Ability>();
    private JsonData Abilities;
    private ItemDatabase database;
    private Item[] weapon = new Item[2];

    private string Ability1Text, Ability2Text, AbilityHText;
    private string Heal1Text, Heal2Text, Heal3Text, Heal4Text, Heal5Text, Heal6Text, Heal7Text;

    private int numberApJ, numberUMM, numberRad, numberCaP, numberBoP, numberNoP, numberMeP, numberHEAL;
    private int numberAmmo = -1, numberThrow = -1;
    private string secType;

    private bool started = false;
    private bool FoundPrimaryWeapon = false;
    public int selectedAbility = -1;
    public int selectedHealer = -1;

    // Use this for initialization, CALLED BY STATEHANDLER
    public void GetStarted () {
        started = true;
        Abilities = JsonMapper.ToObject(File.ReadAllText(Application.dataPath + "/StreamingAssets/Abilities.json"));
        database = GetComponent<ItemDatabase>();
        ConstructAbilityDatabase();
        FindUsables();
        GetNumberOfHealers();
        weapon[0] = database.FetchItemById(UsableAbilities[0].WeaponID);
        try {
            weapon[1] = database.FetchItemById(UsableAbilities[1].WeaponID);
            secType = weapon[1].Subtype;
        }
        catch (Exception e) { }
        GetAmmo();
        WhiteText(true);
        selectedAbility = 1;
        SelectAbility(1);
    }

    private void Update() {
        if (started) {
            //Debug {
            if (Input.GetKeyDown(KeyCode.Keypad0)) {
                SelectAbility(-1);
            }
            //}

            A1T.text = Ability1Text;
            A2T.text = Ability2Text;
            AHT.text = AbilityHText;
            H1T.text = Heal1Text;
            H2T.text = Heal2Text;
            H3T.text = Heal3Text;
            H4T.text = Heal4Text;
            H5T.text = Heal5Text;
            H6T.text = Heal6Text;
            H7T.text = Heal7Text;
        }
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
                FoundPrimaryWeapon = true;
            }
            if (AbilityDatabase[i].WeaponID == equippedRanged) {
                UsableAbilities[1] = AbilityDatabase[i];
            }
        }
        if (!FoundPrimaryWeapon) {
            UsableAbilities[0] = AbilityDatabase[0];
        }
    }

    private void GetAmmo() {
        if (secType == "ranged") {
            numberAmmo = PersistentInventoryScript.instance.returnNumberOfItems(weapon[1].Linked);
        }
        else if (secType == "projectile") {
            numberThrow = PersistentInventoryScript.instance.returnNumberOfItems(weapon[1].ID);
        }
        if (numberAmmo <= 0 && numberThrow <= 0) {
            numberAmmo = -1;
            numberThrow = -1;
        }
    }

    private void GetNumberOfHealers() {
        numberApJ = PersistentInventoryScript.instance.returnNumberOfItems(100);
        numberUMM = PersistentInventoryScript.instance.returnNumberOfItems(101);
        numberRad = PersistentInventoryScript.instance.returnNumberOfItems(102);
        numberCaP = PersistentInventoryScript.instance.returnNumberOfItems(103);
        numberBoP = PersistentInventoryScript.instance.returnNumberOfItems(104);
        numberNoP = PersistentInventoryScript.instance.returnNumberOfItems(105);
        numberMeP = PersistentInventoryScript.instance.returnNumberOfItems(106);
        numberHEAL = numberApJ + numberUMM + numberRad + numberCaP + numberBoP + numberNoP + numberMeP;
    }

    public void WhiteText(bool t) {
        GetAmmo();
        GetNumberOfHealers();
        if (t) {
            Ability1Text = UsableAbilities[0].Title + " <color=#bbbbbb>using your</color> <color=#cccccc>" + weapon[0].Title + "</color> (Damage: " + weapon[0].Damage + ")";
            if (secType == "ranged" && numberAmmo != -1) {
                Ability2Text = UsableAbilities[1].Title + " <color=#bbbbbb>using your</color> <color=#cccccc>" + weapon[1].Title + "</color> (Damage: " + weapon[1].Damage + ") (" + numberAmmo + ")";
            }
            else if (secType == "projectile" && numberThrow != -1) {
                Ability2Text = UsableAbilities[1].Title + " <color=#bbbbbb>using your</color> <color=#cccccc>" + weapon[1].Title + "</color> (Damage: " + weapon[1].Damage + ") (" + numberThrow + ")";
            }
            if (numberHEAL != 0) {
                AbilityHText = "Heal <color=#bbbbbb>with</color> <color=#cccccc>Drinks</color> <color=#bbbbbb>or</color> <color=#cccccc>Food</color>";
            }
            else {
                AbilityHText = "";
            }
        }
        else {
            Ability1Text = "";
            Ability2Text = "";
            if (numberHEAL != 0) {
                AbilityHText = "Heal <color=#bbbbbb>with</color> <color=#cccccc>Drinks</color> <color=#bbbbbb>or</color> <color=#cccccc>Food</color>";
            }
            else {
                AbilityHText = "";
            }
        }
    }

    public void WhiteHealText(bool t) {
        if (t) {
            if (numberApJ != 0) {
                Heal1Text = "Apple Juice (" + numberApJ + "), heals " + database.FetchItemById(100).Heal + "%";
            }
            if (numberUMM != 0) {
                Heal2Text = "Unprepared Microwave Meal (" + numberUMM + "), heals " + database.FetchItemById(101).Heal + "%";
            }
            if (numberRad != 0) {
                Heal3Text = "Radler (" + numberRad + "), heals " + database.FetchItemById(102).Heal + "%";
            }
            if (numberCaP != 0) {
                Heal4Text = "Canned Pils (" + numberCaP + "), heals " + database.FetchItemById(103).Heal + "%";
            }
            if (numberBoP != 0) {
                Heal5Text = "Bottled Pils (" + numberBoP + "), heals " + database.FetchItemById(104).Heal + "%";
            }
            if (numberNoP != 0) {
                Heal6Text = "Noni's Pils (" + numberNoP + "), heals " + database.FetchItemById(105).Heal + "%";
            }
            if (numberMeP != 0) {
                Heal7Text = "Mega Pils (" + numberMeP + "), heals " + database.FetchItemById(106).Heal + "%";
            }
        }
        else {
            Heal1Text = "";
            Heal2Text = "";
            Heal3Text = "";
            Heal4Text = "";
            Heal5Text = "";
            Heal6Text = "";
            Heal7Text = "";
        }
    }

    public void SelectAbility(int a) {
        WhiteText(true);
        if (a == -1) {
            selectedAbility = -1;
        }
        if (a == 1) {
            selectedAbility = 1;
            Ability1Text = "<color=red>" + UsableAbilities[0].Title + "</color> <color=#ff7777>using your</color> <color=#ff8888>" + weapon[0].Title + "</color> <color=red>(Damage: " + weapon[0].Damage + ")</color>";
        }
        if (a == 2) {
            if (secType == "ranged" || secType == "projectile") {
                if (numberAmmo != -1) {
                    Ability2Text = "<color=red>" + UsableAbilities[1].Title + "</color> <color=#ff7777>using your</color> <color=#ff8888>" + weapon[1].Title + "</color> <color=red>(Damage: " + weapon[1].Damage + ") (" + numberAmmo + ")</color>";
                }
                if (numberThrow != -1) {
                    Ability2Text = "<color=red>" + UsableAbilities[1].Title + "</color> <color=#ff7777>using your</color> <color=#ff8888>" + weapon[1].Title + "</color> <color=red>(Damage: " + weapon[1].Damage + ") (" + numberThrow + ")</color>";
                }
                selectedAbility = 2;
            }
            else {
                if (selectedAbility == 1) {
                    SelectAbility(3);
                }
                else if (selectedAbility == 3) {
                    SelectAbility(1);
                }
            }
        }
        if (a == 3) {
            selectedAbility = 3;
            if (numberHEAL != 0) {
                AbilityHText = "<color=red>Heal</color> <color=#ff7777>with</color> <color=#ff8888>Drinks</color> <color=#ff7777>or</color> <color=#ff8888>Food</color>";
            }
            else {
                SelectAbility(2);
            }
        }
    }

    public void SelectHealer(int h) {
        WhiteHealText(true);
        switch (h) {
            case (1): {
                    if (numberApJ != 0) {
                        Heal1Text = "<color=red>Apple Juice (" + numberApJ + "), heals " + database.FetchItemById(100).Heal + "%</color>";
                        selectedHealer = 1;
                    }
                    else {
                        selectedHealer = 1;
                        SelectHealer(2);
                    }
                    break;
                }
            case (2): {
                    if (numberUMM != 0) {
                        Heal2Text = "<color=red>Unprepared Microwave Meal (" + numberUMM + "), heals " + database.FetchItemById(101).Heal + "%</color>";
                        selectedHealer = 2;
                    }
                    else if (selectedHealer == 1) {
                        selectedHealer = 2;
                        SelectHealer(3);
                    }
                    else {
                        selectedHealer = 2;
                        SelectHealer(1);
                    }
                    break;
                }
            case (3): {
                    if (numberRad != 0) {
                        Heal3Text = "<color=red>Radler (" + numberRad + "), heals " + database.FetchItemById(102).Heal + "%</color>";
                        selectedHealer = 3;
                    }
                    else if (selectedHealer == 2) {
                        selectedHealer = 3;
                        SelectHealer(4);
                    }
                    else {
                        selectedHealer = 3;
                        SelectHealer(2);
                    }
                    break;
                }
            case (4): {
                    if (numberCaP != 0) {
                        Heal4Text = "<color=red>Canned Pils (" + numberCaP + "), heals " + database.FetchItemById(103).Heal + "%</color>";
                        selectedHealer = 4;
                    }
                    else if (selectedHealer == 3) {
                        selectedHealer = 4;
                        SelectHealer(5);
                    }
                    else {
                        selectedHealer = 4;
                        SelectHealer(3);
                    }
                    break;
                }
            case (5): {
                    if (numberBoP != 0) {
                        Heal5Text = "<color=red>Bottled Pils (" + numberBoP + "), heals " + database.FetchItemById(104).Heal + "%</color>";
                        selectedHealer = 5;
                    }
                    else if (selectedHealer == 4) {
                        selectedHealer = 5;
                        SelectHealer(6);
                    }
                    else {
                        selectedHealer = 5;
                        SelectHealer(4);
                    }
                    break;
                }
            case (6): {
                    if (numberNoP != 0) {
                        Heal6Text = "<color=red>Noni's Pils (" + numberNoP + "), heals " + database.FetchItemById(105).Heal + "%</color>";
                        selectedHealer = 6;
                    }
                    else if (selectedHealer == 5) {
                        selectedHealer = 6;
                        SelectHealer(7);
                    }
                    else {
                        selectedHealer = 6;
                        SelectHealer(5);
                    }
                    break;
                }
            case (7): {
                    if (numberMeP != 0) {
                        Heal7Text = "<color=red>Mega Pils (" + numberMeP + "), heals " + database.FetchItemById(106).Heal + "%</color>";
                        selectedHealer = 7;
                    }
                    else {
                        selectedHealer = 7;
                        SelectHealer(6);
                    }
                    break;
                }
            default: {
                    WhiteHealText(true);
                    selectedHealer = -1;
                    break;
                }
        }
    }

    public int GetLastDoneDamage() {
        if (selectedAbility != 3) {
            return weapon[selectedAbility - 1].Damage;
        }
        else {
            float maxHealth = healthManager.GetMaxHealth();
            switch (selectedHealer) {
                case (1): PersistentInventoryScript.instance.removeItemFromEnd(100); return Mathf.CeilToInt(maxHealth * (database.FetchItemById(100).Heal / 100f)) * -1;
                case (2): PersistentInventoryScript.instance.removeItemFromEnd(101); return Mathf.CeilToInt(maxHealth * (database.FetchItemById(101).Heal / 100f)) * -1;
                case (3): PersistentInventoryScript.instance.removeItemFromEnd(102); return Mathf.CeilToInt(maxHealth * (database.FetchItemById(102).Heal / 100f)) * -1;
                case (4): PersistentInventoryScript.instance.removeItemFromEnd(103); return Mathf.CeilToInt(maxHealth * (database.FetchItemById(103).Heal / 100f)) * -1;
                case (5): PersistentInventoryScript.instance.removeItemFromEnd(104); return Mathf.CeilToInt(maxHealth * (database.FetchItemById(104).Heal / 100f)) * -1;
                case (6): PersistentInventoryScript.instance.removeItemFromEnd(105); return Mathf.CeilToInt(maxHealth * (database.FetchItemById(105).Heal / 100f)) * -1;
                case (7): PersistentInventoryScript.instance.removeItemFromEnd(106); return Mathf.CeilToInt(maxHealth * (database.FetchItemById(106).Heal / 100f)) * -1;
                default: return 0;
            }
        }
    }

    public Item GetWeaponUsed() {
        return weapon[selectedAbility - 1];
    }
}