using System;
using System.Collections.Generic;
using Config;
using GameData.Common;

namespace GameData.Domains.Combat
{
	// Token: 0x02000698 RID: 1688
	public class CombatCharacterStateChangeBossPhase : CombatCharacterStateBase
	{
		// Token: 0x060061F3 RID: 25075 RVA: 0x0037B42D File Offset: 0x0037962D
		public CombatCharacterStateChangeBossPhase(CombatDomain combatDomain, CombatCharacter combatChar) : base(combatDomain, combatChar, CombatCharacterStateType.ChangeBossPhase)
		{
			this.IsUpdateOnPause = true;
		}

		// Token: 0x060061F4 RID: 25076 RVA: 0x0037B444 File Offset: 0x00379644
		public override void OnEnter()
		{
			DataContext context = this.CombatChar.GetDataContext();
			int effectIndex = (int)this.CombatChar.GetBossPhase();
			BossItem bossConfig = this.CombatChar.BossConfig;
			string fallAni = bossConfig.FailAnimation;
			string fullAniName = bossConfig.AniPrefix[effectIndex] + fallAni;
			bool isRanChenZi = bossConfig.TemplateId == 10;
			short aniFrame = (short)AnimDataCollection.GetDurationFrame(fullAniName);
			this._setDataFrame = 1;
			this._setIdleAniFrame = ((isRanChenZi && effectIndex == 4) ? (this._effectFrame - 240) : aniFrame);
			this._effectFrame = ((bossConfig.HasSceneChangeEffect && this.CurrentCombatDomain.CombatConfig.Scene >= 0 && !isRanChenZi) ? ((short)Math.Round(600.0)) : aniFrame);
			this.CombatChar.NeedChangeBossPhase = false;
			bool flag = !this.CurrentCombatDomain.CombatConfig.StartInSecondPhase;
			if (flag)
			{
				this.CombatChar.SetAnimationToPlayOnce(fallAni, context);
				this.CombatChar.SetAnimationToLoop(null, context);
				this.CombatChar.SetParticleToPlay(bossConfig.FailParticles[effectIndex], context);
				this.CombatChar.SetDieSoundToPlay(bossConfig.FailSounds[effectIndex], context);
				bool flag2 = bossConfig.HasSceneChangeEffect && this.CurrentCombatDomain.CombatConfig.Scene >= 0 && !isRanChenZi && bossConfig.TemplateId != 9;
				if (flag2)
				{
					this.CombatChar.SetSkillSoundToPlay("ui_battle_rupture", context);
				}
			}
			else
			{
				this._setIdleAniFrame = 1;
				this._effectFrame = 2;
			}
			List<string> failPlayerAni = bossConfig.FailPlayerAni;
			bool flag3 = failPlayerAni != null && failPlayerAni.Count > effectIndex;
			if (flag3)
			{
				this.CurrentCombatDomain.GetCombatCharacter(!this.CombatChar.IsAlly, false).SetAnimationToPlayOnce(bossConfig.FailPlayerAni[effectIndex], context);
				this.CombatChar.SetDisplayPosition(this.CurrentCombatDomain.GetDisplayPosition(this.CombatChar.IsAlly, (short)bossConfig.FailAniDistance[effectIndex]), context);
			}
		}

		// Token: 0x060061F5 RID: 25077 RVA: 0x0037B64F File Offset: 0x0037984F
		public override void OnExit()
		{
		}

		// Token: 0x060061F6 RID: 25078 RVA: 0x0037B654 File Offset: 0x00379854
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
				bool flag2 = this._setDataFrame > 0;
				if (flag2)
				{
					this._setDataFrame -= 1;
					bool flag3 = this._setDataFrame == 0;
					if (flag3)
					{
						this.CombatChar.SetBossPhase(this.CombatChar.GetBossPhase() + 1, this.CombatChar.GetDataContext());
					}
				}
				bool flag4 = this._setIdleAniFrame > 0;
				if (flag4)
				{
					this._setIdleAniFrame -= 1;
					bool flag5 = this._setIdleAniFrame == 0;
					if (flag5)
					{
						this.CombatChar.SetAnimationToLoop(this.CurrentCombatDomain.GetIdleAni(this.CombatChar), this.CombatChar.GetDataContext());
					}
				}
				bool flag6 = this._effectFrame > 0;
				if (flag6)
				{
					this._effectFrame -= 1;
					bool flag7 = this._effectFrame == 0;
					if (flag7)
					{
						DataContext context = this.CombatChar.GetDataContext();
						bool flag8 = this.CombatChar.ChangeBossPhaseEffectId >= 0;
						if (flag8)
						{
							DomainManager.Combat.ShowSpecialEffectTips(this.CombatChar.GetId(), this.CombatChar.ChangeBossPhaseEffectId, 0);
							this.CombatChar.SetXiangshuEffectId((short)this.CombatChar.ChangeBossPhaseEffectId, context);
							this.CombatChar.ChangeBossPhaseEffectId = -1;
						}
						this.CombatChar.SetDisplayPosition(int.MinValue, context);
						this.CombatChar.StateMachine.TranslateState();
					}
				}
				result = false;
			}
			return result;
		}

		// Token: 0x04001AA5 RID: 6821
		private const float ChangePhaseEffectTime = 10f;

		// Token: 0x04001AA6 RID: 6822
		private const string SceneRuptureSound = "ui_battle_rupture";

		// Token: 0x04001AA7 RID: 6823
		private short _setDataFrame;

		// Token: 0x04001AA8 RID: 6824
		private short _setIdleAniFrame;

		// Token: 0x04001AA9 RID: 6825
		private short _effectFrame;
	}
}
