using System;
using Config;
using GameData.Common;

namespace GameData.Domains.Combat
{
	// Token: 0x0200069C RID: 1692
	public class CombatCharacterStateJumpMove : CombatCharacterStateBase
	{
		// Token: 0x06006201 RID: 25089 RVA: 0x0037BC4E File Offset: 0x00379E4E
		public CombatCharacterStateJumpMove(CombatDomain combatDomain, CombatCharacter combatChar) : base(combatDomain, combatChar, CombatCharacterStateType.JumpMove)
		{
			this.IsUpdateOnPause = true;
		}

		// Token: 0x06006202 RID: 25090 RVA: 0x0037BC64 File Offset: 0x00379E64
		public override void OnEnter()
		{
			DataContext context = this.CombatChar.GetDataContext();
			CombatSkillItem skillConfig = CombatSkill.Instance[this.CombatChar.PauseJumpMoveSkillId];
			string moveAni = skillConfig.JumpAni[this.CombatChar.MoveForward ? 0 : 1];
			string moveParticle = skillConfig.JumpParticle[this.CombatChar.MoveForward ? 0 : 1];
			float jumpMoveDuration = (skillConfig.JumpChangeDistanceDuration > 0) ? ((float)skillConfig.JumpChangeDistanceDuration / 60f) : AnimDataCollection.Data[moveAni].Duration;
			this.CombatChar.NeedPauseJumpMove = false;
			this.CombatChar.SetAnimationToPlayOnce(moveAni, context);
			this.CombatChar.SetParticleToPlay(moveParticle, context);
			this.CombatChar.SetJumpChangeDistanceDuration(jumpMoveDuration, context);
			this.CurrentCombatDomain.UpdateAllTeammateCommandUsable(context, this.CombatChar.IsAlly, -1);
			this._aniFrame = (short)AnimDataCollection.GetDurationFrame(moveAni);
			this._changeDistanceFrame = Math.Max(skillConfig.JumpChangeDistanceFrame, 1);
		}

		// Token: 0x06006203 RID: 25091 RVA: 0x0037BD60 File Offset: 0x00379F60
		public override void OnExit()
		{
			this.CombatChar.SetJumpChangeDistanceDuration(-1f, this.CombatChar.GetDataContext());
			bool flag = this.CombatChar.MoveData.JumpMoveSkillId < 0;
			if (flag)
			{
				this.CombatChar.PauseJumpMoveSkillId = -1;
			}
		}

		// Token: 0x06006204 RID: 25092 RVA: 0x0037BDB0 File Offset: 0x00379FB0
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
				bool flag2 = this._changeDistanceFrame > 0;
				if (flag2)
				{
					this._changeDistanceFrame -= 1;
					bool flag3 = this._changeDistanceFrame == 0;
					if (flag3)
					{
						DomainManager.Combat.ChangeDistance(this.CombatChar.GetDataContext(), this.CombatChar, this.CombatChar.PauseJumpMoveDistance);
					}
				}
				bool flag4 = this._aniFrame > 0;
				if (flag4)
				{
					this._aniFrame -= 1;
					bool flag5 = this._aniFrame == 0;
					if (flag5)
					{
						this.CombatChar.StateMachine.TranslateState();
					}
				}
				result = false;
			}
			return result;
		}

		// Token: 0x04001AAC RID: 6828
		private short _aniFrame;

		// Token: 0x04001AAD RID: 6829
		private short _changeDistanceFrame;
	}
}
