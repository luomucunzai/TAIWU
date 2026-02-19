using GameData.Utilities;

namespace Config;

public static class PredefinedLogExtensions
{
	public static void Log(this PredefinedLogItem item)
	{
		AdaptableLog.Warning("[" + item.Name + "]: " + item.Info, appendWarningMessage: true);
	}

	public static void Log(this PredefinedLogItem item, object arg0)
	{
		AdaptableLog.Warning("[" + item.Name + "]: " + item.Info.GetFormat(arg0), appendWarningMessage: true);
	}

	public static void Log(this PredefinedLogItem item, object arg0, object arg1)
	{
		AdaptableLog.Warning("[" + item.Name + "]: " + item.Info.GetFormat(arg0, arg1), appendWarningMessage: true);
	}

	public static void Log(this PredefinedLogItem item, object arg0, object arg1, object arg2)
	{
		AdaptableLog.Warning("[" + item.Name + "]: " + item.Info.GetFormat(arg0, arg1, arg2), appendWarningMessage: true);
	}

	public static void Log(this PredefinedLogItem item, params object[] parameters)
	{
		AdaptableLog.Warning("[" + item.Name + "]: " + item.Info.GetFormat(parameters), appendWarningMessage: true);
	}
}
