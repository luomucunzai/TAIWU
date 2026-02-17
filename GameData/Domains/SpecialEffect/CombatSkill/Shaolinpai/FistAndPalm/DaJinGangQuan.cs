using System;
using GameData.Combat.Math;
using GameData.Common;
using GameData.DomainEvents;
using GameData.Domains.Character;
using GameData.Domains.CombatSkill;

namespace GameData.Domains.SpecialEffect.CombatSkill.Shaolinpai.FistAndPalm
{
	// Token: 0x02000423 RID: 1059
	public class DaJinGangQuan : CombatSkillEffectBase
	{
		// Token: 0x0600395E RID: 14686 RVA: 0x0023E1C1 File Offset: 0x0023C3C1
		public DaJinGangQuan()
		{
		}

		// Token: 0x0600395F RID: 14687 RVA: 0x0023E1CB File Offset: 0x0023C3CB
		public DaJinGangQuan(CombatSkillKey skillKey) : base(skillKey, 1104, -1)
		{
		}

		// Token: 0x06003960 RID: 14688 RVA: 0x0023E1DC File Offset: 0x0023C3DC
		public override void OnEnable(DataContext context)
		{
			Events.RegisterHandler_PrepareSkillEnd(new Events.OnPrepareSkillEnd(this.OnPrepareSkillEnd));
			Events.RegisterHandler_CastSkillEnd(new Events.OnCastSkillEnd(this.OnCastSkillEnd));
		}

		// Token: 0x06003961 RID: 14689 RVA: 0x0023E203 File Offset: 0x0023C403
		public override void OnDisable(DataContext context)
		{
			Events.UnRegisterHandler_PrepareSkillEnd(new Events.OnPrepareSkillEnd(this.OnPrepareSkillEnd));
			Events.UnRegisterHandler_CastSkillEnd(new Events.OnCastSkillEnd(this.OnCastSkillEnd));
		}

		// Token: 0x06003962 RID: 14690 RVA: 0x0023E22C File Offset: 0x0023C42C
		private void OnPrepareSkillEnd(DataContext context, int charId, bool isAlly, short skillId)
		{
			bool flag = charId != base.CharacterId || skillId != base.SkillTemplateId || !DomainManager.Combat.InAttackRange(base.CombatChar);
			if (!flag)
			{
				MainAttributes selfAttributes = this.CharObj.GetMaxMainAttributes();
				MainAttributes enemyAttributes = base.CurrEnemyChar.GetCharacter().GetMaxMainAttributes();
				bool flag2 = selfAttributes.Items.FixedElementField > enemyAttributes.Items.FixedElementField;
				if (flag2)
				{
					OuterAndInnerInts penetrations = this.CharObj.GetPenetrations();
					this._transferPenetrate = (base.IsDirect ? penetrations.Inner : penetrations.Outer) * 40 / 100;
					base.AppendAffectedData(context, base.CharacterId, 44, EDataModifyType.Add, -1);
					base.AppendAffectedData(context, base.CharacterId, 45, EDataModifyType.Add, -1);
					base.ShowSpecialEffectTips(0);
				}
			}
		}

		// Token: 0x06003963 RID: 14691 RVA: 0x0023E308 File Offset: 0x0023C508
		private void OnCastSkillEnd(DataContext context, int charId, bool isAlly, short skillId, sbyte power, bool interrupted)
		{
			bool flag = charId != base.CharacterId || skillId != base.SkillTemplateId;
			if (!flag)
			{
				MainAttributes selfAttributes = this.CharObj.GetCurrMainAttributes();
				bool flag2 = base.PowerMatchAffectRequire((int)power, 0) && selfAttributes.Items.FixedElementField >= 10;
				if (flag2)
				{
					this.CharObj.ChangeCurrMainAttribute(context, 0, -10);
					DomainManager.Combat.AddCombatState(context, base.CombatChar, 1, base.IsDirect ? 4 : 6, 250);
					DomainManager.Combat.AddCombatState(context, base.CombatChar, 2, base.IsDirect ? 5 : 7, 250);
					base.ShowSpecialEffectTips(1);
				}
				base.RemoveSelf(context);
			}
		}

		// Token: 0x06003964 RID: 14692 RVA: 0x0023E3D4 File Offset: 0x0023C5D4
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
					result = this._transferPenetrate;
				}
				else
				{
					bool flag3 = dataKey.FieldId == (base.IsDirect ? 45 : 44);
					if (flag3)
					{
						result = -this._transferPenetrate;
					}
					else
					{
						result = 0;
					}
				}
			}
			return result;
		}

		// Token: 0x040010C1 RID: 4289
		private const sbyte TransferPenetratePercent = 40;

		// Token: 0x040010C2 RID: 4290
		private const sbyte CostAttribute = 10;

		// Token: 0x040010C3 RID: 4291
		private const short StatePower = 250;

		// Token: 0x040010C4 RID: 4292
		private int _transferPenetrate;
	}
}
