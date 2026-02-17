using System;
using System.IO;

namespace GameData.Domains.TaiwuEvent
{
	// Token: 0x02000073 RID: 115
	public readonly ref struct EventPackagePathInfo
	{
		// Token: 0x060016BD RID: 5821 RVA: 0x00153692 File Offset: 0x00151892
		public EventPackagePathInfo(string dllDirPath, string languageDirPath, string scriptDirPath)
		{
			this.DllDirPath = dllDirPath;
			this.LanguageDirPath = languageDirPath;
			this.ScriptDirPath = scriptDirPath;
		}

		// Token: 0x060016BE RID: 5822 RVA: 0x001536AA File Offset: 0x001518AA
		public EventPackagePathInfo(string rootPath)
		{
			this.DllDirPath = Path.Combine(rootPath, "EventLib");
			this.LanguageDirPath = Path.Combine(rootPath, "EventLanguages");
			this.ScriptDirPath = Path.Combine(rootPath, "EventScript");
		}

		// Token: 0x0400047A RID: 1146
		public readonly string DllDirPath;

		// Token: 0x0400047B RID: 1147
		public readonly string LanguageDirPath;

		// Token: 0x0400047C RID: 1148
		public readonly string ScriptDirPath;
	}
}
