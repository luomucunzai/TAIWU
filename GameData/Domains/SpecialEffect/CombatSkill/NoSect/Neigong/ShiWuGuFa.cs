using System;
using System.Collections.Generic;
using Config;
using GameData.Combat.Math;
using GameData.Common;
using GameData.DomainEvents;
using GameData.Domains.Combat;
using GameData.Domains.CombatSkill;

namespace GameData.Domains.SpecialEffect.CombatSkill.NoSect.Neigong
{
	// Token: 0x02000472 RID: 1138
	public class ShiWuGuFa : CombatSkillEffectBase
	{
		// Token: 0x06003B4C RID: 15180 RVA: 0x00247590 File Offset: 0x00245790
		private static bool IsAttack(short skillId)
		{
			return Config.CombatSkill.Instance[skillId].EquipType == 1;
		}

		// Token: 0x06003B4D RID: 15181 RVA: 0x002475A5 File Offset: 0x002457A5
		public ShiWuGuFa()
		{
		}

		// Token: 0x06003B4E RID: 15182 RVA: 0x002475AF File Offset: 0x002457AF
		public ShiWuGuFa(CombatSkillKey skillKey) : base(skillKey, 4, -1)
		{
		}

		// Token: 0x06003B4F RID: 15183 RVA: 0x002475BC File Offset: 0x002457BC
		public override void OnEnable(DataContext context)
		{
			base.OnEnable(context);
			base.CreateAffectedData(217, EDataModifyType.Custom, base.SkillTemplateId);
			base.CreateAffectedData(306, EDataModifyType.Custom, -1);
			base.CreateAffectedData(208, EDataModifyType.Custom, -1);
			base.CreateAffectedData(204, EDataModifyType.TotalPercent, -1);
			base.CreateAffectedData(212, EDataModifyType.AddPercent, -1);
			Events.RegisterHandler_PrepareSkillBegin(new Events.OnPrepareSkillBegin(this.OnPrepareSkillBegin));
			Events.RegisterHandler_AttackSkillAttackHit(new Events.OnAttackSkillAttackHit(this.OnAttackSkillAttackHit));
			Events.RegisterHandler_CastSkillAllEnd(new Events.OnCastSkillAllEnd(this.OnCastSkillAllEnd));
		}

		// Token: 0x06003B50 RID: 15184 RVA: 0x00247654 File Offset: 0x00245854
		public override void OnDisable(DataContext context)
		{
			Events.UnRegisterHandler_PrepareSkillBegin(new Events.OnPrepareSkillBegin(this.OnPrepareSkillBegin));
			Events.UnRegisterHandler_AttackSkillAttackHit(new Events.OnAttackSkillAttackHit(this.OnAttackSkillAttackHit));
			Events.UnRegisterHandler_CastSkillAllEnd(new Events.OnCastSkillAllEnd(this.OnCastSkillAllEnd));
			base.OnDisable(context);
		}

		// Token: 0x06003B51 RID: 15185 RVA: 0x002476A0 File Offset: 0x002458A0
		private void OnPrepareSkillBegin(DataContext context, int charId, bool isAlly, short skillId)
		{
			bool flag = charId == base.CharacterId && ShiWuGuFa.IsAttack(skillId);
			if (flag)
			{
				base.ShowSpecialEffectTips(0);
			}
		}

		// Token: 0x06003B52 RID: 15186 RVA: 0x002476D0 File Offset: 0x002458D0
		private void OnAttackSkillAttackHit(DataContext context, CombatCharacter attacker, CombatCharacter defender, short skillId, int index, bool critical)
		{
			bool flag = defender.GetId() != base.CharacterId || skillId != defender.GetPreparingSkillId() || index != 3 || !ShiWuGuFa.IsAttack(skillId);
			if (!flag)
			{
				DomainManager.Combat.AddGoneMadInjury(context, defender, base.SkillTemplateId, 0);
				base.ShowSpecialEffectTips(1);
				this._castingBeGoneMadSkillId = defender.GetPreparingSkillId();
			}
		}

		// Token: 0x06003B53 RID: 15187 RVA: 0x00247738 File Offset: 0x00245938
		private void OnCastSkillAllEnd(DataContext context, int charId, short skillId)
		{
			bool flag = charId != base.CharacterId;
			if (!flag)
			{
				this._castingBeGoneMadSkillId = -1;
			}
		}

		// Token: 0x06003B54 RID: 15188 RVA: 0x00247760 File Offset: 0x00245960
		public override bool GetModifiedValue(AffectedDataKey dataKey, bool dataValue)
		{
			bool flag = dataKey.SkillKey != this.SkillKey || dataKey.FieldId != 217;
			return flag && dataValue;
		}

		// Token: 0x06003B55 RID: 15189 RVA: 0x002477A4 File Offset: 0x002459A4
		public override int GetModifiedValue(AffectedDataKey dataKey, int dataValue)
		{
			bool flag = dataKey.CharId != base.CharacterId || dataKey.FieldId != 306;
			int result;
			if (flag)
			{
				result = dataValue;
			}
			else
			{
				result = ((this._castingBeGoneMadSkillId == dataKey.CombatSkillId) ? 0 : dataValue);
			}
			return result;
		}

		// Token: 0x06003B56 RID: 15190 RVA: 0x002477F4 File Offset: 0x002459F4
		public override List<NeedTrick> GetModifiedValue(AffectedDataKey dataKey, List<NeedTrick> dataValue)
		{
			bool flag = dataKey.CharId != base.CharacterId;
			List<NeedTrick> result;
			if (flag)
			{
				result = dataValue;
			}
			else
			{
				dataValue.Clear();
				result = dataValue;
			}
			return result;
		}

		// Token: 0x06003B57 RID: 15191 RVA: 0x00247828 File Offset: 0x00245A28
		public override int GetModifyValue(AffectedDataKey dataKey, int currModifyValue)
		{
			bool flag = dataKey.CharId != base.CharacterId || !ShiWuGuFa.IsAttack(dataKey.CombatSkillId);
			int result;
			if (flag)
			{
				result = 0;
			}
			else
			{
				ushort fieldId = dataKey.FieldId;
				if (!true)
				{
				}
				int num;
				if (fieldId != 204)
				{
					if (fieldId != 212)
					{
						num = 0;
					}
					else
					{
						num = 25;
					}
				}
				else
				{
					num = -50;
				}
				if (!true)
				{
				}
				result = num;
			}
			return result;
		}

		// Token: 0x0400115E RID: 4446
		private short _castingBeGoneMadSkillId;

		// Token: 0x0400115F RID: 4447
		private const int CostBreathAndStanceAddPercent = -50;

		// Token: 0x04001160 RID: 4448
		private const int PrepareTotalProgressAddPercent = 25;
	}
}
