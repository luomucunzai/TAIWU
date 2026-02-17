using System;
using System.Collections.Generic;
using Config;
using GameData.Combat.Math;
using GameData.Common;
using GameData.DomainEvents;
using GameData.Domains.CombatSkill;
using GameData.GameDataBridge;
using GameData.Utilities;

namespace GameData.Domains.SpecialEffect.CombatSkill.Common.Assist
{
	// Token: 0x020005AF RID: 1455
	public class TrickAddHitOrAvoid : AssistSkillBase
	{
		// Token: 0x06004344 RID: 17220 RVA: 0x0026ABA2 File Offset: 0x00268DA2
		protected TrickAddHitOrAvoid()
		{
		}

		// Token: 0x06004345 RID: 17221 RVA: 0x0026ABAC File Offset: 0x00268DAC
		protected TrickAddHitOrAvoid(CombatSkillKey skillKey, int type) : base(skillKey, type)
		{
		}

		// Token: 0x06004346 RID: 17222 RVA: 0x0026ABB8 File Offset: 0x00268DB8
		public override void OnEnable(DataContext context)
		{
			base.OnEnable(context);
			this._trickCounts = new int[3];
			this.AffectDatas = new Dictionary<AffectedDataKey, EDataModifyType>();
			for (sbyte hitType = 0; hitType < 3; hitType += 1)
			{
				this.AffectDatas.Add(new AffectedDataKey(base.CharacterId, (ushort)((base.IsDirect ? 32 : 38) + hitType), -1, -1, -1, -1), EDataModifyType.AddPercent);
			}
			this._tricksUid = new DataUid(8, 10, (ulong)((long)base.CharacterId), 28U);
			GameDataBridge.AddPostDataModificationHandler(this._tricksUid, base.DataHandlerKey, new Action<DataContext, DataUid>(this.UpdateEffect));
			Events.RegisterHandler_CastSkillEnd(new Events.OnCastSkillEnd(this.OnCastSkillEnd));
		}

		// Token: 0x06004347 RID: 17223 RVA: 0x0026AC6A File Offset: 0x00268E6A
		public override void OnDisable(DataContext context)
		{
			base.OnDisable(context);
			GameDataBridge.RemovePostDataModificationHandler(this._tricksUid, base.DataHandlerKey);
			Events.UnRegisterHandler_CastSkillEnd(new Events.OnCastSkillEnd(this.OnCastSkillEnd));
		}

		// Token: 0x06004348 RID: 17224 RVA: 0x0026AC9C File Offset: 0x00268E9C
		protected override void OnCanUseChanged(DataContext context, DataUid dataUid)
		{
			for (sbyte hitType = 0; hitType < 3; hitType += 1)
			{
				DomainManager.SpecialEffect.InvalidateCache(context, base.CharacterId, (ushort)((base.IsDirect ? 32 : 38) + hitType));
			}
		}

		// Token: 0x06004349 RID: 17225 RVA: 0x0026ACE0 File Offset: 0x00268EE0
		private void UpdateEffect(DataContext context, DataUid dataUid)
		{
			bool flag = base.CombatChar.NeedUseSkillId < 0 && base.CombatChar.GetPreparingSkillId() < 0;
			if (flag)
			{
				this.UpdateEffect(context);
			}
		}

		// Token: 0x0600434A RID: 17226 RVA: 0x0026AD1C File Offset: 0x00268F1C
		private void OnCastSkillEnd(DataContext context, int charId, bool isAlly, short skillId, sbyte power, bool interrupted)
		{
			bool flag = charId != base.CharacterId || Config.CombatSkill.Instance[skillId].EquipType != 1;
			if (!flag)
			{
				this.UpdateEffect(context);
			}
		}

		// Token: 0x0600434B RID: 17227 RVA: 0x0026AD5C File Offset: 0x00268F5C
		private void UpdateEffect(DataContext context)
		{
			for (sbyte hitType = 0; hitType < 3; hitType += 1)
			{
				int trickCount = (int)base.CombatChar.GetTrickCount(this.RequireTrickTypes[(int)hitType]);
				bool flag = this._trickCounts[(int)hitType] != trickCount;
				if (flag)
				{
					this._trickCounts[(int)hitType] = trickCount;
					DomainManager.SpecialEffect.InvalidateCache(context, base.CharacterId, (ushort)((base.IsDirect ? 32 : 38) + hitType));
				}
				base.SetConstAffecting(context, this._trickCounts.Sum() > 0);
			}
		}

		// Token: 0x0600434C RID: 17228 RVA: 0x0026ADE8 File Offset: 0x00268FE8
		public override int GetModifyValue(AffectedDataKey dataKey, int currModifyValue)
		{
			bool flag = dataKey.CharId != base.CharacterId || !base.CanAffect;
			int result;
			if (flag)
			{
				result = 0;
			}
			else
			{
				int hitType = (int)(dataKey.FieldId - (base.IsDirect ? 32 : 38));
				result = 5 * this._trickCounts[hitType];
			}
			return result;
		}

		// Token: 0x040013F5 RID: 5109
		private const sbyte AddPropertyUnit = 5;

		// Token: 0x040013F6 RID: 5110
		protected sbyte[] RequireTrickTypes;

		// Token: 0x040013F7 RID: 5111
		private DataUid _tricksUid;

		// Token: 0x040013F8 RID: 5112
		private int[] _trickCounts;
	}
}
