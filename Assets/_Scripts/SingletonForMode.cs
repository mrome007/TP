using UnityEngine;
using System.Collections;

public class SingletonForMode
{
	public enum PlayMode
	{
		BUYmode,
		SHOOTmode
	}

	public static PlayMode Play_Mode;
	private static SingletonForMode instance = null;
	private SingletonForMode()
	{
		Play_Mode = PlayMode.SHOOTmode;
	}

	public static SingletonForMode GetInstance()
	{
		if(instance == null)
			instance = new SingletonForMode();
		return instance;
	}

	public void ChangeMode()
	{
		if(Play_Mode == PlayMode.BUYmode)
			Play_Mode = PlayMode.SHOOTmode;
		else
			Play_Mode = PlayMode.BUYmode;
	}
}
