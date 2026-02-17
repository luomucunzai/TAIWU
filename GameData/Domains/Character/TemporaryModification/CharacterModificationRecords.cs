using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using GameData.Serializer;
using GameData.Utilities;

namespace GameData.Domains.Character.TemporaryModification
{
	// Token: 0x02000819 RID: 2073
	[SerializableGameData(NotForDisplayModule = true)]
	public class CharacterModificationRecords : RawDataBlock
	{
		// Token: 0x060074CA RID: 29898 RVA: 0x00446DDD File Offset: 0x00444FDD
		public CharacterModificationRecords()
		{
			this.Savepoint = -1;
		}

		// Token: 0x060074CB RID: 29899 RVA: 0x00446DEE File Offset: 0x00444FEE
		public CharacterModificationRecords(int initialCapacity) : base(initialCapacity)
		{
			this.Savepoint = -1;
		}

		// Token: 0x060074CC RID: 29900 RVA: 0x00446E00 File Offset: 0x00445000
		public void RecordHappiness(sbyte oriValue, sbyte currValue)
		{
			int delta = (int)(currValue - oriValue);
			int offset = this.BeginAddingRecord(RevertibleCharacterPropertyType.Happiness, 4);
			this.WriteDelta(offset, delta);
		}

		// Token: 0x060074CD RID: 29901 RVA: 0x00446E24 File Offset: 0x00445024
		public unsafe static int RetrieveHappiness(byte* pModificationData)
		{
			return CharacterModificationRecords.ReadDeltaInt(pModificationData);
		}

		// Token: 0x060074CE RID: 29902 RVA: 0x00446E3C File Offset: 0x0044503C
		public void RecordBaseMorality(short oriValue, short currValue)
		{
			int delta = (int)(currValue - oriValue);
			int offset = this.BeginAddingRecord(RevertibleCharacterPropertyType.BaseMorality, 4);
			this.WriteDelta(offset, delta);
		}

		// Token: 0x060074CF RID: 29903 RVA: 0x00446E60 File Offset: 0x00445060
		public unsafe static int RetrieveBaseMorality(byte* pModificationData)
		{
			return CharacterModificationRecords.ReadDeltaInt(pModificationData);
		}

		// Token: 0x060074D0 RID: 29904 RVA: 0x00446E78 File Offset: 0x00445078
		public void RecordFeatureIds(List<short> oriValue, List<short> currValue)
		{
			List<ShortListModification> modifications = CharacterModificationRecords.CalcDelta(oriValue, currValue);
			ushort dataSize = CharacterModificationRecords.CalcDataSize(modifications);
			int offset = this.BeginAddingRecord(RevertibleCharacterPropertyType.FeatureIds, dataSize);
			this.WriteDelta(offset, modifications);
		}

		// Token: 0x060074D1 RID: 29905 RVA: 0x00446EA8 File Offset: 0x004450A8
		public unsafe static List<ShortListModification> RetrieveFeatureIds(byte* pModificationData, ushort dataSize)
		{
			return CharacterModificationRecords.ReadDeltaShortList(pModificationData, dataSize);
		}

		// Token: 0x060074D2 RID: 29906 RVA: 0x00446EC4 File Offset: 0x004450C4
		public void RecordBaseMainAttributes(MainAttributes oriValue, MainAttributes currValue)
		{
			MainAttributes delta = currValue.Subtract(oriValue);
			ushort dataSize = (ushort)delta.GetSerializedSize();
			int offset = this.BeginAddingRecord(RevertibleCharacterPropertyType.BaseMainAttributes, dataSize);
			this.WriteDelta(offset, delta);
		}

		// Token: 0x060074D3 RID: 29907 RVA: 0x00446EF8 File Offset: 0x004450F8
		public unsafe static MainAttributes RetrieveBaseMainAttributes(byte* pModificationData)
		{
			return CharacterModificationRecords.ReadDeltaMainAttributes(pModificationData);
		}

		// Token: 0x060074D4 RID: 29908 RVA: 0x00446F10 File Offset: 0x00445110
		public void RecordDisorderOfQi(short oriValue, short currValue)
		{
			int delta = (int)(currValue - oriValue);
			int offset = this.BeginAddingRecord(RevertibleCharacterPropertyType.DisorderOfQi, 4);
			this.WriteDelta(offset, delta);
		}

		// Token: 0x060074D5 RID: 29909 RVA: 0x00446F34 File Offset: 0x00445134
		public unsafe static int RetrieveDisorderOfQi(byte* pModificationData)
		{
			return CharacterModificationRecords.ReadDeltaInt(pModificationData);
		}

		// Token: 0x060074D6 RID: 29910 RVA: 0x00446F4C File Offset: 0x0044514C
		public void RecordInjuries(Injuries oriValue, Injuries currValue)
		{
			Injuries delta = currValue.Subtract(oriValue);
			ushort dataSize = (ushort)delta.GetSerializedSize();
			int offset = this.BeginAddingRecord(RevertibleCharacterPropertyType.Injuries, dataSize);
			this.WriteDelta(offset, delta);
		}

		// Token: 0x060074D7 RID: 29911 RVA: 0x00446F80 File Offset: 0x00445180
		public unsafe static Injuries RetrieveInjuries(byte* pModificationData)
		{
			return CharacterModificationRecords.ReadDeltaInjuries(pModificationData);
		}

		// Token: 0x060074D8 RID: 29912 RVA: 0x00446F98 File Offset: 0x00445198
		public void RecordExtraNeili(int oriValue, int currValue)
		{
			int delta = currValue - oriValue;
			int offset = this.BeginAddingRecord(RevertibleCharacterPropertyType.ExtraNeili, 4);
			this.WriteDelta(offset, delta);
		}

		// Token: 0x060074D9 RID: 29913 RVA: 0x00446FBC File Offset: 0x004451BC
		public unsafe static int RetrieveExtraNeili(byte* pModificationData)
		{
			return CharacterModificationRecords.ReadDeltaInt(pModificationData);
		}

		// Token: 0x060074DA RID: 29914 RVA: 0x00446FD4 File Offset: 0x004451D4
		public void RecordConsummateLevel(sbyte oriValue, sbyte currValue)
		{
			int delta = (int)(currValue - oriValue);
			int offset = this.BeginAddingRecord(RevertibleCharacterPropertyType.ConsummateLevel, 4);
			this.WriteDelta(offset, delta);
		}

		// Token: 0x060074DB RID: 29915 RVA: 0x00446FF8 File Offset: 0x004451F8
		public unsafe static int RetrieveConsummateLevel(byte* pModificationData)
		{
			return CharacterModificationRecords.ReadDeltaInt(pModificationData);
		}

		// Token: 0x060074DC RID: 29916 RVA: 0x00447010 File Offset: 0x00445210
		public void RecordBaseLifeSkillQualifications(ref LifeSkillShorts oriValue, ref LifeSkillShorts currValue)
		{
			LifeSkillShorts delta = currValue.Subtract(ref oriValue);
			ushort dataSize = (ushort)delta.GetSerializedSize();
			int offset = this.BeginAddingRecord(RevertibleCharacterPropertyType.BaseLifeSkillQualifications, dataSize);
			this.WriteDelta(offset, ref delta);
		}

		// Token: 0x060074DD RID: 29917 RVA: 0x00447044 File Offset: 0x00445244
		public unsafe static LifeSkillShorts RetrieveBaseLifeSkillQualifications(byte* pModificationData)
		{
			return CharacterModificationRecords.ReadDeltaLifeSkillShorts(pModificationData);
		}

		// Token: 0x060074DE RID: 29918 RVA: 0x0044705C File Offset: 0x0044525C
		public void RecordBaseCombatSkillQualifications(ref CombatSkillShorts oriValue, ref CombatSkillShorts currValue)
		{
			CombatSkillShorts delta = currValue.Subtract(ref oriValue);
			ushort dataSize = (ushort)delta.GetSerializedSize();
			int offset = this.BeginAddingRecord(RevertibleCharacterPropertyType.BaseCombatSkillQualifications, dataSize);
			this.WriteDelta(offset, ref delta);
		}

		// Token: 0x060074DF RID: 29919 RVA: 0x00447090 File Offset: 0x00445290
		public unsafe static CombatSkillShorts RetrieveBaseCombatSkillQualifications(byte* pModificationData)
		{
			return CharacterModificationRecords.ReadDeltaCombatSkillShorts(pModificationData);
		}

		// Token: 0x060074E0 RID: 29920 RVA: 0x004470A8 File Offset: 0x004452A8
		public void RecordResources(ref ResourceInts oriValue, ref ResourceInts currValue)
		{
			ResourceInts delta = currValue.Subtract(ref oriValue);
			ushort dataSize = (ushort)delta.GetSerializedSize();
			int offset = this.BeginAddingRecord(RevertibleCharacterPropertyType.Resources, dataSize);
			this.WriteDelta(offset, ref delta);
		}

		// Token: 0x060074E1 RID: 29921 RVA: 0x004470DC File Offset: 0x004452DC
		public unsafe static ResourceInts RetrieveResources(byte* pModificationData)
		{
			return CharacterModificationRecords.ReadDeltaResourceInts(pModificationData);
		}

		// Token: 0x060074E2 RID: 29922 RVA: 0x004470F4 File Offset: 0x004452F4
		public void RecordCurrMainAttributes(MainAttributes oriValue, MainAttributes currValue)
		{
			MainAttributes delta = currValue.Subtract(oriValue);
			ushort dataSize = (ushort)delta.GetSerializedSize();
			int offset = this.BeginAddingRecord(RevertibleCharacterPropertyType.CurrMainAttributes, dataSize);
			this.WriteDelta(offset, delta);
		}

		// Token: 0x060074E3 RID: 29923 RVA: 0x00447128 File Offset: 0x00445328
		public unsafe static MainAttributes RetrieveCurrMainAttributes(byte* pModificationData)
		{
			return CharacterModificationRecords.ReadDeltaMainAttributes(pModificationData);
		}

		// Token: 0x060074E4 RID: 29924 RVA: 0x00447140 File Offset: 0x00445340
		public void RecordPoisoned(ref PoisonInts oriValue, ref PoisonInts currValue)
		{
			PoisonInts delta = currValue.Subtract(ref oriValue);
			ushort dataSize = (ushort)delta.GetSerializedSize();
			int offset = this.BeginAddingRecord(RevertibleCharacterPropertyType.Poisoned, dataSize);
			this.WriteDelta(offset, ref delta);
		}

		// Token: 0x060074E5 RID: 29925 RVA: 0x00447174 File Offset: 0x00445374
		public unsafe static PoisonInts RetrievePoisoned(byte* pModificationData)
		{
			return CharacterModificationRecords.ReadDeltaPoisonInts(pModificationData);
		}

		// Token: 0x060074E6 RID: 29926 RVA: 0x0044718C File Offset: 0x0044538C
		public void RecordCurrNeili(int oriValue, int currValue)
		{
			int delta = currValue - oriValue;
			int offset = this.BeginAddingRecord(RevertibleCharacterPropertyType.CurrNeili, 4);
			this.WriteDelta(offset, delta);
		}

		// Token: 0x060074E7 RID: 29927 RVA: 0x004471B4 File Offset: 0x004453B4
		public unsafe static int RetrieveCurrNeili(byte* pModificationData)
		{
			return CharacterModificationRecords.ReadDeltaInt(pModificationData);
		}

		// Token: 0x060074E8 RID: 29928 RVA: 0x004471CC File Offset: 0x004453CC
		public void RecordExtraNeiliAllocation(NeiliAllocation oriValue, NeiliAllocation currValue)
		{
			NeiliAllocation delta = currValue.Subtract(oriValue);
			ushort dataSize = (ushort)delta.GetSerializedSize();
			int offset = this.BeginAddingRecord(RevertibleCharacterPropertyType.ExtraNeiliAllocation, dataSize);
			this.WriteDelta(offset, delta);
		}

		// Token: 0x060074E9 RID: 29929 RVA: 0x00447200 File Offset: 0x00445400
		public unsafe static NeiliAllocation RetrieveExtraNeiliAllocation(byte* pModificationData)
		{
			return CharacterModificationRecords.ReadDeltaNeiliAllocation(pModificationData);
		}

		// Token: 0x060074EA RID: 29930 RVA: 0x00447218 File Offset: 0x00445418
		public void RecordXiangshuInfection(byte oriValue, byte currValue)
		{
			int delta = (int)(currValue - oriValue);
			int offset = this.BeginAddingRecord(RevertibleCharacterPropertyType.XiangshuInfection, 4);
			this.WriteDelta(offset, delta);
		}

		// Token: 0x060074EB RID: 29931 RVA: 0x00447240 File Offset: 0x00445440
		public unsafe static int RetrieveXiangshuInfection(byte* pModificationData)
		{
			return CharacterModificationRecords.ReadDeltaInt(pModificationData);
		}

		// Token: 0x060074EC RID: 29932 RVA: 0x00447258 File Offset: 0x00445458
		public void RecordCurrAge(short oriValue, short currValue)
		{
			int offset = this.BeginAddingRecord(RevertibleCharacterPropertyType.CurrAge, 4);
			this.WriteDelta(offset, (int)oriValue);
		}

		// Token: 0x060074ED RID: 29933 RVA: 0x0044727C File Offset: 0x0044547C
		public unsafe static int RetrieveCurrAge(byte* pModificationData)
		{
			return CharacterModificationRecords.ReadDeltaInt(pModificationData);
		}

		// Token: 0x060074EE RID: 29934 RVA: 0x00447294 File Offset: 0x00445494
		public void RecordHealth(short oriValue, short currValue)
		{
			int offset = this.BeginAddingRecord(RevertibleCharacterPropertyType.Health, 4);
			this.WriteDelta(offset, (int)oriValue);
		}

		// Token: 0x060074EF RID: 29935 RVA: 0x004472B8 File Offset: 0x004454B8
		public unsafe static int RetrieveHealth(byte* pModificationData)
		{
			return CharacterModificationRecords.ReadDeltaInt(pModificationData);
		}

		// Token: 0x060074F0 RID: 29936 RVA: 0x004472D0 File Offset: 0x004454D0
		public void CreateSavepoint()
		{
			bool flag = this.Savepoint >= 0;
			if (flag)
			{
				throw new Exception("Cannot create savepoint when there is already a savepoint");
			}
			this.Savepoint = this.Size;
		}

		// Token: 0x060074F1 RID: 29937 RVA: 0x00447308 File Offset: 0x00445508
		public void DeleteSavepoint()
		{
			bool flag = this.Savepoint < 0;
			if (flag)
			{
				throw new Exception("Cannot delete savepoint when there is no savepoint");
			}
			this.Savepoint = -1;
		}

		// Token: 0x060074F2 RID: 29938 RVA: 0x00447338 File Offset: 0x00445538
		[return: TupleElementNames(new string[]
		{
			"propertyType",
			"offset",
			"dataSize"
		})]
		public unsafe ValueTuple<RevertibleCharacterPropertyType, int, ushort> Pop(byte* pRawData)
		{
			byte* pEnd = pRawData + this.Size;
			ushort dataSize = *(ushort*)(pEnd - 2);
			this.Size -= (int)(1 + dataSize + 2);
			bool flag = this.Size < 0;
			if (flag)
			{
				throw new Exception("Index of RawData out of range");
			}
			return new ValueTuple<RevertibleCharacterPropertyType, int, ushort>((RevertibleCharacterPropertyType)pRawData[this.Size], this.Size + 1, dataSize);
		}

		// Token: 0x060074F3 RID: 29939 RVA: 0x0044739C File Offset: 0x0044559C
		private unsafe int BeginAddingRecord(RevertibleCharacterPropertyType propertyType, ushort dataSize)
		{
			int offset = this.Size;
			int newSize = this.Size + 1 + (int)dataSize + 2;
			base.EnsureCapacity(newSize);
			this.Size = newSize;
			byte[] array;
			byte* pRawData;
			if ((array = this.RawData) == null || array.Length == 0)
			{
				pRawData = null;
			}
			else
			{
				pRawData = &array[0];
			}
			byte* pCurrData = pRawData + offset;
			*(int*)pCurrData = (int)propertyType;
			*(short*)(pCurrData + 1 + dataSize) = (short)dataSize;
			array = null;
			return offset + 1;
		}

		// Token: 0x060074F4 RID: 29940 RVA: 0x00447408 File Offset: 0x00445608
		private unsafe void WriteDelta(int offset, int delta)
		{
			byte[] array;
			byte* pRawData;
			if ((array = this.RawData) == null || array.Length == 0)
			{
				pRawData = null;
			}
			else
			{
				pRawData = &array[0];
			}
			byte* pCurrData = pRawData + offset;
			*(int*)pCurrData = delta;
			array = null;
		}

		// Token: 0x060074F5 RID: 29941 RVA: 0x00447440 File Offset: 0x00445640
		private unsafe static int ReadDeltaInt(byte* pModificationData)
		{
			return *(int*)pModificationData;
		}

		// Token: 0x060074F6 RID: 29942 RVA: 0x00447454 File Offset: 0x00445654
		private static List<ShortListModification> CalcDelta(List<short> srcElements, List<short> destElements)
		{
			List<ShortListModification> modifications = new List<ShortListModification>();
			int srcIndex = 0;
			int destIndex = 0;
			int srcCount = srcElements.Count;
			int destCount = destElements.Count;
			short currIndex = 0;
			while (srcIndex < srcCount && destIndex < destCount)
			{
				short srcElement = srcElements[srcIndex];
				int index = destElements.IndexOf(srcElement, destIndex);
				bool flag = index == destIndex;
				if (flag)
				{
					srcIndex++;
					destIndex++;
					currIndex += 1;
				}
				else
				{
					bool flag2 = index < 0;
					if (flag2)
					{
						modifications.Add(new ShortListModification(1, currIndex, srcElement));
						srcIndex++;
					}
					else
					{
						for (int i = destIndex; i < index; i++)
						{
							List<ShortListModification> list = modifications;
							sbyte modificationType = 0;
							short num = currIndex;
							currIndex = num + 1;
							list.Add(new ShortListModification(modificationType, num, destElements[i]));
						}
						srcIndex++;
						destIndex = index + 1;
						currIndex += 1;
					}
				}
			}
			bool flag3 = srcIndex < srcCount;
			if (flag3)
			{
				for (int j = srcIndex; j < srcCount; j++)
				{
					modifications.Add(new ShortListModification(1, currIndex, srcElements[j]));
				}
			}
			else
			{
				bool flag4 = destIndex < destCount;
				if (flag4)
				{
					for (int k = destIndex; k < destCount; k++)
					{
						List<ShortListModification> list2 = modifications;
						sbyte modificationType2 = 0;
						short num2 = currIndex;
						currIndex = num2 + 1;
						list2.Add(new ShortListModification(modificationType2, num2, destElements[k]));
					}
				}
			}
			return modifications;
		}

		// Token: 0x060074F7 RID: 29943 RVA: 0x004475BC File Offset: 0x004457BC
		private static ushort CalcDataSize(List<ShortListModification> modifications)
		{
			int dataSize = ShortListModification.GetFixedSerializedSize() * modifications.Count;
			bool flag = dataSize > 65535;
			if (flag)
			{
				throw new Exception("Data size of modifications must be less than 64KB");
			}
			return (ushort)dataSize;
		}

		// Token: 0x060074F8 RID: 29944 RVA: 0x004475F4 File Offset: 0x004457F4
		private unsafe void WriteDelta(int offset, List<ShortListModification> modifications)
		{
			byte[] array;
			byte* pRawData;
			if ((array = this.RawData) == null || array.Length == 0)
			{
				pRawData = null;
			}
			else
			{
				pRawData = &array[0];
			}
			byte* pCurrData = pRawData + offset;
			int i = 0;
			int elementsCount = modifications.Count;
			while (i < elementsCount)
			{
				pCurrData += modifications[i].Serialize(pCurrData);
				i++;
			}
			array = null;
		}

		// Token: 0x060074F9 RID: 29945 RVA: 0x00447658 File Offset: 0x00445858
		private unsafe static List<ShortListModification> ReadDeltaShortList(byte* pModificationData, ushort dataSize)
		{
			int elementsCount = (int)dataSize / ShortListModification.GetFixedSerializedSize();
			Tester.Assert(ShortListModification.GetFixedSerializedSize() * elementsCount == (int)dataSize, "");
			List<ShortListModification> modifications = new List<ShortListModification>();
			byte* pCurrData = pModificationData;
			for (int i = 0; i < elementsCount; i++)
			{
				ShortListModification modification = default(ShortListModification);
				pCurrData += modification.Deserialize(pCurrData);
				modifications.Add(modification);
			}
			return modifications;
		}

		// Token: 0x060074FA RID: 29946 RVA: 0x004476C4 File Offset: 0x004458C4
		private unsafe void WriteDelta(int offset, MainAttributes delta)
		{
			byte[] array;
			byte* pRawData;
			if ((array = this.RawData) == null || array.Length == 0)
			{
				pRawData = null;
			}
			else
			{
				pRawData = &array[0];
			}
			byte* pCurrData = pRawData + offset;
			delta.Serialize(pCurrData);
			array = null;
		}

		// Token: 0x060074FB RID: 29947 RVA: 0x00447700 File Offset: 0x00445900
		private unsafe static MainAttributes ReadDeltaMainAttributes(byte* pModificationData)
		{
			MainAttributes delta = default(MainAttributes);
			delta.Deserialize(pModificationData);
			return delta;
		}

		// Token: 0x060074FC RID: 29948 RVA: 0x00447724 File Offset: 0x00445924
		private unsafe void WriteDelta(int offset, Injuries delta)
		{
			byte[] array;
			byte* pRawData;
			if ((array = this.RawData) == null || array.Length == 0)
			{
				pRawData = null;
			}
			else
			{
				pRawData = &array[0];
			}
			byte* pCurrData = pRawData + offset;
			delta.Serialize(pCurrData);
			array = null;
		}

		// Token: 0x060074FD RID: 29949 RVA: 0x00447760 File Offset: 0x00445960
		private unsafe static Injuries ReadDeltaInjuries(byte* pModificationData)
		{
			Injuries delta = default(Injuries);
			delta.Deserialize(pModificationData);
			return delta;
		}

		// Token: 0x060074FE RID: 29950 RVA: 0x00447784 File Offset: 0x00445984
		private unsafe void WriteDelta(int offset, ref LifeSkillShorts delta)
		{
			byte[] array;
			byte* pRawData;
			if ((array = this.RawData) == null || array.Length == 0)
			{
				pRawData = null;
			}
			else
			{
				pRawData = &array[0];
			}
			byte* pCurrData = pRawData + offset;
			delta.Serialize(pCurrData);
			array = null;
		}

		// Token: 0x060074FF RID: 29951 RVA: 0x004477C0 File Offset: 0x004459C0
		private unsafe static LifeSkillShorts ReadDeltaLifeSkillShorts(byte* pModificationData)
		{
			LifeSkillShorts delta = default(LifeSkillShorts);
			delta.Deserialize(pModificationData);
			return delta;
		}

		// Token: 0x06007500 RID: 29952 RVA: 0x004477E4 File Offset: 0x004459E4
		private unsafe void WriteDelta(int offset, ref CombatSkillShorts delta)
		{
			byte[] array;
			byte* pRawData;
			if ((array = this.RawData) == null || array.Length == 0)
			{
				pRawData = null;
			}
			else
			{
				pRawData = &array[0];
			}
			byte* pCurrData = pRawData + offset;
			delta.Serialize(pCurrData);
			array = null;
		}

		// Token: 0x06007501 RID: 29953 RVA: 0x00447820 File Offset: 0x00445A20
		private unsafe static CombatSkillShorts ReadDeltaCombatSkillShorts(byte* pModificationData)
		{
			CombatSkillShorts delta = default(CombatSkillShorts);
			delta.Deserialize(pModificationData);
			return delta;
		}

		// Token: 0x06007502 RID: 29954 RVA: 0x00447844 File Offset: 0x00445A44
		private unsafe void WriteDelta(int offset, ref ResourceInts delta)
		{
			byte[] array;
			byte* pRawData;
			if ((array = this.RawData) == null || array.Length == 0)
			{
				pRawData = null;
			}
			else
			{
				pRawData = &array[0];
			}
			byte* pCurrData = pRawData + offset;
			delta.Serialize(pCurrData);
			array = null;
		}

		// Token: 0x06007503 RID: 29955 RVA: 0x00447880 File Offset: 0x00445A80
		private unsafe static ResourceInts ReadDeltaResourceInts(byte* pModificationData)
		{
			ResourceInts delta = default(ResourceInts);
			delta.Deserialize(pModificationData);
			return delta;
		}

		// Token: 0x06007504 RID: 29956 RVA: 0x004478A4 File Offset: 0x00445AA4
		private unsafe void WriteDelta(int offset, ref PoisonInts delta)
		{
			byte[] array;
			byte* pRawData;
			if ((array = this.RawData) == null || array.Length == 0)
			{
				pRawData = null;
			}
			else
			{
				pRawData = &array[0];
			}
			byte* pCurrData = pRawData + offset;
			delta.Serialize(pCurrData);
			array = null;
		}

		// Token: 0x06007505 RID: 29957 RVA: 0x004478E0 File Offset: 0x00445AE0
		private unsafe static PoisonInts ReadDeltaPoisonInts(byte* pModificationData)
		{
			PoisonInts delta = default(PoisonInts);
			delta.Deserialize(pModificationData);
			return delta;
		}

		// Token: 0x06007506 RID: 29958 RVA: 0x00447904 File Offset: 0x00445B04
		private unsafe void WriteDelta(int offset, NeiliAllocation delta)
		{
			byte[] array;
			byte* pRawData;
			if ((array = this.RawData) == null || array.Length == 0)
			{
				pRawData = null;
			}
			else
			{
				pRawData = &array[0];
			}
			byte* pCurrData = pRawData + offset;
			delta.Serialize(pCurrData);
			array = null;
		}

		// Token: 0x06007507 RID: 29959 RVA: 0x00447940 File Offset: 0x00445B40
		private unsafe static NeiliAllocation ReadDeltaNeiliAllocation(byte* pModificationData)
		{
			NeiliAllocation delta = default(NeiliAllocation);
			delta.Deserialize(pModificationData);
			return delta;
		}

		// Token: 0x04001EF1 RID: 7921
		public int Savepoint;
	}
}
