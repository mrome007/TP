using UnityEngine;
using System.Collections;

public class SingletonManager
{
	public enum PlayMode
	{
		BUYmode,
		SHOOTmode
	}

	//Gameplay Modes
	public static PlayMode Play_Mode;

	private static SingletonManager instance = null;

	//Levels
	private int NumberOfLevels = 1;
	public static string[] Levels;
	public static int CurLevelIndex = 0;
	public static string LevelName = "";
	public static int Resources = 500;
	private SingletonManager()
	{
		Play_Mode = PlayMode.BUYmode;
		Levels = new string[NumberOfLevels];
		Levels [0] = "testLevel";
		NumberOfLevels = Levels.Length;
		CurLevelIndex = 0;
		LevelName = Levels [CurLevelIndex];
	}

	public static SingletonManager GetInstance()
	{
		if(instance == null)
			instance = new SingletonManager();
		return instance;
	}

	public void ChangeMode()
	{
		if(Play_Mode == PlayMode.BUYmode)
			Play_Mode = PlayMode.SHOOTmode;
		else
			Play_Mode = PlayMode.BUYmode;
	}

	public void ResetValues()
	{
		CurLevelIndex = 0;
		Resources = 500;
	}
}
