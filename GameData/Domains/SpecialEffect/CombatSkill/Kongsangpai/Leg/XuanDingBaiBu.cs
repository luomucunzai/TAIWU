using System;
using GameData.Combat.Math;
using GameData.Common;
using GameData.DomainEvents;
using GameData.Domains.Combat;
using GameData.Domains.CombatSkill;
using GameData.Utilities;

namespace GameData.Domains.SpecialEffect.CombatSkill.Kongsangpai.Leg
{
	// Token: 0x0200048B RID: 1163
	public class XuanDingBaiBu : CombatSkillEffectBase
	{
		// Token: 0x06003BED RID: 15341 RVA: 0x0024AF51 File Offset: 0x00249151
		public XuanDingBaiBu()
		{
		}

		// Token: 0x06003BEE RID: 15342 RVA: 0x0024AF5B File Offset: 0x0024915B
		public XuanDingBaiBu(CombatSkillKey skillKey) : base(skillKey, 10307, -1)
		{
		}

		// Token: 0x06003BEF RID: 15343 RVA: 0x0024AF6C File Offset: 0x0024916C
		public override void OnEnable(DataContext context)
		{
			Events.RegisterHandler_NormalAttackBegin(new Events.OnNormalAttackBegin(this.OnNormalAttackBegin));
			Events.RegisterHandler_NormalAttackAllEnd(new Events.OnNormalAttackAllEnd(this.OnNormalAttackAllEnd));
			Events.RegisterHandler_CastAttackSkillBegin(new Events.OnCastAttackSkillBegin(this.OnCastAttackSkillBegin));
			Events.RegisterHandler_CastSkillEnd(new Events.OnCastSkillEnd(this.OnCastSkillEnd));
			Events.RegisterHandler_SkillEffectChange(new Events.OnSkillEffectChange(this.OnSkillEffectChange));
		}

		// Token: 0x06003BF0 RID: 15344 RVA: 0x0024AFD4 File Offset: 0x002491D4
		public override void OnDisable(DataContext context)
		{
			Events.UnRegisterHandler_NormalAttackBegin(new Events.OnNormalAttackBegin(this.OnNormalAttackBegin));
			Events.UnRegisterHandler_NormalAttackAllEnd(new Events.OnNormalAttackAllEnd(this.OnNormalAttackAllEnd));
			Events.UnRegisterHandler_CastAttackSkillBegin(new Events.OnCastAttackSkillBegin(this.OnCastAttackSkillBegin));
			Events.UnRegisterHandler_CastSkillEnd(new Events.OnCastSkillEnd(this.OnCastSkillEnd));
			Events.UnRegisterHandler_SkillEffectChange(new Events.OnSkillEffectChange(this.OnSkillEffectChange));
		}

		// Token: 0x06003BF1 RID: 15345 RVA: 0x0024B03C File Offset: 0x0024923C
		private void OnNormalAttackBegin(DataContext context, CombatCharacter attacker, CombatCharacter defender, sbyte trickType, int pursueIndex)
		{
			bool flag = !this.IsSrcSkillPerformed || pursueIndex > 0 || attacker.NormalAttackHitType == 3 || base.CombatChar != ((base.EffectCount % 2 == 0) ? (base.IsDirect ? attacker : defender) : (base.IsDirect ? defender : attacker)) || !DomainManager.Combat.InAttackRange(attacker);
			if (!flag)
			{
				this._affecting = true;
				base.ShowSpecialEffectTips(base.EffectCount % 2 == 0, 0, 1);
			}
		}

		// Token: 0x06003BF2 RID: 15346 RVA: 0x0024B0C0 File Offset: 0x002492C0
		private void OnNormalAttackAllEnd(DataContext context, CombatCharacter attacker, CombatCharacter defender)
		{
			bool flag = !this.IsSrcSkillPerformed || !this._affecting;
			if (!flag)
			{
				this._affecting = false;
				base.ReduceEffectCount(1);
			}
		}

		// Token: 0x06003BF3 RID: 15347 RVA: 0x0024B0F8 File Offset: 0x002492F8
		private void OnCastAttackSkillBegin(DataContext context, CombatCharacter attacker, CombatCharacter defender, short skillId)
		{
			bool flag;
			if (this.IsSrcSkillPerformed)
			{
				if (base.CombatChar.SkillHitType.Exist((sbyte type) => 0 <= type && type <= 2) && base.CombatChar == ((base.EffectCount % 2 == 0) ? (base.IsDirect ? attacker : defender) : (base.IsDirect ? defender : attacker)))
				{
					flag = !DomainManager.Combat.InAttackRange(attacker);
					goto IL_77;
				}
			}
			flag = true;
			IL_77:
			bool flag2 = flag;
			if (!flag2)
			{
				this._affecting = true;
				base.ShowSpecialEffectTips(base.EffectCount % 2 == 0, 0, 1);
			}
		}

		// Token: 0x06003BF4 RID: 15348 RVA: 0x0024B1A0 File Offset: 0x002493A0
		private void OnCastSkillEnd(DataContext context, int charId, bool isAlly, short skillId, sbyte power, bool interrupted)
		{
			bool flag = !this.IsSrcSkillPerformed;
			if (flag)
			{
				bool flag2 = charId != base.CharacterId || skillId != base.SkillTemplateId;
				if (!flag2)
				{
					bool flag3 = base.PowerMatchAffectRequire((int)power, 0);
					if (flag3)
					{
						this.IsSrcSkillPerformed = true;
						this._affecting = false;
						base.AddMaxEffectCount(true);
						base.AppendAffectedData(context, base.CharacterId, base.IsDirect ? 56 : 90, EDataModifyType.AddPercent, -1);
						base.AppendAffectedData(context, base.CharacterId, base.IsDirect ? 57 : 91, EDataModifyType.AddPercent, -1);
						base.AppendAffectedData(context, base.CharacterId, base.IsDirect ? 58 : 92, EDataModifyType.AddPercent, -1);
						base.AppendAffectedData(context, base.CharacterId, base.IsDirect ? 94 : 60, EDataModifyType.AddPercent, -1);
						base.AppendAffectedData(context, base.CharacterId, base.IsDirect ? 95 : 61, EDataModifyType.AddPercent, -1);
						base.AppendAffectedData(context, base.CharacterId, base.IsDirect ? 96 : 62, EDataModifyType.AddPercent, -1);
					}
					else
					{
						base.RemoveSelf(context);
					}
				}
			}
			else
			{
				bool flag4 = charId == base.CharacterId && skillId == base.SkillTemplateId && base.PowerMatchAffectRequire((int)power, 0);
				if (flag4)
				{
					base.RemoveSelf(context);
				}
				else
				{
					bool affecting = this._affecting;
					if (affecting)
					{
						this._affecting = false;
						base.ReduceEffectCount(1);
					}
				}
			}
		}

		// Token: 0x06003BF5 RID: 15349 RVA: 0x0024B31C File Offset: 0x0024951C
		private void OnSkillEffectChange(DataContext context, int charId, SkillEffectKey key, short oldCount, short newCount, bool removed)
		{
			bool flag = removed && this.IsSrcSkillPerformed && charId == base.CharacterId && key.SkillId == base.SkillTemplateId && key.IsDirect == base.IsDirect;
			if (flag)
			{
				base.RemoveSelf(context);
			}
		}

		// Token: 0x06003BF6 RID: 15350 RVA: 0x0024B36C File Offset: 0x0024956C
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
				bool flag2 = base.EffectCount % 2 == 0;
				if (flag2)
				{
					bool flag3 = dataKey.FieldId == 56 || dataKey.FieldId == 57 || dataKey.FieldId == 58;
					if (flag3)
					{
						return (dataKey.CombatSkillId == base.SkillTemplateId) ? 60 : 40;
					}
					bool flag4 = dataKey.FieldId == 90 || dataKey.FieldId == 91 || dataKey.FieldId == 92;
					if (flag4)
					{
						return (base.CombatChar.GetPreparingSkillId() == base.SkillTemplateId) ? -60 : -40;
					}
				}
				else
				{
					bool flag5 = dataKey.FieldId == 94 || dataKey.FieldId == 95 || dataKey.FieldId == 96;
					if (flag5)
					{
						return (base.CombatChar.GetPreparingSkillId() == base.SkillTemplateId) ? 60 : 40;
					}
					bool flag6 = dataKey.FieldId == 60 || dataKey.FieldId == 61 || dataKey.FieldId == 62;
					if (flag6)
					{
						return (dataKey.CombatSkillId == base.SkillTemplateId) ? -60 : -40;
					}
				}
				result = 0;
			}
			return result;
		}

		// Token: 0x0400119E RID: 4510
		private const sbyte ChangeHitOrAvoid = 40;

		// Token: 0x0400119F RID: 4511
		private const sbyte ChangeSelfHitOrAvoid = 60;

		// Token: 0x040011A0 RID: 4512
		private bool _affecting;
	}
}
