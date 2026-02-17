using System;
using System.Collections.Generic;
using System.Linq;
using GameData.Combat.Math;
using GameData.Common;
using GameData.DomainEvents;
using GameData.Domains.Combat;
using GameData.Domains.CombatSkill;
using GameData.Utilities;

namespace GameData.Domains.SpecialEffect.CombatSkill.Baihuagu.Finger
{
	// Token: 0x020005D2 RID: 1490
	public class XueZhuHuaBaFa : CombatSkillEffectBase
	{
		// Token: 0x170002BC RID: 700
		// (get) Token: 0x0600440C RID: 17420 RVA: 0x0026DCBE File Offset: 0x0026BEBE
		private CValuePercent AbsorbsHealthPercent
		{
			get
			{
				return base.IsDirect ? 20 : 40;
			}
		}

		// Token: 0x0600440D RID: 17421 RVA: 0x0026DCD3 File Offset: 0x0026BED3
		public XueZhuHuaBaFa()
		{
		}

		// Token: 0x0600440E RID: 17422 RVA: 0x0026DCE8 File Offset: 0x0026BEE8
		public XueZhuHuaBaFa(CombatSkillKey skillKey, sbyte direction) : base(skillKey, 3108, direction)
		{
		}

		// Token: 0x0600440F RID: 17423 RVA: 0x0026DD04 File Offset: 0x0026BF04
		public override void OnEnable(DataContext context)
		{
			base.CreateAffectedData(53, EDataModifyType.Add, -1);
			base.CreateAffectedData(210, EDataModifyType.Custom, base.SkillTemplateId);
			base.CreateAffectedData(222, EDataModifyType.Custom, base.SkillTemplateId);
			Events.RegisterHandler_CombatBegin(new Events.OnCombatBegin(this.OnCombatBegin));
			Events.RegisterHandler_CombatSettlement(new Events.OnCombatSettlement(this.OnCombatSettlement));
			Events.RegisterHandler_CastSkillEnd(new Events.OnCastSkillEnd(this.OnCastSkillEnd));
		}

		// Token: 0x06004410 RID: 17424 RVA: 0x0026DD79 File Offset: 0x0026BF79
		public override void OnDisable(DataContext context)
		{
			Events.UnRegisterHandler_CombatBegin(new Events.OnCombatBegin(this.OnCombatBegin));
			Events.UnRegisterHandler_CombatSettlement(new Events.OnCombatSettlement(this.OnCombatSettlement));
			Events.UnRegisterHandler_CastSkillEnd(new Events.OnCastSkillEnd(this.OnCastSkillEnd));
		}

		// Token: 0x06004411 RID: 17425 RVA: 0x0026DDB4 File Offset: 0x0026BFB4
		private void OnCombatBegin(DataContext context)
		{
			bool flag = !DomainManager.Combat.IsCharInCombat(base.CharacterId, true) || !base.CombatChar.GetAttackSkillList().Exist(base.SkillTemplateId);
			if (!flag)
			{
				bool flag2 = this._effectCount > 0;
				if (flag2)
				{
					this.UpdateEffectCount(context);
				}
				bool isDirect = base.IsDirect;
				if (isDirect)
				{
					this._affectingCharIds.Add(base.CharacterId);
				}
				else
				{
					this._affectingCharIds.AddRange(from x in DomainManager.Combat.GetCharacterList(!base.CombatChar.IsAlly)
					where x >= 0
					select x);
				}
				foreach (int charId in this._affectingCharIds)
				{
					base.AppendAffectedData(context, charId, 304, EDataModifyType.Custom, -1);
				}
			}
		}

		// Token: 0x06004412 RID: 17426 RVA: 0x0026DEC8 File Offset: 0x0026C0C8
		private void OnCombatSettlement(DataContext context, sbyte combatStatus)
		{
			bool flag = !DomainManager.Combat.IsCharInCombat(base.CharacterId, true);
			if (!flag)
			{
				foreach (int charId in this._affectingCharIds)
				{
					base.RemoveAffectedData(context, charId, 304);
				}
				this._affectingCharIds.Clear();
			}
		}

		// Token: 0x06004413 RID: 17427 RVA: 0x0026DF4C File Offset: 0x0026C14C
		private void OnCastSkillEnd(DataContext context, int charId, bool isAlly, short skillId, sbyte power, bool interrupted)
		{
			bool flag = !this.SkillKey.IsMatch(charId, skillId) || !base.PowerMatchAffectRequire((int)power, 0) || (short)this._effectCount >= base.MaxEffectCount;
			if (!flag)
			{
				bool flag2 = DomainManager.Combat.CheckHealthImmunity(context, base.EnemyChar);
				if (!flag2)
				{
					short health = base.EnemyChar.GetCharacter().GetHealth();
					int unit = Math.Min((int)health * this.AbsorbsHealthPercent / 12, (int)(base.MaxEffectCount - (short)this._effectCount));
					int absorbs = unit * 12;
					bool flag3 = absorbs <= 0;
					if (!flag3)
					{
						base.EnemyChar.GetCharacter().ChangeHealth(context, -absorbs);
						this.ChangeEffectCount(context, unit);
						base.ShowSpecialEffectTips(0);
					}
				}
			}
		}

		// Token: 0x06004414 RID: 17428 RVA: 0x0026E018 File Offset: 0x0026C218
		private void ChangeEffectCount(DataContext context, int deltaValue)
		{
			bool flag = deltaValue == 0;
			if (!flag)
			{
				this._effectCount = (sbyte)Math.Clamp((int)this._effectCount + deltaValue, 0, 127);
				this.UpdateEffectCount(context);
				DomainManager.SpecialEffect.SaveEffect(context, this.Id);
				base.InvalidateCache(context, 53);
			}
		}

		// Token: 0x06004415 RID: 17429 RVA: 0x0026E06B File Offset: 0x0026C26B
		private void UpdateEffectCount(DataContext context)
		{
			DomainManager.Combat.AddSkillEffect(context, base.CombatChar, base.EffectKey, (short)this._effectCount, base.MaxEffectCount, true);
		}

		// Token: 0x06004416 RID: 17430 RVA: 0x0026E094 File Offset: 0x0026C294
		public override int GetModifyValue(AffectedDataKey dataKey, int currModifyValue)
		{
			bool flag = dataKey.CharId != base.CharacterId;
			int result;
			if (flag)
			{
				result = 0;
			}
			else
			{
				bool flag2 = dataKey.FieldId == 53;
				if (flag2)
				{
					result = (int)(12 * this._effectCount);
				}
				else
				{
					result = 0;
				}
			}
			return result;
		}

		// Token: 0x06004417 RID: 17431 RVA: 0x0026E0DC File Offset: 0x0026C2DC
		public override int GetModifiedValue(AffectedDataKey dataKey, int dataValue)
		{
			bool flag = dataKey.FieldId != 304;
			int result;
			if (flag)
			{
				result = dataValue;
			}
			else
			{
				EDamageType damageType = (EDamageType)dataKey.CustomParam0;
				bool flag2 = damageType != EDamageType.Direct;
				if (flag2)
				{
					result = dataValue;
				}
				else
				{
					int maxUnit = Math.Min((int)(this._effectCount / 4), dataValue);
					bool flag3 = maxUnit <= 0;
					if (flag3)
					{
						result = dataValue;
					}
					else
					{
						bool isDirect = base.IsDirect;
						if (isDirect)
						{
							dataValue -= maxUnit;
						}
						else
						{
							dataValue += maxUnit;
						}
						this.ChangeEffectCount(base.CombatChar.GetDataContext(), -maxUnit * 4);
						result = dataValue;
					}
				}
			}
			return result;
		}

		// Token: 0x06004418 RID: 17432 RVA: 0x0026E170 File Offset: 0x0026C370
		public override bool GetModifiedValue(AffectedDataKey dataKey, bool dataValue)
		{
			bool flag = dataKey.CharId != base.CharacterId || dataKey.CombatSkillId != base.SkillTemplateId;
			bool result;
			if (flag)
			{
				result = dataValue;
			}
			else
			{
				ushort fieldId = dataKey.FieldId;
				bool flag2 = fieldId == 210 || fieldId == 222;
				bool flag3 = flag2;
				result = (!flag3 && dataValue);
			}
			return result;
		}

		// Token: 0x06004419 RID: 17433 RVA: 0x0026E1DC File Offset: 0x0026C3DC
		protected override int GetSubClassSerializedSize()
		{
			return base.GetSubClassSerializedSize() + 1;
		}

		// Token: 0x0600441A RID: 17434 RVA: 0x0026E1F8 File Offset: 0x0026C3F8
		protected unsafe override int SerializeSubClass(byte* pData)
		{
			byte* pCurrData = pData + base.SerializeSubClass(pData);
			*pCurrData = (byte)this._effectCount;
			return this.GetSubClassSerializedSize();
		}

		// Token: 0x0600441B RID: 17435 RVA: 0x0026E224 File Offset: 0x0026C424
		protected unsafe override int DeserializeSubClass(byte* pData)
		{
			byte* pCurrData = pData + base.DeserializeSubClass(pData);
			this._effectCount = *(sbyte*)pCurrData;
			return this.GetSubClassSerializedSize();
		}

		// Token: 0x04001433 RID: 5171
		private const int PerFatalMarkCostEffectCount = 4;

		// Token: 0x04001434 RID: 5172
		private const int AbsorbsHealthUnit = 12;

		// Token: 0x04001435 RID: 5173
		private sbyte _effectCount;

		// Token: 0x04001436 RID: 5174
		private readonly List<int> _affectingCharIds = new List<int>();
	}
}
