using System;
using GameData.Combat.Math;
using GameData.Common;
using GameData.DomainEvents;
using GameData.Domains.Combat;
using GameData.Domains.CombatSkill;
using GameData.Utilities;

namespace GameData.Domains.SpecialEffect.CombatSkill.Jieqingmen.Throw
{
	// Token: 0x020004DD RID: 1245
	public class LuanFeiHuang : CombatSkillEffectBase
	{
		// Token: 0x06003DAB RID: 15787 RVA: 0x00252B7C File Offset: 0x00250D7C
		public LuanFeiHuang()
		{
		}

		// Token: 0x06003DAC RID: 15788 RVA: 0x00252B86 File Offset: 0x00250D86
		public LuanFeiHuang(CombatSkillKey skillKey) : base(skillKey, 13304, -1)
		{
		}

		// Token: 0x06003DAD RID: 15789 RVA: 0x00252B98 File Offset: 0x00250D98
		public override void OnEnable(DataContext context)
		{
			base.CreateAffectedData(199, EDataModifyType.TotalPercent, base.SkillTemplateId);
			Events.RegisterHandler_PrepareSkillBegin(new Events.OnPrepareSkillBegin(this.OnPrepareSkillBegin));
			Events.RegisterHandler_ChangeWeapon(new Events.OnChangeWeapon(this.OnChangeWeapon));
			Events.RegisterHandler_CastSkillEnd(new Events.OnCastSkillEnd(this.OnCastSkillEnd));
		}

		// Token: 0x06003DAE RID: 15790 RVA: 0x00252BEF File Offset: 0x00250DEF
		public override void OnDisable(DataContext context)
		{
			Events.UnRegisterHandler_PrepareSkillBegin(new Events.OnPrepareSkillBegin(this.OnPrepareSkillBegin));
			Events.UnRegisterHandler_ChangeWeapon(new Events.OnChangeWeapon(this.OnChangeWeapon));
			Events.UnRegisterHandler_CastSkillEnd(new Events.OnCastSkillEnd(this.OnCastSkillEnd));
		}

		// Token: 0x06003DAF RID: 15791 RVA: 0x00252C28 File Offset: 0x00250E28
		private void OnPrepareSkillBegin(DataContext context, int charId, bool isAlly, short skillId)
		{
			bool flag = !this.SkillKey.IsMatch(charId, skillId);
			if (!flag)
			{
				this.DoAddFlaw(context);
				bool flag2 = !this._autoCasting;
				if (!flag2)
				{
					DomainManager.Combat.ChangeSkillPrepareProgress(base.CombatChar, base.CombatChar.SkillPrepareTotalProgress * LuanFeiHuang.ProgressPercent);
				}
			}
		}

		// Token: 0x06003DB0 RID: 15792 RVA: 0x00252C8C File Offset: 0x00250E8C
		private void OnChangeWeapon(DataContext context, int charId, bool isAlly, CombatWeaponData newWeapon, CombatWeaponData oldWeapon)
		{
			bool flag = charId == base.CharacterId && newWeapon.Item.GetItemSubType() == 2;
			if (flag)
			{
				this.DoCastFree(context);
			}
		}

		// Token: 0x06003DB1 RID: 15793 RVA: 0x00252CC4 File Offset: 0x00250EC4
		private void OnCastSkillEnd(DataContext context, int charId, bool isAlly, short skillId, sbyte power, bool interrupted)
		{
			bool flag = !this.SkillKey.IsMatch(charId, skillId) || !this._autoCasting;
			if (!flag)
			{
				this._autoCasting = false;
				base.InvalidateCache(context, 199);
			}
		}

		// Token: 0x06003DB2 RID: 15794 RVA: 0x00252D08 File Offset: 0x00250F08
		private void DoAddFlaw(DataContext context)
		{
			int count = (base.IsDirect ? base.CombatChar : base.EnemyChar).GetContinueTricksAtStart(19);
			bool flag = count <= 0;
			if (!flag)
			{
				bool autoCasting = this._autoCasting;
				if (autoCasting)
				{
					count = Math.Max(count * LuanFeiHuang.ReduceFlawPercent, 1);
				}
				base.ShowSpecialEffectTips(0);
				for (int i = 0; i < count; i++)
				{
					sbyte level = LuanFeiHuang.RandomFlawLevels.GetRandom(context.Random);
					DomainManager.Combat.AddFlaw(context, base.EnemyChar, level, this.SkillKey, -1, 1, true);
				}
			}
		}

		// Token: 0x06003DB3 RID: 15795 RVA: 0x00252DA8 File Offset: 0x00250FA8
		private void DoCastFree(DataContext context)
		{
			bool flag = !DomainManager.Combat.CanCastSkill(base.CombatChar, base.SkillTemplateId, true, true);
			if (!flag)
			{
				this._autoCasting = true;
				base.InvalidateCache(context, 199);
				DomainManager.Combat.CastSkillFree(context, base.CombatChar, base.SkillTemplateId, ECombatCastFreePriority.Normal);
				base.ShowSpecialEffectTips(1);
			}
		}

		// Token: 0x06003DB4 RID: 15796 RVA: 0x00252E0C File Offset: 0x0025100C
		public override int GetModifyValue(AffectedDataKey dataKey, int currModifyValue)
		{
			bool flag = dataKey.SkillKey == this.SkillKey && dataKey.FieldId == 199 && this._autoCasting;
			int result;
			if (flag)
			{
				result = -50;
			}
			else
			{
				result = base.GetModifyValue(dataKey, currModifyValue);
			}
			return result;
		}

		// Token: 0x04001229 RID: 4649
		private const int ReducePowerPercent = -50;

		// Token: 0x0400122A RID: 4650
		private static readonly CValuePercentBonus ReduceFlawPercent = -50;

		// Token: 0x0400122B RID: 4651
		private static readonly CValuePercent ProgressPercent = 50;

		// Token: 0x0400122C RID: 4652
		private static readonly sbyte[] RandomFlawLevels = new sbyte[]
		{
			0,
			1
		};

		// Token: 0x0400122D RID: 4653
		private bool _autoCasting;
	}
}
