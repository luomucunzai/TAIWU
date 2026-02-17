using System;
using System.Collections.Generic;
using Config;
using GameData.Combat.Math;
using GameData.Common;
using GameData.DomainEvents;
using GameData.Domains.CombatSkill;
using GameData.Domains.SpecialEffect.CombatSkill.Common.Agile;

namespace GameData.Domains.SpecialEffect.CombatSkill.Wudangpai.Agile
{
	// Token: 0x020003E2 RID: 994
	public class TaiYiJiuGongBu : AgileSkillBase
	{
		// Token: 0x06003803 RID: 14339 RVA: 0x002384D5 File Offset: 0x002366D5
		public TaiYiJiuGongBu()
		{
		}

		// Token: 0x06003804 RID: 14340 RVA: 0x002384DF File Offset: 0x002366DF
		public TaiYiJiuGongBu(CombatSkillKey skillKey) : base(skillKey, 4404)
		{
			this.ListenCanAffectChange = true;
		}

		// Token: 0x06003805 RID: 14341 RVA: 0x002384F8 File Offset: 0x002366F8
		public override void OnEnable(DataContext context)
		{
			base.OnEnable(context);
			this.AffectDatas = new Dictionary<AffectedDataKey, EDataModifyType>();
			bool isDirect = base.IsDirect;
			if (isDirect)
			{
				this.AffectDatas.Add(new AffectedDataKey(base.CharacterId, 15, -1, -1, -1, -1), EDataModifyType.Add);
				this.AffectDatas.Add(new AffectedDataKey(base.CharacterId, 199, -1, -1, -1, -1), EDataModifyType.AddPercent);
			}
			else
			{
				foreach (int enemyCharId in DomainManager.Combat.GetCharacterList(!base.CombatChar.IsAlly))
				{
					bool flag = enemyCharId >= 0;
					if (flag)
					{
						this.AffectDatas.Add(new AffectedDataKey(enemyCharId, 15, -1, -1, -1, -1), EDataModifyType.Add);
						this.AffectDatas.Add(new AffectedDataKey(enemyCharId, 199, -1, -1, -1, -1), EDataModifyType.AddPercent);
					}
				}
			}
			this._affecting = false;
			this._changePowerSkill.CharId = -1;
			this._changePower = -1;
			this.UpdateCanAffect(context, default(DataUid));
			bool affecting = this._affecting;
			if (affecting)
			{
				base.ShowSpecialEffectTips(0);
			}
			Events.RegisterHandler_PrepareSkillEnd(new Events.OnPrepareSkillEnd(this.OnPrepareSkillEnd));
			Events.RegisterHandler_CastSkillEnd(new Events.OnCastSkillEnd(this.OnCastSkillEnd));
		}

		// Token: 0x06003806 RID: 14342 RVA: 0x00238643 File Offset: 0x00236843
		public override void OnDisable(DataContext context)
		{
			base.OnDisable(context);
			Events.UnRegisterHandler_PrepareSkillEnd(new Events.OnPrepareSkillEnd(this.OnPrepareSkillEnd));
			Events.UnRegisterHandler_CastSkillEnd(new Events.OnCastSkillEnd(this.OnCastSkillEnd));
		}

		// Token: 0x06003807 RID: 14343 RVA: 0x00238674 File Offset: 0x00236874
		protected override void OnMoveSkillCanAffectChanged(DataContext context, DataUid dataUid)
		{
			this.UpdateCanAffect(context, default(DataUid));
		}

		// Token: 0x06003808 RID: 14344 RVA: 0x00238694 File Offset: 0x00236894
		private void OnPrepareSkillEnd(DataContext context, int charId, bool isAlly, short skillId)
		{
			bool flag = !this._affecting || Config.CombatSkill.Instance[skillId].EquipType != 1 || !(base.IsDirect ? (charId == base.CharacterId) : (base.CombatChar.IsAlly != isAlly));
			if (!flag)
			{
				CombatSkillKey skillKey = new CombatSkillKey(charId, skillId);
				int skillInnerRatio = (int)DomainManager.CombatSkill.GetElement_CombatSkills(skillKey).GetCurrInnerRatio();
				this._changePower = (base.IsDirect ? Math.Max(40 - Math.Abs(skillInnerRatio - 50), 0) : (-Math.Min(Math.Abs(skillInnerRatio - 50), 40)));
				bool flag2 = this._changePower != 0;
				if (flag2)
				{
					this._changePowerSkill = skillKey;
					DomainManager.SpecialEffect.InvalidateCache(context, charId, 199);
					base.ShowSpecialEffectTips(1);
				}
			}
		}

		// Token: 0x06003809 RID: 14345 RVA: 0x0023876C File Offset: 0x0023696C
		private void OnCastSkillEnd(DataContext context, int charId, bool isAlly, short skillId, sbyte power, bool interrupted)
		{
			bool flag = charId != this._changePowerSkill.CharId || skillId != this._changePowerSkill.SkillTemplateId;
			if (!flag)
			{
				this._changePowerSkill.CharId = -1;
				DomainManager.SpecialEffect.InvalidateCache(context, charId, 199);
			}
		}

		// Token: 0x0600380A RID: 14346 RVA: 0x002387C4 File Offset: 0x002369C4
		private void UpdateCanAffect(DataContext context, DataUid dataUid)
		{
			bool canAffect = base.CanAffect;
			bool flag = this._affecting == canAffect;
			if (!flag)
			{
				this._affecting = canAffect;
				bool isDirect = base.IsDirect;
				if (isDirect)
				{
					DomainManager.SpecialEffect.InvalidateCache(context, base.CharacterId, 15);
				}
				else
				{
					int[] charList = DomainManager.Combat.GetCharacterList(!base.CombatChar.IsAlly);
					for (int i = 0; i < charList.Length; i++)
					{
						bool flag2 = charList[i] >= 0;
						if (flag2)
						{
							DomainManager.SpecialEffect.InvalidateCache(context, charList[i], 15);
						}
					}
				}
			}
		}

		// Token: 0x0600380B RID: 14347 RVA: 0x0023886C File Offset: 0x00236A6C
		public override int GetModifyValue(AffectedDataKey dataKey, int currModifyValue)
		{
			bool flag = !this._affecting;
			int result;
			if (flag)
			{
				result = 0;
			}
			else
			{
				bool flag2 = dataKey.FieldId == 15;
				if (flag2)
				{
					result = (base.IsDirect ? 50 : -50);
				}
				else
				{
					bool flag3 = dataKey.FieldId == 199 && dataKey.CharId == this._changePowerSkill.CharId && dataKey.CombatSkillId == this._changePowerSkill.SkillTemplateId;
					if (flag3)
					{
						result = this._changePower;
					}
					else
					{
						result = 0;
					}
				}
			}
			return result;
		}

		// Token: 0x0400105F RID: 4191
		private const sbyte ChangeInnerRatio = 50;

		// Token: 0x04001060 RID: 4192
		private const int MaxChangePower = 40;

		// Token: 0x04001061 RID: 4193
		private bool _affecting;

		// Token: 0x04001062 RID: 4194
		private CombatSkillKey _changePowerSkill;

		// Token: 0x04001063 RID: 4195
		private int _changePower;
	}
}
