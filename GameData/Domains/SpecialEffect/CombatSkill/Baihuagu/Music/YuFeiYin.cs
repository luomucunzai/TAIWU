using System;
using GameData.Common;
using GameData.DomainEvents;
using GameData.Domains.Character;
using GameData.Domains.Combat;
using GameData.Domains.CombatSkill;

namespace GameData.Domains.SpecialEffect.CombatSkill.Baihuagu.Music
{
	// Token: 0x020005C9 RID: 1481
	public class YuFeiYin : CombatSkillEffectBase
	{
		// Token: 0x060043DC RID: 17372 RVA: 0x0026D17F File Offset: 0x0026B37F
		public YuFeiYin()
		{
		}

		// Token: 0x060043DD RID: 17373 RVA: 0x0026D189 File Offset: 0x0026B389
		public YuFeiYin(CombatSkillKey skillKey) : base(skillKey, 3303, -1)
		{
		}

		// Token: 0x060043DE RID: 17374 RVA: 0x0026D19A File Offset: 0x0026B39A
		public override void OnEnable(DataContext context)
		{
			this.ChangeAllNeiliAllocation(context, 3);
			Events.RegisterHandler_CastSkillEnd(new Events.OnCastSkillEnd(this.OnCastSkillEnd));
		}

		// Token: 0x060043DF RID: 17375 RVA: 0x0026D1B8 File Offset: 0x0026B3B8
		public override void OnDisable(DataContext context)
		{
			Events.UnRegisterHandler_CastSkillEnd(new Events.OnCastSkillEnd(this.OnCastSkillEnd));
		}

		// Token: 0x060043E0 RID: 17376 RVA: 0x0026D1D0 File Offset: 0x0026B3D0
		private void OnCastSkillEnd(DataContext context, int charId, bool isAlly, short skillId, sbyte power, bool interrupted)
		{
			bool flag = charId != base.CharacterId || skillId != base.SkillTemplateId;
			if (!flag)
			{
				bool flag2 = base.PowerMatchAffectRequire((int)power, 0);
				if (flag2)
				{
					this.ChangeAllNeiliAllocation(context, 6);
				}
				base.RemoveSelf(context);
			}
		}

		// Token: 0x060043E1 RID: 17377 RVA: 0x0026D21C File Offset: 0x0026B41C
		private unsafe void ChangeAllNeiliAllocation(DataContext context, sbyte changeValue)
		{
			CombatCharacter changeChar = base.IsDirect ? base.CombatChar : base.CurrEnemyChar;
			NeiliAllocation currNeiliAllocation = changeChar.GetNeiliAllocation();
			NeiliAllocation originNeiliAllocation = changeChar.GetOriginNeiliAllocation();
			bool affected = false;
			for (byte type = 0; type < 4; type += 1)
			{
				bool flag = base.IsDirect ? (*(ref currNeiliAllocation.Items.FixedElementField + (IntPtr)type * 2) < *(ref originNeiliAllocation.Items.FixedElementField + (IntPtr)type * 2)) : (*(ref currNeiliAllocation.Items.FixedElementField + (IntPtr)type * 2) > *(ref originNeiliAllocation.Items.FixedElementField + (IntPtr)type * 2));
				if (flag)
				{
					changeChar.ChangeNeiliAllocation(context, type, (int)(base.IsDirect ? changeValue : (-(int)changeValue)), true, true);
					affected = true;
				}
			}
			bool flag2 = affected;
			if (flag2)
			{
				base.ShowSpecialEffectTips(0);
			}
		}

		// Token: 0x04001427 RID: 5159
		private const sbyte StartCastNeiliAllocation = 3;

		// Token: 0x04001428 RID: 5160
		private const sbyte FullPowerNeiliAllocation = 6;
	}
}
