using System;
using System.Collections.Generic;
using GameData.Combat.Math;
using GameData.Common;
using GameData.DomainEvents;
using GameData.Domains.CombatSkill;

namespace GameData.Domains.SpecialEffect.CombatSkill.XiangShu.Attack.RanChenZi
{
	// Token: 0x020002EA RID: 746
	public class JiuJianGuiShenXia : CombatSkillEffectBase
	{
		// Token: 0x06003342 RID: 13122 RVA: 0x00224094 File Offset: 0x00222294
		public JiuJianGuiShenXia()
		{
		}

		// Token: 0x06003343 RID: 13123 RVA: 0x0022409E File Offset: 0x0022229E
		public JiuJianGuiShenXia(CombatSkillKey skillKey) : base(skillKey, 17135, -1)
		{
		}

		// Token: 0x06003344 RID: 13124 RVA: 0x002240B0 File Offset: 0x002222B0
		public override void OnEnable(DataContext context)
		{
			DomainManager.Combat.AddMoveDistInSkillPrepare(base.CombatChar, 1000, true);
			DomainManager.Combat.AddMoveDistInSkillPrepare(base.CombatChar, 1000, false);
			base.CombatChar.CanNormalAttackInPrepareSkill = true;
			this.AffectDatas = new Dictionary<AffectedDataKey, EDataModifyType>();
			this.AffectDatas.Add(new AffectedDataKey(base.CharacterId, 145, base.SkillTemplateId, -1, -1, -1), EDataModifyType.Add);
			this.AffectDatas.Add(new AffectedDataKey(base.CharacterId, 146, base.SkillTemplateId, -1, -1, -1), EDataModifyType.Add);
			this.AffectDatas.Add(new AffectedDataKey(base.CharacterId, 217, base.SkillTemplateId, -1, -1, -1), EDataModifyType.Custom);
			this.AffectDatas.Add(new AffectedDataKey(base.CharacterId, 215, base.SkillTemplateId, -1, -1, -1), EDataModifyType.Custom);
			Events.RegisterHandler_PrepareSkillBegin(new Events.OnPrepareSkillBegin(this.OnPrepareSkillBegin));
			Events.RegisterHandler_CastSkillEnd(new Events.OnCastSkillEnd(this.OnCastSkillEnd));
		}

		// Token: 0x06003345 RID: 13125 RVA: 0x002241BF File Offset: 0x002223BF
		public override void OnDisable(DataContext context)
		{
			base.CombatChar.CanNormalAttackInPrepareSkill = false;
			Events.UnRegisterHandler_PrepareSkillBegin(new Events.OnPrepareSkillBegin(this.OnPrepareSkillBegin));
			Events.UnRegisterHandler_CastSkillEnd(new Events.OnCastSkillEnd(this.OnCastSkillEnd));
		}

		// Token: 0x06003346 RID: 13126 RVA: 0x002241F4 File Offset: 0x002223F4
		private void OnPrepareSkillBegin(DataContext context, int charId, bool isAlly, short skillId)
		{
			bool flag = charId != base.CharacterId || skillId != base.SkillTemplateId;
			if (!flag)
			{
				sbyte taskStatus = DomainManager.World.GetElement_XiangshuAvatarTaskStatuses(8).JuniorXiangshuTaskStatus;
				bool flag2 = taskStatus > 4;
				if (flag2)
				{
					bool goodEnding = taskStatus == 6;
					bool flag3 = goodEnding;
					if (flag3)
					{
						base.CombatChar.SkillPrepareTotalProgress /= 2;
					}
					else
					{
						base.CombatChar.SkillPrepareTotalProgress = base.CombatChar.SkillPrepareTotalProgress * 150 / 100;
					}
					base.ShowSpecialEffectTips(goodEnding, 1, 2);
				}
			}
		}

		// Token: 0x06003347 RID: 13127 RVA: 0x00224288 File Offset: 0x00222488
		private void OnCastSkillEnd(DataContext context, int charId, bool isAlly, short skillId, sbyte power, bool interrupted)
		{
			bool flag = charId != base.CharacterId || skillId != base.SkillTemplateId;
			if (!flag)
			{
				DomainManager.Combat.AddToCheckFallenSet(base.CurrEnemyChar.GetId());
				bool flag2 = !DomainManager.Combat.CheckFallenImmediate(context);
				if (flag2)
				{
					base.CombatChar.ForceDefeat = true;
					base.CombatChar.Immortal = false;
					DomainManager.Combat.AddToCheckFallenSet(base.CombatChar.GetId());
				}
				base.RemoveSelf(context);
			}
		}

		// Token: 0x06003348 RID: 13128 RVA: 0x00224318 File Offset: 0x00222518
		public override int GetModifyValue(AffectedDataKey dataKey, int currModifyValue)
		{
			bool flag = dataKey.CharId != base.CharacterId || dataKey.CombatSkillId != base.SkillTemplateId;
			int result;
			if (flag)
			{
				result = 0;
			}
			else
			{
				bool flag2 = dataKey.FieldId == 145 || dataKey.FieldId == 146;
				if (flag2)
				{
					result = 10000;
				}
				else
				{
					result = 0;
				}
			}
			return result;
		}

		// Token: 0x06003349 RID: 13129 RVA: 0x00224380 File Offset: 0x00222580
		public override bool GetModifiedValue(AffectedDataKey dataKey, bool dataValue)
		{
			bool flag = dataKey.CharId != base.CharacterId || dataKey.CombatSkillId != base.SkillTemplateId;
			bool result;
			if (flag)
			{
				result = dataValue;
			}
			else
			{
				bool flag2 = dataKey.FieldId == 217 || dataKey.FieldId == 215;
				result = (!flag2 && dataValue);
			}
			return result;
		}
	}
}
