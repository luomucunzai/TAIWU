using System;
using System.Collections.Generic;
using GameData.Combat.Math;
using GameData.Common;
using GameData.Domains.Combat;
using GameData.Domains.CombatSkill;
using GameData.Domains.SpecialEffect.CombatSkill.Common.Defense;
using GameData.Utilities;

namespace GameData.Domains.SpecialEffect.CombatSkill.Baihuagu.DefenseAndAssist
{
	// Token: 0x020005DA RID: 1498
	public class ShiErJingDaNuoYi : DefenseSkillBase
	{
		// Token: 0x06004446 RID: 17478 RVA: 0x0026ED7F File Offset: 0x0026CF7F
		public ShiErJingDaNuoYi()
		{
		}

		// Token: 0x06004447 RID: 17479 RVA: 0x0026ED89 File Offset: 0x0026CF89
		public ShiErJingDaNuoYi(CombatSkillKey skillKey) : base(skillKey, 3505)
		{
		}

		// Token: 0x06004448 RID: 17480 RVA: 0x0026ED99 File Offset: 0x0026CF99
		public override void OnEnable(DataContext context)
		{
			base.OnEnable(context);
			base.CreateAffectedData(134, EDataModifyType.Custom, -1);
			base.CreateAffectedData(300, EDataModifyType.AddPercent, -1);
			base.ShowSpecialEffectTips(1);
		}

		// Token: 0x06004449 RID: 17481 RVA: 0x0026EDC8 File Offset: 0x0026CFC8
		public override int GetModifyValue(AffectedDataKey dataKey, int currModifyValue)
		{
			bool flag = dataKey.CharId != base.CharacterId || dataKey.FieldId != 300 || !base.CanAffect;
			int result;
			if (flag)
			{
				result = 0;
			}
			else
			{
				result = 200;
			}
			return result;
		}

		// Token: 0x0600444A RID: 17482 RVA: 0x0026EE10 File Offset: 0x0026D010
		public override int GetModifiedValue(AffectedDataKey dataKey, int dataValue)
		{
			bool flag = dataKey.CharId != base.CharacterId || dataValue <= 0 || !base.CanAffect;
			int result;
			if (flag)
			{
				result = dataValue;
			}
			else
			{
				DataContext context = DomainManager.Combat.Context;
				sbyte bodyPart = (sbyte)dataKey.CustomParam0;
				CombatCharacter affectChar = base.IsDirect ? base.CombatChar : base.CurrEnemyChar;
				byte[] acupointCount = affectChar.GetAcupointCount();
				bool flag2 = bodyPart < 0;
				if (flag2)
				{
					List<sbyte> partRandomPool = ObjectPool<List<sbyte>>.Instance.Get();
					partRandomPool.Clear();
					for (sbyte part = 0; part < 7; part += 1)
					{
						bool flag3 = !base.IsDirect || acupointCount[(int)part] > 0;
						if (flag3)
						{
							partRandomPool.Add(part);
						}
					}
					bool flag4 = partRandomPool.Count > 0;
					if (flag4)
					{
						bodyPart = partRandomPool[context.Random.Next(partRandomPool.Count)];
					}
					ObjectPool<List<sbyte>>.Instance.Return(partRandomPool);
				}
				bool flag5 = bodyPart < 0;
				if (flag5)
				{
					result = 0;
				}
				else
				{
					bool isDirect = base.IsDirect;
					if (isDirect)
					{
						bool flag6 = acupointCount[(int)bodyPart] > 0;
						if (flag6)
						{
							DomainManager.Combat.RemoveAcupoint(context, affectChar, bodyPart, 0, false, true);
							base.ShowSpecialEffectTips(0);
						}
					}
					else
					{
						DomainManager.Combat.AddAcupoint(context, affectChar, (sbyte)dataKey.CustomParam1, new CombatSkillKey(-1, -1), bodyPart, 1, false);
						base.ShowSpecialEffectTips(0);
					}
					result = 0;
				}
			}
			return result;
		}

		// Token: 0x04001440 RID: 5184
		private const int HealAcupointSpeedAddPercent = 200;
	}
}
