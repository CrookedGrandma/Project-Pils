using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
using LitJson;

public class AbilityChooser : MonoBehaviour {

    public List<Ability> UsableAbilities = new List<Ability>();
    public List<Ability> AbilityDatabase = new List<Ability>();
    public JsonData Abilities;

	// Use this for initialization
	void Start () {
        Abilities = JsonMapper.ToObject(File.ReadAllText(Application.dataPath + "/StreamingAssets/Abilities.json"));
        ConstructAbilityDatabase();
        FindUsables();
    }
	
	// Update is called once per frame
	void Update () {
	}

    private void ConstructAbilityDatabase() {
        for (int i = 0; i < Abilities.Count; i++) {
            AbilityDatabase.Add(new Ability((int)Abilities[i]["id"], Abilities[i]["type"].ToString(),
                                            Abilities[i]["title"].ToString(), (int)Abilities[i]["weaponid"]));
        }
    }

    private void FindUsables() {
        for (int i = 0; i < AbilityDatabase.Count; i++) {

        }
    }
}