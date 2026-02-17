using System;
using System.Collections.Generic;
using GameData.Common;
using GameData.DomainEvents;
using GameData.Domains.Character;
using GameData.Domains.Combat;
using GameData.Domains.CombatSkill;
using GameData.Domains.SpecialEffect.CombatSkill.Common.Assist;
using GameData.GameDataBridge;

namespace GameData.Domains.SpecialEffect.CombatSkill.Xuehoujiao.DefenseAndAssist
{
	// Token: 0x02000244 RID: 580
	public class XinWoLiangMieZhou : AssistSkillBase
	{
		// Token: 0x06002FD3 RID: 12243 RVA: 0x00214830 File Offset: 0x00212A30
		public XinWoLiangMieZhou()
		{
		}

		// Token: 0x06002FD4 RID: 12244 RVA: 0x00214850 File Offset: 0x00212A50
		public XinWoLiangMieZhou(CombatSkillKey skillKey) : base(skillKey, 15805)
		{
		}

		// Token: 0x06002FD5 RID: 12245 RVA: 0x00214876 File Offset: 0x00212A76
		public override void OnEnable(DataContext context)
		{
			base.OnEnable(context);
			Events.RegisterHandler_CombatBegin(new Events.OnCombatBegin(this.OnCombatBegin));
		}

		// Token: 0x06002FD6 RID: 12246 RVA: 0x00214894 File Offset: 0x00212A94
		public override void OnDisable(DataContext context)
		{
			Events.UnRegisterHandler_CombatBegin(new Events.OnCombatBegin(this.OnCombatBegin));
			foreach (DataUid uid in this._teammateDefeatMarkUid)
			{
				GameDataBridge.RemovePostDataModificationHandler(uid, base.DataHandlerKey);
			}
			base.OnDisable(context);
		}

		// Token: 0x06002FD7 RID: 12247 RVA: 0x0021490C File Offset: 0x00212B0C
		private void OnCombatBegin(DataContext context)
		{
			bool flag = !DomainManager.Combat.IsMainCharacter(base.CombatChar);
			if (!flag)
			{
				int[] teamCharIds = DomainManager.Combat.GetCharacterList(base.CombatChar.IsAlly);
				foreach (int teamCharId in teamCharIds)
				{
					bool flag2 = teamCharId == base.CharacterId;
					if (!flag2)
					{
						bool flag3 = teamCharId < 0;
						if (!flag3)
						{
							CombatCharacter teamCombatChar;
							bool flag4 = !DomainManager.Combat.TryGetElement_CombatCharacterDict(teamCharId, out teamCombatChar);
							if (!flag4)
							{
								DataUid uid = new DataUid(8, 10, (ulong)((long)teamCharId), 50U);
								GameDataBridge.AddPostDataModificationHandler(uid, base.DataHandlerKey, new Action<DataContext, DataUid>(this.OnDefeatMarkChanged));
								this._teammateDefeatMarkUid.Add(uid);
								bool flag5 = this.TeammateReachAffectedMarkCount(teamCombatChar);
								if (flag5)
								{
									this._ignoreTeammateCharIds.Add(teamCharId);
								}
							}
						}
					}
				}
			}
		}

		// Token: 0x06002FD8 RID: 12248 RVA: 0x002149F8 File Offset: 0x00212BF8
		private unsafe void OnDefeatMarkChanged(DataContext context, DataUid dataUid)
		{
			int teammateCharId = (int)dataUid.SubId0;
			CombatCharacter combatChar;
			bool flag = !DomainManager.Combat.TryGetElement_CombatCharacterDict(teammateCharId, out combatChar);
			if (!flag)
			{
				bool flag2 = combatChar.IsAlly != base.CombatChar.IsAlly || combatChar == base.CombatChar || !base.CanAffect;
				if (!flag2)
				{
					bool flag3 = !this.TeammateReachAffectedMarkCount(combatChar);
					if (flag3)
					{
						this._ignoreTeammateCharIds.Remove(teammateCharId);
					}
					else
					{
						bool flag4 = this._ignoreTeammateCharIds.Contains(teammateCharId);
						if (!flag4)
						{
							this._ignoreTeammateCharIds.Add(teammateCharId);
							NeiliAllocation neiliAllocation = combatChar.GetNeiliAllocation();
							CombatCharacter affectChar = base.IsDirect ? base.CombatChar : DomainManager.Combat.GetCombatCharacter(!base.CombatChar.IsAlly, false);
							for (byte type = 0; type < 4; type += 1)
							{
								int changeValue = (int)(*(ref neiliAllocation.Items.FixedElementField + (IntPtr)type * 2) * 60 / 100);
								bool flag5 = changeValue > 0;
								if (flag5)
								{
									affectChar.ChangeNeiliAllocation(context, type, changeValue * (base.IsDirect ? 1 : -1), true, true);
								}
							}
							base.ShowEffectTips(context);
							base.ShowSpecialEffectTips(0);
						}
					}
				}
			}
		}

		// Token: 0x06002FD9 RID: 12249 RVA: 0x00214B40 File Offset: 0x00212D40
		private bool TeammateReachAffectedMarkCount(CombatCharacter combatChar)
		{
			return combatChar.GetDefeatMarkCollection().GetTotalCount() >= (int)(GlobalConfig.NeedDefeatMarkCount[(int)DomainManager.Combat.GetCombatType()] * 50 / 100);
		}

		// Token: 0x04000E2A RID: 3626
		private const sbyte ChangeNeiliAllocationPercent = 60;

		// Token: 0x04000E2B RID: 3627
		private const sbyte AffectedDefeatMarkThreshold = 50;

		// Token: 0x04000E2C RID: 3628
		private readonly List<DataUid> _teammateDefeatMarkUid = new List<DataUid>();

		// Token: 0x04000E2D RID: 3629
		private readonly List<int> _ignoreTeammateCharIds = new List<int>();
	}
}
