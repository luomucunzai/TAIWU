using System;
using Config;
using GameData.Common;
using GameData.Domains.Item;

namespace GameData.Domains.Combat
{
	// Token: 0x020006A5 RID: 1701
	public class CombatCharacterStateSelectMercy : CombatCharacterStateBase
	{
		// Token: 0x06006230 RID: 25136 RVA: 0x0037E3E6 File Offset: 0x0037C5E6
		public CombatCharacterStateSelectMercy(CombatDomain combatDomain, CombatCharacter combatChar) : base(combatDomain, combatChar, CombatCharacterStateType.SelectMercy)
		{
			this.IsUpdateOnPause = true;
		}

		// Token: 0x06006231 RID: 25137 RVA: 0x0037E3FC File Offset: 0x0037C5FC
		public override void OnEnter()
		{
			base.OnEnter();
			DataContext context = this.CombatChar.GetDataContext();
			this.CombatChar.NeedSelectMercyOption = false;
			this._optionType = ((!this.CombatChar.IsAlly) ? EShowMercyOption.EnemyShowMercy : (this.CurrentCombatDomain.IsInfectedCombat() ? EShowMercyOption.FuyuSword : EShowMercyOption.PlayerShowMercy));
			this.CurrentCombatDomain.SetShowMercyOption(context, this._optionType);
			this._selected = EShowMercySelect.Unselected;
			this.CurrentCombatDomain.SetSelectedMercyOption(context, this._selected);
		}

		// Token: 0x06006232 RID: 25138 RVA: 0x0037E480 File Offset: 0x0037C680
		public override void OnExit()
		{
			DataContext context = this.CombatChar.GetDataContext();
			this.CurrentCombatDomain.SetShowMercyOption(context, EShowMercyOption.Invalid);
			bool flag = this.CurrentCombatDomain.IsInCombat();
			if (flag)
			{
				this.CurrentCombatDomain.SetDisplayPosition(context, this.CombatChar.IsAlly, int.MinValue);
			}
			base.OnExit();
		}

		// Token: 0x06006233 RID: 25139 RVA: 0x0037E4DC File Offset: 0x0037C6DC
		public override bool OnUpdate()
		{
			bool flag = !base.OnUpdate();
			bool result;
			if (flag)
			{
				result = false;
			}
			else
			{
				DataContext context = this.CombatChar.GetDataContext();
				EShowMercySelect selected = (EShowMercySelect)this.CurrentCombatDomain.GetSelectedMercyOption();
				bool flag2 = selected <= EShowMercySelect.Unselected || this._selected > EShowMercySelect.Unselected;
				if (flag2)
				{
					result = false;
				}
				else
				{
					this._selected = selected;
					bool flag3 = selected == EShowMercySelect.Cancel;
					if (flag3)
					{
						this.ApplyFailEffect();
					}
					else
					{
						bool flag4 = this._optionType == EShowMercyOption.FuyuSword;
						if (flag4)
						{
							CombatItemUseItem fuyuConfig = CombatItemUse.DefValue.PrepareFuyuSword;
							this.CombatChar.SetAnimationToPlayOnce(fuyuConfig.Animation, context);
							this.CombatChar.SetParticleToPlay(fuyuConfig.Particle, context);
							this.CombatChar.SetSkillSoundToPlay(fuyuConfig.Sound, context);
							this.CombatChar.SetAnimationToLoop(null, context);
							base.DelayCall(new CombatCharacterStateBase.CombatCharacterStateDelayCallRequest(this.OnPreparedFuyu), Config.Misc.DefValue.FuyuSwordFragment.UseFrame);
						}
						else
						{
							string flashAni = "C_007_1";
							base.DelayCall(new CombatCharacterStateBase.CombatCharacterStateDelayCallRequest(this.OnFlash), AnimDataCollection.GetDurationFrame(flashAni));
							this.CombatChar.SetAnimationToPlayOnce(flashAni, context);
							this.CombatChar.SetAnimationToLoop(null, context);
							this.CombatChar.SetSkillSoundToPlay("se_combat_preskill", context);
						}
					}
					result = false;
				}
			}
			return result;
		}

		// Token: 0x06006234 RID: 25140 RVA: 0x0037E628 File Offset: 0x0037C828
		private void OnFlash()
		{
			DataContext context = this.CurrentCombatDomain.Context;
			sbyte trickType = this.CombatChar.GetWeaponTricks()[(int)this.CombatChar.GetWeaponTrickIndex()];
			GameData.Domains.Item.Weapon weapon = DomainManager.Item.GetElement_Weapons(this.CurrentCombatDomain.GetUsingWeaponKey(this.CombatChar).Id);
			ValueTuple<string, string> prepareAni = this.CurrentCombatDomain.GetPrepareAttackAni(this.CombatChar, trickType, (int)weapon.GetWeaponAction());
			base.DelayCall(new CombatCharacterStateBase.CombatCharacterStateDelayCallRequest(this.OnPrepared), AnimDataCollection.GetDurationFrame(prepareAni.Item2));
			this.CombatChar.SetAnimationToPlayOnce(prepareAni.Item1, context);
		}

		// Token: 0x06006235 RID: 25141 RVA: 0x0037E6C8 File Offset: 0x0037C8C8
		private void OnPrepared()
		{
			DataContext context = this.CurrentCombatDomain.Context;
			sbyte trickType = this.CombatChar.GetWeaponTricks()[(int)this.CombatChar.GetWeaponTrickIndex()];
			int weaponIndex = this.CombatChar.GetUsingWeaponIndex();
			BossItem bossConfig = this.CombatChar.BossConfig;
			sbyte b;
			if (bossConfig == null)
			{
				AnimalItem animalConfig = this.CombatChar.AnimalConfig;
				b = ((animalConfig != null) ? animalConfig.AttackDistances[weaponIndex] : TrickType.Instance[trickType].AttackDistance[0]);
			}
			else
			{
				b = bossConfig.AttackDistances[(int)this.CombatChar.GetBossPhase()][weaponIndex];
			}
			sbyte displayDistance = b;
			int delayFrame = 0;
			bool flag = displayDistance > 0 && (short)displayDistance != this.CurrentCombatDomain.GetCurrentDistance();
			if (flag)
			{
				delayFrame = 9;
				this.CurrentCombatDomain.SetDisplayPosition(context, this.CombatChar.IsAlly, this.CurrentCombatDomain.GetDisplayPosition(this.CombatChar.IsAlly, (short)displayDistance));
			}
			base.DelayCall(new CombatCharacterStateBase.CombatCharacterStateDelayCallRequest(this.PlayAttackAnimation), delayFrame);
		}

		// Token: 0x06006236 RID: 25142 RVA: 0x0037E7C8 File Offset: 0x0037C9C8
		private void PlayAttackAnimation()
		{
			DataContext context = this.CombatChar.GetDataContext();
			int weaponId = this.CurrentCombatDomain.GetUsingWeaponKey(this.CombatChar).Id;
			GameData.Domains.Item.Weapon weapon = DomainManager.Item.GetElement_Weapons(weaponId);
			sbyte trickType = this.CombatChar.GetWeaponTricks()[(int)this.CombatChar.GetWeaponTrickIndex()];
			ValueTuple<string, string, string, string> attackEffect = this.CurrentCombatDomain.GetAttackEffect(this.CombatChar, weapon, trickType);
			base.DelayCall(new CombatCharacterStateBase.CombatCharacterStateDelayCallRequest(this.ApplyFailEffect), AnimDataCollection.GetEventFrame(attackEffect.Item2, "act0", 0));
			base.DelayCall(new CombatCharacterStateBase.CombatCharacterStateDelayCallRequest(this.OnAttacked), AnimDataCollection.GetDurationFrame(attackEffect.Item2));
			this.CombatChar.SetAnimationToPlayOnce(attackEffect.Item1, context);
			this.CombatChar.SetParticleToPlay(attackEffect.Item3, context);
			this.CombatChar.SetAttackSoundToPlay(attackEffect.Item4, context);
		}

		// Token: 0x06006237 RID: 25143 RVA: 0x0037E8B4 File Offset: 0x0037CAB4
		private void OnAttacked()
		{
			DataContext context = this.CombatChar.GetDataContext();
			this.CombatChar.SetAnimationToLoop(this.CurrentCombatDomain.GetIdleAni(this.CombatChar), context);
		}

		// Token: 0x06006238 RID: 25144 RVA: 0x0037E8EC File Offset: 0x0037CAEC
		private void ApplyFailEffect()
		{
			bool flag = this._optionType != EShowMercyOption.FuyuSword || this._selected == EShowMercySelect.Cancel;
			if (flag)
			{
				this.SetFailAnimation();
			}
			base.DelayCall(new CombatCharacterStateBase.CombatCharacterStateDelayCallRequest(this.OnSettlement), (int)((short)(Math.Ceiling((double)DomainManager.Combat.GetTimeScale()) + 1.0)));
		}

		// Token: 0x06006239 RID: 25145 RVA: 0x0037E948 File Offset: 0x0037CB48
		private void SetFailAnimation()
		{
			DataContext context = this.CombatChar.GetDataContext();
			CombatCharacter enemyChar = this.CurrentCombatDomain.GetCombatCharacter(!this.CombatChar.IsAlly, false);
			bool kill = this._selected > EShowMercySelect.Cancel;
			ValueTuple<string, string, string> failAnimationAndSound = this.CurrentCombatDomain.GetFailAnimationAndSound(context, this.CombatChar, false, kill);
			string anim = failAnimationAndSound.Item1;
			string particle = failAnimationAndSound.Item2;
			string sound = failAnimationAndSound.Item3;
			bool flag = kill;
			if (flag)
			{
				int weaponId = this.CurrentCombatDomain.GetUsingWeaponKey(this.CombatChar).Id;
				GameData.Domains.Item.Weapon weapon = DomainManager.Item.GetElement_Weapons(weaponId);
				WeaponItem configData = Config.Weapon.Instance[weapon.GetTemplateId()];
				this.CurrentCombatDomain.PlayHitSound(context, enemyChar, configData);
				this.CurrentCombatDomain.ClearBurstBodyPartFlawAndAcupoint(context, enemyChar, anim);
			}
			else
			{
				this.CombatChar.PlayWinAnimation(context);
			}
			enemyChar.SetAnimationToPlayOnce(anim, context);
			bool flag2 = !string.IsNullOrEmpty(particle);
			if (flag2)
			{
				enemyChar.SetParticleToPlay(particle, context);
			}
			bool flag3 = !string.IsNullOrEmpty(sound);
			if (flag3)
			{
				enemyChar.SetDieSoundToPlay(sound, context);
			}
		}

		// Token: 0x0600623A RID: 25146 RVA: 0x0037EA60 File Offset: 0x0037CC60
		private void OnSettlement()
		{
			DataContext context = this.CombatChar.GetDataContext();
			this.CurrentCombatDomain.CombatSettlement(context, this.CombatChar.IsAlly ? CombatStatusType.EnemyFail : CombatStatusType.SelfFail);
			this.CombatChar.StateMachine.TranslateState();
		}

		// Token: 0x0600623B RID: 25147 RVA: 0x0037EAB4 File Offset: 0x0037CCB4
		private void OnPreparedFuyu()
		{
			DataContext context = this.CurrentCombatDomain.Context;
			CombatItemUseItem useConfig = CombatItemUse.DefValue.UseFuyuSword;
			int delayFrame = 0;
			short displayDistance = useConfig.Distance;
			bool flag = displayDistance > 0 && displayDistance != this.CurrentCombatDomain.GetCurrentDistance();
			if (flag)
			{
				delayFrame = 9;
				this.CurrentCombatDomain.SetDisplayPosition(context, this.CombatChar.IsAlly, this.CurrentCombatDomain.GetDisplayPosition(this.CombatChar.IsAlly, displayDistance));
			}
			base.DelayCall(new CombatCharacterStateBase.CombatCharacterStateDelayCallRequest(this.PlayUseFuyuAnim), delayFrame);
		}

		// Token: 0x0600623C RID: 25148 RVA: 0x0037EB44 File Offset: 0x0037CD44
		private void PlayUseFuyuAnim()
		{
			DataContext context = this.CurrentCombatDomain.Context;
			CombatItemUseItem useConfig = CombatItemUse.DefValue.UseFuyuSword;
			this.CombatChar.SetParticleToPlay(useConfig.Particle, context);
			this.CombatChar.SetSkillSoundToPlay(useConfig.Sound, context);
			this.CombatChar.SetAnimationToPlayOnce(useConfig.Animation, context);
			base.DelayCall(new CombatCharacterStateBase.CombatCharacterStateDelayCallRequest(this.PlayFuyuHitAnim), AnimDataCollection.GetEventFrame(useConfig.Animation, "act0", 0));
			base.DelayCall(new CombatCharacterStateBase.CombatCharacterStateDelayCallRequest(this.PlayFuyuCastAnim), AnimDataCollection.GetDurationFrame(useConfig.Animation));
		}

		// Token: 0x0600623D RID: 25149 RVA: 0x0037EBE0 File Offset: 0x0037CDE0
		private void PlayFuyuHitAnim()
		{
			DataContext context = this.CurrentCombatDomain.Context;
			CombatItemUseItem useConfig = CombatItemUse.DefValue.UseFuyuSword;
			CombatCharacter enemyChar = this.CurrentCombatDomain.GetCombatCharacter(!this.CombatChar.IsAlly, false);
			bool flag = !string.IsNullOrEmpty(useConfig.BeHitAnimation);
			if (flag)
			{
				enemyChar.SetAnimationToPlayOnce(useConfig.BeHitAnimation, context);
			}
		}

		// Token: 0x0600623E RID: 25150 RVA: 0x0037EC3C File Offset: 0x0037CE3C
		private void PlayFuyuCastAnim()
		{
			DataContext context = this.CurrentCombatDomain.Context;
			CombatCharacter enemyChar = this.CurrentCombatDomain.GetCombatCharacter(!this.CombatChar.IsAlly, false);
			DomainManager.Combat.AppendGetChar(enemyChar.GetId());
			DomainManager.Combat.AppendEvaluation((this.CombatChar.GetCharacter().GetConsummateLevel() > enemyChar.GetCharacter().GetConsummateLevel()) ? 23 : 24);
			DomainManager.TaiwuEvent.SetListenerEventActionBoolArg("CombatOver", "UsedFuyuSwordInCombat", true);
			this.CurrentCombatDomain.EndCombat(context, enemyChar, false, false);
		}

		// Token: 0x04001AB8 RID: 6840
		private EShowMercyOption _optionType;

		// Token: 0x04001AB9 RID: 6841
		private EShowMercySelect _selected;
	}
}
