using System;
using System.Collections.Generic;
using GameData.Combat.Math;
using GameData.Common;
using GameData.DomainEvents;
using GameData.Domains.Combat;
using GameData.Domains.CombatSkill;
using GameData.Domains.SpecialEffect.CombatSkill.Common.Agile;

namespace GameData.Domains.SpecialEffect.CombatSkill.Jingangzong.Agile
{
	// Token: 0x020004D8 RID: 1240
	public class ShenZuTong : AgileSkillBase
	{
		// Token: 0x06003D90 RID: 15760 RVA: 0x002525DA File Offset: 0x002507DA
		public ShenZuTong()
		{
		}

		// Token: 0x06003D91 RID: 15761 RVA: 0x002525E4 File Offset: 0x002507E4
		public ShenZuTong(CombatSkillKey skillKey) : base(skillKey, 11503)
		{
			this.ListenCanAffectChange = true;
		}

		// Token: 0x06003D92 RID: 15762 RVA: 0x002525FC File Offset: 0x002507FC
		public override void OnEnable(DataContext context)
		{
			base.OnEnable(context);
			this._moveState = 0;
			this.AffectDatas = new Dictionary<AffectedDataKey, EDataModifyType>();
			this.AffectDatas.Add(new AffectedDataKey(base.CharacterId, 55, -1, -1, -1, -1), EDataModifyType.Custom);
			this.AffectDatas.Add(new AffectedDataKey(base.CharacterId, 157, -1, -1, -1, -1), EDataModifyType.Custom);
			Events.RegisterHandler_MoveStateChanged(new Events.OnMoveStateChanged(this.OnMoveStateChanged));
			base.ShowSpecialEffectTips(0);
		}

		// Token: 0x06003D93 RID: 15763 RVA: 0x0025267D File Offset: 0x0025087D
		public override void OnDisable(DataContext context)
		{
			base.OnDisable(context);
			Events.UnRegisterHandler_MoveStateChanged(new Events.OnMoveStateChanged(this.OnMoveStateChanged));
		}

		// Token: 0x06003D94 RID: 15764 RVA: 0x0025269C File Offset: 0x0025089C
		private void OnMoveStateChanged(DataContext context, CombatCharacter character, byte moveState)
		{
			bool flag = character != base.CombatChar;
			if (!flag)
			{
				this._moveState = moveState;
				DomainManager.SpecialEffect.InvalidateCache(context, base.CharacterId, 55);
			}
		}

		// Token: 0x06003D95 RID: 15765 RVA: 0x002526D7 File Offset: 0x002508D7
		protected override void OnMoveSkillCanAffectChanged(DataContext context, DataUid dataUid)
		{
			DomainManager.SpecialEffect.InvalidateCache(context, base.CharacterId, 55);
		}

		// Token: 0x06003D96 RID: 15766 RVA: 0x002526F0 File Offset: 0x002508F0
		public override bool GetModifiedValue(AffectedDataKey dataKey, bool dataValue)
		{
			bool flag = dataKey.CharId != base.CharacterId || !base.CanAffect || this._moveState != (base.IsDirect ? 1 : 2);
			bool result;
			if (flag)
			{
				result = dataValue;
			}
			else
			{
				bool flag2 = dataKey.FieldId == 55 && dataKey.CustomParam0 == 1;
				if (flag2)
				{
					result = false;
				}
				else
				{
					bool flag3 = dataKey.FieldId == 157;
					result = (!flag3 && dataValue);
				}
			}
			return result;
		}

		// Token: 0x04001224 RID: 4644
		private byte _moveState;
	}
}
