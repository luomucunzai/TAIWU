using System;
using GameData.Combat.Math;
using GameData.Common;
using GameData.DomainEvents;
using GameData.Domains.Combat;
using GameData.Domains.CombatSkill;

namespace GameData.Domains.SpecialEffect.CombatSkill.Jieqingmen.Sword
{
	// Token: 0x020004E3 RID: 1251
	public class JieQingAnShouKuaiJian : CombatSkillEffectBase
	{
		// Token: 0x06003DDC RID: 15836 RVA: 0x00253AD4 File Offset: 0x00251CD4
		public JieQingAnShouKuaiJian()
		{
		}

		// Token: 0x06003DDD RID: 15837 RVA: 0x00253ADE File Offset: 0x00251CDE
		public JieQingAnShouKuaiJian(CombatSkillKey skillKey) : base(skillKey, 13203, -1)
		{
		}

		// Token: 0x06003DDE RID: 15838 RVA: 0x00253AF0 File Offset: 0x00251CF0
		public override void OnEnable(DataContext context)
		{
			this._canCastFree = true;
			this._autoCastSkillId = -1;
			base.CreateAffectedData(154, EDataModifyType.Custom, base.SkillTemplateId);
			Events.RegisterHandler_PrepareSkillBegin(new Events.OnPrepareSkillBegin(this.OnPrepareSkillBegin));
			Events.RegisterHandler_CastSkillEnd(new Events.OnCastSkillEnd(this.OnCastSkillEnd));
		}

		// Token: 0x06003DDF RID: 15839 RVA: 0x00253B43 File Offset: 0x00251D43
		public override void OnDisable(DataContext context)
		{
			Events.UnRegisterHandler_PrepareSkillBegin(new Events.OnPrepareSkillBegin(this.OnPrepareSkillBegin));
			Events.UnRegisterHandler_CastSkillEnd(new Events.OnCastSkillEnd(this.OnCastSkillEnd));
		}

		// Token: 0x06003DE0 RID: 15840 RVA: 0x00253B6C File Offset: 0x00251D6C
		private void OnPrepareSkillBegin(DataContext context, int charId, bool isAlly, short skillId)
		{
			bool flag = this.SkillKey.IsMatch(charId, skillId) && this._canCastFree;
			if (flag)
			{
				this._castFree = true;
			}
			bool flag2 = charId == base.CharacterId && CombatSkillTemplateHelper.IsAttack(skillId) && this._canCastFree;
			if (flag2)
			{
				this.DisableCanCastFree(context);
			}
			bool flag3 = charId != base.CharacterId || skillId != this._autoCastSkillId;
			if (!flag3)
			{
				this._autoCastSkillId = -1;
				DomainManager.Combat.ChangeSkillPrepareProgress(base.CombatChar, base.CombatChar.SkillPrepareTotalProgress * JieQingAnShouKuaiJian.ProgressPercent);
			}
		}

		// Token: 0x06003DE1 RID: 15841 RVA: 0x00253C14 File Offset: 0x00251E14
		private void OnCastSkillEnd(DataContext context, int charId, bool isAlly, short skillId, sbyte power, bool interrupted)
		{
			bool flag = charId != base.CharacterId || skillId != base.SkillTemplateId;
			if (!flag)
			{
				this.TryAutoCast(context, power);
				bool flag2 = !this._castFree;
				if (!flag2)
				{
					this._castFree = false;
					DomainManager.Combat.AddTrick(context, base.IsDirect ? base.CombatChar : base.EnemyChar, 19, 4, base.IsDirect, false);
					base.ShowSpecialEffectTips(1);
				}
			}
		}

		// Token: 0x06003DE2 RID: 15842 RVA: 0x00253C98 File Offset: 0x00251E98
		private void TryAutoCast(DataContext context, sbyte power)
		{
			bool flag = !base.PowerMatchAffectRequire((int)power, 0);
			if (!flag)
			{
				CombatCharacter trickChar = base.IsDirect ? base.CombatChar : base.EnemyChar;
				int trickCount = (int)trickChar.GetTrickCount(19);
				int maxSkillGrade = Math.Min(trickCount / 2, 2);
				this._autoCastSkillId = DomainManager.Combat.GetRandomAttackSkill(base.CombatChar, 7, (sbyte)maxSkillGrade, context.Random, true, -1);
				bool flag2 = this._autoCastSkillId < 0;
				if (!flag2)
				{
					DomainManager.Combat.CastSkillFree(context, base.CombatChar, this._autoCastSkillId, ECombatCastFreePriority.Normal);
					base.ShowSpecialEffectTips(0);
				}
			}
		}

		// Token: 0x06003DE3 RID: 15843 RVA: 0x00253D34 File Offset: 0x00251F34
		private void DisableCanCastFree(DataContext context)
		{
			this._canCastFree = false;
			DomainManager.Combat.UpdateSkillCanUse(context, base.CombatChar, base.SkillTemplateId);
		}

		// Token: 0x06003DE4 RID: 15844 RVA: 0x00253D58 File Offset: 0x00251F58
		public override bool GetModifiedValue(AffectedDataKey dataKey, bool dataValue)
		{
			bool flag = dataKey.SkillKey == this.SkillKey && dataKey.FieldId == 154;
			bool result;
			if (flag)
			{
				result = (!this._canCastFree && dataValue);
			}
			else
			{
				result = base.GetModifiedValue(dataKey, dataValue);
			}
			return result;
		}

		// Token: 0x0400123A RID: 4666
		private const int AddShaCount = 4;

		// Token: 0x0400123B RID: 4667
		private const sbyte NeedTrickCountPerGrade = 2;

		// Token: 0x0400123C RID: 4668
		private const sbyte MaxSkillGrade = 2;

		// Token: 0x0400123D RID: 4669
		private static readonly CValuePercent ProgressPercent = 50;

		// Token: 0x0400123E RID: 4670
		private bool _canCastFree;

		// Token: 0x0400123F RID: 4671
		private bool _castFree;

		// Token: 0x04001240 RID: 4672
		private short _autoCastSkillId;
	}
}
