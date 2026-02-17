using System;
using System.Collections.Generic;
using GameData.Combat.Math;
using GameData.Common;
using GameData.Domains.CombatSkill;
using GameData.Domains.SpecialEffect.CombatSkill.Common.Agile;
using GameData.GameDataBridge;

namespace GameData.Domains.SpecialEffect.CombatSkill.Shaolinpai.Agile
{
	// Token: 0x0200043B RID: 1083
	public class ChuanZongGong : AgileSkillBase
	{
		// Token: 0x060039F2 RID: 14834 RVA: 0x00241477 File Offset: 0x0023F677
		public ChuanZongGong()
		{
		}

		// Token: 0x060039F3 RID: 14835 RVA: 0x00241481 File Offset: 0x0023F681
		public ChuanZongGong(CombatSkillKey skillKey) : base(skillKey, 1400)
		{
			this.ListenCanAffectChange = true;
		}

		// Token: 0x060039F4 RID: 14836 RVA: 0x00241498 File Offset: 0x0023F698
		public override void OnEnable(DataContext context)
		{
			base.OnEnable(context);
			this.AffectDatas = new Dictionary<AffectedDataKey, EDataModifyType>();
			bool isDirect = base.IsDirect;
			if (isDirect)
			{
				this.AffectDatas.Add(new AffectedDataKey(base.CharacterId, 9, -1, -1, -1, -1), EDataModifyType.TotalPercent);
			}
			else
			{
				int[] charList = DomainManager.Combat.GetCharacterList(!base.CombatChar.IsAlly);
				for (int i = 0; i < charList.Length; i++)
				{
					bool flag = charList[i] >= 0;
					if (flag)
					{
						this.AffectDatas.Add(new AffectedDataKey(charList[i], 9, -1, -1, -1, -1), EDataModifyType.TotalPercent);
					}
				}
			}
			this._defendSkillUid = new DataUid(8, 10, (ulong)((long)base.CharacterId), 63U);
			GameDataBridge.AddPostDataModificationHandler(this._defendSkillUid, base.DataHandlerKey, new Action<DataContext, DataUid>(this.OnDefendSkillChanged));
			this._affecting = false;
			this.UpdateCanAffect(context, default(DataUid));
			bool affecting = this._affecting;
			if (affecting)
			{
				base.ShowSpecialEffectTips(0);
			}
		}

		// Token: 0x060039F5 RID: 14837 RVA: 0x002415A0 File Offset: 0x0023F7A0
		public override void OnDisable(DataContext context)
		{
			base.OnDisable(context);
			GameDataBridge.RemovePostDataModificationHandler(this._defendSkillUid, base.DataHandlerKey);
		}

		// Token: 0x060039F6 RID: 14838 RVA: 0x002415C0 File Offset: 0x0023F7C0
		protected override void OnMoveSkillCanAffectChanged(DataContext context, DataUid dataUid)
		{
			this.UpdateCanAffect(context, default(DataUid));
		}

		// Token: 0x060039F7 RID: 14839 RVA: 0x002415E0 File Offset: 0x0023F7E0
		private void OnDefendSkillChanged(DataContext context, DataUid dataUid)
		{
			this.UpdateCanAffect(context, default(DataUid));
			bool affecting = this._affecting;
			if (affecting)
			{
				base.ShowSpecialEffectTips(0);
			}
		}

		// Token: 0x060039F8 RID: 14840 RVA: 0x00241614 File Offset: 0x0023F814
		private void UpdateCanAffect(DataContext context, DataUid dataUid)
		{
			bool canAffect = base.CanAffect && base.CombatChar.GetAffectingDefendSkillId() >= 0;
			bool flag = this._affecting == canAffect;
			if (!flag)
			{
				this._affecting = canAffect;
				bool isDirect = base.IsDirect;
				if (isDirect)
				{
					DomainManager.SpecialEffect.InvalidateCache(context, base.CharacterId, 9);
				}
				else
				{
					int[] charList = DomainManager.Combat.GetCharacterList(!base.CombatChar.IsAlly);
					for (int i = 0; i < charList.Length; i++)
					{
						bool flag2 = charList[i] >= 0;
						if (flag2)
						{
							DomainManager.SpecialEffect.InvalidateCache(context, charList[i], 9);
						}
					}
				}
			}
		}

		// Token: 0x060039F9 RID: 14841 RVA: 0x002416D4 File Offset: 0x0023F8D4
		public override int GetModifyValue(AffectedDataKey dataKey, int currModifyValue)
		{
			bool flag = !this._affecting;
			int result;
			if (flag)
			{
				result = 0;
			}
			else
			{
				bool flag2 = dataKey.FieldId == 9;
				if (flag2)
				{
					result = (base.IsDirect ? 40 : -40);
				}
				else
				{
					result = 0;
				}
			}
			return result;
		}

		// Token: 0x040010F4 RID: 4340
		private const sbyte ChangeMoveSpeed = 40;

		// Token: 0x040010F5 RID: 4341
		private DataUid _defendSkillUid;

		// Token: 0x040010F6 RID: 4342
		private bool _affecting;
	}
}
