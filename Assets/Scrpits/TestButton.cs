using UnityEngine;
using System.Collections;
using Mono.Data.Sqlite;

public class TestButton : MonoBehaviour {
	string  name = null;
	string  mail = null;
	string 	path = null;

	// Use this for initialization
	void Start () {
	
	}

	void OnClick () {
		Debug.Log("Button Clicked!");
		// 数据库文件路径
		//string appDBPath = Application.persistentDataPath + "/test.db";
		string appDBPath = Application.dataPath  + "/test.db";
		
		DbAccess db = new DbAccess(@"Data Source=" + appDBPath);
		
		path = appDBPath;
		
		// 创建test表
		db.CreateTable("test",new string[]{"name","qq","mail","www"}, new string[]{"text","text","text","text"});
		// 插入数据,插入字符串加上'' 不然会报错
		db.InsertInto("test", new string[]{ "'哈哈哈'","'1213213213'","'123214214212@163.com'","'www.jianshu.com'"   });
		db.InsertInto("test", new string[]{ "'嘿嘿嘿'","'1241124214'","'1232455666@gmail.com'","'www.csdn.com'"   });
		db.InsertInto("test", new string[]{ "'嘻嘻嘻'","'1235667767'","'1090824108@gmail.com'","'www.manew.com'"   });
		
		// 删掉数据
		db.Delete("test",new string[]{"mail","mail"}, new string[]{"'123214214212@163.com'","'1232455666@gmail.com'"});
		
		// 查询数据
		using (SqliteDataReader sqReader = db.SelectWhere("test",new string[]{"name","mail"},new string[]{"qq"},new string[]{"="},new string[]{"1235667767"})) {
			while (sqReader.Read()) { 	
				//目前中文无法显示
				Debug.Log("name:" + sqReader.GetString(sqReader.GetOrdinal("name")));
				Debug.Log("mail:" + sqReader.GetString(sqReader.GetOrdinal("mail")));
				name = sqReader.GetString(sqReader.GetOrdinal("name"));
				mail = sqReader.GetString(sqReader.GetOrdinal("mail"));
			} 
			sqReader.Close();
		}
		
		db.CloseSqlConnection();
	}

	// Raises the GUI event
	void OnGUI() {
		if(name != null) {
			GUILayout.Label(name);
		}
		if(mail != null) {
			GUILayout.Label(mail);
		}
		if(path != null) {
			GUILayout.Label(path);
		}
	}
	
	// Update is called once per frame
	void Update() {
		if (Input.GetKeyDown(KeyCode.Escape) ||Input.GetKeyDown(KeyCode.Home) )
		{
			Application.Quit();
		}
	}
}
