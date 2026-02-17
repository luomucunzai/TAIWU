using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using GameData.Combat.Math;
using GameData.Common;
using GameData.DomainEvents;
using GameData.Domains.Character;
using GameData.Domains.Combat;
using GameData.Domains.CombatSkill;
using GameData.Domains.Item;
using GameData.GameDataBridge;
using GameData.Utilities;

namespace GameData.Domains.SpecialEffect.CombatSkill.Ranshanpai.Neigong
{
	// Token: 0x02000457 RID: 1111
	public class TianSuiBaoLu : CombatSkillEffectBase
	{
		// Token: 0x06003AA7 RID: 15015 RVA: 0x0024458D File Offset: 0x0024278D
		public TianSuiBaoLu()
		{
		}

		// Token: 0x06003AA8 RID: 15016 RVA: 0x002445A2 File Offset: 0x002427A2
		public TianSuiBaoLu(CombatSkillKey skillKey) : base(skillKey, 7008, -1)
		{
		}

		// Token: 0x06003AA9 RID: 15017 RVA: 0x002445C0 File Offset: 0x002427C0
		public override void OnEnable(DataContext context)
		{
			base.OnEnable(context);
			Events.RegisterHandler_CombatBegin(new Events.OnCombatBegin(this.OnCombatBegin));
			Events.RegisterHandler_UsedCustomItem(new Events.OnUsedCustomItem(this.OnUsedCustomItem));
			Events.RegisterHandler_CombatSettlement(new Events.OnCombatSettlement(this.OnCombatSettlement));
		}

		// Token: 0x06003AAA RID: 15018 RVA: 0x0024460C File Offset: 0x0024280C
		public override void OnDisable(DataContext context)
		{
			Events.UnRegisterHandler_CombatBegin(new Events.OnCombatBegin(this.OnCombatBegin));
			Events.UnRegisterHandler_UsedCustomItem(new Events.OnUsedCustomItem(this.OnUsedCustomItem));
			Events.UnRegisterHandler_CombatSettlement(new Events.OnCombatSettlement(this.OnCombatSettlement));
			base.OnDisable(context);
		}

		// Token: 0x06003AAB RID: 15019 RVA: 0x00244658 File Offset: 0x00242858
		private void OnCombatBegin(DataContext context)
		{
			if (this._wordItemKeys == null)
			{
				this._wordItemKeys = new List<ItemKeyAndCount>();
			}
			this._wordItemKeys.Clear();
			short[] wordTemplateIds = base.IsDirect ? TianSuiBaoLu.DirectWords : TianSuiBaoLu.ReverseWords;
			foreach (short templateId in wordTemplateIds)
			{
				this._wordItemKeys.Add(new ValueTuple<ItemKey, int>(DomainManager.Item.CreateMisc(context, templateId), 3));
			}
			base.AppendAffectedData(context, 325, EDataModifyType.Custom, -1);
			base.ShowSpecialEffectTips(0);
			GameDataBridge.AddDisplayEvent<bool, int>(DisplayEventType.CombatShowTianSuiBaoLu, base.IsDirect, base.CharacterId);
		}

		// Token: 0x06003AAC RID: 15020 RVA: 0x00244700 File Offset: 0x00242900
		private void OnUsedCustomItem(DataContext context, int charId, ItemKey itemKey)
		{
			bool flag = charId != base.CharacterId || itemKey.ItemType != 12;
			if (!flag)
			{
				int index = this._wordItemKeys.FindIndex((ItemKeyAndCount x) => x.ItemKey == itemKey);
				bool flag2 = index < 0;
				if (!flag2)
				{
					this._wordItemKeys[index] = new ValueTuple<ItemKey, int>(this._wordItemKeys[index].ItemKey, this._wordItemKeys[index].Count - 1);
					bool flag3 = this._wordItemKeys[index].Count <= 0;
					if (flag3)
					{
						DomainManager.Item.RemoveItem(context, itemKey);
						this._wordItemKeys.RemoveAt(index);
					}
					base.InvalidateCache(context, 325);
					short templateId = itemKey.TemplateId;
					bool flag4 = templateId == 375 || templateId == 380;
					bool flag5 = flag4;
					if (flag5)
					{
						this.DoAffectShenJian(context);
					}
					else
					{
						flag4 = (templateId == 376 || templateId == 381);
						bool flag6 = flag4;
						if (flag6)
						{
							this.DoAffectHuanShe(context);
						}
						else
						{
							flag4 = (templateId == 377 || templateId == 382);
							bool flag7 = flag4;
							if (flag7)
							{
								this.DoAffectFuCang(context);
							}
							else
							{
								flag4 = (templateId == 378 || templateId == 383);
								bool flag8 = flag4;
								if (flag8)
								{
									this.DoAffectYinShen(context);
								}
								else
								{
									flag4 = (templateId == 379 || templateId == 384);
									bool flag9 = flag4;
									if (flag9)
									{
										this.DoAffectQuLiuWu(context);
									}
									else
									{
										DefaultInterpolatedStringHandler defaultInterpolatedStringHandler = new DefaultInterpolatedStringHandler(23, 1);
										defaultInterpolatedStringHandler.AppendLiteral("Unexpected template id ");
										defaultInterpolatedStringHandler.AppendFormatted<short>(templateId);
										AdaptableLog.Warning(defaultInterpolatedStringHandler.ToStringAndClear(), false);
									}
								}
							}
						}
					}
				}
			}
		}

		// Token: 0x06003AAD RID: 15021 RVA: 0x00244918 File Offset: 0x00242B18
		private void OnCombatSettlement(DataContext context, sbyte combatStatus)
		{
			foreach (ItemKeyAndCount itemKeyAndCount in this._wordItemKeys)
			{
				ItemKey itemKey2;
				int num;
				itemKeyAndCount.Deconstruct(out itemKey2, out num);
				ItemKey itemKey = itemKey2;
				DomainManager.Item.RemoveItem(context, itemKey);
			}
			this._wordItemKeys.Clear();
		}

		// Token: 0x06003AAE RID: 15022 RVA: 0x00244990 File Offset: 0x00242B90
		private void DoAffectShenJian(DataContext context)
		{
			base.ShowSpecialEffectTips(1);
			bool isDirect = base.IsDirect;
			if (isDirect)
			{
				DomainManager.Combat.AddTrick(context, base.CombatChar, base.CombatChar.GetWeaponTricks());
			}
			else
			{
				CombatCharacter enemyChar = base.EnemyChar;
				List<sbyte> usableTricks = ObjectPool<List<sbyte>>.Instance.Get();
				usableTricks.AddRange(enemyChar.GetTricks().Tricks.Values.Where(new Func<sbyte, bool>(enemyChar.IsTrickUsable)));
				foreach (sbyte trickType in RandomUtils.GetRandomUnrepeated<sbyte>(context.Random, 6, usableTricks, null))
				{
					DomainManager.Combat.RemoveTrick(context, enemyChar, trickType, 1, false, -1);
				}
				ObjectPool<List<sbyte>>.Instance.Return(usableTricks);
			}
		}

		// Token: 0x06003AAF RID: 15023 RVA: 0x00244A70 File Offset: 0x00242C70
		private void DoAffectHuanShe(DataContext context)
		{
			base.ShowSpecialEffectTips(2);
			bool isDirect = base.IsDirect;
			if (isDirect)
			{
				base.ChangeBreathValue(context, base.CombatChar, base.CombatChar.GetMaxBreathValue());
				base.ChangeStanceValue(context, base.CombatChar, base.CombatChar.GetMaxStanceValue());
			}
			else
			{
				CombatCharacter enemyChar = base.EnemyChar;
				base.ChangeBreathValue(context, enemyChar, -enemyChar.GetMaxBreathValue());
				base.ChangeStanceValue(context, enemyChar, -enemyChar.GetMaxStanceValue());
			}
		}

		// Token: 0x06003AB0 RID: 15024 RVA: 0x00244AF0 File Offset: 0x00242CF0
		private void DoAffectFuCang(DataContext context)
		{
			base.ShowSpecialEffectTips(3);
			bool isDirect = base.IsDirect;
			if (isDirect)
			{
				base.ChangeMobilityValue(context, base.CombatChar, base.CombatChar.GetMaxMobility());
			}
			else
			{
				base.ChangeMobilityValue(context, base.EnemyChar, -base.EnemyChar.GetMaxMobility());
			}
		}

		// Token: 0x06003AB1 RID: 15025 RVA: 0x00244B48 File Offset: 0x00242D48
		private unsafe void DoAffectYinShen(DataContext context)
		{
			base.ShowSpecialEffectTips(4);
			CombatCharacter target = base.IsDirect ? base.CombatChar : base.EnemyChar;
			NeiliAllocation neiliAllocation = target.GetNeiliAllocation();
			NeiliAllocation originNeiliAllocation = target.GetOriginNeiliAllocation();
			for (byte i = 0; i < 4; i += 1)
			{
				short value = *neiliAllocation[(int)i];
				short originValue = *originNeiliAllocation[(int)i];
				bool flag = base.IsDirect ? (value >= originValue) : (value <= originValue);
				if (!flag)
				{
					int delta = base.IsDirect ? ((int)value) : (-((int)value * CValueHalf.RoundDown));
					target.ChangeNeiliAllocation(context, i, delta, false, true);
				}
			}
		}

		// Token: 0x06003AB2 RID: 15026 RVA: 0x00244BFC File Offset: 0x00242DFC
		private void DoAffectQuLiuWu(DataContext context)
		{
			base.ShowSpecialEffectTips(5);
			CombatCharacter target = base.IsDirect ? base.CombatChar : base.EnemyChar;
			foreach (short bannedSkillId in target.GetBannedSkillIds(true))
			{
				bool isDirect = base.IsDirect;
				if (isDirect)
				{
					DomainManager.Combat.ClearSkillCd(context, target, bannedSkillId);
				}
				else
				{
					DomainManager.Combat.DoubleSkillCd(context, target, bannedSkillId);
				}
			}
		}

		// Token: 0x06003AB3 RID: 15027 RVA: 0x00244C90 File Offset: 0x00242E90
		public override List<ItemKeyAndCount> GetModifiedValue(AffectedDataKey dataKey, List<ItemKeyAndCount> dataValue)
		{
			bool flag = dataKey.CharId == base.CharacterId && dataKey.FieldId == 325;
			if (flag)
			{
				foreach (ItemKeyAndCount itemKey in this._wordItemKeys)
				{
					dataValue.Add(itemKey);
				}
			}
			return base.GetModifiedValue(dataKey, dataValue);
		}

		// Token: 0x04001129 RID: 4393
		private static readonly short[] DirectWords = new short[]
		{
			375,
			376,
			377,
			378,
			379
		};

		// Token: 0x0400112A RID: 4394
		private static readonly short[] ReverseWords = new short[]
		{
			380,
			381,
			382,
			383,
			384
		};

		// Token: 0x0400112B RID: 4395
		private const int RemoveTrickCount = 6;

		// Token: 0x0400112C RID: 4396
		private const int WordCount = 3;

		// Token: 0x0400112D RID: 4397
		private List<ItemKeyAndCount> _wordItemKeys = new List<ItemKeyAndCount>();
	}
}
