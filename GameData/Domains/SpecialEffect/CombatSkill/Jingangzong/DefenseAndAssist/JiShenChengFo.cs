using System;
using System.Collections.Generic;
using GameData.Combat.Math;
using GameData.Common;
using GameData.Domains.Character;
using GameData.Domains.Combat;
using GameData.Domains.CombatSkill;
using GameData.Domains.SpecialEffect.CombatSkill.Common.Defense;
using GameData.GameDataBridge;

namespace GameData.Domains.SpecialEffect.CombatSkill.Jingangzong.DefenseAndAssist
{
	// Token: 0x020004C5 RID: 1221
	public class JiShenChengFo : DefenseSkillBase
	{
		// Token: 0x06003D26 RID: 15654 RVA: 0x002503B3 File Offset: 0x0024E5B3
		public JiShenChengFo()
		{
		}

		// Token: 0x06003D27 RID: 15655 RVA: 0x002503C8 File Offset: 0x0024E5C8
		public JiShenChengFo(CombatSkillKey skillKey) : base(skillKey, 11607)
		{
		}

		// Token: 0x06003D28 RID: 15656 RVA: 0x002503E4 File Offset: 0x0024E5E4
		public override void OnEnable(DataContext context)
		{
			base.OnEnable(context);
			base.CreateAffectedData(191, EDataModifyType.Custom, -1);
			base.CreateAffectedData(192, EDataModifyType.Custom, -1);
			this._lastMarks = new DefeatMarkCollection(base.CombatChar.GetDefeatMarkCollection());
			this._defeatMarkUid = base.ParseCombatCharacterDataUid(50);
			GameDataBridge.AddPostDataModificationHandler(this._defeatMarkUid, base.DataHandlerKey, new Action<DataContext, DataUid>(this.OnDefeatMarkChanged));
		}

		// Token: 0x06003D29 RID: 15657 RVA: 0x00250458 File Offset: 0x0024E658
		public override void OnDisable(DataContext context)
		{
			base.OnDisable(context);
			GameDataBridge.RemovePostDataModificationHandler(this._defeatMarkUid, base.DataHandlerKey);
		}

		// Token: 0x06003D2A RID: 15658 RVA: 0x00250478 File Offset: 0x0024E678
		private unsafe void OnDefeatMarkChanged(DataContext context, DataUid dataUid)
		{
			bool flag = !base.CanAffect;
			if (!flag)
			{
				NeiliAllocation neiliAllocation = base.CombatChar.GetNeiliAllocation();
				byte neiliAllocationType = 2;
				int canHealCount = (int)(*(ref neiliAllocation.Items.FixedElementField + (IntPtr)neiliAllocationType * 2) / 3);
				bool flag2 = canHealCount <= 0;
				if (flag2)
				{
					this.UpdateLastDefeatMarks();
				}
				else
				{
					DefeatMarkCollection markCollection = base.CombatChar.GetDefeatMarkCollection();
					this._newMarkList.Clear();
					bool isDirect = base.IsDirect;
					if (isDirect)
					{
						Injuries newInjuries = base.CombatChar.GetInjuries().Subtract(base.CombatChar.GetOldInjuries());
						for (sbyte part = 0; part < 7; part += 1)
						{
							int outerCount = Math.Min((int)newInjuries.Get(part, false), (int)(markCollection.OuterInjuryMarkList[(int)part] - this._lastMarks.OuterInjuryMarkList[(int)part]));
							for (int i = 0; i < outerCount; i++)
							{
								this._newMarkList.Add(new ValueTuple<sbyte, sbyte>(0, part));
							}
							int innerCount = Math.Min((int)newInjuries.Get(part, true), (int)(markCollection.InnerInjuryMarkList[(int)part] - this._lastMarks.InnerInjuryMarkList[(int)part]));
							for (int j = 0; j < innerCount; j++)
							{
								this._newMarkList.Add(new ValueTuple<sbyte, sbyte>(1, part));
							}
						}
					}
					else
					{
						for (sbyte part2 = 0; part2 < 7; part2 += 1)
						{
							int newFlawCount = markCollection.FlawMarkList[(int)part2].Count - this._lastMarks.FlawMarkList[(int)part2].Count;
							for (int k = 0; k < newFlawCount; k++)
							{
								this._newMarkList.Add(new ValueTuple<sbyte, sbyte>(2, part2));
							}
							int newAcupointCount = markCollection.AcupointMarkList[(int)part2].Count - this._lastMarks.AcupointMarkList[(int)part2].Count;
							for (int l = 0; l < newAcupointCount; l++)
							{
								this._newMarkList.Add(new ValueTuple<sbyte, sbyte>(3, part2));
							}
						}
						int newMindMarkCount = markCollection.MindMarkList.Count - this._lastMarks.MindMarkList.Count;
						for (int m = 0; m < newMindMarkCount; m++)
						{
							this._newMarkList.Add(new ValueTuple<sbyte, sbyte>(4, -1));
						}
					}
					this.UpdateLastDefeatMarks();
					bool flag3 = this._newMarkList.Count == 0;
					if (!flag3)
					{
						int n = 0;
						while (n < canHealCount && this._newMarkList.Count > 0)
						{
							int index = context.Random.Next(0, this._newMarkList.Count);
							ValueTuple<sbyte, sbyte> markInfo = this._newMarkList[index];
							this._newMarkList.RemoveAt(index);
							base.CombatChar.ChangeNeiliAllocation(context, neiliAllocationType, -3, true, true);
							DomainManager.Combat.ChangeDisorderOfQiRandomRecovery(context, base.CombatChar, 200, false);
							bool flag4 = markInfo.Item1 == 0;
							if (flag4)
							{
								DomainManager.Combat.RemoveInjury(context, base.CombatChar, markInfo.Item2, false, 1, true, false);
							}
							else
							{
								bool flag5 = markInfo.Item1 == 1;
								if (flag5)
								{
									DomainManager.Combat.RemoveInjury(context, base.CombatChar, markInfo.Item2, true, 1, true, false);
								}
								else
								{
									bool flag6 = markInfo.Item1 == 2;
									if (flag6)
									{
										DomainManager.Combat.RemoveFlaw(context, base.CombatChar, markInfo.Item2, (int)(base.CombatChar.GetFlawCount()[(int)markInfo.Item2] - 1), true, true);
									}
									else
									{
										bool flag7 = markInfo.Item1 == 3;
										if (flag7)
										{
											DomainManager.Combat.RemoveAcupoint(context, base.CombatChar, markInfo.Item2, (int)(base.CombatChar.GetAcupointCount()[(int)markInfo.Item2] - 1), true, true);
										}
										else
										{
											bool flag8 = markInfo.Item1 == 4;
											if (flag8)
											{
												DomainManager.Combat.RemoveMindDefeatMark(context, base.CombatChar, 1, false, base.CombatChar.GetMindMarkTime().MarkList.Count - 1);
											}
										}
									}
								}
							}
							n++;
						}
						base.ShowSpecialEffectTips(0);
					}
				}
			}
		}

		// Token: 0x06003D2B RID: 15659 RVA: 0x002508D0 File Offset: 0x0024EAD0
		private void UpdateLastDefeatMarks()
		{
			DefeatMarkCollection markCollection = base.CombatChar.GetDefeatMarkCollection();
			bool isDirect = base.IsDirect;
			if (isDirect)
			{
				for (sbyte part = 0; part < 7; part += 1)
				{
					this._lastMarks.OuterInjuryMarkList[(int)part] = markCollection.OuterInjuryMarkList[(int)part];
					this._lastMarks.InnerInjuryMarkList[(int)part] = markCollection.InnerInjuryMarkList[(int)part];
				}
			}
			else
			{
				for (sbyte part2 = 0; part2 < 7; part2 += 1)
				{
					this._lastMarks.FlawMarkList[(int)part2].Clear();
					this._lastMarks.FlawMarkList[(int)part2].AddRange(markCollection.FlawMarkList[(int)part2]);
					this._lastMarks.AcupointMarkList[(int)part2].Clear();
					this._lastMarks.AcupointMarkList[(int)part2].AddRange(markCollection.AcupointMarkList[(int)part2]);
				}
				this._lastMarks.MindMarkList.Clear();
				this._lastMarks.MindMarkList.AddRange(markCollection.MindMarkList);
			}
		}

		// Token: 0x06003D2C RID: 15660 RVA: 0x002509E0 File Offset: 0x0024EBE0
		public override int GetModifiedValue(AffectedDataKey dataKey, int dataValue)
		{
			bool flag = dataKey.CharId != base.CharacterId || !base.CanAffect || base.CombatChar.BeCriticalDuringCalcAddInjury;
			int result;
			if (flag)
			{
				result = dataValue;
			}
			else
			{
				ushort fieldId = dataKey.FieldId;
				bool flag2 = fieldId - 191 <= 1;
				bool flag3 = flag2;
				if (flag3)
				{
					result = 0;
				}
				else
				{
					result = dataValue;
				}
			}
			return result;
		}

		// Token: 0x040011FB RID: 4603
		private const sbyte RequireNeiliAllocation = 3;

		// Token: 0x040011FC RID: 4604
		private const short AddQiDisorder = 200;

		// Token: 0x040011FD RID: 4605
		private DataUid _defeatMarkUid;

		// Token: 0x040011FE RID: 4606
		private DefeatMarkCollection _lastMarks;

		// Token: 0x040011FF RID: 4607
		private readonly List<ValueTuple<sbyte, sbyte>> _newMarkList = new List<ValueTuple<sbyte, sbyte>>();
	}
}
