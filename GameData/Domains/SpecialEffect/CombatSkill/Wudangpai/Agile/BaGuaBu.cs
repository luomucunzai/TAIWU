using System;
using System.Collections.Generic;
using GameData.Combat.Math;
using GameData.Common;
using GameData.Domains.CombatSkill;
using GameData.Domains.SpecialEffect.CombatSkill.Common.Agile;
using GameData.GameDataBridge;

namespace GameData.Domains.SpecialEffect.CombatSkill.Wudangpai.Agile
{
	// Token: 0x020003DF RID: 991
	public class BaGuaBu : AgileSkillBase
	{
		// Token: 0x060037EE RID: 14318 RVA: 0x00237EAB File Offset: 0x002360AB
		public BaGuaBu()
		{
		}

		// Token: 0x060037EF RID: 14319 RVA: 0x00237EB5 File Offset: 0x002360B5
		public BaGuaBu(CombatSkillKey skillKey) : base(skillKey, 4401)
		{
			this.ListenCanAffectChange = true;
		}

		// Token: 0x060037F0 RID: 14320 RVA: 0x00237ECC File Offset: 0x002360CC
		public override void OnEnable(DataContext context)
		{
			base.OnEnable(context);
			this.AffectDatas = new Dictionary<AffectedDataKey, EDataModifyType>();
			int[] charList = DomainManager.Combat.GetCharacterList(!base.CombatChar.IsAlly);
			for (int i = 0; i < charList.Length; i++)
			{
				bool flag = charList[i] >= 0;
				if (flag)
				{
					this.AffectDatas.Add(new AffectedDataKey(charList[i], base.IsDirect ? 7 : 8, -1, -1, -1, -1), EDataModifyType.TotalPercent);
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

		// Token: 0x060037F1 RID: 14321 RVA: 0x00237FAE File Offset: 0x002361AE
		public override void OnDisable(DataContext context)
		{
			base.OnDisable(context);
			GameDataBridge.RemovePostDataModificationHandler(this._defendSkillUid, base.DataHandlerKey);
		}

		// Token: 0x060037F2 RID: 14322 RVA: 0x00237FCC File Offset: 0x002361CC
		protected override void OnMoveSkillCanAffectChanged(DataContext context, DataUid dataUid)
		{
			this.UpdateCanAffect(context, default(DataUid));
		}

		// Token: 0x060037F3 RID: 14323 RVA: 0x00237FEC File Offset: 0x002361EC
		private void OnDefendSkillChanged(DataContext context, DataUid dataUid)
		{
			this.UpdateCanAffect(context, default(DataUid));
			bool affecting = this._affecting;
			if (affecting)
			{
				base.ShowSpecialEffectTips(0);
			}
		}

		// Token: 0x060037F4 RID: 14324 RVA: 0x00238020 File Offset: 0x00236220
		private void UpdateCanAffect(DataContext context, DataUid dataUid)
		{
			bool canAffect = base.CanAffect && base.CombatChar.GetAffectingDefendSkillId() >= 0;
			bool flag = this._affecting == canAffect;
			if (!flag)
			{
				this._affecting = canAffect;
				int[] charList = DomainManager.Combat.GetCharacterList(!base.CombatChar.IsAlly);
				for (int i = 0; i < charList.Length; i++)
				{
					bool flag2 = charList[i] >= 0;
					if (flag2)
					{
						DomainManager.SpecialEffect.InvalidateCache(context, charList[i], base.IsDirect ? 7 : 8);
					}
				}
			}
		}

		// Token: 0x060037F5 RID: 14325 RVA: 0x002380BC File Offset: 0x002362BC
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
				bool flag2 = dataKey.FieldId == 7 || dataKey.FieldId == 8;
				if (flag2)
				{
					result = -40;
				}
				else
				{
					result = 0;
				}
			}
			return result;
		}

		// Token: 0x04001054 RID: 4180
		private const sbyte ReduceRecoverPercent = -40;

		// Token: 0x04001055 RID: 4181
		private DataUid _defendSkillUid;

		// Token: 0x04001056 RID: 4182
		private bool _affecting;
	}
}
