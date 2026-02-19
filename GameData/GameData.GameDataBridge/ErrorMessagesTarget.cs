using NLog;
using NLog.Targets;

namespace GameData.GameDataBridge;

[Target("ErrorMessages")]
public class ErrorMessagesTarget : TargetWithLayout
{
	public ErrorMessagesTarget(string name)
	{
		((Target)this).Name = name;
		((Target)this).OptimizeBufferReuse = true;
	}

	protected override void Write(LogEventInfo logEvent)
	{
		string message = ((TargetWithLayout)this).Layout.Render(logEvent);
		GameDataBridge.AppendErrorMessage(message);
	}
}
