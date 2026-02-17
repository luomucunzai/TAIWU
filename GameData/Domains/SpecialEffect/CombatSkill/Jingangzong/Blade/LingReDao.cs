using System;
using System.Collections.Generic;
using GameData.Combat.Math;
using GameData.Common;
using GameData.DomainEvents;
using GameData.Domains.Character;
using GameData.Domains.Combat;
using GameData.Domains.CombatSkill;

namespace GameData.Domains.SpecialEffect.CombatSkill.Jingangzong.Blade
{
	// Token: 0x020004D0 RID: 1232
	public class LingReDao : CombatSkillEffectBase
	{
		// Token: 0x06003D65 RID: 15717 RVA: 0x002517FA File Offset: 0x0024F9FA
		public LingReDao()
		{
		}

		// Token: 0x06003D66 RID: 15718 RVA: 0x00251804 File Offset: 0x0024FA04
		public LingReDao(CombatSkillKey skillKey) : base(skillKey, 11203, -1)
		{
		}

		// Token: 0x06003D67 RID: 15719 RVA: 0x00251818 File Offset: 0x0024FA18
		public override void OnEnable(DataContext context)
		{
			this._effectReady = false;
			this.AffectDatas = new Dictionary<AffectedDataKey, EDataModifyType>();
			this.AffectDatas.Add(new AffectedDataKey(base.CharacterId, 199, base.SkillTemplateId, -1, -1, -1), EDataModifyType.AddPercent);
			this.AffectDatas.Add(new AffectedDataKey(base.CharacterId, 145, -1, -1, -1, -1), EDataModifyType.Add);
			this.AffectDatas.Add(new AffectedDataKey(base.CharacterId, 146, -1, -1, -1, -1), EDataModifyType.Add);
			Events.RegisterHandler_ChangeNeiliAllocationAfterCombatBegin(new Events.OnChangeNeiliAllocationAfterCombatBegin(this.OnChangeNeiliAllocationAfterCombatBegin));
			Events.RegisterHandler_NeiliAllocationChanged(new Events.OnNeiliAllocationChanged(this.OnNeiliAllocationChanged));
		}

		// Token: 0x06003D68 RID: 15720 RVA: 0x002518C4 File Offset: 0x0024FAC4
		public override void OnDisable(DataContext context)
		{
			Events.UnRegisterHandler_ChangeNeiliAllocationAfterCombatBegin(new Events.OnChangeNeiliAllocationAfterCombatBegin(this.OnChangeNeiliAllocationAfterCombatBegin));
			Events.UnRegisterHandler_NeiliAllocationChanged(new Events.OnNeiliAllocationChanged(this.OnNeiliAllocationChanged));
		}

		// Token: 0x06003D69 RID: 15721 RVA: 0x002518EC File Offset: 0x0024FAEC
		private void OnChangeNeiliAllocationAfterCombatBegin(DataContext context, CombatCharacter character, NeiliAllocation _)
		{
			bool flag = character.GetId() != base.CharacterId;
			if (!flag)
			{
				this._effectReady = true;
				DomainManager.Combat.AddSkillEffect(context, base.CombatChar, new SkillEffectKey(base.SkillTemplateId, base.IsDirect), 0, base.MaxEffectCount, false);
			}
		}

		// Token: 0x06003D6A RID: 15722 RVA: 0x00251944 File Offset: 0x0024FB44
		private void OnNeiliAllocationChanged(DataContext context, int charId, byte type, int changeValue)
		{
			bool flag = charId != base.CharacterId || !(base.IsDirect ? (changeValue > 0) : (changeValue < 0)) || !this._effectReady;
			if (!flag)
			{
				changeValue = Math.Abs(changeValue);
				bool flag2 = base.EffectCount + changeValue > (int)base.MaxEffectCount;
				if (flag2)
				{
					DomainManager.Combat.ChangeSkillEffectCount(context, base.CombatChar, new SkillEffectKey(base.SkillTemplateId, base.IsDirect), (short)(-(short)base.EffectCount), true, false);
					DomainManager.Combat.AddGoneMadInjury(context, base.CombatChar, base.SkillTemplateId, 0);
				}
				else
				{
					DomainManager.Combat.ChangeSkillEffectCount(context, base.CombatChar, new SkillEffectKey(base.SkillTemplateId, base.IsDirect), (short)changeValue, true, false);
					base.ShowSpecialEffectTips(0);
				}
				DomainManager.SpecialEffect.InvalidateCache(context, base.CharacterId, 199);
				DomainManager.SpecialEffect.InvalidateCache(context, base.CharacterId, 145);
				DomainManager.SpecialEffect.InvalidateCache(context, base.CharacterId, 146);
			}
		}

		// Token: 0x06003D6B RID: 15723 RVA: 0x00251A68 File Offset: 0x0024FC68
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
				bool flag2 = dataKey.FieldId == 199;
				if (flag2)
				{
					result = base.EffectCount;
				}
				else
				{
					bool flag3 = dataKey.FieldId == 145 || dataKey.FieldId == 146;
					if (flag3)
					{
						result = base.EffectCount;
					}
					else
					{
						result = 0;
					}
				}
			}
			return result;
		}

		// Token: 0x04001217 RID: 4631
		private const sbyte AddPowerUnit = 1;

		// Token: 0x04001218 RID: 4632
		private const sbyte AddRangeUnit = 1;

		// Token: 0x04001219 RID: 4633
		private bool _effectReady;
	}
}
