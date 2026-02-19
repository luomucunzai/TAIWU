using System;
using System.Collections.Generic;
using System.Linq;
using GameData.Common;
using GameData.Domains.Character;
using GameData.Domains.Combat;
using GameData.Domains.CombatSkill;
using GameData.Domains.SpecialEffect.CombatSkill.Common.Assist;
using GameData.Utilities;
using Redzen.Random;

namespace GameData.Domains.SpecialEffect.CombatSkill.Kongsangpai.DefenseAndAssist;

public class JiuSeYuChanFa : AssistSkillBase
{
	private enum EMarkType : sbyte
	{
		Flaw,
		Acupoint,
		Mind,
		Fatal,
		OuterInjury,
		InnerInjury
	}

	private const sbyte CostNeiliAllocationUnit = 9;

	private const int DirectRemoveMarkCount = 9;

	private const int ReverseRemoveMarkCount = int.MaxValue;

	private const int ReverseCostNeiliAllocationUnitMaxCount = 11;

	private List<(EMarkType, sbyte)> _markTypeRandomPool;

	private List<(EMarkType, sbyte)> _tempMarkTypeRandomPool;

	private BoolArray8 _markTypeChanged;

	private int _affectCount = 0;

	private int RemoveMarkCount => base.IsDirect ? 9 : int.MaxValue;

	private int CostNeiliAllocation => base.IsDirect ? 9 : (9 * Math.Min(_affectCount + 1, 11));

	public JiuSeYuChanFa()
	{
	}

	public JiuSeYuChanFa(CombatSkillKey skillKey)
		: base(skillKey, 10708)
	{
	}

	public override void OnEnable(DataContext context)
	{
		CombatDomain.RegisterHandler_CombatCharAboutToFall(OnCharAboutToFall);
	}

	public override void OnDisable(DataContext context)
	{
		CombatDomain.UnRegisterHandler_CombatCharAboutToFall(OnCharAboutToFall);
	}

	private unsafe void OnCharAboutToFall(DataContext context, CombatCharacter combatChar, int eventIndex)
	{
		if (!base.CanAffect || combatChar != base.CombatChar || eventIndex != 1 || !DomainManager.Combat.DefeatMarkReachFailCount(base.CombatChar))
		{
			return;
		}
		NeiliAllocation neiliAllocation = base.CombatChar.GetNeiliAllocation();
		if (neiliAllocation.Items[3] >= CostNeiliAllocation)
		{
			GenerateMarkRandomPool();
			if (base.IsDirect)
			{
				RemoveByMarkRandomPool(context.Random);
			}
			else
			{
				RemoveByTempRandomPool(context.Random, IsInjuryMark);
				RemoveByTempRandomPool(context.Random, IsNotInjuryMark);
				RemoveByTempRandomPool(context.Random, IsFatalMark);
			}
			SetAllChangedFields(context);
			base.CombatChar.ChangeNeiliAllocation(context, 3, -CostNeiliAllocation, applySpecialEffect: false);
			_affectCount++;
			ShowSpecialEffectTips(0);
			ShowEffectTips(context);
		}
	}

	private void GenerateMarkRandomPool()
	{
		if (_markTypeRandomPool == null)
		{
			_markTypeRandomPool = new List<(EMarkType, sbyte)>();
		}
		_markTypeRandomPool.Clear();
		((BoolArray8)(ref _markTypeChanged)).Reset();
		DefeatMarkCollection defeatMarkCollection = base.CombatChar.GetDefeatMarkCollection();
		byte[] flawCount = base.CombatChar.GetFlawCount();
		byte[] acupointCount = base.CombatChar.GetAcupointCount();
		MindMarkList mindMarkTime = base.CombatChar.GetMindMarkTime();
		Injuries injuries = base.CombatChar.GetInjuries().Subtract(base.CombatChar.GetOldInjuries());
		for (sbyte b = 0; b < 7; b++)
		{
			for (int i = 0; i < flawCount[b]; i++)
			{
				_markTypeRandomPool.Add((EMarkType.Flaw, b));
			}
			for (int j = 0; j < acupointCount[b]; j++)
			{
				_markTypeRandomPool.Add((EMarkType.Acupoint, b));
			}
		}
		if (mindMarkTime.MarkList != null)
		{
			for (int k = 0; k < mindMarkTime.MarkList.Count; k++)
			{
				_markTypeRandomPool.Add((EMarkType.Mind, -1));
			}
		}
		for (int l = 0; l < defeatMarkCollection.FatalDamageMarkCount; l++)
		{
			_markTypeRandomPool.Add((EMarkType.Fatal, -1));
		}
		if (!injuries.HasAnyInjury())
		{
			return;
		}
		for (sbyte b2 = 0; b2 < 7; b2++)
		{
			(sbyte, sbyte) tuple = injuries.Get(b2);
			for (int m = 0; m < tuple.Item1; m++)
			{
				_markTypeRandomPool.Add((EMarkType.OuterInjury, b2));
			}
			for (int n = 0; n < tuple.Item2; n++)
			{
				_markTypeRandomPool.Add((EMarkType.InnerInjury, b2));
			}
		}
	}

	private static bool IsInjuryMark((EMarkType markType, sbyte bodyPart) tup)
	{
		var (eMarkType, _) = tup;
		if ((uint)(eMarkType - 4) <= 1u)
		{
			return true;
		}
		return false;
	}

	private static bool IsNotInjuryMark((EMarkType markType, sbyte bodyPart) tup)
	{
		var (eMarkType, _) = tup;
		if ((uint)eMarkType <= 2u)
		{
			return true;
		}
		return false;
	}

	private static bool IsFatalMark((EMarkType markType, sbyte bodyPart) tup)
	{
		return tup.markType == EMarkType.Fatal;
	}

	private int RemoveByMarkRandomPool(IRandomSource random)
	{
		int num = Math.Min(RemoveMarkCount, _markTypeRandomPool.Count);
		for (int i = 0; i < num; i++)
		{
			(EMarkType, sbyte) random2 = _markTypeRandomPool.GetRandom(random);
			var (type, bodyPart) = random2;
			ErasureDefeatMark(random, type, bodyPart);
			_markTypeRandomPool.Remove(random2);
		}
		return num;
	}

	private int RemoveByTempRandomPool(IRandomSource random, Func<(EMarkType, sbyte), bool> predicate)
	{
		if (_tempMarkTypeRandomPool == null)
		{
			_tempMarkTypeRandomPool = new List<(EMarkType, sbyte)>();
		}
		_tempMarkTypeRandomPool.Clear();
		_tempMarkTypeRandomPool.AddRange(_markTypeRandomPool.Where(predicate));
		int num = Math.Min(RemoveMarkCount, _tempMarkTypeRandomPool.Count);
		for (int i = 0; i < num; i++)
		{
			(EMarkType, sbyte) random2 = _tempMarkTypeRandomPool.GetRandom(random);
			var (type, bodyPart) = random2;
			ErasureDefeatMark(random, type, bodyPart);
			_tempMarkTypeRandomPool.Remove(random2);
		}
		return num;
	}

	private void ErasureFlawOrAcupoint(sbyte bodyPart, bool isFlaw, IRandomSource random)
	{
		FlawOrAcupointCollection flawOrAcupointCollection = (isFlaw ? base.CombatChar.GetFlawCollection() : base.CombatChar.GetAcupointCollection());
		byte[] array = (isFlaw ? base.CombatChar.GetFlawCount() : base.CombatChar.GetAcupointCount());
		List<(sbyte, int, int)> list = flawOrAcupointCollection.BodyPartDict[bodyPart];
		int index = random.Next(0, list.Count);
		array[bodyPart]--;
		list.RemoveAt(index);
		DefeatMarkCollection defeatMarkCollection = base.CombatChar.GetDefeatMarkCollection();
		ByteList[] array2 = (isFlaw ? defeatMarkCollection.FlawMarkList : defeatMarkCollection.AcupointMarkList);
		array2[bodyPart].RemoveAt(index);
	}

	private void ErasureMind(IRandomSource random)
	{
		MindMarkList mindMarkTime = base.CombatChar.GetMindMarkTime();
		int index = random.Next(0, mindMarkTime.MarkList.Count);
		mindMarkTime.MarkList.RemoveAt(index);
		DefeatMarkCollection defeatMarkCollection = base.CombatChar.GetDefeatMarkCollection();
		defeatMarkCollection.MindMarkList.RemoveAt(index);
	}

	private void ErasureFatal()
	{
		DefeatMarkCollection defeatMarkCollection = base.CombatChar.GetDefeatMarkCollection();
		defeatMarkCollection.FatalDamageMarkCount--;
	}

	private void ErasureInjury(sbyte bodyPart, bool isInner)
	{
		base.CombatChar.OfflineChangeInjuries(bodyPart, isInner, -1);
	}

	private void ErasureDefeatMark(IRandomSource random, EMarkType type, sbyte bodyPart)
	{
		((BoolArray8)(ref _markTypeChanged))[(int)type] = true;
		switch (type)
		{
		case EMarkType.Flaw:
		case EMarkType.Acupoint:
			ErasureFlawOrAcupoint(bodyPart, type == EMarkType.Flaw, random);
			break;
		case EMarkType.Mind:
			ErasureMind(random);
			break;
		case EMarkType.Fatal:
			ErasureFatal();
			break;
		case EMarkType.OuterInjury:
		case EMarkType.InnerInjury:
			ErasureInjury(bodyPart, type == EMarkType.InnerInjury);
			break;
		default:
			throw new ArgumentOutOfRangeException("type", type, null);
		}
	}

	private void SetAllChangedFields(DataContext context)
	{
		DefeatMarkCollection defeatMarkCollection = base.CombatChar.GetDefeatMarkCollection();
		byte[] flawCount = base.CombatChar.GetFlawCount();
		FlawOrAcupointCollection flawCollection = base.CombatChar.GetFlawCollection();
		byte[] acupointCount = base.CombatChar.GetAcupointCount();
		FlawOrAcupointCollection acupointCollection = base.CombatChar.GetAcupointCollection();
		MindMarkList mindMarkTime = base.CombatChar.GetMindMarkTime();
		Injuries injuries = base.CombatChar.GetInjuries();
		if (((BoolArray8)(ref _markTypeChanged))[0])
		{
			base.CombatChar.SetFlawCount(flawCount, context);
			base.CombatChar.SetFlawCollection(flawCollection, context);
		}
		if (((BoolArray8)(ref _markTypeChanged))[1])
		{
			base.CombatChar.SetAcupointCount(acupointCount, context);
			base.CombatChar.SetAcupointCollection(acupointCollection, context);
		}
		if (((BoolArray8)(ref _markTypeChanged))[2])
		{
			base.CombatChar.SetMindMarkTime(mindMarkTime, context);
		}
		if (((BoolArray8)(ref _markTypeChanged))[4] || ((BoolArray8)(ref _markTypeChanged))[5])
		{
			DomainManager.Combat.SetInjuries(context, base.CombatChar, injuries);
		}
		if (((BoolArray8)(ref _markTypeChanged)).Any())
		{
			base.CombatChar.SetDefeatMarkCollection(defeatMarkCollection, context);
		}
	}
}
