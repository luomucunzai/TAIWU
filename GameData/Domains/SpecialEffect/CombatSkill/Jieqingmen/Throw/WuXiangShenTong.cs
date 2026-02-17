using System;
using System.Collections.Generic;
using Config;
using GameData.Common;
using GameData.DomainEvents;
using GameData.Domains.Combat;
using GameData.Domains.CombatSkill;
using GameData.Domains.Item;

namespace GameData.Domains.SpecialEffect.CombatSkill.Jieqingmen.Throw
{
	// Token: 0x020004DF RID: 1247
	public class WuXiangShenTong : CombatSkillEffectBase
	{
		// Token: 0x06003DBD RID: 15805 RVA: 0x002531FB File Offset: 0x002513FB
		public WuXiangShenTong()
		{
		}

		// Token: 0x06003DBE RID: 15806 RVA: 0x00253210 File Offset: 0x00251410
		public WuXiangShenTong(CombatSkillKey skillKey) : base(skillKey, 13308, -1)
		{
		}

		// Token: 0x06003DBF RID: 15807 RVA: 0x0025322C File Offset: 0x0025142C
		public override void OnEnable(DataContext context)
		{
			base.OnEnable(context);
			Events.RegisterHandler_CombatBegin(new Events.OnCombatBegin(this.OnCombatBegin));
			Events.RegisterHandler_GetShaTrick(new Events.OnGetShaTrick(this.OnGetShaTrick));
			Events.RegisterHandler_CombatStateMachineUpdateEnd(new Events.OnCombatStateMachineUpdateEnd(this.OnCombatStateMachineUpdateEnd));
		}

		// Token: 0x06003DC0 RID: 15808 RVA: 0x00253278 File Offset: 0x00251478
		public override void OnDisable(DataContext context)
		{
			Events.UnRegisterHandler_CombatBegin(new Events.OnCombatBegin(this.OnCombatBegin));
			Events.UnRegisterHandler_GetShaTrick(new Events.OnGetShaTrick(this.OnGetShaTrick));
			Events.UnRegisterHandler_CombatStateMachineUpdateEnd(new Events.OnCombatStateMachineUpdateEnd(this.OnCombatStateMachineUpdateEnd));
			base.OnDisable(context);
		}

		// Token: 0x06003DC1 RID: 15809 RVA: 0x002532C4 File Offset: 0x002514C4
		private void OnCombatBegin(DataContext context)
		{
			ItemKey[] weapons = base.CombatChar.GetWeapons();
			for (int i = 0; i < 3; i++)
			{
				bool flag = !weapons[i].IsValid();
				if (!flag)
				{
					bool flag2 = Config.Weapon.Instance[weapons[i].TemplateId].ItemSubType != 2;
					if (!flag2)
					{
						CombatWeaponData weaponData = base.CombatChar.GetWeaponData(i);
						bool flag3 = weaponData.GetAutoAttackEffect().SkillId >= 0;
						if (!flag3)
						{
							weaponData.SetAutoAttackEffect(base.EffectKey, context);
							base.ShowSpecialEffectTipsOnceInFrame(1);
						}
					}
				}
			}
		}

		// Token: 0x06003DC2 RID: 15810 RVA: 0x00253374 File Offset: 0x00251574
		private void OnGetShaTrick(DataContext context, int charId, bool isAlly, bool real)
		{
			bool flag = base.IsDirect ? (charId != base.CharacterId) : (base.CombatChar.IsAlly == isAlly);
			if (!flag)
			{
				bool flag2 = !base.IsCurrent;
				if (!flag2)
				{
					DomainManager.Combat.AddSkillPowerInCombat(context, this.SkillKey, base.EffectKey, 1);
					base.ShowSpecialEffectTips(0);
					ItemKey[] weapons = base.CombatChar.GetWeapons();
					for (int i = 0; i < 7; i++)
					{
						bool flag3 = weapons[i].IsValid() && base.CombatChar.GetWeaponData(i).GetAutoAttackEffect().SkillId == base.SkillTemplateId;
						if (flag3)
						{
							this._autoAttackWeapons.Enqueue(i);
						}
					}
				}
			}
		}

		// Token: 0x06003DC3 RID: 15811 RVA: 0x00253444 File Offset: 0x00251644
		private void OnCombatStateMachineUpdateEnd(DataContext context, CombatCharacter combatChar)
		{
			bool flag = DomainManager.Combat.Pause || combatChar.GetId() != base.CharacterId;
			if (!flag)
			{
				bool flag2 = this._internalDelayFrame > 0;
				if (flag2)
				{
					this._internalDelayFrame--;
				}
				else
				{
					bool flag3 = this._autoAttackWeapons.Count > 0 && base.CombatChar.StateMachine.GetCurrentStateType() == CombatCharacterStateType.Idle;
					if (flag3)
					{
						this.InvokeAutoAttack(context);
					}
				}
			}
		}

		// Token: 0x06003DC4 RID: 15812 RVA: 0x002534C8 File Offset: 0x002516C8
		private void InvokeAutoAttack(DataContext context)
		{
			bool anyInvoked = false;
			while (this._autoAttackWeapons.Count > 0)
			{
				int weaponIndex = this._autoAttackWeapons.Dequeue();
				ItemKey weaponKey = base.CombatChar.GetWeapons()[weaponIndex];
				bool flag = DomainManager.Item.GetBaseItem(weaponKey).GetCurrDurability() <= 0;
				if (!flag)
				{
					anyInvoked = true;
					bool flag2 = DomainManager.Combat.CalcSpiritAttack(base.CombatChar, weaponIndex);
					if (flag2)
					{
						base.ShowSpecialEffectTips(3);
					}
					this._internalDelayFrame = 12;
					break;
				}
			}
			bool flag3 = anyInvoked;
			if (flag3)
			{
				int index = context.Random.Next(WuXiangShenTong.SpiritWeaponAttackParticleName.Length);
				base.CombatChar.SetSkillSoundToPlay("se_effect_spirit_attack", context);
				base.CombatChar.SetParticleToPlay(WuXiangShenTong.SpiritWeaponAttackParticleName[index], context);
				base.CurrEnemyChar.SetParticleToPlay(WuXiangShenTong.SpiritWeaponHitParticleName[index], context);
				base.ShowSpecialEffectTips(2);
			}
		}

		// Token: 0x04001233 RID: 4659
		private static readonly string[] SpiritWeaponAttackParticleName = new string[]
		{
			"Particle_Effect_SpiritWeaponAttack1",
			"Particle_Effect_SpiritWeaponAttack2",
			"Particle_Effect_SpiritWeaponAttack3"
		};

		// Token: 0x04001234 RID: 4660
		private static readonly string[] SpiritWeaponHitParticleName = new string[]
		{
			"Particle_Effect_SpiritWeaponHit1",
			"Particle_Effect_SpiritWeaponHit2",
			"Particle_Effect_SpiritWeaponHit3"
		};

		// Token: 0x04001235 RID: 4661
		private const string SpiritWeaponSoundName = "se_effect_spirit_attack";

		// Token: 0x04001236 RID: 4662
		private const sbyte AddPowerPerTrick = 1;

		// Token: 0x04001237 RID: 4663
		private const int InvokeInternalDelayFrame = 12;

		// Token: 0x04001238 RID: 4664
		private readonly Queue<int> _autoAttackWeapons = new Queue<int>();

		// Token: 0x04001239 RID: 4665
		private int _internalDelayFrame;
	}
}
