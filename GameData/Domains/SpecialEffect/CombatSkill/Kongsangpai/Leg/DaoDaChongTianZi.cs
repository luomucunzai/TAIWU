using System;
using GameData.Combat.Math;
using GameData.Common;
using GameData.DomainEvents;
using GameData.Domains.Combat;
using GameData.Domains.CombatSkill;

namespace GameData.Domains.SpecialEffect.CombatSkill.Kongsangpai.Leg
{
	// Token: 0x02000485 RID: 1157
	public class DaoDaChongTianZi : CombatSkillEffectBase
	{
		// Token: 0x06003BB8 RID: 15288 RVA: 0x00249603 File Offset: 0x00247803
		public DaoDaChongTianZi()
		{
		}

		// Token: 0x06003BB9 RID: 15289 RVA: 0x0024960D File Offset: 0x0024780D
		public DaoDaChongTianZi(CombatSkillKey skillKey) : base(skillKey, 10300, -1)
		{
		}

		// Token: 0x06003BBA RID: 15290 RVA: 0x00249620 File Offset: 0x00247820
		public override void OnEnable(DataContext context)
		{
			CombatCharacter enemyChar = DomainManager.Combat.GetCombatCharacter(!base.CombatChar.IsAlly, false);
			this._canAffect = !enemyChar.AiController.Memory.EnemyRecordDict[base.CharacterId].SkillRecord.ContainsKey(base.SkillTemplateId);
			base.CreateAffectedAllEnemyData(287, EDataModifyType.Custom, -1);
			base.CreateAffectedAllEnemyData(285, EDataModifyType.Custom, -1);
			bool canAffect = this._canAffect;
			if (canAffect)
			{
				int[] charList = DomainManager.Combat.GetCharacterList(!base.CombatChar.IsAlly);
				for (int i = 0; i < charList.Length; i++)
				{
					bool flag = charList[i] >= 0;
					if (flag)
					{
						DomainManager.Combat.GetElement_CombatCharacterDict(charList[i]).AiController.AllowDefense = false;
					}
				}
				Events.RegisterHandler_CompareDataCalcFinished(new Events.OnCompareDataCalcFinished(this.OnCompareDataCalcFinished));
				Events.RegisterHandler_CastAttackSkillBegin(new Events.OnCastAttackSkillBegin(this.OnCastAttackSkillBegin));
				base.ShowSpecialEffectTips(0);
			}
			Events.RegisterHandler_CastSkillEnd(new Events.OnCastSkillEnd(this.OnCastSkillEnd));
		}

		// Token: 0x06003BBB RID: 15291 RVA: 0x0024973C File Offset: 0x0024793C
		public override void OnDisable(DataContext context)
		{
			bool canAffect = this._canAffect;
			if (canAffect)
			{
				Events.UnRegisterHandler_CompareDataCalcFinished(new Events.OnCompareDataCalcFinished(this.OnCompareDataCalcFinished));
				Events.UnRegisterHandler_CastAttackSkillBegin(new Events.OnCastAttackSkillBegin(this.OnCastAttackSkillBegin));
			}
			Events.UnRegisterHandler_CastSkillEnd(new Events.OnCastSkillEnd(this.OnCastSkillEnd));
		}

		// Token: 0x06003BBC RID: 15292 RVA: 0x0024978C File Offset: 0x0024798C
		private void OnCompareDataCalcFinished(CombatContext context, DamageCompareData compareData)
		{
			bool flag = context.Attacker != base.CombatChar || context.SkillTemplateId != base.SkillTemplateId;
			if (!flag)
			{
				bool isDirect = base.IsDirect;
				if (isDirect)
				{
					compareData.OuterDefendValue /= 2;
				}
				else
				{
					compareData.InnerDefendValue /= 2;
				}
			}
		}

		// Token: 0x06003BBD RID: 15293 RVA: 0x002497EC File Offset: 0x002479EC
		private void OnCastAttackSkillBegin(DataContext context, CombatCharacter attacker, CombatCharacter defender, short skillId)
		{
			bool flag = attacker != base.CombatChar || skillId != base.SkillTemplateId;
			if (!flag)
			{
				this._disableSkills = true;
				base.InvalidateCache(context, base.CurrEnemyChar.GetId(), 287);
				base.InvalidateCache(context, base.CurrEnemyChar.GetId(), 285);
			}
		}

		// Token: 0x06003BBE RID: 15294 RVA: 0x00249850 File Offset: 0x00247A50
		private void OnCastSkillEnd(DataContext context, int charId, bool isAlly, short skillId, sbyte power, bool interrupted)
		{
			bool flag = charId != base.CharacterId || skillId != base.SkillTemplateId;
			if (!flag)
			{
				bool canAffect = this._canAffect;
				if (canAffect)
				{
					int[] charList = DomainManager.Combat.GetCharacterList(!base.CombatChar.IsAlly);
					for (int i = 0; i < charList.Length; i++)
					{
						bool flag2 = charList[i] >= 0;
						if (flag2)
						{
							DomainManager.Combat.GetElement_CombatCharacterDict(charList[i]).AiController.AllowDefense = true;
						}
					}
					bool flag3 = base.PowerMatchAffectRequire((int)power, 0);
					if (flag3)
					{
						DomainManager.Combat.AppendFatalDamageMark(context, base.CurrEnemyChar, 1, -1, -1, false, EDamageType.None);
					}
					this._disableSkills = false;
				}
				base.RemoveSelf(context);
			}
		}

		// Token: 0x06003BBF RID: 15295 RVA: 0x0024991C File Offset: 0x00247B1C
		public override bool GetModifiedValue(AffectedDataKey dataKey, bool dataValue)
		{
			ushort fieldId = dataKey.FieldId;
			bool flag = fieldId == 285 || fieldId == 287;
			bool flag2 = !flag;
			bool result;
			if (flag2)
			{
				result = dataValue;
			}
			else
			{
				result = (dataValue && !this._disableSkills);
			}
			return result;
		}

		// Token: 0x0400117F RID: 4479
		private bool _canAffect;

		// Token: 0x04001180 RID: 4480
		private bool _disableSkills;
	}
}
