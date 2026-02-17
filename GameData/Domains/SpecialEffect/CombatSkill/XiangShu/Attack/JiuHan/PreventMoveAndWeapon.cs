using System;
using GameData.Combat.Math;
using GameData.Common;
using GameData.DomainEvents;
using GameData.Domains.CombatSkill;

namespace GameData.Domains.SpecialEffect.CombatSkill.XiangShu.Attack.JiuHan
{
	// Token: 0x02000302 RID: 770
	public class PreventMoveAndWeapon : CombatSkillEffectBase
	{
		// Token: 0x060033C5 RID: 13253 RVA: 0x00226981 File Offset: 0x00224B81
		protected PreventMoveAndWeapon()
		{
		}

		// Token: 0x060033C6 RID: 13254 RVA: 0x0022698B File Offset: 0x00224B8B
		protected PreventMoveAndWeapon(CombatSkillKey skillKey, int type) : base(skillKey, type, -1)
		{
		}

		// Token: 0x060033C7 RID: 13255 RVA: 0x00226998 File Offset: 0x00224B98
		public override void OnEnable(DataContext context)
		{
			base.CreateAffectedAllEnemyData(151, EDataModifyType.Custom, -1);
			base.CreateAffectedAllEnemyData(197, EDataModifyType.AddPercent, -1);
			Events.RegisterHandler_CastSkillEnd(new Events.OnCastSkillEnd(this.OnCastSkillEnd));
		}

		// Token: 0x060033C8 RID: 13256 RVA: 0x002269C9 File Offset: 0x00224BC9
		public override void OnDisable(DataContext context)
		{
			Events.UnRegisterHandler_CastSkillEnd(new Events.OnCastSkillEnd(this.OnCastSkillEnd));
		}

		// Token: 0x060033C9 RID: 13257 RVA: 0x002269E0 File Offset: 0x00224BE0
		private void OnCastSkillEnd(DataContext context, int charId, bool isAlly, short skillId, sbyte power, bool interrupted)
		{
			bool flag = !this.SkillKey.IsMatch(charId, skillId) || !base.PowerMatchAffectRequire((int)power, 0);
			if (!flag)
			{
				base.AddMaxEffectCount(true);
				base.ShowSpecialEffectTips(0);
			}
		}

		// Token: 0x060033CA RID: 13258 RVA: 0x00226A24 File Offset: 0x00224C24
		public override int GetModifyValue(AffectedDataKey dataKey, int currModifyValue)
		{
			bool flag = base.EffectCount <= 0 || dataKey.FieldId != 197;
			int result;
			if (flag)
			{
				result = 0;
			}
			else
			{
				result = -50;
			}
			return result;
		}

		// Token: 0x060033CB RID: 13259 RVA: 0x00226A5C File Offset: 0x00224C5C
		public override int GetModifiedValue(AffectedDataKey dataKey, int dataValue)
		{
			bool flag = DomainManager.Combat.GetElement_CombatCharacterDict(dataKey.CharId).IsAlly == base.CombatChar.IsAlly || !DomainManager.Combat.IsCurrentCombatCharacter(base.CombatChar) || base.EffectCount <= 0;
			int result;
			if (flag)
			{
				result = dataValue;
			}
			else
			{
				bool flag2 = dataKey.FieldId == 151 && dataValue != 0;
				if (flag2)
				{
					int costEffectCount = Math.Min(Math.Abs(dataValue), base.EffectCount);
					base.ReduceEffectCount(costEffectCount);
					result = dataValue + ((dataValue > 0) ? (-costEffectCount) : costEffectCount);
				}
				else
				{
					result = dataValue;
				}
			}
			return result;
		}

		// Token: 0x04000F52 RID: 3922
		private const int ReduceMobilityRecoverSpeedPercent = -50;
	}
}
