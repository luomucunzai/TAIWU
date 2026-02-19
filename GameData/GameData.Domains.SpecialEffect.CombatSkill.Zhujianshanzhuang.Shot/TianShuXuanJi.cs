using System.Collections.Generic;
using Config;
using GameData.Common;
using GameData.DomainEvents;
using GameData.Domains.Character;
using GameData.Domains.Combat;
using GameData.Domains.CombatSkill;
using GameData.Utilities;

namespace GameData.Domains.SpecialEffect.CombatSkill.Zhujianshanzhuang.Shot;

public class TianShuXuanJi : CombatSkillEffectBase
{
	private const sbyte TransferMarkPercent = 50;

	private DefeatMarkCollection _removedMarks;

	private Dictionary<sbyte, List<(sbyte level, int totalFrame, int leftFrame)>> _flawTimeDict;

	private Dictionary<sbyte, List<(sbyte level, int totalFrame, int leftFrame)>> _acupointTimeDict;

	private List<SilenceFrameData> _mindMarkTimeList;

	public TianShuXuanJi()
	{
	}

	public TianShuXuanJi(CombatSkillKey skillKey)
		: base(skillKey, 9407, -1)
	{
	}

	public override void OnEnable(DataContext context)
	{
		Events.RegisterHandler_CastSkillEnd(OnCastSkillEnd);
		CombatDomain.RegisterHandler_CombatCharAboutToFall(OnCharAboutToFall);
	}

	public override void OnDisable(DataContext context)
	{
		Events.UnRegisterHandler_CastSkillEnd(OnCastSkillEnd);
		CombatDomain.UnRegisterHandler_CombatCharAboutToFall(OnCharAboutToFall);
	}

	private void OnCastSkillEnd(DataContext context, int charId, bool isAlly, short skillId, sbyte power, bool interrupted)
	{
		if (SkillKey.IsMatch(charId, skillId) && PowerMatchAffectRequire(power) && !base.SkillData.GetSilencing())
		{
			AddMaxEffectCount();
			OnCharAboutToFall(context, base.CombatChar, 0);
		}
	}

	private void OnCharAboutToFall(DataContext context, CombatCharacter combatChar, int eventIndex)
	{
		if (combatChar == base.CombatChar && eventIndex == 0 && DomainManager.Combat.DefeatMarkReachFailCount(base.CombatChar) && base.EffectCount > 0)
		{
			DoAffect(context);
			ReduceEffectCount();
			DomainManager.Combat.SilenceSkill(context, base.CombatChar, base.SkillTemplateId, -1, -1);
		}
	}

	private void DoAffect(DataContext context)
	{
		_removedMarks = new DefeatMarkCollection();
		if (base.IsDirect)
		{
			Injuries injuries = base.CombatChar.GetInjuries();
			Injuries injuries2 = injuries.Subtract(base.CombatChar.GetOldInjuries());
			for (sbyte b = 0; b < 7; b++)
			{
				(sbyte, sbyte) tuple = injuries2.Get(b);
				if (tuple.Item1 > 0)
				{
					injuries.Change(b, isInnerInjury: false, (sbyte)(-tuple.Item1));
					_removedMarks.OuterInjuryMarkList[b] = (byte)tuple.Item1;
				}
				if (tuple.Item2 > 0)
				{
					injuries.Change(b, isInnerInjury: true, (sbyte)(-tuple.Item2));
					_removedMarks.InnerInjuryMarkList[b] = (byte)tuple.Item2;
				}
			}
			if (injuries2.HasAnyInjury())
			{
				DomainManager.Combat.SetInjuries(context, base.CombatChar, injuries);
			}
		}
		else
		{
			_flawTimeDict = new Dictionary<sbyte, List<(sbyte, int, int)>>();
			_acupointTimeDict = new Dictionary<sbyte, List<(sbyte, int, int)>>();
			_mindMarkTimeList = new List<SilenceFrameData>();
			DefeatMarkCollection defeatMarkCollection = base.CombatChar.GetDefeatMarkCollection();
			byte[] flawCount = base.CombatChar.GetFlawCount();
			FlawOrAcupointCollection flawCollection = base.CombatChar.GetFlawCollection();
			byte[] acupointCount = base.CombatChar.GetAcupointCount();
			FlawOrAcupointCollection acupointCollection = base.CombatChar.GetAcupointCollection();
			MindMarkList mindMarkTime = base.CombatChar.GetMindMarkTime();
			for (sbyte b2 = 0; b2 < 7; b2++)
			{
				List<(sbyte, int, int)> list = flawCollection.BodyPartDict[b2];
				List<(sbyte, int, int)> list2 = acupointCollection.BodyPartDict[b2];
				_flawTimeDict.Add(b2, new List<(sbyte, int, int)>());
				_flawTimeDict[b2].AddRange(list);
				for (int i = 0; i < list.Count; i++)
				{
					_removedMarks.FlawMarkList[b2].Add((byte)list[i].Item1);
				}
				defeatMarkCollection.FlawMarkList[b2].Clear();
				flawCount[b2] = 0;
				list.Clear();
				_acupointTimeDict.Add(b2, new List<(sbyte, int, int)>());
				_acupointTimeDict[b2].AddRange(list2);
				for (int j = 0; j < list.Count; j++)
				{
					_removedMarks.FlawMarkList[b2].Add((byte)list2[j].Item1);
				}
				defeatMarkCollection.AcupointMarkList[b2].Clear();
				acupointCount[b2] = 0;
				list2.Clear();
			}
			if (mindMarkTime.MarkList != null)
			{
				_mindMarkTimeList.AddRange(mindMarkTime.MarkList);
				_removedMarks.MindMarkList.AddRange(defeatMarkCollection.MindMarkList);
				mindMarkTime.MarkList.Clear();
				defeatMarkCollection.MindMarkList.Clear();
			}
			base.CombatChar.SetFlawCount(flawCount, context);
			base.CombatChar.SetFlawCollection(flawCollection, context);
			base.CombatChar.SetAcupointCount(acupointCount, context);
			base.CombatChar.SetAcupointCollection(acupointCollection, context);
			base.CombatChar.SetMindMarkTime(mindMarkTime, context);
			base.CombatChar.SetDefeatMarkCollection(defeatMarkCollection, context);
		}
		_removedMarks.FatalDamageMarkCount = base.CombatChar.GetDefeatMarkCollection().FatalDamageMarkCount;
		DomainManager.Combat.RemoveFatalDamageMark(context, base.CombatChar, base.CombatChar.GetDefeatMarkCollection().FatalDamageMarkCount);
		if (_removedMarks.GetTotalCount() > 0)
		{
			Events.RegisterHandler_CombatStateMachineUpdateEnd(OnStateMachineUpdateEnd);
		}
	}

	private void OnStateMachineUpdateEnd(DataContext context, CombatCharacter combatChar)
	{
		int totalCount = _removedMarks.GetTotalCount();
		int num = totalCount * 50 / 100;
		int num2 = totalCount - num;
		if (num2 > 0)
		{
			List<DefeatMarkKey> preferredItems = new List<DefeatMarkKey>(_removedMarks.GetAllKeysWithoutOld());
			foreach (DefeatMarkKey item in RandomUtils.GetRandomUnrepeated(context.Random, num2, preferredItems))
			{
				ApplyMarkKey(context, item);
			}
			base.EnemyChar.SetMindMarkTime(base.EnemyChar.GetMindMarkTime(), context);
			base.EnemyChar.GetDefeatMarkCollection().SyncMindMark(context, base.EnemyChar);
			DomainManager.Combat.UpdateBodyDefeatMark(context, base.EnemyChar);
			DomainManager.Combat.AddToCheckFallenSet(base.EnemyChar.GetId());
			ShowSpecialEffectTips(0);
		}
		if (num > 0)
		{
			DomainManager.Combat.AddTrick(context, base.CombatChar, 12, num);
			ShowSpecialEffectTips(1);
		}
		Events.UnRegisterHandler_CombatStateMachineUpdateEnd(OnStateMachineUpdateEnd);
	}

	private void ApplyMarkKey(DataContext context, DefeatMarkKey markKey)
	{
		EMarkType type = markKey.Type;
		if ((uint)type <= 1u)
		{
			bool isInner = markKey.Type == EMarkType.Inner;
			DomainManager.Combat.AddInjury(context, base.EnemyChar, markKey.BodyPart, isInner, 1);
			return;
		}
		type = markKey.Type;
		if ((uint)(type - 2) <= 1u)
		{
			bool flag = markKey.Type == EMarkType.Flaw;
			Dictionary<sbyte, List<(sbyte, int, int)>> dictionary = (flag ? _flawTimeDict : _acupointTimeDict);
			int index = context.Random.Next(0, dictionary[markKey.BodyPart].Count);
			var (level, leftFrames, totalFrames) = _flawTimeDict[markKey.BodyPart][index];
			dictionary[markKey.BodyPart].RemoveAt(index);
			base.EnemyChar.AddOrUpdateFlawOrAcupoint(context, markKey.BodyPart, flag, level, raiseEvent: true, leftFrames, totalFrames);
		}
		else if (markKey.Type == EMarkType.Mind)
		{
			int index2 = context.Random.Next(0, _mindMarkTimeList.Count);
			SilenceFrameData silenceFrameData = _mindMarkTimeList[index2];
			_mindMarkTimeList.RemoveAt(index2);
			CombatCharacter enemyChar = base.EnemyChar;
			MindMarkList mindMarkTime = enemyChar.GetMindMarkTime();
			List<SilenceFrameData> list = mindMarkTime.MarkList ?? (mindMarkTime.MarkList = new List<SilenceFrameData>());
			short mindMarkBaseKeepTime = GlobalConfig.Instance.MindMarkBaseKeepTime;
			list.Add(silenceFrameData.Infinity ? SilenceFrameData.Create(mindMarkBaseKeepTime) : silenceFrameData);
		}
		else if (markKey.Type == EMarkType.Fatal)
		{
			DomainManager.Combat.AppendFatalDamageMark(context, base.EnemyChar, 1, -1, -1);
		}
		else
		{
			PredefinedLog.Show(7, base.EffectId, $"Cannot analysis markKey {markKey}");
		}
	}
}
