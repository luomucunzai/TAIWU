namespace GameData.Utilities;

public static class SelectionUtils
{
	public unsafe static int SelectTopK(int* pList, int count, int topK)
	{
		if (topK == 1)
		{
			return SelectTop1(pList, count);
		}
		int num = 0;
		int num2 = count - 1;
		int num3 = count - topK;
		while (true)
		{
			if (num == num2)
			{
				return num;
			}
			int pivotIndex = (num + num2) / 2;
			pivotIndex = Partition(pList, num, num2, pivotIndex);
			if (pivotIndex == num3)
			{
				break;
			}
			if (pivotIndex < num3)
			{
				num = pivotIndex + 1;
			}
			else
			{
				num2 = pivotIndex - 1;
			}
		}
		return num3;
	}

	public unsafe static int SelectTop1(int* pList, int count)
	{
		int num = int.MinValue;
		int result = 0;
		for (int i = 0; i < count; i++)
		{
			int num2 = pList[i];
			if (num2 > num)
			{
				num = num2;
				result = i;
			}
		}
		return result;
	}

	private unsafe static int GetMedianOfThree(int* pList, int leftIdx, int rightIdx)
	{
		int num = (leftIdx + rightIdx) / 2;
		int num2 = pList[leftIdx];
		int num3 = pList[num];
		int num4 = pList[rightIdx];
		if (num3 > num2 != num3 > num4)
		{
			return num;
		}
		if (num4 > num2 != num4 > num3)
		{
			return rightIdx;
		}
		return leftIdx;
	}

	private unsafe static int Partition(int* pList, int left, int right, int pivotIndex)
	{
		int num = pList[pivotIndex];
		pList[pivotIndex] = pList[right];
		pList[right] = num;
		int num2 = left;
		for (int i = left; i < right; i++)
		{
			int num3 = pList[i];
			if (num3 < num)
			{
				pList[i] = pList[num2];
				pList[num2] = num3;
				num2++;
			}
		}
		pList[right] = pList[num2];
		pList[num2] = num;
		return num2;
	}
}
