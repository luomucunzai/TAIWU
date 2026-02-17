using System;
using GameData.Combat.Math;
using GameData.Common;
using GameData.Domains.Combat;
using GameData.Domains.CombatSkill;
using GameData.Domains.SpecialEffect.CombatSkill.Common.Assist;
using GameData.Utilities;

namespace GameData.Domains.SpecialEffect.CombatSkill.Jingangzong.DefenseAndAssist
{
	// Token: 0x020004C9 RID: 1225
	public class QiLunGanYingFa : AssistSkillBase
	{
		// Token: 0x06003D41 RID: 15681 RVA: 0x00250EA3 File Offset: 0x0024F0A3
		static QiLunGanYingFa()
		{
			SpecialEffectDomain.RegisterResetHandler(new Action(QiLunGanYingFa.StateEffect.Reset));
		}

		// Token: 0x06003D42 RID: 15682 RVA: 0x00250EC5 File Offset: 0x0024F0C5
		public QiLunGanYingFa()
		{
		}

		// Token: 0x06003D43 RID: 15683 RVA: 0x00250ECF File Offset: 0x0024F0CF
		public QiLunGanYingFa(CombatSkillKey skillKey) : base(skillKey, 11703)
		{
			this.SetConstAffectingOnCombatBegin = true;
		}

		// Token: 0x06003D44 RID: 15684 RVA: 0x00250EE6 File Offset: 0x0024F0E6
		public override void OnEnable(DataContext context)
		{
			base.OnEnable(context);
			QiLunGanYingFa.StateEffect.Setup();
		}

		// Token: 0x06003D45 RID: 15685 RVA: 0x00250EFC File Offset: 0x0024F0FC
		public override void OnDataAdded(DataContext context)
		{
			base.OnDataAdded(context);
			bool isDirect = base.IsDirect;
			if (isDirect)
			{
				base.AppendAffectedData(context, base.CharacterId, 167, EDataModifyType.TotalPercent, -1);
			}
			else
			{
				base.AppendAffectedAllEnemyData(context, 167, EDataModifyType.TotalPercent, -1);
			}
		}

		// Token: 0x06003D46 RID: 15686 RVA: 0x00250F42 File Offset: 0x0024F142
		public override void OnDisable(DataContext context)
		{
			QiLunGanYingFa.StateEffect.Close();
			base.OnDisable(context);
		}

		// Token: 0x06003D47 RID: 15687 RVA: 0x00250F58 File Offset: 0x0024F158
		protected override void OnCanUseChanged(DataContext context, DataUid dataUid)
		{
			base.SetConstAffecting(context, base.CanAffect);
		}

		// Token: 0x06003D48 RID: 15688 RVA: 0x00250F6C File Offset: 0x0024F16C
		public override int GetModifyValue(AffectedDataKey dataKey, int currModifyValue)
		{
			bool flag = !base.CanAffect;
			int result;
			if (flag)
			{
				result = 0;
			}
			else
			{
				bool flag2 = !base.IsDirect && !base.IsCurrent;
				if (flag2)
				{
					result = 0;
				}
				else
				{
					bool flag3 = dataKey.FieldId != 167;
					if (flag3)
					{
						result = 0;
					}
					else
					{
						bool flag4 = base.IsDirect ? (dataKey.CharId != base.CharacterId) : (dataKey.CharId == base.CharacterId);
						if (flag4)
						{
							result = 0;
						}
						else
						{
							sbyte stateType = (sbyte)dataKey.CustomParam0;
							bool flag5 = stateType != (base.IsDirect ? 1 : 2);
							if (flag5)
							{
								result = 0;
							}
							else
							{
								BoolArray8 extraParam = (byte)dataKey.CustomParam2;
								bool isReducePower = extraParam[0];
								bool isFirst = extraParam[1];
								bool flag6 = isReducePower;
								if (flag6)
								{
									result = 0;
								}
								else
								{
									int stateId = dataKey.CustomParam1;
									bool flag7 = stateId - 166 <= 1;
									bool flag8 = !flag7;
									if (flag8)
									{
										DataContext context = base.CombatChar.GetDataContext();
										CombatCharacter affectChar = DomainManager.Combat.GetElement_CombatCharacterDict(dataKey.CharId);
										DomainManager.Combat.AddCombatState(context, affectChar, stateType, base.IsDirect ? 166 : 167, 50);
										base.ShowEffectTips(context);
									}
									base.ShowSpecialEffectTipsOnceInFrame(0);
									result = (isFirst ? 100 : 50);
								}
							}
						}
					}
				}
			}
			return result;
		}

		// Token: 0x04001205 RID: 4613
		private static readonly QiLunGanYingFaStateEffect StateEffect = new QiLunGanYingFaStateEffect();

		// Token: 0x04001206 RID: 4614
		private const sbyte ChangePowerPercentTheFirst = 100;

		// Token: 0x04001207 RID: 4615
		private const sbyte ChangePowerPercentTheAfter = 50;

		// Token: 0x04001208 RID: 4616
		private const sbyte StatePowerUnit = 50;
	}
}
