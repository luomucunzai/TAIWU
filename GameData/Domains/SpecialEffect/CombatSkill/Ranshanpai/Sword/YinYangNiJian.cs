using System;
using GameData.Combat.Math;
using GameData.Common;
using GameData.DomainEvents;
using GameData.Domains.Character;
using GameData.Domains.CombatSkill;

namespace GameData.Domains.SpecialEffect.CombatSkill.Ranshanpai.Sword
{
	// Token: 0x02000448 RID: 1096
	public class YinYangNiJian : CombatSkillEffectBase
	{
		// Token: 0x06003A37 RID: 14903 RVA: 0x002429B0 File Offset: 0x00240BB0
		public YinYangNiJian()
		{
		}

		// Token: 0x06003A38 RID: 14904 RVA: 0x002429BA File Offset: 0x00240BBA
		public YinYangNiJian(CombatSkillKey skillKey) : base(skillKey, 7206, -1)
		{
		}

		// Token: 0x06003A39 RID: 14905 RVA: 0x002429CB File Offset: 0x00240BCB
		public override void OnEnable(DataContext context)
		{
			Events.RegisterHandler_PrepareSkillEnd(new Events.OnPrepareSkillEnd(this.OnPrepareSkillEnd));
			Events.RegisterHandler_CastSkillEnd(new Events.OnCastSkillEnd(this.OnCastSkillEnd));
		}

		// Token: 0x06003A3A RID: 14906 RVA: 0x002429F2 File Offset: 0x00240BF2
		public override void OnDisable(DataContext context)
		{
			Events.UnRegisterHandler_PrepareSkillEnd(new Events.OnPrepareSkillEnd(this.OnPrepareSkillEnd));
			Events.UnRegisterHandler_CastSkillEnd(new Events.OnCastSkillEnd(this.OnCastSkillEnd));
		}

		// Token: 0x06003A3B RID: 14907 RVA: 0x00242A1C File Offset: 0x00240C1C
		private void OnPrepareSkillEnd(DataContext context, int charId, bool isAlly, short skillId)
		{
			bool flag = charId != base.CharacterId || skillId != base.SkillTemplateId || !DomainManager.Combat.InAttackRange(base.CombatChar) || base.SkillInstance.GetCurrInnerRatio() == (base.IsDirect ? 0 : 100);
			if (!flag)
			{
				OuterAndInnerInts selfPenetrate = this.CharObj.GetPenetrations();
				OuterAndInnerInts enemyPenetrateResist = base.CurrEnemyChar.GetCharacter().GetPenetrationResists();
				bool flag2 = base.IsDirect ? (selfPenetrate.Inner < enemyPenetrateResist.Inner) : (selfPenetrate.Outer < enemyPenetrateResist.Outer);
				if (flag2)
				{
					base.AppendAffectedData(context, base.CharacterId, base.IsDirect ? 44 : 45, EDataModifyType.AddPercent, -1);
					base.AppendAffectedData(context, base.CharacterId, 69, EDataModifyType.AddPercent, base.SkillTemplateId);
					base.ShowSpecialEffectTips(0);
				}
			}
		}

		// Token: 0x06003A3C RID: 14908 RVA: 0x00242AFC File Offset: 0x00240CFC
		private void OnCastSkillEnd(DataContext context, int charId, bool isAlly, short skillId, sbyte power, bool interrupted)
		{
			bool flag = charId != base.CharacterId || skillId != base.SkillTemplateId;
			if (!flag)
			{
				base.RemoveSelf(context);
			}
		}

		// Token: 0x06003A3D RID: 14909 RVA: 0x00242B34 File Offset: 0x00240D34
		public override int GetModifyValue(AffectedDataKey dataKey, int currModifyValue)
		{
			bool flag = dataKey.CharId != base.CharacterId;
			int result;
			if (flag)
			{
				result = 0;
			}
			else
			{
				bool flag2 = dataKey.FieldId == (base.IsDirect ? 44 : 45);
				if (flag2)
				{
					result = 80;
				}
				else
				{
					bool flag3 = dataKey.FieldId == 69 && dataKey.CombatSkillId == base.SkillTemplateId && dataKey.CustomParam0 == (base.IsDirect ? 0 : 1);
					if (flag3)
					{
						result = 80;
					}
					else
					{
						result = 0;
					}
				}
			}
			return result;
		}

		// Token: 0x04001109 RID: 4361
		private const sbyte AddPenetrate = 80;

		// Token: 0x0400110A RID: 4362
		private const sbyte AddDamage = 80;
	}
}
