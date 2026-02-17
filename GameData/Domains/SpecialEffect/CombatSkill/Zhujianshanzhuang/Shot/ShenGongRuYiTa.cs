using System;
using System.Linq;
using Config;
using GameData.Combat.Math;
using GameData.Common;
using GameData.DomainEvents;
using GameData.Domains.Combat;
using GameData.Domains.CombatSkill;

namespace GameData.Domains.SpecialEffect.CombatSkill.Zhujianshanzhuang.Shot
{
	// Token: 0x020001BE RID: 446
	public class ShenGongRuYiTa : CombatSkillEffectBase
	{
		// Token: 0x06002C9E RID: 11422 RVA: 0x002082CA File Offset: 0x002064CA
		public ShenGongRuYiTa()
		{
		}

		// Token: 0x06002C9F RID: 11423 RVA: 0x002082D4 File Offset: 0x002064D4
		public ShenGongRuYiTa(CombatSkillKey skillKey) : base(skillKey, 9408, -1)
		{
		}

		// Token: 0x06002CA0 RID: 11424 RVA: 0x002082E8 File Offset: 0x002064E8
		public override void OnEnable(DataContext context)
		{
			base.OnEnable(context);
			this._affected = false;
			this._addingPowerSkill = -1;
			this._addingPowerPercent = 0;
			base.CreateAffectedData(199, EDataModifyType.AddPercent, -1);
			base.CreateAffectedData(base.IsDirect ? 313 : 314, EDataModifyType.Custom, -1);
			Events.RegisterHandler_CombatBegin(new Events.OnCombatBegin(this.OnCombatBegin));
			Events.RegisterHandler_PrepareSkillBegin(new Events.OnPrepareSkillBegin(this.OnPrepareSkillBegin));
			Events.RegisterHandler_CastSkillEnd(new Events.OnCastSkillEnd(this.OnCastSkillEnd));
			Events.RegisterHandler_CombatCharChanged(new Events.OnCombatCharChanged(this.OnCombatCharChanged));
			Events.RegisterHandler_WisdomCosted(new Events.OnWisdomCosted(this.OnWisdomCosted));
			Events.RegisterHandler_JiTrickInsteadCostTricks(new Events.OnJiTrickInsteadCostTricks(this.OnInsteadTrick));
			Events.RegisterHandler_UselessTrickInsteadJiTricks(new Events.OnUselessTrickInsteadJiTricks(this.OnInsteadTrick));
		}

		// Token: 0x06002CA1 RID: 11425 RVA: 0x002083BC File Offset: 0x002065BC
		public override void OnDisable(DataContext context)
		{
			Events.UnRegisterHandler_CombatBegin(new Events.OnCombatBegin(this.OnCombatBegin));
			Events.UnRegisterHandler_PrepareSkillBegin(new Events.OnPrepareSkillBegin(this.OnPrepareSkillBegin));
			Events.UnRegisterHandler_CastSkillEnd(new Events.OnCastSkillEnd(this.OnCastSkillEnd));
			Events.UnRegisterHandler_CombatCharChanged(new Events.OnCombatCharChanged(this.OnCombatCharChanged));
			Events.UnRegisterHandler_WisdomCosted(new Events.OnWisdomCosted(this.OnWisdomCosted));
			Events.UnRegisterHandler_JiTrickInsteadCostTricks(new Events.OnJiTrickInsteadCostTricks(this.OnInsteadTrick));
			Events.UnRegisterHandler_UselessTrickInsteadJiTricks(new Events.OnUselessTrickInsteadJiTricks(this.OnInsteadTrick));
			base.OnDisable(context);
		}

		// Token: 0x06002CA2 RID: 11426 RVA: 0x00208450 File Offset: 0x00206650
		private void OnCombatBegin(DataContext context)
		{
			base.AddMaxEffectCount(true);
			this.UpdateAffected(context);
		}

		// Token: 0x06002CA3 RID: 11427 RVA: 0x00208464 File Offset: 0x00206664
		private void OnPrepareSkillBegin(DataContext context, int charId, bool isAlly, short skillId)
		{
			bool flag = charId != base.CharacterId || base.EffectCount <= 0;
			if (!flag)
			{
				bool flag2 = !Boss.Instance.Any((BossItem x) => x.PlayerCastSkills.Contains(skillId));
				if (!flag2)
				{
					this._addingPowerSkill = skillId;
					this._addingPowerPercent = base.EffectCount * 10;
					this.ReduceEffectCountAndWisdom(context, base.EffectCount);
					base.InvalidateCache(context, 199);
					base.ShowSpecialEffectTips(2);
				}
			}
		}

		// Token: 0x06002CA4 RID: 11428 RVA: 0x002084FC File Offset: 0x002066FC
		private void OnCastSkillEnd(DataContext context, int charId, bool isAlly, short skillId, sbyte power, bool interrupted)
		{
			bool flag = this.SkillKey.IsMatch(charId, skillId) && base.PowerMatchAffectRequire((int)power, 0);
			if (flag)
			{
				bool prevAffected = this._affected;
				int prevEffectCount = base.EffectCount;
				base.AddMaxEffectCount(true);
				this.UpdateAffected(context);
				bool flag2 = prevAffected && this._affected;
				if (flag2)
				{
					this.TryAddWisdom(context, prevEffectCount);
				}
			}
			else
			{
				bool flag3 = charId == base.CharacterId && skillId == this._addingPowerSkill;
				if (flag3)
				{
					this._addingPowerSkill = -1;
					this._addingPowerPercent = 0;
					base.InvalidateCache(context, 199);
				}
			}
		}

		// Token: 0x06002CA5 RID: 11429 RVA: 0x002085A0 File Offset: 0x002067A0
		private void OnCombatCharChanged(DataContext context, bool isAlly)
		{
			bool flag = isAlly == base.CombatChar.IsAlly;
			if (flag)
			{
				this.UpdateAffected(context);
			}
		}

		// Token: 0x06002CA6 RID: 11430 RVA: 0x002085C8 File Offset: 0x002067C8
		private void OnWisdomCosted(DataContext context, bool isAlly, int value)
		{
			bool flag = isAlly != base.CombatChar.IsAlly || !this._affected;
			if (!flag)
			{
				base.ReduceEffectCount(Math.Min(base.EffectCount, value));
				this.UpdateAffected(context);
				base.ShowSpecialEffectTips(1);
			}
		}

		// Token: 0x06002CA7 RID: 11431 RVA: 0x0020861C File Offset: 0x0020681C
		private void OnInsteadTrick(DataContext context, CombatCharacter character, int count)
		{
			bool flag = character.GetId() != base.CharacterId || count <= 0;
			if (!flag)
			{
				this.ReduceEffectCountAndWisdom(context, count);
				base.ShowSpecialEffectTips(0);
			}
		}

		// Token: 0x06002CA8 RID: 11432 RVA: 0x0020865C File Offset: 0x0020685C
		private void ReduceEffectCountAndWisdom(DataContext context, int count)
		{
			bool affected = this._affected;
			if (affected)
			{
				DomainManager.Combat.ChangeWisdom(context, base.CombatChar.IsAlly, -count);
			}
			base.ReduceEffectCount(count);
			this.UpdateAffected(context);
		}

		// Token: 0x06002CA9 RID: 11433 RVA: 0x002086A0 File Offset: 0x002068A0
		private void UpdateAffected(DataContext context)
		{
			bool affected = base.IsCurrent && base.EffectCount > 0;
			bool flag = affected == this._affected;
			if (!flag)
			{
				this._affected = affected;
				bool flag2 = affected;
				if (flag2)
				{
					DomainManager.Combat.ChangeWisdom(context, base.CombatChar.IsAlly, base.EffectCount);
				}
				else
				{
					DomainManager.Combat.ChangeWisdom(context, base.CombatChar.IsAlly, -base.EffectCount);
				}
			}
		}

		// Token: 0x06002CAA RID: 11434 RVA: 0x0020871C File Offset: 0x0020691C
		private void TryAddWisdom(DataContext context, int prevEffectCount)
		{
			int delta = base.EffectCount - prevEffectCount;
			bool flag = delta == 0;
			if (!flag)
			{
				DomainManager.Combat.ChangeWisdom(context, base.CombatChar.IsAlly, delta);
			}
		}

		// Token: 0x06002CAB RID: 11435 RVA: 0x00208758 File Offset: 0x00206958
		public override int GetModifyValue(AffectedDataKey dataKey, int currModifyValue)
		{
			bool flag = dataKey.FieldId == 199 && dataKey.CharId == base.CharacterId && dataKey.CombatSkillId == this._addingPowerSkill;
			int result;
			if (flag)
			{
				result = this._addingPowerPercent;
			}
			else
			{
				result = base.GetModifyValue(dataKey, currModifyValue);
			}
			return result;
		}

		// Token: 0x06002CAC RID: 11436 RVA: 0x002087AC File Offset: 0x002069AC
		public override int GetModifiedValue(AffectedDataKey dataKey, int dataValue)
		{
			ushort fieldId = dataKey.FieldId;
			bool flag = fieldId - 313 <= 1;
			bool flag2 = flag;
			int result;
			if (flag2)
			{
				result = dataValue + base.EffectCount;
			}
			else
			{
				result = base.GetModifiedValue(dataKey, dataValue);
			}
			return result;
		}

		// Token: 0x04000D74 RID: 3444
		private const int AddPowerPercentPerEffectCount = 10;

		// Token: 0x04000D75 RID: 3445
		private bool _affected;

		// Token: 0x04000D76 RID: 3446
		private short _addingPowerSkill;

		// Token: 0x04000D77 RID: 3447
		private int _addingPowerPercent;
	}
}
