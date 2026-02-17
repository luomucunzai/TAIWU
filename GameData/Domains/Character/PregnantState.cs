using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using GameData.Serializer;
using GameData.Utilities;
using Redzen.Random;

namespace GameData.Domains.Character
{
	// Token: 0x02000817 RID: 2071
	[SerializableGameData(NotForDisplayModule = true)]
	public class PregnantState : ISerializableGameData
	{
		// Token: 0x060074C0 RID: 29888 RVA: 0x00446778 File Offset: 0x00444978
		public unsafe PregnantState(Character mother, Character father, bool isRaped)
		{
			this.MotherFeatureIds = new List<short>(mother.GetFeatureIds());
			this.CreateMotherRelation = true;
			bool flag = father != null;
			if (flag)
			{
				this.CreateFatherRelation = !isRaped;
				this.FatherId = father.GetId();
				this.FatherFeatureIds = new List<short>(father.GetFeatureIds());
				this.FatherGenome = *father.GetGenome();
			}
			else
			{
				this.CreateFatherRelation = false;
				this.FatherId = -1;
			}
		}

		// Token: 0x060074C1 RID: 29889 RVA: 0x004467FC File Offset: 0x004449FC
		public static bool CheckPregnant(IRandomSource random, Character father, Character mother, bool isRape)
		{
			sbyte baseFertility = isRape ? 20 : 60;
			int fatherId = father.GetId();
			int motherId = mother.GetId();
			bool flag = father.GetGender() == mother.GetGender();
			bool result;
			if (flag)
			{
				result = false;
			}
			else
			{
				bool flag2 = mother.GetFeatureIds().Contains(197);
				if (flag2)
				{
					result = false;
				}
				else
				{
					int num;
					bool flag3 = DomainManager.Character.TryGetElement_PregnancyLockEndDates(motherId, out num);
					if (flag3)
					{
						result = false;
					}
					else
					{
						int taiwuCharId = DomainManager.Taiwu.GetTaiwuCharId();
						bool anyTaiwu = motherId == taiwuCharId || fatherId == taiwuCharId;
						int divisor = anyTaiwu ? 20 : 40;
						short fatherFertility = father.GetFertility();
						int fatherChildCount = DomainManager.Character.GetRelatedCharIds(fatherId, 2).Count;
						bool flag4 = (int)fatherFertility / divisor < fatherChildCount;
						if (flag4)
						{
							result = false;
						}
						else
						{
							short motherFertility = mother.GetFertility();
							int motherChildCount = DomainManager.Character.GetRelatedCharIds(motherId, 2).Count;
							bool flag5 = (int)motherFertility / divisor < motherChildCount;
							if (flag5)
							{
								result = false;
							}
							else
							{
								bool flag6 = anyTaiwu;
								if (flag6)
								{
									result = random.CheckPercentProb((int)((short)baseFertility * fatherFertility * motherFertility / 10000));
								}
								else
								{
									int chance = (int)((short)baseFertility * fatherFertility * motherFertility / 10000);
									bool flag7 = father.GetOrganizationInfo().OrgTemplateId == 16 || mother.GetOrganizationInfo().OrgTemplateId == 16;
									if (flag7)
									{
										int homelessCount = DomainManager.Building.GetHomeless().GetCount();
										bool flag8 = homelessCount > 0;
										if (flag8)
										{
											chance /= 10 * homelessCount;
										}
									}
									result = random.CheckPercentProb(chance);
								}
							}
						}
					}
				}
			}
			return result;
		}

		// Token: 0x060074C2 RID: 29890 RVA: 0x00446990 File Offset: 0x00444B90
		public override string ToString()
		{
			DefaultInterpolatedStringHandler defaultInterpolatedStringHandler = new DefaultInterpolatedStringHandler(40, 3);
			defaultInterpolatedStringHandler.AppendLiteral("{{Father:");
			defaultInterpolatedStringHandler.AppendFormatted<int>(this.FatherId);
			defaultInterpolatedStringHandler.AppendLiteral(", ExpectedBirthDate:");
			defaultInterpolatedStringHandler.AppendFormatted<int>(this.ExpectedBirthDate);
			defaultInterpolatedStringHandler.AppendLiteral(", IsHuman:");
			defaultInterpolatedStringHandler.AppendFormatted<bool>(this.IsHuman);
			defaultInterpolatedStringHandler.AppendLiteral("}");
			return defaultInterpolatedStringHandler.ToStringAndClear();
		}

		// Token: 0x060074C3 RID: 29891 RVA: 0x00446A11 File Offset: 0x00444C11
		public PregnantState()
		{
		}

		// Token: 0x060074C4 RID: 29892 RVA: 0x00446A1C File Offset: 0x00444C1C
		public bool IsSerializedSizeFixed()
		{
			return false;
		}

		// Token: 0x060074C5 RID: 29893 RVA: 0x00446A30 File Offset: 0x00444C30
		public int GetSerializedSize()
		{
			int totalSize = 75;
			bool flag = this.MotherFeatureIds != null;
			if (flag)
			{
				totalSize += 1 + 2 * this.MotherFeatureIds.Count;
			}
			else
			{
				totalSize++;
			}
			bool flag2 = this.FatherFeatureIds != null;
			if (flag2)
			{
				totalSize += 1 + 2 * this.FatherFeatureIds.Count;
			}
			else
			{
				totalSize++;
			}
			return (totalSize <= 4) ? totalSize : ((totalSize + 3) / 4 * 4);
		}

		// Token: 0x060074C6 RID: 29894 RVA: 0x00446AA4 File Offset: 0x00444CA4
		public unsafe int Serialize(byte* pData)
		{
			bool flag = this.MotherFeatureIds != null;
			byte* pCurrData;
			if (flag)
			{
				int elementsCount = this.MotherFeatureIds.Count;
				Tester.Assert(elementsCount <= 255, "");
				*pData = (byte)elementsCount;
				pCurrData = pData + 1;
				for (int i = 0; i < elementsCount; i++)
				{
					*(short*)(pCurrData + (IntPtr)i * 2) = this.MotherFeatureIds[i];
				}
				pCurrData += 2 * elementsCount;
			}
			else
			{
				*pData = 0;
				pCurrData = pData + 1;
			}
			*pCurrData = (this.CreateMotherRelation ? 1 : 0);
			pCurrData++;
			*pCurrData = (this.CreateFatherRelation ? 1 : 0);
			pCurrData++;
			*(int*)pCurrData = this.FatherId;
			pCurrData += 4;
			bool flag2 = this.FatherFeatureIds != null;
			if (flag2)
			{
				int elementsCount2 = this.FatherFeatureIds.Count;
				Tester.Assert(elementsCount2 <= 255, "");
				*pCurrData = (byte)elementsCount2;
				pCurrData++;
				for (int j = 0; j < elementsCount2; j++)
				{
					*(short*)(pCurrData + (IntPtr)j * 2) = this.FatherFeatureIds[j];
				}
				pCurrData += 2 * elementsCount2;
			}
			else
			{
				*pCurrData = 0;
				pCurrData++;
			}
			pCurrData += this.FatherGenome.Serialize(pCurrData);
			*(int*)pCurrData = this.ExpectedBirthDate;
			pCurrData += 4;
			*pCurrData = (this.IsHuman ? 1 : 0);
			pCurrData++;
			int totalSize = (int)((long)(pCurrData - pData));
			return (totalSize <= 4) ? totalSize : ((totalSize + 3) / 4 * 4);
		}

		// Token: 0x060074C7 RID: 29895 RVA: 0x00446C14 File Offset: 0x00444E14
		public unsafe int Deserialize(byte* pData)
		{
			byte elementsCount = *pData;
			byte* pCurrData = pData + 1;
			bool flag = elementsCount > 0;
			if (flag)
			{
				bool flag2 = this.MotherFeatureIds == null;
				if (flag2)
				{
					this.MotherFeatureIds = new List<short>((int)elementsCount);
				}
				else
				{
					this.MotherFeatureIds.Clear();
				}
				for (int i = 0; i < (int)elementsCount; i++)
				{
					this.MotherFeatureIds.Add(*(short*)(pCurrData + (IntPtr)i * 2));
				}
				pCurrData += 2 * elementsCount;
			}
			else
			{
				List<short> motherFeatureIds = this.MotherFeatureIds;
				if (motherFeatureIds != null)
				{
					motherFeatureIds.Clear();
				}
			}
			this.CreateMotherRelation = (*pCurrData != 0);
			pCurrData++;
			this.CreateFatherRelation = (*pCurrData != 0);
			pCurrData++;
			this.FatherId = *(int*)pCurrData;
			pCurrData += 4;
			byte elementsCount2 = *pCurrData;
			pCurrData++;
			bool flag3 = elementsCount2 > 0;
			if (flag3)
			{
				bool flag4 = this.FatherFeatureIds == null;
				if (flag4)
				{
					this.FatherFeatureIds = new List<short>((int)elementsCount2);
				}
				else
				{
					this.FatherFeatureIds.Clear();
				}
				for (int j = 0; j < (int)elementsCount2; j++)
				{
					this.FatherFeatureIds.Add(*(short*)(pCurrData + (IntPtr)j * 2));
				}
				pCurrData += 2 * elementsCount2;
			}
			else
			{
				List<short> fatherFeatureIds = this.FatherFeatureIds;
				if (fatherFeatureIds != null)
				{
					fatherFeatureIds.Clear();
				}
			}
			pCurrData += this.FatherGenome.Deserialize(pCurrData);
			this.ExpectedBirthDate = *(int*)pCurrData;
			pCurrData += 4;
			this.IsHuman = (*pCurrData != 0);
			pCurrData++;
			int totalSize = (int)((long)(pCurrData - pData));
			return (totalSize <= 4) ? totalSize : ((totalSize + 3) / 4 * 4);
		}

		// Token: 0x04001EE9 RID: 7913
		[SerializableGameDataField(CollectionMaxElementsCount = 255)]
		public List<short> MotherFeatureIds;

		// Token: 0x04001EEA RID: 7914
		[SerializableGameDataField]
		public bool CreateMotherRelation;

		// Token: 0x04001EEB RID: 7915
		[SerializableGameDataField]
		public bool CreateFatherRelation;

		// Token: 0x04001EEC RID: 7916
		[SerializableGameDataField]
		public int FatherId;

		// Token: 0x04001EED RID: 7917
		[SerializableGameDataField(CollectionMaxElementsCount = 255)]
		public List<short> FatherFeatureIds;

		// Token: 0x04001EEE RID: 7918
		[SerializableGameDataField]
		public Genome FatherGenome;

		// Token: 0x04001EEF RID: 7919
		[SerializableGameDataField]
		public int ExpectedBirthDate;

		// Token: 0x04001EF0 RID: 7920
		[SerializableGameDataField]
		public bool IsHuman;
	}
}
