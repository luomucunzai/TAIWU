using System;
using GameData.Combat.Math;
using GameData.Common;
using GameData.DomainEvents;
using GameData.Domains.CombatSkill;

namespace GameData.Domains.SpecialEffect.CombatSkill.Wudangpai.Sword
{
	// Token: 0x020003C1 RID: 961
	public class WuDangXiaoYaoJian : CombatSkillEffectBase
	{
		// Token: 0x06003747 RID: 14151 RVA: 0x00234BD0 File Offset: 0x00232DD0
		public WuDangXiaoYaoJian()
		{
		}

		// Token: 0x06003748 RID: 14152 RVA: 0x00234BDA File Offset: 0x00232DDA
		public WuDangXiaoYaoJian(CombatSkillKey skillKey) : base(skillKey, 4201, -1)
		{
		}

		// Token: 0x06003749 RID: 14153 RVA: 0x00234BEC File Offset: 0x00232DEC
		public override void OnEnable(DataContext context)
		{
			base.CreateAffectedData(175, EDataModifyType.Custom, -1);
			DomainManager.Combat.AddMoveDistInSkillPrepare(base.CombatChar, 20, base.IsDirect);
			base.ShowSpecialEffectTips(0);
			Events.RegisterHandler_CastSkillEnd(new Events.OnCastSkillEnd(this.OnCastSkillEnd));
		}

		// Token: 0x0600374A RID: 14154 RVA: 0x00234C3B File Offset: 0x00232E3B
		public override void OnDisable(DataContext context)
		{
			Events.UnRegisterHandler_CastSkillEnd(new Events.OnCastSkillEnd(this.OnCastSkillEnd));
		}

		// Token: 0x0600374B RID: 14155 RVA: 0x00234C50 File Offset: 0x00232E50
		private void OnCastSkillEnd(DataContext context, int charId, bool isAlly, short skillId, sbyte power, bool interrupted)
		{
			bool flag = charId != base.CharacterId || skillId != base.SkillTemplateId;
			if (!flag)
			{
				base.RemoveSelf(context);
			}
		}

		// Token: 0x0600374C RID: 14156 RVA: 0x00234C88 File Offset: 0x00232E88
		public override int GetModifiedValue(AffectedDataKey dataKey, int dataValue)
		{
			bool flag = dataKey.CharId != base.CharacterId || dataKey.FieldId != 175;
			int result;
			if (flag)
			{
				result = dataValue;
			}
			else
			{
				result = 0;
			}
			return result;
		}

		// Token: 0x04001025 RID: 4133
		private const sbyte MoveDistInCast = 20;
	}
}
