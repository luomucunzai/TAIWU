using System;
using System.IO;

namespace HarmonyLib.Tools;

public static class HarmonyFileLog
{
	private static bool enabled;

	private static TextWriter textWriter;

	public static bool Enabled
	{
		get
		{
			return enabled;
		}
		set
		{
			if (string.IsNullOrEmpty(Environment.GetEnvironmentVariable("HARMONY_NO_LOG")))
			{
				enabled = value;
				ToggleDebug();
			}
		}
	}

	public static TextWriter Writer
	{
		get
		{
			return textWriter;
		}
		set
		{
			textWriter?.Flush();
			textWriter = value;
		}
	}

	public static string FileWriterPath { get; set; }

	static HarmonyFileLog()
	{
		string text = Environment.GetEnvironmentVariable("HARMONY_LOG_FILE");
		if (string.IsNullOrEmpty(text))
		{
			text = "HarmonyFileLog.log";
		}
		FileWriterPath = text;
	}

	private static void ToggleDebug()
	{
		if (Enabled)
		{
			if (Writer == null)
			{
				Writer = new StreamWriter(File.Create(Path.GetFullPath(FileWriterPath)));
			}
			Logger.MessageReceived += OnMessage;
		}
		else
		{
			Logger.MessageReceived -= OnMessage;
		}
	}

	private static void OnMessage(object sender, Logger.LogEventArgs e)
	{
		Writer.WriteLine($"[{e.LogChannel}] {e.Message}");
		Writer.Flush();
	}
}
