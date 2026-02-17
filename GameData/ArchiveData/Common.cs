using System;
using System.IO;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;
using NLog;

namespace GameData.ArchiveData
{
	// Token: 0x02000900 RID: 2304
	public static class Common
	{
		// Token: 0x1700051F RID: 1311
		// (get) Token: 0x06008262 RID: 33378 RVA: 0x004DB011 File Offset: 0x004D9211
		// (set) Token: 0x06008263 RID: 33379 RVA: 0x004DB018 File Offset: 0x004D9218
		public static string ArchiveBaseDir { get; private set; }

		// Token: 0x06008264 RID: 33380
		[DllImport("ArchiveData")]
		private static extern IntPtr get_last_error();

		// Token: 0x06008265 RID: 33381
		[DllImport("ArchiveData")]
		private static extern IntPtr get_async_error();

		// Token: 0x06008266 RID: 33382
		[DllImport("ArchiveData")]
		private static extern bool initialize([MarshalAs(UnmanagedType.LPUTF8Str)] string loggingDir, [MarshalAs(UnmanagedType.LPUTF8Str)] string archiveBaseDir);

		// Token: 0x06008267 RID: 33383
		[DllImport("ArchiveData")]
		private static extern void un_initialize();

		// Token: 0x06008268 RID: 33384
		[DllImport("ArchiveData")]
		private unsafe static extern byte* allocate_memory(uint size);

		// Token: 0x06008269 RID: 33385
		[DllImport("ArchiveData")]
		private unsafe static extern void free_memory(byte* pointer);

		// Token: 0x0600826A RID: 33386 RVA: 0x004DB020 File Offset: 0x004D9220
		public static string GetLastError()
		{
			IntPtr pMessage = Common.get_last_error();
			return (pMessage == IntPtr.Zero) ? null : Marshal.PtrToStringAnsi(pMessage);
		}

		// Token: 0x0600826B RID: 33387 RVA: 0x004DB050 File Offset: 0x004D9250
		public static bool CheckAsyncError()
		{
			IntPtr pMessage = Common.get_async_error();
			bool flag = pMessage == IntPtr.Zero;
			bool result;
			if (flag)
			{
				result = true;
			}
			else
			{
				string message = Marshal.PtrToStringAnsi(pMessage);
				Common.Logger.Error("Async error of ArchiveData module: {message}", message);
				result = false;
			}
			return result;
		}

		// Token: 0x0600826C RID: 33388 RVA: 0x004DB094 File Offset: 0x004D9294
		public static void Initialize(string baseDataDir, string loggingDir, bool isTestVersion)
		{
			Common.ArchiveBaseDir = Path.Combine(baseDataDir, isTestVersion ? "Save_test" : "Save");
			bool succeeded = Common.initialize(loggingDir, Common.ArchiveBaseDir);
			bool flag = !succeeded;
			if (flag)
			{
				throw new Exception("Common.Initialize: " + Common.GetLastError());
			}
		}

		// Token: 0x0600826D RID: 33389 RVA: 0x004DB0E6 File Offset: 0x004D92E6
		public static void UnInitialize()
		{
			Common.un_initialize();
		}

		// Token: 0x0600826E RID: 33390 RVA: 0x004DB0F0 File Offset: 0x004D92F0
		public unsafe static byte* AllocateMemory(uint size)
		{
			return Common.allocate_memory(size);
		}

		// Token: 0x0600826F RID: 33391 RVA: 0x004DB108 File Offset: 0x004D9308
		public unsafe static void FreeMemory(byte* pointer)
		{
			Common.free_memory(pointer);
		}

		// Token: 0x06008270 RID: 33392 RVA: 0x004DB114 File Offset: 0x004D9314
		public static void SetArchiveId(sbyte archiveId)
		{
			bool flag = !Common.CheckArchiveId(archiveId);
			if (flag)
			{
				DefaultInterpolatedStringHandler defaultInterpolatedStringHandler = new DefaultInterpolatedStringHandler(19, 1);
				defaultInterpolatedStringHandler.AppendLiteral("Invalid archiveId: ");
				defaultInterpolatedStringHandler.AppendFormatted<sbyte>(archiveId);
				throw new Exception(defaultInterpolatedStringHandler.ToStringAndClear());
			}
			Common._currArchiveId = archiveId;
		}

		// Token: 0x06008271 RID: 33393 RVA: 0x004DB162 File Offset: 0x004D9362
		public static void ResetArchiveId()
		{
			Common._currArchiveId = -1;
		}

		// Token: 0x06008272 RID: 33394 RVA: 0x004DB16C File Offset: 0x004D936C
		public static sbyte GetCurrArchiveId()
		{
			return Common._currArchiveId;
		}

		// Token: 0x06008273 RID: 33395 RVA: 0x004DB184 File Offset: 0x004D9384
		public static bool CheckArchiveId(sbyte archiveId)
		{
			return archiveId >= 0 && archiveId < 5;
		}

		// Token: 0x06008274 RID: 33396 RVA: 0x004DB1A4 File Offset: 0x004D93A4
		public static bool IsInWorld()
		{
			return Common._currArchiveId >= 0 && Common._currArchiveId < 5;
		}

		// Token: 0x04002478 RID: 9336
		public const sbyte ArchiveSlotsCount = 5;

		// Token: 0x04002479 RID: 9337
		private const sbyte InvalidArchiveId = -1;

		// Token: 0x0400247A RID: 9338
		private static readonly Logger Logger = LogManager.GetCurrentClassLogger();

		// Token: 0x0400247C RID: 9340
		private static sbyte _currArchiveId = -1;
	}
}
