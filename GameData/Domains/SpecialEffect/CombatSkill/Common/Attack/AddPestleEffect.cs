using System;
using GameData.Common;
using GameData.DomainEvents;
using GameData.Domains.Combat;
using GameData.Domains.CombatSkill;
using GameData.Domains.Item;

namespace GameData.Domains.SpecialEffect.CombatSkill.Common.Attack
{
	// Token: 0x0200058A RID: 1418
	public class AddPestleEffect : CombatSkillEffectBase
	{
		// Token: 0x060041FA RID: 16890 RVA: 0x00264E3D File Offset: 0x0026303D
		protected AddPestleEffect()
		{
		}

		// Token: 0x060041FB RID: 16891 RVA: 0x00264E47 File Offset: 0x00263047
		protected AddPestleEffect(CombatSkillKey skillKey, int type) : base(skillKey, type, -1)
		{
		}

		// Token: 0x060041FC RID: 16892 RVA: 0x00264E54 File Offset: 0x00263054
		public override void OnEnable(DataContext context)
		{
			Events.RegisterHandler_CastSkillEnd(new Events.OnCastSkillEnd(this.OnCastSkillEnd));
		}

		// Token: 0x060041FD RID: 16893 RVA: 0x00264E69 File Offset: 0x00263069
		public override void OnDisable(DataContext context)
		{
			Events.UnRegisterHandler_CastSkillEnd(new Events.OnCastSkillEnd(this.OnCastSkillEnd));
		}

		// Token: 0x060041FE RID: 16894 RVA: 0x00264E80 File Offset: 0x00263080
		private void OnCastSkillEnd(DataContext context, int charId, bool isAlly, short skillId, sbyte power, bool interrupted)
		{
			bool flag = charId != base.CharacterId || skillId != base.SkillTemplateId;
			if (!flag)
			{
				bool flag2 = base.PowerMatchAffectRequire((int)power, 0);
				if (flag2)
				{
					ItemKey weaponKey = DomainManager.Combat.GetUsingWeaponKey(base.CombatChar);
					bool flag3 = ItemTemplateHelper.GetItemSubType(weaponKey.ItemType, weaponKey.TemplateId) == 5;
					if (flag3)
					{
						CombatWeaponData weaponData = DomainManager.Combat.GetElement_WeaponDataDict(weaponKey.Id);
						weaponData.SetPestleEffect(context, base.CharacterId, this.PestleEffectName, new SkillEffectKey(base.SkillTemplateId, base.IsDirect));
						base.ShowSpecialEffectTips(0);
					}
				}
				base.RemoveSelf(context);
			}
		}

		// Token: 0x04001379 RID: 4985
		protected string PestleEffectName;
	}
}
