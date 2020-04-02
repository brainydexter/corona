using UnityEngine;
using System;
using System.IO;

using System.Collections.Generic;
using System.Collections;

public class Utility
{
    public static int LetterToInt(char letter){
        Debug.Assert (((letter - 'A') >= 0), "Letter is not between A - Z: " + letter);

        return letter - 'A';
    }

	public static float Square(float value) 
	{
		return value * value;
	}

    public static T LoadResource<T>(string path) where T : UnityEngine.Object
    {
        Debug.Assert(path != null && path.Length != 0, "[Utility]: Resource path cannot be empty");

        T jsonAsset = Resources.Load<T>(path);
        Debug.Assert(jsonAsset != null, "[Utility]: Failed to load " + path);
        return jsonAsset;
    }

    public static T FromJson<T>(TextAsset textAsset)
    {
        return FromJson<T>(textAsset.text);
    }

    public static T FromJson<T>(string text)
    {
        Debug.Assert(text != null && text.Length != 0, "[Utility]: text cannot be null or empty");

        T jsonObject = JsonUtility.FromJson<T>(text);
        Debug.Assert(jsonObject != null, "[Utility]: Failed to parse json for " + text);

        return jsonObject;
    }

    public static T LoadJsonFromFile<T>(string path)
    {
        TextAsset jsonAsset = LoadResource<TextAsset>(path);
        return FromJson<T>(jsonAsset);
    }
}

public static class StopwatchExt
{
    public static string GetTimeString(this System.Diagnostics.Stopwatch stopwatch, int numberofDigits = 4)
    {
        double time = stopwatch.ElapsedTicks / (double)System.Diagnostics.Stopwatch.Frequency;
        if (time > 1)
            return Math.Round(time, numberofDigits) + " s";
        if (time > 1e-3)
            return Math.Round(1e3 * time, numberofDigits) + " ms";
        if (time > 1e-6)
            return Math.Round(1e6 * time, numberofDigits) + " µs";
        if (time > 1e-9)
            return Math.Round(1e9 * time, numberofDigits) + " ns";
        return stopwatch.ElapsedTicks + " ticks";
    }
}

public static class ExtensionMethods
{
    // Deep clone
    public static T DeepClone<T>(this T source)
    {
        var jsonString = JsonUtility.ToJson(source);
        return JsonUtility.FromJson<T>(jsonString);
    }

    /// <summary>
    /// Shuffles a list in-place and returns the same list
    /// Uses unity's random
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <param name="list"></param>
    /// <returns></returns>
    public static IList<T> Shuffle<T>(this IList<T> list)
    {
        int count = list.Count;

        for (int i = 0; i < count; i++)
        {
            // Pick a random index
            int index = UnityEngine.Random.Range(i, count);

            // swap i and index
            var temp = list[i];
            list[i] = list[index];
            list[index] = temp;
        }

        return list;
    }
}

public class CaptureScreenshot : MonoBehaviour
{
    public bool IsCompleted { get { return File.Exists(ScreenshotPath); } }
    public string ScreenshotPath { get; internal set; }

    internal void Capture(Action onScreenshotCaptureComplete)
    {
        Debug.Log("share begin");
        
        ScreenshotPath = Path.Combine(Application.persistentDataPath, cScreenShotName);

        DestroyFile();

#if UNITY_EDITOR
        cScreenShotName = ScreenshotPath;
#endif

        StartCoroutine(CaptureScreen(onScreenshotCaptureComplete));
    }

    private IEnumerator CaptureScreen(Action onScreenshotCaptureComplete)
    {
        // todo capture screenshot when meter has finished filling
        ScreenCapture.CaptureScreenshot(cScreenShotName, 2); // do i need to supersize it by 2 ?
        yield return new WaitForEndOfFrame();

        onScreenshotCaptureComplete.Invoke();
    }

    private void DestroyFile()
    {
        if (File.Exists(ScreenshotPath))
            File.Delete(ScreenshotPath);
    }

    private void OnDestroy()
    {
        DestroyFile();
    }

    string cScreenShotName = "screenshot.png";
}