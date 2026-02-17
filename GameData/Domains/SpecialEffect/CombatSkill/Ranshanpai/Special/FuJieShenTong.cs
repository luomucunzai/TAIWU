using System;
using System.Collections.Generic;
using Config;
using GameData.Combat.Math;
using GameData.Common;
using GameData.DomainEvents;
using GameData.Domains.Combat;
using GameData.Domains.CombatSkill;
using GameData.GameDataBridge;
using GameData.Utilities;

namespace GameData.Domains.SpecialEffect.CombatSkill.Ranshanpai.Special
{
	// Token: 0x0200044B RID: 1099
	public class FuJieShenTong : CombatSkillEffectBase
	{
		// Token: 0x06003A4D RID: 14925 RVA: 0x00242DA3 File Offset: 0x00240FA3
		public FuJieShenTong()
		{
		}

		// Token: 0x06003A4E RID: 14926 RVA: 0x00242DAD File Offset: 0x00240FAD
		public FuJieShenTong(CombatSkillKey skillKey) : base(skillKey, 7308, -1)
		{
		}

		// Token: 0x06003A4F RID: 14927 RVA: 0x00242DC0 File Offset: 0x00240FC0
		public override void OnEnable(DataContext context)
		{
			base.CreateAffectedData(217, EDataModifyType.Custom, base.SkillTemplateId);
			base.CreateAffectedData(290, EDataModifyType.Custom, base.SkillTemplateId);
			this._neiliAllocationUid = base.ParseNeiliAllocationDataUid();
			GameDataBridge.AddPostDataModificationHandler(this._neiliAllocationUid, base.DataHandlerKey, new Action<DataContext, DataUid>(this.OnNeiliAllocationChanged));
			Events.RegisterHandler_CombatBegin(new Events.OnCombatBegin(this.OnCombatBegin));
			Events.RegisterHandler_CastSkillEnd(new Events.OnCastSkillEnd(this.OnCastSkillEnd));
		}

		// Token: 0x06003A50 RID: 14928 RVA: 0x00242E42 File Offset: 0x00241042
		public override void OnDisable(DataContext context)
		{
			GameDataBridge.RemovePostDataModificationHandler(this._neiliAllocationUid, base.DataHandlerKey);
			Events.UnRegisterHandler_CombatBegin(new Events.OnCombatBegin(this.OnCombatBegin));
			Events.UnRegisterHandler_CastSkillEnd(new Events.OnCastSkillEnd(this.OnCastSkillEnd));
		}

		// Token: 0x06003A51 RID: 14929 RVA: 0x00242E7B File Offset: 0x0024107B
		private void OnNeiliAllocationChanged(DataContext context, DataUid arg2)
		{
			this.UpdateAffecting(context);
		}

		// Token: 0x06003A52 RID: 14930 RVA: 0x00242E85 File Offset: 0x00241085
		private void OnCombatBegin(DataContext context)
		{
			this.UpdateAffecting(context);
			base.ShowSpecialEffectTips(0);
		}

		// Token: 0x06003A53 RID: 14931 RVA: 0x00242E98 File Offset: 0x00241098
		private unsafe void OnCastSkillEnd(DataContext context, int charId, bool isAlly, short skillId, sbyte power, bool interrupted)
		{
			bool flag = charId != base.CharacterId || skillId != base.SkillTemplateId || !base.PowerMatchAffectRequire((int)power, 0);
			if (!flag)
			{
				CombatCharacter enemyChar = DomainManager.Combat.GetCombatCharacter(!base.CombatChar.IsAlly, false);
				CombatCharacter targetChar = base.IsDirect ? enemyChar : base.CombatChar;
				Dictionary<byte, int> neiliAllocationTypeCounter = ObjectPool<Dictionary<byte, int>>.Instance.Get();
				foreach (short bannedSkillId in targetChar.GetBannedSkillIds(true))
				{
					bool isDirect = base.IsDirect;
					if (isDirect)
					{
						DomainManager.Combat.ResetSkillCd(context, targetChar, bannedSkillId);
					}
					else
					{
						DomainManager.Combat.ClearSkillCd(context, targetChar, bannedSkillId);
					}
					DomainManager.Combat.AddGoneMadInjury(context, enemyChar, bannedSkillId, 0);
					base.ShowSpecialEffectTipsOnceInFrame(1);
					base.ShowSpecialEffectTipsOnceInFrame(2);
					byte neiliAllocationType = Config.CombatSkill.Instance[bannedSkillId].GetRelatedNeiliAllocationType();
					neiliAllocationTypeCounter[neiliAllocationType] = neiliAllocationTypeCounter.GetOrDefault(neiliAllocationType) + 1;
				}
				foreach (KeyValuePair<byte, int> keyValuePair in neiliAllocationTypeCounter)
				{
					byte b;
					int num;
					keyValuePair.Deconstruct(out b, out num);
					byte neiliAllocationType2 = b;
					int count = num;
					CValuePercent percent = 20 + 5 * count;
					int reduceNeiliAllocation = (int)(*base.CombatChar.GetNeiliAllocation()[(int)neiliAllocationType2]) * percent;
					base.CombatChar.ChangeNeiliAllocation(context, neiliAllocationType2, -reduceNeiliAllocation, true, true);
				}
			}
		}

		// Token: 0x06003A54 RID: 14932 RVA: 0x00243054 File Offset: 0x00241254
		private void UpdateAffecting(DataContext context)
		{
			bool affecting = !base.CombatChar.AnyLowerThanOriginNeiliAllocation();
			bool flag = affecting == this._affecting;
			if (!flag)
			{
				this._affecting = affecting;
				DomainManager.Combat.UpdateSkillCanUse(context, base.CombatChar, base.SkillTemplateId);
			}
		}

		// Token: 0x06003A55 RID: 14933 RVA: 0x002430A0 File Offset: 0x002412A0
		public override bool GetModifiedValue(AffectedDataKey dataKey, bool dataValue)
		{
			bool flag = dataKey.SkillKey != this.SkillKey;
			bool result;
			if (flag)
			{
				result = dataValue;
			}
			else
			{
				ushort fieldId = dataKey.FieldId;
				if (!true)
				{
				}
				bool flag2;
				if (fieldId != 217)
				{
					if (fieldId != 290)
					{
						flag2 = dataValue;
					}
					else
					{
						flag2 = this._affecting;
					}
				}
				else
				{
					flag2 = false;
				}
				if (!true)
				{
				}
				result = flag2;
			}
			return result;
		}

		// Token: 0x0400110F RID: 4367
		private const int ReduceNeiliAllocationBasePercent = 20;

		// Token: 0x04001110 RID: 4368
		private const int ReduceNeiliAllocationUnitPercent = 5;

		// Token: 0x04001111 RID: 4369
		private DataUid _neiliAllocationUid;

		// Token: 0x04001112 RID: 4370
		private bool _affecting;
	}
}
