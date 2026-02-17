using System;
using System.Collections.Generic;
using GameData.Combat.Math;
using GameData.Common;
using GameData.DomainEvents;
using GameData.Domains.Character;
using GameData.Domains.CombatSkill;

namespace GameData.Domains.SpecialEffect.CombatSkill.Shaolinpai.FistAndPalm
{
	// Token: 0x02000424 RID: 1060
	public class DaMoShiBaShou : CombatSkillEffectBase
	{
		// Token: 0x06003965 RID: 14693 RVA: 0x0023E447 File Offset: 0x0023C647
		public DaMoShiBaShou()
		{
		}

		// Token: 0x06003966 RID: 14694 RVA: 0x0023E451 File Offset: 0x0023C651
		public DaMoShiBaShou(CombatSkillKey skillKey) : base(skillKey, 1105, -1)
		{
		}

		// Token: 0x06003967 RID: 14695 RVA: 0x0023E464 File Offset: 0x0023C664
		public override void OnEnable(DataContext context)
		{
			OuterAndInnerInts penetrations = this.CharObj.GetPenetrations();
			OuterAndInnerInts penetrationResists = this.CharObj.GetPenetrationResists();
			bool flag = base.IsDirect ? (penetrationResists.Outer > penetrations.Outer) : (penetrationResists.Inner > penetrations.Inner);
			if (flag)
			{
				this.AffectDatas = new Dictionary<AffectedDataKey, EDataModifyType>();
				this.AffectDatas.Add(new AffectedDataKey(base.CharacterId, 199, base.SkillTemplateId, -1, -1, -1), EDataModifyType.AddPercent);
				base.ShowSpecialEffectTips(0);
			}
			Events.RegisterHandler_PrepareSkillEnd(new Events.OnPrepareSkillEnd(this.OnPrepareSkillEnd));
			Events.RegisterHandler_CastSkillEnd(new Events.OnCastSkillEnd(this.OnCastSkillEnd));
		}

		// Token: 0x06003968 RID: 14696 RVA: 0x0023E514 File Offset: 0x0023C714
		public override void OnDisable(DataContext context)
		{
			Events.UnRegisterHandler_PrepareSkillEnd(new Events.OnPrepareSkillEnd(this.OnPrepareSkillEnd));
			Events.UnRegisterHandler_CastSkillEnd(new Events.OnCastSkillEnd(this.OnCastSkillEnd));
		}

		// Token: 0x06003969 RID: 14697 RVA: 0x0023E53C File Offset: 0x0023C73C
		protected virtual void OnPrepareSkillEnd(DataContext context, int charId, bool isAlly, short skillId)
		{
			bool flag = charId != base.CharacterId || skillId != base.SkillTemplateId || !DomainManager.Combat.InAttackRange(base.CombatChar);
			if (!flag)
			{
				OuterAndInnerInts penetrationResists = this.CharObj.GetPenetrationResists();
				this._addPenetrate = (base.IsDirect ? penetrationResists.Outer : penetrationResists.Inner);
				base.AppendAffectedData(context, base.CharacterId, base.IsDirect ? 44 : 45, EDataModifyType.Add, -1);
				base.ShowSpecialEffectTips(1);
			}
		}

		// Token: 0x0600396A RID: 14698 RVA: 0x0023E5C8 File Offset: 0x0023C7C8
		private void OnCastSkillEnd(DataContext context, int charId, bool isAlly, short skillId, sbyte power, bool interrupted)
		{
			bool flag = charId != base.CharacterId || skillId != base.SkillTemplateId;
			if (!flag)
			{
				base.RemoveSelf(context);
			}
		}

		// Token: 0x0600396B RID: 14699 RVA: 0x0023E600 File Offset: 0x0023C800
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
				bool flag2 = dataKey.FieldId == 199 && dataKey.CombatSkillId == base.SkillTemplateId;
				if (flag2)
				{
					result = 20;
				}
				else
				{
					bool flag3 = dataKey.FieldId == 44 || dataKey.FieldId == 45;
					if (flag3)
					{
						result = this._addPenetrate;
					}
					else
					{
						result = 0;
					}
				}
			}
			return result;
		}

		// Token: 0x040010C5 RID: 4293
		private const sbyte AddPower = 20;

		// Token: 0x040010C6 RID: 4294
		private int _addPenetrate;
	}
}
