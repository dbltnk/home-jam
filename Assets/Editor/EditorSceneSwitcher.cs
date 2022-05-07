using UnityEditor;
using UnityEditor.SceneManagement;

public static class EditorSceneSwitcher {
	[MenuItem("Scenes/SampleScene")]
	static void Open0()
	{
		EditorSceneManager.OpenScene("Assets/Scenes/SampleScene.unity");
	}

	[MenuItem("Scenes/Prefabs")]
	static void Open1()
	{
		EditorSceneManager.OpenScene("Assets/Scenes/Prefabs.unity");
	}
}
