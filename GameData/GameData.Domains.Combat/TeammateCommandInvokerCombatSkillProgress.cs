using GameData.Common;
using GameData.DomainEvents;
using GameData.Utilities;

namespace GameData.Domains.Combat;

public class TeammateCommandInvokerCombatSkillProgress : TeammateCommandInvokerBase
{
	private int _lastProgress;

	public TeammateCommandInvokerCombatSkillProgress(int charId, int index)
		: base(charId, index)
	{
		int[] autoProgress = base.CmdConfig.AutoProgress;
		Tester.Assert(autoProgress != null && autoProgress.Length > 0, "CmdConfig.AutoProgress is { Length: > 0 }");
	}

	public override void Setup()
	{
		Events.RegisterHandler_CastSkillEnd(OnCastSkillEnd);
		Events.RegisterHandler_PrepareSkillProgressChange(OnPrepareSkillProgressChange);
	}

	public override void Close()
	{
		Events.UnRegisterHandler_CastSkillEnd(OnCastSkillEnd);
		Events.UnRegisterHandler_PrepareSkillProgressChange(OnPrepareSkillProgressChange);
	}

	private void OnCastSkillEnd(DataContext context, int charId, bool isAlly, short skillId, sbyte power, bool interrupted)
	{
		if (charId == MainCharId)
		{
			_lastProgress = 0;
		}
	}

	private void OnPrepareSkillProgressChange(DataContext context, int charId, bool isAlly, short skillId, sbyte preparePercent)
	{
		if (charId != MainCharId)
		{
			return;
		}
		bool flag = false;
		int[] autoProgress = base.CmdConfig.AutoProgress;
		foreach (int num in autoProgress)
		{
			if (preparePercent >= num && _lastProgress < num)
			{
				flag = true;
			}
		}
		if (flag && context.Random.CheckPercentProb(base.CmdConfig.AutoProb))
		{
			Execute(context);
		}
		_lastProgress = preparePercent;
	}
}
