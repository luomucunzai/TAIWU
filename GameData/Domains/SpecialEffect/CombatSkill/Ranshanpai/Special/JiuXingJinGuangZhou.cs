using System;
using Config;
using GameData.Combat.Math;
using GameData.Common;
using GameData.DomainEvents;
using GameData.Domains.Combat;
using GameData.Domains.CombatSkill;

namespace GameData.Domains.SpecialEffect.CombatSkill.Ranshanpai.Special
{
	// Token: 0x0200044D RID: 1101
	public class JiuXingJinGuangZhou : CombatSkillEffectBase
	{
		// Token: 0x06003A64 RID: 14948 RVA: 0x0024343E File Offset: 0x0024163E
		private static bool IsCurse(short skillId)
		{
			return Config.CombatSkill.Instance[skillId].SubType == ECombatSkillSubType.Curse;
		}

		// Token: 0x06003A65 RID: 14949 RVA: 0x00243453 File Offset: 0x00241653
		public JiuXingJinGuangZhou()
		{
		}

		// Token: 0x06003A66 RID: 14950 RVA: 0x00243464 File Offset: 0x00241664
		public JiuXingJinGuangZhou(CombatSkillKey skillKey) : base(skillKey, 7307, -1)
		{
		}

		// Token: 0x06003A67 RID: 14951 RVA: 0x0024347C File Offset: 0x0024167C
		public override void OnEnable(DataContext context)
		{
			base.CreateAffectedAllEnemyData(289, EDataModifyType.Custom, -1);
			bool isDirect = base.IsDirect;
			if (isDirect)
			{
				base.CreateAffectedAllEnemyData(265, EDataModifyType.AddPercent, -1);
			}
			else
			{
				base.CreateAffectedData(265, EDataModifyType.AddPercent, -1);
			}
			Events.RegisterHandler_CastAttackSkillBegin(new Events.OnCastAttackSkillBegin(this.OnCastAttackSkillBegin));
			Events.RegisterHandler_AttackSkillAttackEnd(new Events.OnAttackSkillAttackEnd(this.OnAttackSkillAttackEnd));
			Events.RegisterHandler_CastSkillAllEnd(new Events.OnCastSkillAllEnd(this.OnCastSkillAllEnd));
			Events.RegisterHandler_CastSkillEnd(new Events.OnCastSkillEnd(this.OnCastSkillEnd));
		}

		// Token: 0x06003A68 RID: 14952 RVA: 0x00243508 File Offset: 0x00241708
		public override void OnDisable(DataContext context)
		{
			Events.UnRegisterHandler_CastAttackSkillBegin(new Events.OnCastAttackSkillBegin(this.OnCastAttackSkillBegin));
			Events.UnRegisterHandler_AttackSkillAttackEnd(new Events.OnAttackSkillAttackEnd(this.OnAttackSkillAttackEnd));
			Events.UnRegisterHandler_CastSkillAllEnd(new Events.OnCastSkillAllEnd(this.OnCastSkillAllEnd));
			Events.UnRegisterHandler_CastSkillEnd(new Events.OnCastSkillEnd(this.OnCastSkillEnd));
		}

		// Token: 0x06003A69 RID: 14953 RVA: 0x00243560 File Offset: 0x00241760
		public override int GetModifyValue(AffectedDataKey dataKey, int currModifyValue)
		{
			bool flag = (base.IsDirect ? (dataKey.CharId == base.CharacterId) : (!JiuXingJinGuangZhou.IsCurse(dataKey.CombatSkillId))) || this._performingSkillId < 0 || !base.PowerMatchAffectRequire(this._performingSkillPower, 1) || dataKey.FieldId != 265;
			int result;
			if (flag)
			{
				result = 0;
			}
			else
			{
				base.ShowSpecialEffectTipsOnceInFrame(2);
				result = (base.IsDirect ? 80 : -40);
			}
			return result;
		}

		// Token: 0x06003A6A RID: 14954 RVA: 0x002435E8 File Offset: 0x002417E8
		public override bool GetModifiedValue(AffectedDataKey dataKey, bool dataValue)
		{
			bool flag = this._performingSkillId < 0 || dataKey.CombatSkillId != this._performingSkillId || dataKey.FieldId != 289;
			bool result;
			if (flag)
			{
				result = dataValue;
			}
			else
			{
				base.ShowSpecialEffectTipsOnceInFrame(1);
				result = true;
			}
			return result;
		}

		// Token: 0x06003A6B RID: 14955 RVA: 0x00243638 File Offset: 0x00241838
		private void OnCastAttackSkillBegin(DataContext context, CombatCharacter attacker, CombatCharacter defender, short skillId)
		{
			bool flag = attacker.GetId() != base.CharacterId || this._performingSkillId >= 0 || base.EffectCount <= 0 || !JiuXingJinGuangZhou.IsCurse(skillId);
			if (!flag)
			{
				this._performingSkillId = skillId;
				base.ReduceEffectCount(1);
			}
		}

		// Token: 0x06003A6C RID: 14956 RVA: 0x0024368C File Offset: 0x0024188C
		private void OnAttackSkillAttackEnd(CombatContext context, sbyte hitType, bool hit, int index)
		{
			bool flag = context.AttackerId != base.CharacterId || context.SkillTemplateId != this._performingSkillId || index < 3;
			if (!flag)
			{
				this._performingSkillPower = (int)context.Attacker.GetAttackSkillPower();
			}
		}

		// Token: 0x06003A6D RID: 14957 RVA: 0x002436D8 File Offset: 0x002418D8
		private void OnCastSkillAllEnd(DataContext context, int charId, short skillId)
		{
			bool flag = charId != base.CharacterId || skillId != this._performingSkillId;
			if (!flag)
			{
				this._performingSkillId = -1;
				this._performingSkillPower = 0;
			}
		}

		// Token: 0x06003A6E RID: 14958 RVA: 0x00243714 File Offset: 0x00241914
		private void OnCastSkillEnd(DataContext context, int charId, bool isAlly, short skillId, sbyte power, bool interrupted)
		{
			bool flag = charId != base.CharacterId || skillId != base.SkillTemplateId || !base.PowerMatchAffectRequire((int)power, 0);
			if (!flag)
			{
				this.DoReduceHealth(context);
				base.AddMaxEffectCount(true);
			}
		}

		// Token: 0x06003A6F RID: 14959 RVA: 0x0024375C File Offset: 0x0024195C
		private void DoReduceHealth(DataContext context)
		{
			CombatCharacter enemyChar = base.CurrEnemyChar;
			bool flag = enemyChar.HasInfectedFeature(ECharacterFeatureInfectedType.NotInfected);
			if (!flag)
			{
				bool flag2 = DomainManager.Combat.CheckHealthImmunity(context, enemyChar);
				if (!flag2)
				{
					enemyChar.GetCharacter().ChangeHealth(context, -36);
					base.ShowSpecialEffectTips(0);
				}
			}
		}

		// Token: 0x04001117 RID: 4375
		private const sbyte InfectedReduceHealth = -36;

		// Token: 0x04001118 RID: 4376
		private const int DirectSilenceFrameAddPercent = 80;

		// Token: 0x04001119 RID: 4377
		private const int ReverseSilenceFrameAddPercent = -40;

		// Token: 0x0400111A RID: 4378
		private short _performingSkillId = -1;

		// Token: 0x0400111B RID: 4379
		private int _performingSkillPower;
	}
}
