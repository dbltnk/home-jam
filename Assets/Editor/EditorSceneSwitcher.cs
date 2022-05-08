using UnityEditor;
using UnityEditor.SceneManagement;

public static class EditorSceneSwitcher {
	[MenuItem("Scenes/Level0")]
	static void Open0()
	{
		EditorSceneManager.OpenScene("Assets/Scenes/Level0.unity");
	}

	[MenuItem("Scenes/Level1")]
	static void Open1()
	{
		EditorSceneManager.OpenScene("Assets/Scenes/Level1.unity");
	}

	[MenuItem("Scenes/Level2")]
	static void Open2()
	{
		EditorSceneManager.OpenScene("Assets/Scenes/Level2.unity");
	}

	[MenuItem("Scenes/Level3")]
	static void Open3()
	{
		EditorSceneManager.OpenScene("Assets/Scenes/Level3.unity");
	}

	[MenuItem("Scenes/Level4")]
	static void Open4()
	{
		EditorSceneManager.OpenScene("Assets/Scenes/Level4.unity");
	}

	[MenuItem("Scenes/Prefabs")]
	static void Open10()
	{
		EditorSceneManager.OpenScene("Assets/Scenes/Prefabs.unity");
	}

	[MenuItem("Scenes/All")]
	static void OpenAll()
	{
		EditorSceneManager.OpenScene("Assets/Scenes/Level0.unity", OpenSceneMode.Single);
		EditorSceneManager.OpenScene("Assets/Scenes/Level1.unity", OpenSceneMode.Additive);
		EditorSceneManager.OpenScene("Assets/Scenes/Level2.unity", OpenSceneMode.Additive);
		EditorSceneManager.OpenScene("Assets/Scenes/Level3.unity", OpenSceneMode.Additive);
		EditorSceneManager.OpenScene("Assets/Scenes/Level4.unity", OpenSceneMode.Additive);
	}
}
