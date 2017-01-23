using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DuckGameGlobalConfig {
    public static float moveSpeed = 4f;
	public static float sideMoveSpeed = 20f;
    public static float startDistance = 64f;
    public static float winDistance = 8f;
	public static float targetLineDistance = 40f;

	public static float duckPushDistance = 20f;

	public static float preDropOffTime = 10f;
    public static float dropOffTime = 5f;

	public static float quackSpamInterval = 0.5f;

	public static bool drawDebugLines = true;

	public static float removeDuckFatnessInterval = 10f;

	public static float fatness1PushMultiplier = 1f;
	public static float fatness2PushMultiplier = 0.8f;
	public static float fatness3PushMultiplier = 0.6f;

	public static void ResetVariables()
	{
		moveSpeed = 5f;
		sideMoveSpeed = 20f;
		startDistance = 64f;
		winDistance = 8f;
		targetLineDistance = 30f;

		duckPushDistance = 20f;

		preDropOffTime = 10f;
		dropOffTime = 5f;

		quackSpamInterval = 0.5f;

		drawDebugLines = true;

		removeDuckFatnessInterval = 10f;

		fatness1PushMultiplier = 1f;
		fatness2PushMultiplier = 0.8f;
		fatness3PushMultiplier = 0.6f;
	}
}
