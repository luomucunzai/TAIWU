using GameData.Domains.Character.Ai;
using GameData.Utilities;
using Redzen.Random;

namespace GameData.Common;

public class DataContext
{
	public readonly int ThreadId;

	public readonly ParallelModificationsRecorder ParallelModificationsRecorder;

	public IRandomSource Random;

	public readonly Equipping Equipping;

	public readonly AdvanceMonthRelatedData AdvanceMonthRelatedData;

	private IRandomSource _randomSourceBackup;

	public DataContext(int threadId)
	{
		ThreadId = threadId;
		ParallelModificationsRecorder = new ParallelModificationsRecorder();
		Random = RandomDefaults.CreateRandomSource();
		Equipping = new Equipping();
		AdvanceMonthRelatedData = new AdvanceMonthRelatedData();
	}

	public IRandomSource SwitchRandomSource(string purposeText)
	{
		ulong num = Random.NextULong();
		AdaptableLog.Info($"Random seed switched to {num} for {purposeText}");
		return SwitchRandomSource(num);
	}

	public IRandomSource SwitchRandomSource(IRandomSource random)
	{
		if (_randomSourceBackup == null)
		{
			_randomSourceBackup = Random;
		}
		else
		{
			AdaptableLog.TagWarning("Random", "Switching to a new random source before the previous one was restored.");
		}
		IRandomSource random2 = Random;
		Random = random;
		return random2;
	}

	public IRandomSource SwitchRandomSource(ulong seed)
	{
		if (_randomSourceBackup == null)
		{
			_randomSourceBackup = Random;
		}
		else
		{
			AdaptableLog.TagWarning("Random", "Switching to a new random source before the previous one was restored.");
		}
		IRandomSource random = Random;
		Random = RandomDefaults.CreateRandomSource(seed);
		return random;
	}

	public void RestoreRandomSource()
	{
		if (_randomSourceBackup == null)
		{
			AdaptableLog.TagWarning("Random", "Restoring random source when backup doesn't exist.");
		}
		else
		{
			Random = _randomSourceBackup;
		}
		_randomSourceBackup = null;
	}
}
