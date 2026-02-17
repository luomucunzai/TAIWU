using System;
using System.Collections.Generic;
using Config;
using GameData.Combat.Math;
using GameData.Common;
using GameData.DomainEvents;
using GameData.Domains.Combat;
using GameData.Domains.CombatSkill;

namespace GameData.Domains.SpecialEffect.CombatSkill.Shaolinpai.FistAndPalm
{
	// Token: 0x02000425 RID: 1061
	public class DaTongBiQuan : CombatSkillEffectBase
	{
		// Token: 0x0600396C RID: 14700 RVA: 0x0023E677 File Offset: 0x0023C877
		public DaTongBiQuan()
		{
		}

		// Token: 0x0600396D RID: 14701 RVA: 0x0023E681 File Offset: 0x0023C881
		public DaTongBiQuan(CombatSkillKey skillKey) : base(skillKey, 1102, -1)
		{
		}

		// Token: 0x0600396E RID: 14702 RVA: 0x0023E694 File Offset: 0x0023C894
		public override void OnEnable(DataContext context)
		{
			this._addPower = 0;
			this.AffectDatas = new Dictionary<AffectedDataKey, EDataModifyType>();
			this.AffectDatas.Add(new AffectedDataKey(base.CharacterId, 199, base.SkillTemplateId, -1, -1, -1), EDataModifyType.AddPercent);
			this.AffectDatas.Add(new AffectedDataKey(base.CharacterId, 145, -1, -1, -1, -1), EDataModifyType.Add);
			this.AffectDatas.Add(new AffectedDataKey(base.CharacterId, 146, -1, -1, -1, -1), EDataModifyType.Add);
			Events.RegisterHandler_NormalAttackAllEnd(new Events.OnNormalAttackAllEnd(this.OnNormalAttackAllEnd));
			Events.RegisterHandler_PrepareSkillBegin(new Events.OnPrepareSkillBegin(this.OnPrepareSkillBegin));
			Events.RegisterHandler_CastSkillEnd(new Events.OnCastSkillEnd(this.OnCastSkillEnd));
			Events.RegisterHandler_SkillEffectChange(new Events.OnSkillEffectChange(this.OnSkillEffectChange));
		}

		// Token: 0x0600396F RID: 14703 RVA: 0x0023E764 File Offset: 0x0023C964
		public override void OnDisable(DataContext context)
		{
			Events.UnRegisterHandler_NormalAttackAllEnd(new Events.OnNormalAttackAllEnd(this.OnNormalAttackAllEnd));
			Events.UnRegisterHandler_PrepareSkillBegin(new Events.OnPrepareSkillBegin(this.OnPrepareSkillBegin));
			Events.UnRegisterHandler_CastSkillEnd(new Events.OnCastSkillEnd(this.OnCastSkillEnd));
			Events.UnRegisterHandler_SkillEffectChange(new Events.OnSkillEffectChange(this.OnSkillEffectChange));
		}

		// Token: 0x06003970 RID: 14704 RVA: 0x0023E7BC File Offset: 0x0023C9BC
		private void OnNormalAttackAllEnd(DataContext context, CombatCharacter attacker, CombatCharacter defender)
		{
			bool flag = attacker.GetId() != base.CharacterId || base.EffectCount <= 0;
			if (!flag)
			{
				base.ReduceEffectCount(1);
			}
		}

		// Token: 0x06003971 RID: 14705 RVA: 0x0023E7F8 File Offset: 0x0023C9F8
		private void OnPrepareSkillBegin(DataContext context, int charId, bool isAlly, short skillId)
		{
			bool flag = charId != base.CharacterId || skillId != base.SkillTemplateId;
			if (!flag)
			{
				bool flag2 = base.EffectCount > 0;
				if (flag2)
				{
					this._addPower = 20;
					DomainManager.SpecialEffect.InvalidateCache(context, base.CharacterId, 199);
					base.ShowSpecialEffectTips(1);
				}
			}
		}

		// Token: 0x06003972 RID: 14706 RVA: 0x0023E85C File Offset: 0x0023CA5C
		private void OnCastSkillEnd(DataContext context, int charId, bool isAlly, short skillId, sbyte power, bool interrupted)
		{
			bool flag = charId == base.CharacterId && base.EffectCount > 0 && Config.CombatSkill.Instance[skillId].EquipType == 1;
			if (flag)
			{
				base.ReduceEffectCount(1);
			}
			bool flag2 = charId != base.CharacterId || skillId != base.SkillTemplateId;
			if (!flag2)
			{
				bool flag3 = base.PowerMatchAffectRequire((int)power, 0);
				if (flag3)
				{
					base.AddMaxEffectCount(true);
					base.ShowSpecialEffectTips(0);
				}
				bool flag4 = this._addPower > 0;
				if (flag4)
				{
					this._addPower = 0;
					DomainManager.SpecialEffect.InvalidateCache(context, base.CharacterId, 199);
				}
			}
		}

		// Token: 0x06003973 RID: 14707 RVA: 0x0023E90C File Offset: 0x0023CB0C
		private void OnSkillEffectChange(DataContext context, int charId, SkillEffectKey key, short oldCount, short newCount, bool removed)
		{
			bool flag = charId == base.CharacterId && key.SkillId == base.SkillTemplateId && key.IsDirect == base.IsDirect && oldCount > 0 != newCount > 0;
			if (flag)
			{
				DomainManager.SpecialEffect.InvalidateCache(context, base.CharacterId, 145);
				DomainManager.SpecialEffect.InvalidateCache(context, base.CharacterId, 146);
			}
		}

		// Token: 0x06003974 RID: 14708 RVA: 0x0023E988 File Offset: 0x0023CB88
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
				ushort fieldId = dataKey.FieldId;
				bool flag2 = fieldId - 145 <= 1;
				bool flag3 = flag2;
				if (flag3)
				{
					int addRange = 0;
					bool flag4 = dataKey.CombatSkillId == base.SkillTemplateId;
					if (flag4)
					{
						addRange += 10;
					}
					bool flag5 = base.EffectCount > 0 && dataKey.FieldId == (base.IsDirect ? 146 : 145);
					if (flag5)
					{
						addRange += 10;
					}
					result = addRange;
				}
				else
				{
					bool flag6 = dataKey.FieldId == 199 && dataKey.CombatSkillId == base.SkillTemplateId;
					if (flag6)
					{
						result = this._addPower;
					}
					else
					{
						result = 0;
					}
				}
			}
			return result;
		}

		// Token: 0x040010C7 RID: 4295
		private const sbyte AddSkillRange = 10;

		// Token: 0x040010C8 RID: 4296
		private const sbyte AddAttackRange = 10;

		// Token: 0x040010C9 RID: 4297
		private const sbyte AddPower = 20;

		// Token: 0x040010CA RID: 4298
		private int _addPower;
	}
}
