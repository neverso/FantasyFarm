using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using Const;
using System.Collections.Generic;

/**
 * ScrollViewのContentにセットされる。
 * Field変数のprefabには、content下にテーブルのレコード数分追加される
 * MonsterBookItemが指定される
 */
public class MonsterScrollList : MonoBehaviour {

	// MonsterBookItem
	[SerializeField]
	RectTransform prefab = null;
	
	// Use this for initialization
	void Start () {
		dispAllMonsters (selectAllMonsters());
	}
	
	// Update is called once per frame
	void Update () {
		
	}
	
	void dispAllMonsters (List<Entity.Monsters> monsters)
	{
		foreach(Entity.Monsters monster in monsters) {
			var item = GameObject.Instantiate(prefab) as RectTransform;
			item.SetParent(transform, false);
			if (monster.getIsMine() == 0) {
				item.GetComponent<Button>().enabled = false;
				item.GetComponentInChildren<Text>().color = new Color(0f, 0f, 0f, 0.5f);
			}
			var text = item.GetComponentInChildren<Text>();
			text.text = monster.getDispname();
		}
	}
	
	List<Entity.Monsters> selectAllMonsters ()
	{
		Database.MonstersTable mmt = new Database.MonstersTable ();
		List<Entity.Monsters> entities = mmt.selectAllMosters ();
		return entities;
	}
}
