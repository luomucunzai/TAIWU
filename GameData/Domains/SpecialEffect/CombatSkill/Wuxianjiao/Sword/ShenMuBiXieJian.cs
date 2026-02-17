using System;
using System.Collections.Generic;
using System.Linq;
using GameData.Common;
using GameData.DomainEvents;
using GameData.Domains.Combat;
using GameData.Domains.CombatSkill;
using GameData.GameDataBridge;
using GameData.Utilities;

namespace GameData.Domains.SpecialEffect.CombatSkill.Wuxianjiao.Sword
{
	// Token: 0x0200038B RID: 907
	public class ShenMuBiXieJian : CombatSkillEffectBase
	{
		// Token: 0x06003629 RID: 13865 RVA: 0x0022F94C File Offset: 0x0022DB4C
		public ShenMuBiXieJian()
		{
		}

		// Token: 0x0600362A RID: 13866 RVA: 0x0022F961 File Offset: 0x0022DB61
		public ShenMuBiXieJian(CombatSkillKey skillKey) : base(skillKey, 12303, -1)
		{
		}

		// Token: 0x0600362B RID: 13867 RVA: 0x0022F980 File Offset: 0x0022DB80
		public override void OnEnable(DataContext context)
		{
			bool isDirect = base.IsDirect;
			if (isDirect)
			{
				this._addPowerUid = new DataUid(8, 6, ulong.MaxValue, uint.MaxValue);
				GameDataBridge.AddPostDataModificationHandler(this._addPowerUid, base.DataHandlerKey, new Action<DataContext, DataUid>(this.OnAddPowerChanged));
			}
			Events.RegisterHandler_CastAttackSkillBegin(new Events.OnCastAttackSkillBegin(this.OnCastAttackSkillBegin));
			Events.RegisterHandler_AttackSkillAttackBegin(new Events.OnAttackSkillAttackBegin(this.OnAttackSkillAttackBegin));
			Events.RegisterHandler_CastSkillEnd(new Events.OnCastSkillEnd(this.OnCastSkillEnd));
		}

		// Token: 0x0600362C RID: 13868 RVA: 0x0022FA00 File Offset: 0x0022DC00
		public override void OnDisable(DataContext context)
		{
			bool isDirect = base.IsDirect;
			if (isDirect)
			{
				GameDataBridge.RemovePostDataModificationHandler(this._addPowerUid, base.DataHandlerKey);
			}
			Events.UnRegisterHandler_CastAttackSkillBegin(new Events.OnCastAttackSkillBegin(this.OnCastAttackSkillBegin));
			Events.UnRegisterHandler_AttackSkillAttackBegin(new Events.OnAttackSkillAttackBegin(this.OnAttackSkillAttackBegin));
			Events.UnRegisterHandler_CastSkillEnd(new Events.OnCastSkillEnd(this.OnCastSkillEnd));
		}

		// Token: 0x0600362D RID: 13869 RVA: 0x0022FA60 File Offset: 0x0022DC60
		private void OnCastAttackSkillBegin(DataContext context, CombatCharacter attacker, CombatCharacter defender, short skillId)
		{
			bool flag = attacker.GetId() != base.CharacterId || skillId != base.SkillTemplateId;
			if (!flag)
			{
				this._affectInCast = false;
			}
		}

		// Token: 0x0600362E RID: 13870 RVA: 0x0022FA9C File Offset: 0x0022DC9C
		private void OnAttackSkillAttackBegin(DataContext context, CombatCharacter attacker, CombatCharacter defender, short skillId, int index, bool hit)
		{
			bool flag = attacker.GetId() != base.CharacterId || skillId != base.SkillTemplateId || !hit;
			if (!flag)
			{
				this._affectInCast = true;
			}
		}

		// Token: 0x0600362F RID: 13871 RVA: 0x0022FAD8 File Offset: 0x0022DCD8
		private void OnCastSkillEnd(DataContext context, int charId, bool isAlly, short skillId, sbyte power, bool interrupted)
		{
			bool flag = charId != base.CharacterId || skillId != base.SkillTemplateId || interrupted || !this._affectInCast;
			if (!flag)
			{
				bool flag2 = base.IsDirect == !base.PowerMatchAffectRequire((int)power, 0);
				if (flag2)
				{
					this.DoTransferPower(context);
				}
				else
				{
					bool flag3 = this._transferPowerSkillList.Count > 0;
					if (flag3)
					{
						this.DoReturnPower(context);
					}
				}
			}
		}

		// Token: 0x06003630 RID: 13872 RVA: 0x0022FB54 File Offset: 0x0022DD54
		private void DoTransferPower(DataContext context)
		{
			SkillEffectKey effectKey = new SkillEffectKey(base.SkillTemplateId, base.IsDirect);
			int needMinPower = 20;
			bool flag = !base.IsDirect && (int)base.SkillInstance.GetPower() < needMinPower;
			if (!flag)
			{
				List<short> otherAttackSkills = ObjectPool<List<short>>.Instance.Get();
				otherAttackSkills.Clear();
				IEnumerable<short> attackSkills = from x in base.CombatChar.GetAttackSkillList()
				where x >= 0 && x != this.SkillTemplateId
				select x;
				bool isDirect = base.IsDirect;
				if (isDirect)
				{
					attackSkills = from x in attackSkills
					select DomainManager.CombatSkill.GetElement_CombatSkills(new CombatSkillKey(this.CharacterId, x)) into x
					where (int)x.GetPower() >= needMinPower
					select x.GetId().SkillTemplateId;
				}
				otherAttackSkills.AddRange(attackSkills);
				bool flag2 = otherAttackSkills.Count > 0;
				if (flag2)
				{
					int transferCount = Math.Min(2, otherAttackSkills.Count);
					bool flag3 = !base.IsDirect;
					if (flag3)
					{
						transferCount = Math.Min(transferCount, (int)((base.SkillInstance.GetPower() - 10) / 10));
					}
					bool flag4 = transferCount > 0;
					if (flag4)
					{
						CollectionUtils.Shuffle<short>(context.Random, otherAttackSkills);
						for (int i = 0; i < transferCount; i++)
						{
							short attackSkillId = otherAttackSkills[i];
							CombatSkillKey attackSkillKey = new CombatSkillKey(base.CharacterId, attackSkillId);
							this._transferPowerSkillList.Add(attackSkillId);
							DomainManager.Combat.ReduceSkillPowerInCombat(context, base.IsDirect ? attackSkillKey : this.SkillKey, effectKey, -10);
							DomainManager.Combat.AddSkillPowerInCombat(context, base.IsDirect ? this.SkillKey : attackSkillKey, effectKey, 10);
						}
						base.ShowSpecialEffectTips(0);
					}
				}
				ObjectPool<List<short>>.Instance.Return(otherAttackSkills);
			}
		}

		// Token: 0x06003631 RID: 13873 RVA: 0x0022FD3C File Offset: 0x0022DF3C
		private void DoReturnPower(DataContext context)
		{
			SkillEffectKey effectKey = new SkillEffectKey(base.SkillTemplateId, base.IsDirect);
			int totalPower = 0;
			for (int i = 0; i < this._transferPowerSkillList.Count; i++)
			{
				CombatSkillKey attackSkillKey = new CombatSkillKey(base.CharacterId, this._transferPowerSkillList[i]);
				bool isDirect = base.IsDirect;
				if (isDirect)
				{
					DomainManager.Combat.RemoveSkillPowerReduceInCombat(context, attackSkillKey, effectKey);
					DomainManager.Combat.AddSkillPowerInCombat(context, attackSkillKey, effectKey, 20);
				}
				else
				{
					totalPower += DomainManager.Combat.RemoveSkillPowerAddInCombat(context, attackSkillKey, effectKey);
				}
			}
			this._transferPowerSkillList.Clear();
			bool isDirect2 = base.IsDirect;
			if (isDirect2)
			{
				DomainManager.Combat.RemoveSkillPowerAddInCombat(context, this.SkillKey, effectKey);
			}
			else
			{
				bool flag = totalPower > 0;
				if (flag)
				{
					DomainManager.Combat.AddSkillPowerInCombat(context, this.SkillKey, effectKey, totalPower * 2);
				}
			}
			bool flag2 = base.IsDirect || totalPower > 0;
			if (flag2)
			{
				base.ShowSpecialEffectTips(0);
			}
		}

		// Token: 0x06003632 RID: 13874 RVA: 0x0022FE40 File Offset: 0x0022E040
		private void OnAddPowerChanged(DataContext context, DataUid dataUid)
		{
			bool flag = !DomainManager.Combat.GetAllSkillPowerAddInCombat().ContainsKey(this.SkillKey);
			if (flag)
			{
				this._transferPowerSkillList.Clear();
			}
		}

		// Token: 0x04000FC8 RID: 4040
		private const sbyte AffectSkillCount = 2;

		// Token: 0x04000FC9 RID: 4041
		private const sbyte TransferPower = 10;

		// Token: 0x04000FCA RID: 4042
		private readonly List<short> _transferPowerSkillList = new List<short>();

		// Token: 0x04000FCB RID: 4043
		private DataUid _addPowerUid;

		// Token: 0x04000FCC RID: 4044
		private bool _affectInCast;
	}
}
