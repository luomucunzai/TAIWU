using System;
using System.Collections.Generic;
using Config;
using GameData.Combat.Math;
using GameData.Common;
using GameData.DomainEvents;
using GameData.Domains.Combat;
using GameData.Domains.CombatSkill;
using GameData.Utilities;

namespace GameData.Domains.SpecialEffect.CombatSkill.Ranshanpai.Special
{
	// Token: 0x0200044E RID: 1102
	public class JiuZiZhenYanZhou : CombatSkillEffectBase
	{
		// Token: 0x06003A70 RID: 14960 RVA: 0x002437A8 File Offset: 0x002419A8
		private static bool FatalDamageValueExtraChecker(AffectedDataKey dataKey)
		{
			EDamageType damageType = (EDamageType)dataKey.CustomParam1;
			return damageType == EDamageType.Direct || damageType == EDamageType.FightBack;
		}

		// Token: 0x06003A71 RID: 14961 RVA: 0x002437D4 File Offset: 0x002419D4
		public JiuZiZhenYanZhou()
		{
		}

		// Token: 0x06003A72 RID: 14962 RVA: 0x002437E9 File Offset: 0x002419E9
		public JiuZiZhenYanZhou(CombatSkillKey skillKey) : base(skillKey, 7304, -1)
		{
		}

		// Token: 0x06003A73 RID: 14963 RVA: 0x00243808 File Offset: 0x00241A08
		public override void OnEnable(DataContext context)
		{
			base.OnEnable(context);
			this.ResetAffecting();
			bool isDirect = base.IsDirect;
			if (isDirect)
			{
				base.CreateAffectedAllEnemyData(102, EDataModifyType.AddPercent, -1);
				base.CreateAffectedAllEnemyData(191, EDataModifyType.AddPercent, -1);
				base.CreateAffectedAllEnemyData(276, EDataModifyType.AddPercent, -1);
			}
			else
			{
				base.CreateAffectedData(102, EDataModifyType.AddPercent, -1);
				base.CreateAffectedData(191, EDataModifyType.AddPercent, -1);
				base.CreateAffectedData(276, EDataModifyType.AddPercent, -1);
			}
			Events.RegisterHandler_CastAttackSkillBegin(new Events.OnCastAttackSkillBegin(this.OnCastAttackSkillBegin));
			Events.RegisterHandler_CastSkillEnd(new Events.OnCastSkillEnd(this.OnCastSkillEnd));
		}

		// Token: 0x06003A74 RID: 14964 RVA: 0x002438A7 File Offset: 0x00241AA7
		public override void OnDisable(DataContext context)
		{
			Events.UnRegisterHandler_CastAttackSkillBegin(new Events.OnCastAttackSkillBegin(this.OnCastAttackSkillBegin));
			Events.UnRegisterHandler_CastSkillEnd(new Events.OnCastSkillEnd(this.OnCastSkillEnd));
			base.OnDisable(context);
		}

		// Token: 0x06003A75 RID: 14965 RVA: 0x002438D8 File Offset: 0x00241AD8
		private void OnCastAttackSkillBegin(DataContext context, CombatCharacter attacker, CombatCharacter defender, short skillId)
		{
			bool checkTargetPass = base.IsDirect ? (attacker.GetId() == base.CharacterId) : (defender.GetId() == base.CharacterId);
			bool flag = !checkTargetPass || base.EffectCount <= 0 || this._affectingSkillKey.IsValid;
			if (!flag)
			{
				this._affectingSkillKey = new CombatSkillKey(defender.GetId(), skillId);
				CombatCharacter enemyChar = base.IsDirect ? defender : attacker;
				int changePercent = base.IsDirect ? 10 : -5;
				foreach (short bannedSkillId in enemyChar.GetBannedSkillIds(false))
				{
					sbyte equipType = Config.CombatSkill.Instance[bannedSkillId].EquipType;
					ushort fieldId;
					bool flag2 = JiuZiZhenYanZhou.Type2FieldIds.TryGetValue(equipType, out fieldId);
					if (flag2)
					{
						this._fieldId2ChangePercent[fieldId] = this._fieldId2ChangePercent.GetOrDefault(fieldId) + changePercent;
					}
				}
				bool flag3 = this._fieldId2ChangePercent.Count > 0;
				if (flag3)
				{
					base.ReduceEffectCount(1);
					foreach (ushort fieldId2 in this._fieldId2ChangePercent.Keys)
					{
						base.ShowSpecialEffectTips(JiuZiZhenYanZhou.FieldId2SpecialEffectTipsIndex[fieldId2]);
					}
				}
				else
				{
					this._affectingSkillKey = CombatSkillKey.Invalid;
				}
			}
		}

		// Token: 0x06003A76 RID: 14966 RVA: 0x00243A6C File Offset: 0x00241C6C
		private void OnCastSkillEnd(DataContext context, int charId, bool isAlly, short skillId, sbyte power, bool interrupted)
		{
			bool flag = this.SkillKey.IsMatch(charId, skillId) && base.PowerMatchAffectRequire((int)power, 0);
			if (flag)
			{
				base.AddMaxEffectCount(true);
			}
			int affectingCharId = DomainManager.Combat.GetCombatCharacter(!isAlly, true).GetId();
			bool flag2 = affectingCharId != this._affectingSkillKey.CharId || skillId != this._affectingSkillKey.SkillTemplateId;
			if (!flag2)
			{
				this.ResetAffecting();
			}
		}

		// Token: 0x06003A77 RID: 14967 RVA: 0x00243AE7 File Offset: 0x00241CE7
		private void ResetAffecting()
		{
			this._affectingSkillKey = CombatSkillKey.Invalid;
			this._fieldId2ChangePercent.Clear();
		}

		// Token: 0x06003A78 RID: 14968 RVA: 0x00243B04 File Offset: 0x00241D04
		public override int GetModifyValue(AffectedDataKey dataKey, int currModifyValue)
		{
			bool flag = dataKey.SkillKey != this._affectingSkillKey;
			int result;
			if (flag)
			{
				result = 0;
			}
			else
			{
				Func<AffectedDataKey, bool> checker;
				bool flag2 = JiuZiZhenYanZhou.ExtraCheckers.TryGetValue(dataKey.FieldId, out checker) && !checker(dataKey);
				if (flag2)
				{
					result = 0;
				}
				else
				{
					int effect;
					result = (this._fieldId2ChangePercent.TryGetValue(dataKey.FieldId, out effect) ? effect : 0);
				}
			}
			return result;
		}

		// Token: 0x0400111C RID: 4380
		private const int DirectChangeDamagePercent = 10;

		// Token: 0x0400111D RID: 4381
		private const int ReverseChangeDamagePercent = -5;

		// Token: 0x0400111E RID: 4382
		private static readonly IReadOnlyDictionary<sbyte, ushort> Type2FieldIds = new Dictionary<sbyte, ushort>
		{
			{
				1,
				102
			},
			{
				2,
				191
			},
			{
				3,
				276
			}
		};

		// Token: 0x0400111F RID: 4383
		private static readonly IReadOnlyDictionary<ushort, byte> FieldId2SpecialEffectTipsIndex = new Dictionary<ushort, byte>
		{
			{
				102,
				0
			},
			{
				191,
				1
			},
			{
				276,
				2
			}
		};

		// Token: 0x04001120 RID: 4384
		private static readonly IReadOnlyDictionary<ushort, Func<AffectedDataKey, bool>> ExtraCheckers = new Dictionary<ushort, Func<AffectedDataKey, bool>>
		{
			{
				191,
				new Func<AffectedDataKey, bool>(JiuZiZhenYanZhou.FatalDamageValueExtraChecker)
			}
		};

		// Token: 0x04001121 RID: 4385
		private readonly Dictionary<ushort, int> _fieldId2ChangePercent = new Dictionary<ushort, int>();

		// Token: 0x04001122 RID: 4386
		private CombatSkillKey _affectingSkillKey;
	}
}
