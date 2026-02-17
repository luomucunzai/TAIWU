using System;
using System.Runtime.CompilerServices;
using Config;
using GameData.Common;
using GameData.Utilities;

namespace GameData.Domains.Combat
{
	// Token: 0x02000700 RID: 1792
	public abstract class TeammateCommandInvokerBase : ITeammateCommandInvoker
	{
		// Token: 0x17000424 RID: 1060
		// (get) Token: 0x060067D8 RID: 26584 RVA: 0x003B25F9 File Offset: 0x003B07F9
		protected static CombatDomain CombatDomain
		{
			get
			{
				return DomainManager.Combat;
			}
		}

		// Token: 0x17000425 RID: 1061
		// (get) Token: 0x060067D9 RID: 26585 RVA: 0x003B2600 File Offset: 0x003B0800
		protected string DataHandlerKey
		{
			get
			{
				DefaultInterpolatedStringHandler defaultInterpolatedStringHandler = new DefaultInterpolatedStringHandler(0, 2);
				defaultInterpolatedStringHandler.AppendFormatted(base.GetType().Name);
				defaultInterpolatedStringHandler.AppendFormatted<int>(this.CombatChar.GetId());
				return defaultInterpolatedStringHandler.ToStringAndClear();
			}
		}

		// Token: 0x17000426 RID: 1062
		// (get) Token: 0x060067DA RID: 26586 RVA: 0x003B2643 File Offset: 0x003B0843
		protected CombatCharacter MainChar
		{
			get
			{
				return DomainManager.Combat.GetElement_CombatCharacterDict(this.MainCharId);
			}
		}

		// Token: 0x17000427 RID: 1063
		// (get) Token: 0x060067DB RID: 26587 RVA: 0x003B2655 File Offset: 0x003B0855
		protected TeammateCommandItem CmdConfig
		{
			get
			{
				return TeammateCommand.Instance[this._cmdType];
			}
		}

		// Token: 0x060067DC RID: 26588 RVA: 0x003B2668 File Offset: 0x003B0868
		protected TeammateCommandInvokerBase(int charId, int index)
		{
			this.CombatChar = DomainManager.Combat.GetElement_CombatCharacterDict(charId);
			this.MainCharId = DomainManager.Combat.GetMainCharacter(this.CombatChar.IsAlly).GetId();
			Tester.Assert(this.MainCharId != charId, "MainCharId != charId");
			this._index = index;
			this._cmdType = this.CombatChar.GetCurrTeammateCommands()[index];
			Tester.Assert(this.CmdConfig != null, "CmdConfig != null");
		}

		// Token: 0x060067DD RID: 26589
		public abstract void Setup();

		// Token: 0x060067DE RID: 26590
		public abstract void Close();

		// Token: 0x060067DF RID: 26591 RVA: 0x003B26F8 File Offset: 0x003B08F8
		protected void Execute(DataContext context)
		{
			bool invoked = DomainManager.Combat.ExecuteTeammateCommand(context, this.CombatChar.IsAlly, this._index, this.CombatChar.GetId());
			bool flag = invoked;
			if (flag)
			{
				DomainManager.Combat.ShowTeammateCommand(this.CombatChar.GetId(), this._index, false);
			}
		}

		// Token: 0x060067E0 RID: 26592 RVA: 0x003B2750 File Offset: 0x003B0950
		protected void IntoCombat()
		{
			bool flag = !TeammateCommandInvokerBase.CombatDomain.IsInCombat();
			if (!flag)
			{
				bool flag2 = this.MainChar.HasDoingOrReserveCommand();
				if (!flag2)
				{
					bool flag3 = this.CombatChar.GetExecutingTeammateCommand() != this._cmdType;
					if (!flag3)
					{
						bool intoCombatField = this.CombatChar.ExecutingTeammateCommandConfig.IntoCombatField;
						if (!intoCombatField)
						{
							bool isBefore = this.CombatChar.ExecutingTeammateCommandConfig.PosOffset > 0;
							bool flag4 = isBefore ? (this.MainChar.TeammateBeforeMainChar >= 0) : (this.MainChar.TeammateAfterMainChar >= 0);
							if (!flag4)
							{
								bool flag5 = isBefore;
								if (flag5)
								{
									this.MainChar.TeammateBeforeMainChar = this.CombatChar.GetId();
								}
								else
								{
									this.MainChar.TeammateAfterMainChar = this.CombatChar.GetId();
								}
								DataContext context = TeammateCommandInvokerBase.CombatDomain.Context;
								bool isAttack = this.CombatChar.ExecutingTeammateCommandImplement.IsAttack();
								int displayPos = (isAttack && TeammateCommandInvokerBase.CombatDomain.InAttackRange(this.CombatChar)) ? TeammateCommandInvokerBase.CombatDomain.GetDisplayPosition(this.CombatChar.IsAlly, (short)this.CombatChar.GetNormalAttackPosition(this.CombatChar.GetAttackCommandTrickType())) : int.MinValue;
								TeammateCommandInvokerBase.CombatDomain.SetDisplayPosition(context, this.CombatChar.IsAlly, displayPos);
								this.CombatChar.SetAnimationToLoop(TeammateCommandInvokerBase.CombatDomain.GetIdleAni(this.CombatChar), context);
								this.CombatChar.SetVisible(true, context);
								this.CombatChar.SetTeammateCommandPreparePercent(0, context);
								this.MainChar.ActingTeammateCommandChar = this.CombatChar;
								this.MainChar.StateMachine.TranslateState(CombatCharacterStateType.TeammateCommand);
							}
						}
					}
				}
			}
		}

		// Token: 0x04001C52 RID: 7250
		protected readonly CombatCharacter CombatChar;

		// Token: 0x04001C53 RID: 7251
		protected readonly int MainCharId;

		// Token: 0x04001C54 RID: 7252
		private readonly int _index;

		// Token: 0x04001C55 RID: 7253
		private readonly sbyte _cmdType;
	}
}
