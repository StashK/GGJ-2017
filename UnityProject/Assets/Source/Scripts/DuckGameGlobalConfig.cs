using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DuckGameGlobalConfig {
    public static float distanceSpeed = 0.01f;
    public static float moveSpeed = 0.8f;
	public static float sideMoveSpeed = 10f;
    public static float startDistance = 20f;
    public static float winDistance = 2.0f;

	public static float duckPushDistance = 5f;

	public static float preDropOffTime = 10f;
    public static float dropOffTime = 10.0f;
    public static float gridStartDistance = 25f;

	public static float quackSpamInterval = 0.5f;

	public static bool drawDebugLines = true;

	public static float removeDuckFatnessInterval = 10f;

	public static float fatness1PushMultiplier = 1f;
	public static float fatness2PushMultiplier = 0.8f;
	public static float fatness3PushMultiplier = 0.6f;
}
