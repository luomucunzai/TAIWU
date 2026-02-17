using System;
using GameData.Common;
using GameData.Domains.CombatSkill;
using GameData.Domains.SpecialEffect.CombatSkill.Common.Assist;
using GameData.GameDataBridge;
using GameData.Utilities;

namespace GameData.Domains.SpecialEffect.CombatSkill.Jieqingmen.DefenseAndAssist
{
	// Token: 0x02000501 RID: 1281
	public class TianGuanTong : AssistSkillBase
	{
		// Token: 0x06003E85 RID: 16005 RVA: 0x00256299 File Offset: 0x00254499
		public TianGuanTong()
		{
		}

		// Token: 0x06003E86 RID: 16006 RVA: 0x002562A3 File Offset: 0x002544A3
		public TianGuanTong(CombatSkillKey skillKey) : base(skillKey, 13600)
		{
		}

		// Token: 0x06003E87 RID: 16007 RVA: 0x002562B4 File Offset: 0x002544B4
		public override void OnEnable(DataContext context)
		{
			base.OnEnable(context);
			this._flawOrAcupointUid = new DataUid(8, 10, (ulong)((long)base.CharacterId), base.IsDirect ? 43U : 41U);
			GameDataBridge.AddPostDataModificationHandler(this._flawOrAcupointUid, base.DataHandlerKey, new Action<DataContext, DataUid>(this.TryAffect));
		}

		// Token: 0x06003E88 RID: 16008 RVA: 0x0025630B File Offset: 0x0025450B
		public override void OnDisable(DataContext context)
		{
			base.OnDisable(context);
			GameDataBridge.RemovePostDataModificationHandler(this._flawOrAcupointUid, base.DataHandlerKey);
		}

		// Token: 0x06003E89 RID: 16009 RVA: 0x00256328 File Offset: 0x00254528
		protected override void OnCanUseChanged(DataContext context, DataUid dataUid)
		{
			bool canAffect = base.CanAffect;
			if (canAffect)
			{
				this.TryAffect(context, default(DataUid));
			}
		}

		// Token: 0x06003E8A RID: 16010 RVA: 0x00256354 File Offset: 0x00254554
		private void TryAffect(DataContext context, DataUid dataUid)
		{
			bool flag = !base.CanAffect || (base.IsDirect ? base.CombatChar.GetAcupointCount() : base.CombatChar.GetFlawCount()).Sum() < 7;
			if (!flag)
			{
				byte[] countList = base.IsDirect ? base.CombatChar.GetAcupointCount() : base.CombatChar.GetFlawCount();
				for (sbyte part = 0; part < 7; part += 1)
				{
					byte count = countList[(int)part];
					for (int i = 0; i < (int)count; i++)
					{
						bool isDirect = base.IsDirect;
						if (isDirect)
						{
							DomainManager.Combat.RemoveAcupoint(context, base.CombatChar, part, 0, false, false);
						}
						else
						{
							DomainManager.Combat.RemoveFlaw(context, base.CombatChar, part, 0, false, false);
						}
					}
				}
				DomainManager.Combat.UpdateBodyDefeatMark(context, base.CombatChar);
				base.ShowEffectTips(context);
				base.ShowSpecialEffectTips(0);
			}
		}

		// Token: 0x04001272 RID: 4722
		private const sbyte RequireCount = 7;

		// Token: 0x04001273 RID: 4723
		private DataUid _flawOrAcupointUid;
	}
}
