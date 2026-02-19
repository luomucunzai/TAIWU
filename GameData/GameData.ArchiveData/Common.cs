using System;
using System.IO;
using System.Runtime.InteropServices;
using NLog;

namespace GameData.ArchiveData;

public static class Common
{
	public const sbyte ArchiveSlotsCount = 5;

	private const sbyte InvalidArchiveId = -1;

	private static readonly Logger Logger = LogManager.GetCurrentClassLogger();

	private static sbyte _currArchiveId = -1;

	public static string ArchiveBaseDir { get; private set; }

	[DllImport("ArchiveData")]
	private static extern IntPtr get_last_error();

	[DllImport("ArchiveData")]
	private static extern IntPtr get_async_error();

	[DllImport("ArchiveData")]
	private static extern bool initialize([MarshalAs(UnmanagedType.LPUTF8Str)] string loggingDir, [MarshalAs(UnmanagedType.LPUTF8Str)] string archiveBaseDir);

	[DllImport("ArchiveData")]
	private static extern void un_initialize();

	[DllImport("ArchiveData")]
	private unsafe static extern byte* allocate_memory(uint size);

	[DllImport("ArchiveData")]
	private unsafe static extern void free_memory(byte* pointer);

	public static string GetLastError()
	{
		IntPtr last_error = get_last_error();
		return (last_error == IntPtr.Zero) ? null : Marshal.PtrToStringAnsi(last_error);
	}

	public static bool CheckAsyncError()
	{
		IntPtr async_error = get_async_error();
		if (async_error == IntPtr.Zero)
		{
			return true;
		}
		string text = Marshal.PtrToStringAnsi(async_error);
		Logger.Error("Async error of ArchiveData module: {message}", text);
		return false;
	}

	public static void Initialize(string baseDataDir, string loggingDir, bool isTestVersion)
	{
		ArchiveBaseDir = Path.Combine(baseDataDir, isTestVersion ? "Save_test" : "Save");
		if (!initialize(loggingDir, ArchiveBaseDir))
		{
			throw new Exception("Common.Initialize: " + GetLastError());
		}
	}

	public static void UnInitialize()
	{
		un_initialize();
	}

	public unsafe static byte* AllocateMemory(uint size)
	{
		return allocate_memory(size);
	}

	public unsafe static void FreeMemory(byte* pointer)
	{
		free_memory(pointer);
	}

	public static void SetArchiveId(sbyte archiveId)
	{
		if (!CheckArchiveId(archiveId))
		{
			throw new Exception($"Invalid archiveId: {archiveId}");
		}
		_currArchiveId = archiveId;
	}

	public static void ResetArchiveId()
	{
		_currArchiveId = -1;
	}

	public static sbyte GetCurrArchiveId()
	{
		return _currArchiveId;
	}

	public static bool CheckArchiveId(sbyte archiveId)
	{
		return archiveId >= 0 && archiveId < 5;
	}

	public static bool IsInWorld()
	{
		return _currArchiveId >= 0 && _currArchiveId < 5;
	}
}
