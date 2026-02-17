using System;
using System.Collections.Generic;
using Config;
using GameData.Combat.Math;
using GameData.Common;
using GameData.DomainEvents;
using GameData.Domains.Character;
using GameData.Domains.Combat;
using GameData.Domains.CombatSkill;
using GameData.Domains.SpecialEffect.CombatSkill.Common.Agile;

namespace GameData.Domains.SpecialEffect.CombatSkill.Fulongtan.Agile
{
	// Token: 0x02000537 RID: 1335
	public class ZuiWoDongHai : AgileSkillBase
	{
		// Token: 0x17000257 RID: 599
		// (get) Token: 0x06003FB9 RID: 16313 RVA: 0x0025B235 File Offset: 0x00259435
		private CValuePercent AddPowerPercent
		{
			get
			{
				return this.EatingWine ? 40 : 20;
			}
		}

		// Token: 0x17000258 RID: 600
		// (get) Token: 0x06003FBA RID: 16314 RVA: 0x0025B24A File Offset: 0x0025944A
		private CValuePercent ReducePowerPercent
		{
			get
			{
				return this.EatingWine ? 50 : 100;
			}
		}

		// Token: 0x17000259 RID: 601
		// (get) Token: 0x06003FBB RID: 16315 RVA: 0x0025B25F File Offset: 0x0025945F
		private unsafe bool EatingWine
		{
			get
			{
				return (*this.CharObj.GetEatingItems()).ContainsWine();
			}
		}

		// Token: 0x06003FBC RID: 16316 RVA: 0x0025B276 File Offset: 0x00259476
		public ZuiWoDongHai()
		{
		}

		// Token: 0x06003FBD RID: 16317 RVA: 0x0025B280 File Offset: 0x00259480
		public ZuiWoDongHai(CombatSkillKey skillKey) : base(skillKey, 14405)
		{
			this.ListenCanAffectChange = !base.IsDirect;
		}

		// Token: 0x06003FBE RID: 16318 RVA: 0x0025B2A0 File Offset: 0x002594A0
		public override void OnEnable(DataContext context)
		{
			base.OnEnable(context);
			this._affectingSkill = -1;
			this._changePower = 0;
			this.AffectDatas = new Dictionary<AffectedDataKey, EDataModifyType>();
			this.AffectDatas.Add(new AffectedDataKey(base.CharacterId, 199, -1, -1, -1, -1), EDataModifyType.TotalPercent);
			bool isDirect = base.IsDirect;
			if (isDirect)
			{
				Events.RegisterHandler_CostBreathAndStance(new Events.OnCostBreathAndStance(this.OnCostBreathAndStance));
			}
			else
			{
				this.AffectDatas.Add(new AffectedDataKey(base.CharacterId, 225, -1, -1, -1, -1), EDataModifyType.Custom);
				this.AffectDatas.Add(new AffectedDataKey(base.CharacterId, 226, -1, -1, -1, -1), EDataModifyType.Custom);
				Events.RegisterHandler_CastSkillOnLackBreathStance(new Events.OnCastSkillOnLackBreathStance(this.OnCastSkillOnLackBreathStance));
			}
			Events.RegisterHandler_CastSkillEnd(new Events.OnCastSkillEnd(this.OnCastSkillEnd));
		}

		// Token: 0x06003FBF RID: 16319 RVA: 0x0025B378 File Offset: 0x00259578
		public override void OnDisable(DataContext context)
		{
			base.OnDisable(context);
			bool isDirect = base.IsDirect;
			if (isDirect)
			{
				Events.UnRegisterHandler_CostBreathAndStance(new Events.OnCostBreathAndStance(this.OnCostBreathAndStance));
			}
			else
			{
				Events.UnRegisterHandler_CastSkillOnLackBreathStance(new Events.OnCastSkillOnLackBreathStance(this.OnCastSkillOnLackBreathStance));
			}
			Events.UnRegisterHandler_CastSkillEnd(new Events.OnCastSkillEnd(this.OnCastSkillEnd));
		}

		// Token: 0x06003FC0 RID: 16320 RVA: 0x0025B3D0 File Offset: 0x002595D0
		private void OnCostBreathAndStance(DataContext context, int charId, bool isAlly, int costBreath, int costStance, short skillId)
		{
			bool flag = charId != base.CharacterId || !base.CanAffect || Config.CombatSkill.Instance[skillId].EquipType != 1;
			if (!flag)
			{
				int breathValue = base.CombatChar.GetBreathValue();
				int stanceValue = base.CombatChar.GetStanceValue();
				this._affectingSkill = skillId;
				this._changePower = (breathValue * 100 / 30000 + stanceValue * 100 / 4000) * this.AddPowerPercent;
				bool flag2 = breathValue > 0;
				if (flag2)
				{
					DomainManager.Combat.ChangeBreathValue(context, base.CombatChar, -breathValue, false, null);
				}
				bool flag3 = stanceValue > 0;
				if (flag3)
				{
					DomainManager.Combat.ChangeStanceValue(context, base.CombatChar, -stanceValue, false, null);
				}
				bool flag4 = this._changePower > 0;
				if (flag4)
				{
					DomainManager.SpecialEffect.InvalidateCache(context, base.CharacterId, 199);
					base.ShowSpecialEffectTips(0);
				}
				this.AutoRemove = false;
			}
		}

		// Token: 0x06003FC1 RID: 16321 RVA: 0x0025B4D0 File Offset: 0x002596D0
		private void OnCastSkillOnLackBreathStance(DataContext context, CombatCharacter combatChar, short skillId, int lackBreath, int lackStance, int costBreath, int costStance)
		{
			bool flag = combatChar != base.CombatChar || !base.CanAffect || Config.CombatSkill.Instance[skillId].EquipType != 1;
			if (!flag)
			{
				int breathStancePercent = 0;
				bool flag2 = lackBreath < 0;
				if (flag2)
				{
					breathStancePercent += lackBreath * 100 / costBreath;
				}
				bool flag3 = lackStance < 0;
				if (flag3)
				{
					breathStancePercent += lackStance * 100 / costStance;
				}
				this._affectingSkill = skillId;
				this._changePower = breathStancePercent * this.ReducePowerPercent;
				DomainManager.SpecialEffect.InvalidateCache(context, base.CharacterId, 199);
				base.ShowSpecialEffectTips(0);
				this.AutoRemove = false;
			}
		}

		// Token: 0x06003FC2 RID: 16322 RVA: 0x0025B578 File Offset: 0x00259778
		private void OnCastSkillEnd(DataContext context, int charId, bool isAlly, short skillId, sbyte power, bool interrupted)
		{
			bool flag = charId != base.CharacterId || Config.CombatSkill.Instance[skillId].EquipType != 1;
			if (!flag)
			{
				bool agileSkillChanged = this.AgileSkillChanged;
				if (agileSkillChanged)
				{
					base.RemoveSelf(context);
				}
				else
				{
					this._affectingSkill = -1;
					bool flag2 = this._changePower != 0;
					if (flag2)
					{
						this._changePower = 0;
						DomainManager.SpecialEffect.InvalidateCache(context, base.CharacterId, 199);
					}
					this.AutoRemove = true;
				}
			}
		}

		// Token: 0x06003FC3 RID: 16323 RVA: 0x0025B602 File Offset: 0x00259802
		protected override void OnMoveSkillCanAffectChanged(DataContext context, DataUid dataUid)
		{
			DomainManager.Combat.UpdateSkillCanUse(context, base.CombatChar);
		}

		// Token: 0x06003FC4 RID: 16324 RVA: 0x0025B618 File Offset: 0x00259818
		public override bool GetModifiedValue(AffectedDataKey dataKey, bool dataValue)
		{
			bool flag = dataKey.CharId != base.CharacterId || !base.CanAffect || Config.CombatSkill.Instance[dataKey.CombatSkillId].EquipType != 1;
			bool result;
			if (flag)
			{
				result = dataValue;
			}
			else
			{
				bool flag2 = dataKey.FieldId == 225;
				if (flag2)
				{
					int costBreath = dataKey.CustomParam0;
					result = (base.CombatChar.GetBreathValue() >= costBreath * ZuiWoDongHai.MinCostBreathOrStancePercent);
				}
				else
				{
					bool flag3 = dataKey.FieldId == 226;
					if (flag3)
					{
						int costStance = dataKey.CustomParam0;
						result = (base.CombatChar.GetStanceValue() >= costStance * ZuiWoDongHai.MinCostBreathOrStancePercent);
					}
					else
					{
						result = dataValue;
					}
				}
			}
			return result;
		}

		// Token: 0x06003FC5 RID: 16325 RVA: 0x0025B6DC File Offset: 0x002598DC
		public override int GetModifyValue(AffectedDataKey dataKey, int currModifyValue)
		{
			bool flag = dataKey.CharId != base.CharacterId || dataKey.CombatSkillId != this._affectingSkill;
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
					result = this._changePower;
				}
				else
				{
					result = 0;
				}
			}
			return result;
		}

		// Token: 0x040012C7 RID: 4807
		private static readonly CValuePercent MinCostBreathOrStancePercent = 50;

		// Token: 0x040012C8 RID: 4808
		private short _affectingSkill;

		// Token: 0x040012C9 RID: 4809
		private int _changePower;
	}
}
