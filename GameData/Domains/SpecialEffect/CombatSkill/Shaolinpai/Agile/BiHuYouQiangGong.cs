using System;
using System.Collections.Generic;
using Config;
using GameData.Combat.Math;
using GameData.Common;
using GameData.Domains.CombatSkill;
using GameData.Domains.SpecialEffect.CombatSkill.Common.Agile;
using GameData.GameDataBridge;

namespace GameData.Domains.SpecialEffect.CombatSkill.Shaolinpai.Agile
{
	// Token: 0x0200043A RID: 1082
	public class BiHuYouQiangGong : AgileSkillBase
	{
		// Token: 0x060039EA RID: 14826 RVA: 0x00240FEB File Offset: 0x0023F1EB
		public BiHuYouQiangGong()
		{
		}

		// Token: 0x060039EB RID: 14827 RVA: 0x00240FFC File Offset: 0x0023F1FC
		public BiHuYouQiangGong(CombatSkillKey skillKey) : base(skillKey, 1401)
		{
			this.AutoRemove = false;
			this.ListenCanAffectChange = true;
		}

		// Token: 0x060039EC RID: 14828 RVA: 0x00241024 File Offset: 0x0023F224
		public override void OnEnable(DataContext context)
		{
			base.OnEnable(context);
			this._affecting = false;
			this.OnMoveSkillCanAffectChanged(context, default(DataUid));
			this.AffectDatas = new Dictionary<AffectedDataKey, EDataModifyType>();
			bool isDirect = base.IsDirect;
			if (isDirect)
			{
				this.AffectDatas.Add(new AffectedDataKey(base.CharacterId, 199, -1, -1, -1, -1), EDataModifyType.AddPercent);
				this._directAffectingDefendSkill = base.CombatChar.GetAffectingDefendSkillId();
				this._directDefendSkillUid = new DataUid(8, 10, (ulong)((long)base.CharacterId), 63U);
				GameDataBridge.AddPostDataModificationHandler(this._directDefendSkillUid, base.DataHandlerKey, new Action<DataContext, DataUid>(this.OnDefendSkillChanged));
			}
			else
			{
				this._reverseDefendSkillUidDict = new Dictionary<int, DataUid>();
				this._reverseAffectingDefendSkillDict = new Dictionary<int, short>();
				foreach (int enemyId in DomainManager.Combat.GetCharacterList(!base.CombatChar.IsAlly))
				{
					bool flag = enemyId >= 0;
					if (flag)
					{
						this.AffectDatas.Add(new AffectedDataKey(enemyId, 199, -1, -1, -1, -1), EDataModifyType.AddPercent);
						short defendSkillId = DomainManager.Combat.GetElement_CombatCharacterDict(enemyId).GetAffectingDefendSkillId();
						bool flag2 = defendSkillId >= 0;
						if (flag2)
						{
							this._reverseAffectingDefendSkillDict.Add(enemyId, defendSkillId);
						}
						DataUid defendSkillUid = new DataUid(8, 10, (ulong)((long)enemyId), 63U);
						this._reverseDefendSkillUidDict.Add(enemyId, defendSkillUid);
						GameDataBridge.AddPostDataModificationHandler(defendSkillUid, base.DataHandlerKey, new Action<DataContext, DataUid>(this.OnDefendSkillChanged));
					}
				}
			}
			base.ShowSpecialEffectTips(0);
		}

		// Token: 0x060039ED RID: 14829 RVA: 0x002411CC File Offset: 0x0023F3CC
		public override void OnDisable(DataContext context)
		{
			base.OnDisable(context);
			bool isDirect = base.IsDirect;
			if (isDirect)
			{
				GameDataBridge.RemovePostDataModificationHandler(this._directDefendSkillUid, base.DataHandlerKey);
			}
			else
			{
				foreach (DataUid uid in this._reverseDefendSkillUidDict.Values)
				{
					GameDataBridge.RemovePostDataModificationHandler(uid, base.DataHandlerKey);
				}
			}
		}

		// Token: 0x060039EE RID: 14830 RVA: 0x00241258 File Offset: 0x0023F458
		private void OnDefendSkillChanged(DataContext context, DataUid dataUid)
		{
			bool isDirect = base.IsDirect;
			if (isDirect)
			{
				this._directAffectingDefendSkill = base.CombatChar.GetAffectingDefendSkillId();
			}
			else
			{
				int enemyId = (int)dataUid.SubId0;
				short defendSkillId = DomainManager.Combat.GetElement_CombatCharacterDict(enemyId).GetAffectingDefendSkillId();
				bool flag = defendSkillId >= 0;
				if (flag)
				{
					this._reverseAffectingDefendSkillDict.Add(enemyId, defendSkillId);
				}
				else
				{
					this._reverseAffectingDefendSkillDict.Remove(enemyId);
				}
			}
			bool flag2 = this.AgileSkillChanged && (base.IsDirect ? (this._directAffectingDefendSkill < 0) : (this._reverseAffectingDefendSkillDict.Count <= 0));
			if (flag2)
			{
				base.RemoveSelf(context);
			}
		}

		// Token: 0x060039EF RID: 14831 RVA: 0x00241308 File Offset: 0x0023F508
		protected override void OnMoveSkillChanged(DataContext context, DataUid dataUid)
		{
			bool firstMoveSkillChanged = this._firstMoveSkillChanged;
			if (firstMoveSkillChanged)
			{
				this._firstMoveSkillChanged = false;
			}
			else
			{
				bool flag = base.IsDirect ? (this._directAffectingDefendSkill < 0) : (this._reverseAffectingDefendSkillDict.Count <= 0);
				if (flag)
				{
					base.RemoveSelf(context);
				}
				else
				{
					this.AgileSkillChanged = true;
				}
			}
		}

		// Token: 0x060039F0 RID: 14832 RVA: 0x00241364 File Offset: 0x0023F564
		protected override void OnMoveSkillCanAffectChanged(DataContext context, DataUid dataUid)
		{
			bool canAffect = base.CanAffect;
			bool flag = this._affecting == canAffect;
			if (!flag)
			{
				this._affecting = canAffect;
				bool isDirect = base.IsDirect;
				if (isDirect)
				{
					DomainManager.SpecialEffect.InvalidateCache(context, base.CharacterId, 199);
				}
				else
				{
					int[] charList = DomainManager.Combat.GetCharacterList(!base.CombatChar.IsAlly);
					for (int i = 0; i < charList.Length; i++)
					{
						bool flag2 = charList[i] >= 0;
						if (flag2)
						{
							DomainManager.SpecialEffect.InvalidateCache(context, charList[i], 199);
						}
					}
				}
			}
		}

		// Token: 0x060039F1 RID: 14833 RVA: 0x00241414 File Offset: 0x0023F614
		public override int GetModifyValue(AffectedDataKey dataKey, int currModifyValue)
		{
			bool flag = !this._affecting || Config.CombatSkill.Instance[dataKey.CombatSkillId].EquipType != 3;
			int result;
			if (flag)
			{
				result = 0;
			}
			else
			{
				bool flag2 = dataKey.FieldId == 199;
				if (flag2)
				{
					result = (base.IsDirect ? 20 : -20);
				}
				else
				{
					result = 0;
				}
			}
			return result;
		}

		// Token: 0x040010ED RID: 4333
		private const sbyte ChangePower = 20;

		// Token: 0x040010EE RID: 4334
		private bool _affecting;

		// Token: 0x040010EF RID: 4335
		private bool _firstMoveSkillChanged = true;

		// Token: 0x040010F0 RID: 4336
		private DataUid _directDefendSkillUid;

		// Token: 0x040010F1 RID: 4337
		private short _directAffectingDefendSkill;

		// Token: 0x040010F2 RID: 4338
		private Dictionary<int, DataUid> _reverseDefendSkillUidDict;

		// Token: 0x040010F3 RID: 4339
		private Dictionary<int, short> _reverseAffectingDefendSkillDict;
	}
}
