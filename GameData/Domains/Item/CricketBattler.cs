using System;
using Config;
using GameData.Domains.Item.Display;

namespace GameData.Domains.Item
{
	// Token: 0x02000663 RID: 1635
	public class CricketBattler
	{
		// Token: 0x17000329 RID: 809
		// (get) Token: 0x06004F6D RID: 20333 RVA: 0x002B4531 File Offset: 0x002B2731
		public bool IsTrash
		{
			get
			{
				return this._colorConfig.TemplateId == 0;
			}
		}

		// Token: 0x1700032A RID: 810
		// (get) Token: 0x06004F6E RID: 20334 RVA: 0x002B4541 File Offset: 0x002B2741
		public int Level
		{
			get
			{
				return this._isCombineCricket ? ((int)MathF.Max((float)this._colorConfig.Level, (float)this._partConfig.Level)) : ((int)this._colorConfig.Level);
			}
		}

		// Token: 0x1700032B RID: 811
		// (get) Token: 0x06004F6F RID: 20335 RVA: 0x002B4576 File Offset: 0x002B2776
		public int Vigor
		{
			get
			{
				int num = (int)(this._colorConfig.Vigor + (this._isCombineCricket ? this._partConfig.Vigor : 0));
				CricketData cricketData = this.CricketData;
				return num - (int)((cricketData != null) ? cricketData[2] : 0);
			}
		}

		// Token: 0x1700032C RID: 812
		// (get) Token: 0x06004F70 RID: 20336 RVA: 0x002B45AE File Offset: 0x002B27AE
		public int Strength
		{
			get
			{
				int num = (int)(this._colorConfig.Strength + (this._isCombineCricket ? this._partConfig.Strength : 0));
				CricketData cricketData = this.CricketData;
				return num - (int)((cricketData != null) ? cricketData[3] : 0);
			}
		}

		// Token: 0x1700032D RID: 813
		// (get) Token: 0x06004F71 RID: 20337 RVA: 0x002B45E6 File Offset: 0x002B27E6
		public int Bite
		{
			get
			{
				int num = (int)(this._colorConfig.Bite + (this._isCombineCricket ? this._partConfig.Bite : 0));
				CricketData cricketData = this.CricketData;
				return num - (int)((cricketData != null) ? cricketData[4] : 0);
			}
		}

		// Token: 0x1700032E RID: 814
		// (get) Token: 0x06004F72 RID: 20338 RVA: 0x002B461E File Offset: 0x002B281E
		public int Deadliness
		{
			get
			{
				return (int)(this._isCombineCricket ? (this._colorConfig.Deadliness + this._partConfig.Deadliness) : this._colorConfig.Deadliness);
			}
		}

		// Token: 0x1700032F RID: 815
		// (get) Token: 0x06004F73 RID: 20339 RVA: 0x002B464C File Offset: 0x002B284C
		public int Damage
		{
			get
			{
				return (int)(this._isCombineCricket ? (this._colorConfig.Damage + this._partConfig.Damage) : this._colorConfig.Damage);
			}
		}

		// Token: 0x17000330 RID: 816
		// (get) Token: 0x06004F74 RID: 20340 RVA: 0x002B467A File Offset: 0x002B287A
		public int Cripple
		{
			get
			{
				return (int)(this._isCombineCricket ? (this._colorConfig.Cripple + this._partConfig.Cripple) : this._colorConfig.Cripple);
			}
		}

		// Token: 0x17000331 RID: 817
		// (get) Token: 0x06004F75 RID: 20341 RVA: 0x002B46A8 File Offset: 0x002B28A8
		public int Defence
		{
			get
			{
				return (int)(this._isCombineCricket ? (this._colorConfig.Defence + this._partConfig.Defence) : this._colorConfig.Defence);
			}
		}

		// Token: 0x17000332 RID: 818
		// (get) Token: 0x06004F76 RID: 20342 RVA: 0x002B46D6 File Offset: 0x002B28D6
		public int DamageReduce
		{
			get
			{
				return (int)(this._isCombineCricket ? (this._colorConfig.DamageReduce + this._partConfig.DamageReduce) : this._colorConfig.DamageReduce);
			}
		}

		// Token: 0x17000333 RID: 819
		// (get) Token: 0x06004F77 RID: 20343 RVA: 0x002B4704 File Offset: 0x002B2904
		public int Counter
		{
			get
			{
				return (int)(this._isCombineCricket ? (this._colorConfig.Counter + this._partConfig.Counter) : this._colorConfig.Counter);
			}
		}

		// Token: 0x17000334 RID: 820
		// (get) Token: 0x06004F78 RID: 20344 RVA: 0x002B4732 File Offset: 0x002B2932
		public int MaxHP
		{
			get
			{
				int num = (int)(this._colorConfig.HP + (this._isCombineCricket ? this._partConfig.HP : 0));
				CricketData cricketData = this.CricketData;
				return num - (int)((cricketData != null) ? cricketData[0] : 0);
			}
		}

		// Token: 0x17000335 RID: 821
		// (get) Token: 0x06004F79 RID: 20345 RVA: 0x002B476A File Offset: 0x002B296A
		public int MaxSP
		{
			get
			{
				int num = (int)(this._colorConfig.SP + (this._isCombineCricket ? this._partConfig.SP : 0));
				CricketData cricketData = this.CricketData;
				return num - (int)((cricketData != null) ? cricketData[1] : 0);
			}
		}

		// Token: 0x17000336 RID: 822
		// (get) Token: 0x06004F7A RID: 20346 RVA: 0x002B47A2 File Offset: 0x002B29A2
		public bool IsFail
		{
			get
			{
				return this.HP <= 0 || this.SP <= 0 || this.Durability <= 0;
			}
		}

		// Token: 0x06004F7B RID: 20347 RVA: 0x002B47C8 File Offset: 0x002B29C8
		public CricketBattler(ItemKey itemKey)
		{
			this.Key = itemKey;
			this.CricketData = DomainManager.Item.GetCricketData(itemKey.Id);
			ItemDisplayData itemDisplayData = DomainManager.Item.GetItemDisplayData(itemKey, -1);
			this._colorConfig = CricketParts.Instance.GetItem(itemDisplayData.CricketColorId);
			this._partConfig = CricketParts.Instance.GetItem(itemDisplayData.CricketPartId);
			this._isCombineCricket = (this._partConfig != null);
			this.HP = this.MaxHP;
			this.SP = this.MaxSP;
			this.Durability = (int)itemDisplayData.Durability;
		}

		// Token: 0x0400159F RID: 5535
		public ItemKey Key;

		// Token: 0x040015A0 RID: 5536
		private CricketPartsItem _colorConfig;

		// Token: 0x040015A1 RID: 5537
		private CricketPartsItem _partConfig;

		// Token: 0x040015A2 RID: 5538
		private bool _isCombineCricket;

		// Token: 0x040015A3 RID: 5539
		public CricketData CricketData;

		// Token: 0x040015A4 RID: 5540
		public int HP;

		// Token: 0x040015A5 RID: 5541
		public int SP;

		// Token: 0x040015A6 RID: 5542
		public int Durability;
	}
}
