using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using Config;
using GameData.Common;
using GameData.DomainEvents;
using GameData.Domains.Character;
using GameData.Domains.Combat;
using GameData.Domains.CombatSkill;
using GameData.Utilities;

namespace GameData.Domains.SpecialEffect.CombatSkill.Zhujianshanzhuang.Shot
{
	// Token: 0x020001C1 RID: 449
	public class TianShuXuanJi : CombatSkillEffectBase
	{
		// Token: 0x06002CB9 RID: 11449 RVA: 0x00208C3B File Offset: 0x00206E3B
		public TianShuXuanJi()
		{
		}

		// Token: 0x06002CBA RID: 11450 RVA: 0x00208C45 File Offset: 0x00206E45
		public TianShuXuanJi(CombatSkillKey skillKey) : base(skillKey, 9407, -1)
		{
		}

		// Token: 0x06002CBB RID: 11451 RVA: 0x00208C56 File Offset: 0x00206E56
		public override void OnEnable(DataContext context)
		{
			Events.RegisterHandler_CastSkillEnd(new Events.OnCastSkillEnd(this.OnCastSkillEnd));
			CombatDomain.RegisterHandler_CombatCharAboutToFall(new CombatDomain.OnCombatCharAboutToFall(this.OnCharAboutToFall));
		}

		// Token: 0x06002CBC RID: 11452 RVA: 0x00208C7D File Offset: 0x00206E7D
		public override void OnDisable(DataContext context)
		{
			Events.UnRegisterHandler_CastSkillEnd(new Events.OnCastSkillEnd(this.OnCastSkillEnd));
			CombatDomain.UnRegisterHandler_CombatCharAboutToFall(new CombatDomain.OnCombatCharAboutToFall(this.OnCharAboutToFall));
		}

		// Token: 0x06002CBD RID: 11453 RVA: 0x00208CA4 File Offset: 0x00206EA4
		private void OnCastSkillEnd(DataContext context, int charId, bool isAlly, short skillId, sbyte power, bool interrupted)
		{
			bool flag = !this.SkillKey.IsMatch(charId, skillId) || !base.PowerMatchAffectRequire((int)power, 0) || base.SkillData.GetSilencing();
			if (!flag)
			{
				base.AddMaxEffectCount(true);
				this.OnCharAboutToFall(context, base.CombatChar, 0);
			}
		}

		// Token: 0x06002CBE RID: 11454 RVA: 0x00208CF8 File Offset: 0x00206EF8
		private void OnCharAboutToFall(DataContext context, CombatCharacter combatChar, int eventIndex)
		{
			bool flag = combatChar != base.CombatChar || eventIndex != 0 || !DomainManager.Combat.DefeatMarkReachFailCount(base.CombatChar) || base.EffectCount <= 0;
			if (!flag)
			{
				this.DoAffect(context);
				base.ReduceEffectCount(1);
				DomainManager.Combat.SilenceSkill(context, base.CombatChar, base.SkillTemplateId, -1, -1);
			}
		}

		// Token: 0x06002CBF RID: 11455 RVA: 0x00208D64 File Offset: 0x00206F64
		private void DoAffect(DataContext context)
		{
			this._removedMarks = new DefeatMarkCollection();
			bool isDirect = base.IsDirect;
			if (isDirect)
			{
				Injuries injuries = base.CombatChar.GetInjuries();
				Injuries newInjuries = injuries.Subtract(base.CombatChar.GetOldInjuries());
				for (sbyte bodyPart = 0; bodyPart < 7; bodyPart += 1)
				{
					ValueTuple<sbyte, sbyte> injury = newInjuries.Get(bodyPart);
					bool flag = injury.Item1 > 0;
					if (flag)
					{
						injuries.Change(bodyPart, false, (int)(-injury.Item1));
						this._removedMarks.OuterInjuryMarkList[(int)bodyPart] = (byte)injury.Item1;
					}
					bool flag2 = injury.Item2 > 0;
					if (flag2)
					{
						injuries.Change(bodyPart, true, (int)(-injury.Item2));
						this._removedMarks.InnerInjuryMarkList[(int)bodyPart] = (byte)injury.Item2;
					}
				}
				bool flag3 = newInjuries.HasAnyInjury();
				if (flag3)
				{
					DomainManager.Combat.SetInjuries(context, base.CombatChar, injuries, true, true);
				}
			}
			else
			{
				this._flawTimeDict = new Dictionary<sbyte, List<ValueTuple<sbyte, int, int>>>();
				this._acupointTimeDict = new Dictionary<sbyte, List<ValueTuple<sbyte, int, int>>>();
				this._mindMarkTimeList = new List<SilenceFrameData>();
				DefeatMarkCollection markCollection = base.CombatChar.GetDefeatMarkCollection();
				byte[] flawCounts = base.CombatChar.GetFlawCount();
				FlawOrAcupointCollection flaws = base.CombatChar.GetFlawCollection();
				byte[] acupointCounts = base.CombatChar.GetAcupointCount();
				FlawOrAcupointCollection acupoints = base.CombatChar.GetAcupointCollection();
				MindMarkList mindMarks = base.CombatChar.GetMindMarkTime();
				for (sbyte bodyPart2 = 0; bodyPart2 < 7; bodyPart2 += 1)
				{
					List<ValueTuple<sbyte, int, int>> flawList = flaws.BodyPartDict[bodyPart2];
					List<ValueTuple<sbyte, int, int>> acupointList = acupoints.BodyPartDict[bodyPart2];
					this._flawTimeDict.Add(bodyPart2, new List<ValueTuple<sbyte, int, int>>());
					this._flawTimeDict[bodyPart2].AddRange(flawList);
					for (int i = 0; i < flawList.Count; i++)
					{
						this._removedMarks.FlawMarkList[(int)bodyPart2].Add((byte)flawList[i].Item1);
					}
					markCollection.FlawMarkList[(int)bodyPart2].Clear();
					flawCounts[(int)bodyPart2] = 0;
					flawList.Clear();
					this._acupointTimeDict.Add(bodyPart2, new List<ValueTuple<sbyte, int, int>>());
					this._acupointTimeDict[bodyPart2].AddRange(acupointList);
					for (int j = 0; j < flawList.Count; j++)
					{
						this._removedMarks.FlawMarkList[(int)bodyPart2].Add((byte)acupointList[j].Item1);
					}
					markCollection.AcupointMarkList[(int)bodyPart2].Clear();
					acupointCounts[(int)bodyPart2] = 0;
					acupointList.Clear();
				}
				bool flag4 = mindMarks.MarkList != null;
				if (flag4)
				{
					this._mindMarkTimeList.AddRange(mindMarks.MarkList);
					this._removedMarks.MindMarkList.AddRange(markCollection.MindMarkList);
					mindMarks.MarkList.Clear();
					markCollection.MindMarkList.Clear();
				}
				base.CombatChar.SetFlawCount(flawCounts, context);
				base.CombatChar.SetFlawCollection(flaws, context);
				base.CombatChar.SetAcupointCount(acupointCounts, context);
				base.CombatChar.SetAcupointCollection(acupoints, context);
				base.CombatChar.SetMindMarkTime(mindMarks, context);
				base.CombatChar.SetDefeatMarkCollection(markCollection, context);
			}
			this._removedMarks.FatalDamageMarkCount = base.CombatChar.GetDefeatMarkCollection().FatalDamageMarkCount;
			DomainManager.Combat.RemoveFatalDamageMark(context, base.CombatChar, base.CombatChar.GetDefeatMarkCollection().FatalDamageMarkCount);
			bool flag5 = this._removedMarks.GetTotalCount() > 0;
			if (flag5)
			{
				Events.RegisterHandler_CombatStateMachineUpdateEnd(new Events.OnCombatStateMachineUpdateEnd(this.OnStateMachineUpdateEnd));
			}
		}

		// Token: 0x06002CC0 RID: 11456 RVA: 0x0020913C File Offset: 0x0020733C
		private void OnStateMachineUpdateEnd(DataContext context, CombatCharacter combatChar)
		{
			int totalCount = this._removedMarks.GetTotalCount();
			int addTrickCount = totalCount * 50 / 100;
			int transferCount = totalCount - addTrickCount;
			bool flag = transferCount > 0;
			if (flag)
			{
				List<DefeatMarkKey> pool = new List<DefeatMarkKey>(this._removedMarks.GetAllKeysWithoutOld());
				foreach (DefeatMarkKey markKey in RandomUtils.GetRandomUnrepeated<DefeatMarkKey>(context.Random, transferCount, pool, null))
				{
					this.ApplyMarkKey(context, markKey);
				}
				base.EnemyChar.SetMindMarkTime(base.EnemyChar.GetMindMarkTime(), context);
				base.EnemyChar.GetDefeatMarkCollection().SyncMindMark(context, base.EnemyChar);
				DomainManager.Combat.UpdateBodyDefeatMark(context, base.EnemyChar);
				DomainManager.Combat.AddToCheckFallenSet(base.EnemyChar.GetId());
				base.ShowSpecialEffectTips(0);
			}
			bool flag2 = addTrickCount > 0;
			if (flag2)
			{
				DomainManager.Combat.AddTrick(context, base.CombatChar, 12, addTrickCount, true, false);
				base.ShowSpecialEffectTips(1);
			}
			Events.UnRegisterHandler_CombatStateMachineUpdateEnd(new Events.OnCombatStateMachineUpdateEnd(this.OnStateMachineUpdateEnd));
		}

		// Token: 0x06002CC1 RID: 11457 RVA: 0x00209274 File Offset: 0x00207474
		private void ApplyMarkKey(DataContext context, DefeatMarkKey markKey)
		{
			EMarkType type = markKey.Type;
			bool flag = type <= EMarkType.Inner;
			bool flag2 = flag;
			if (flag2)
			{
				bool inner = markKey.Type == EMarkType.Inner;
				DomainManager.Combat.AddInjury(context, base.EnemyChar, markKey.BodyPart, inner, 1, false, false);
			}
			else
			{
				type = markKey.Type;
				flag = (type - EMarkType.Flaw <= 1);
				bool flag3 = flag;
				if (flag3)
				{
					bool flaw = markKey.Type == EMarkType.Flaw;
					Dictionary<sbyte, List<ValueTuple<sbyte, int, int>>> dict = flaw ? this._flawTimeDict : this._acupointTimeDict;
					int index = context.Random.Next(0, dict[markKey.BodyPart].Count);
					ValueTuple<sbyte, int, int> valueTuple = this._flawTimeDict[markKey.BodyPart][index];
					sbyte level = valueTuple.Item1;
					int left = valueTuple.Item2;
					int total = valueTuple.Item3;
					dict[markKey.BodyPart].RemoveAt(index);
					base.EnemyChar.AddOrUpdateFlawOrAcupoint(context, markKey.BodyPart, flaw, level, true, left, total);
				}
				else
				{
					bool flag4 = markKey.Type == EMarkType.Mind;
					if (flag4)
					{
						int index2 = context.Random.Next(0, this._mindMarkTimeList.Count);
						SilenceFrameData mindMark = this._mindMarkTimeList[index2];
						this._mindMarkTimeList.RemoveAt(index2);
						CombatCharacter enemyChar = base.EnemyChar;
						MindMarkList mindMarkTime = enemyChar.GetMindMarkTime();
						List<SilenceFrameData> list;
						if ((list = mindMarkTime.MarkList) == null)
						{
							list = (mindMarkTime.MarkList = new List<SilenceFrameData>());
						}
						List<SilenceFrameData> markList = list;
						short keepTime = GlobalConfig.Instance.MindMarkBaseKeepTime;
						markList.Add(mindMark.Infinity ? SilenceFrameData.Create((int)keepTime) : mindMark);
					}
					else
					{
						bool flag5 = markKey.Type == EMarkType.Fatal;
						if (flag5)
						{
							DomainManager.Combat.AppendFatalDamageMark(context, base.EnemyChar, 1, -1, -1, false, EDamageType.None);
						}
						else
						{
							short predefinedLogId = 7;
							object arg = base.EffectId;
							DefaultInterpolatedStringHandler defaultInterpolatedStringHandler = new DefaultInterpolatedStringHandler(24, 1);
							defaultInterpolatedStringHandler.AppendLiteral("Cannot analysis markKey ");
							defaultInterpolatedStringHandler.AppendFormatted<DefeatMarkKey>(markKey);
							PredefinedLog.Show(predefinedLogId, arg, defaultInterpolatedStringHandler.ToStringAndClear());
						}
					}
				}
			}
		}

		// Token: 0x04000D7B RID: 3451
		private const sbyte TransferMarkPercent = 50;

		// Token: 0x04000D7C RID: 3452
		private DefeatMarkCollection _removedMarks;

		// Token: 0x04000D7D RID: 3453
		[TupleElementNames(new string[]
		{
			"level",
			"totalFrame",
			"leftFrame"
		})]
		private Dictionary<sbyte, List<ValueTuple<sbyte, int, int>>> _flawTimeDict;

		// Token: 0x04000D7E RID: 3454
		[TupleElementNames(new string[]
		{
			"level",
			"totalFrame",
			"leftFrame"
		})]
		private Dictionary<sbyte, List<ValueTuple<sbyte, int, int>>> _acupointTimeDict;

		// Token: 0x04000D7F RID: 3455
		private List<SilenceFrameData> _mindMarkTimeList;
	}
}
