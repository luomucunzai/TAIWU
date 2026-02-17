using System;

namespace GameData.ArchiveData
{
	// Token: 0x02000903 RID: 2307
	public static class OperationConstants
	{
		// Token: 0x02000D31 RID: 3377
		public static class OperationType
		{
			// Token: 0x0400374A RID: 14154
			public const byte Read = 0;

			// Token: 0x0400374B RID: 14155
			public const byte Write = 1;

			// Token: 0x0400374C RID: 14156
			public const byte Control = 2;
		}

		// Token: 0x02000D32 RID: 3378
		public static class FixedSingleValueMethodIds
		{
			// Token: 0x0400374D RID: 14157
			public const byte Set = 0;

			// Token: 0x0400374E RID: 14158
			public const byte Get = 1;
		}

		// Token: 0x02000D33 RID: 3379
		public static class DynamicSingleValueMethodIds
		{
			// Token: 0x0400374F RID: 14159
			public const byte Set = 0;

			// Token: 0x04003750 RID: 14160
			public const byte Get = 1;
		}

		// Token: 0x02000D34 RID: 3380
		public static class FixedSingleValueCollectionMethodIds
		{
			// Token: 0x04003751 RID: 14161
			public const byte Add = 0;

			// Token: 0x04003752 RID: 14162
			public const byte Set = 1;

			// Token: 0x04003753 RID: 14163
			public const byte Get = 2;

			// Token: 0x04003754 RID: 14164
			public const byte Remove = 3;

			// Token: 0x04003755 RID: 14165
			public const byte Clear = 4;

			// Token: 0x04003756 RID: 14166
			public const byte GetAll = 5;
		}

		// Token: 0x02000D35 RID: 3381
		public static class DynamicSingleValueCollectionMethodIds
		{
			// Token: 0x04003757 RID: 14167
			public const byte Add = 0;

			// Token: 0x04003758 RID: 14168
			public const byte Set = 1;

			// Token: 0x04003759 RID: 14169
			public const byte Get = 2;

			// Token: 0x0400375A RID: 14170
			public const byte Remove = 3;

			// Token: 0x0400375B RID: 14171
			public const byte Clear = 4;

			// Token: 0x0400375C RID: 14172
			public const byte GetAll = 5;
		}

		// Token: 0x02000D36 RID: 3382
		public static class FixedElementListMethodIds
		{
			// Token: 0x0400375D RID: 14173
			public const byte GetCount = 0;

			// Token: 0x0400375E RID: 14174
			public const byte Set = 1;

			// Token: 0x0400375F RID: 14175
			public const byte Get = 2;

			// Token: 0x04003760 RID: 14176
			public const byte Insert = 3;

			// Token: 0x04003761 RID: 14177
			public const byte InsertRange = 4;

			// Token: 0x04003762 RID: 14178
			public const byte RemoveAt = 5;

			// Token: 0x04003763 RID: 14179
			public const byte RemoveRange = 6;

			// Token: 0x04003764 RID: 14180
			public const byte Clear = 7;

			// Token: 0x04003765 RID: 14181
			public const byte GetAll = 8;
		}

		// Token: 0x02000D37 RID: 3383
		public static class DynamicElementListMethodIds
		{
			// Token: 0x04003766 RID: 14182
			public const byte GetCount = 0;

			// Token: 0x04003767 RID: 14183
			public const byte Set = 1;

			// Token: 0x04003768 RID: 14184
			public const byte Get = 2;

			// Token: 0x04003769 RID: 14185
			public const byte Insert = 3;

			// Token: 0x0400376A RID: 14186
			public const byte InsertRange = 4;

			// Token: 0x0400376B RID: 14187
			public const byte RemoveAt = 5;

			// Token: 0x0400376C RID: 14188
			public const byte RemoveRange = 6;

			// Token: 0x0400376D RID: 14189
			public const byte Clear = 7;

			// Token: 0x0400376E RID: 14190
			public const byte GetAll = 8;
		}

		// Token: 0x02000D38 RID: 3384
		public static class FixedObjectCollectionMethodIds
		{
			// Token: 0x0400376F RID: 14191
			public const byte Add = 0;

			// Token: 0x04003770 RID: 14192
			public const byte Get = 1;

			// Token: 0x04003771 RID: 14193
			public const byte GetList = 2;

			// Token: 0x04003772 RID: 14194
			public const byte Remove = 3;

			// Token: 0x04003773 RID: 14195
			public const byte Clear = 4;

			// Token: 0x04003774 RID: 14196
			public const byte SetFixedField = 5;

			// Token: 0x04003775 RID: 14197
			public const byte GetFixedField = 6;

			// Token: 0x04003776 RID: 14198
			public const byte GetAllIds = 7;

			// Token: 0x04003777 RID: 14199
			public const byte GetAllObjects = 8;
		}

		// Token: 0x02000D39 RID: 3385
		public static class DynamicObjectCollectionMethodIds
		{
			// Token: 0x04003778 RID: 14200
			public const byte Add = 0;

			// Token: 0x04003779 RID: 14201
			public const byte Get = 1;

			// Token: 0x0400377A RID: 14202
			public const byte GetList = 2;

			// Token: 0x0400377B RID: 14203
			public const byte Remove = 3;

			// Token: 0x0400377C RID: 14204
			public const byte Clear = 4;

			// Token: 0x0400377D RID: 14205
			public const byte SetFixedField = 5;

			// Token: 0x0400377E RID: 14206
			public const byte GetFixedField = 6;

			// Token: 0x0400377F RID: 14207
			public const byte SetDynamicField = 7;

			// Token: 0x04003780 RID: 14208
			public const byte GetDynamicField = 8;

			// Token: 0x04003781 RID: 14209
			public const byte GetAllIds = 9;

			// Token: 0x04003782 RID: 14210
			public const byte GetAllObjects = 10;
		}

		// Token: 0x02000D3A RID: 3386
		public static class BinaryMethodIds
		{
			// Token: 0x04003783 RID: 14211
			public const byte Insert = 0;

			// Token: 0x04003784 RID: 14212
			public const byte Write = 1;

			// Token: 0x04003785 RID: 14213
			public const byte Remove = 2;

			// Token: 0x04003786 RID: 14214
			public const byte SetMetadata = 3;

			// Token: 0x04003787 RID: 14215
			public const byte Get = 4;
		}

		// Token: 0x02000D3B RID: 3387
		public static class GlobalMethodIds
		{
			// Token: 0x04003788 RID: 14216
			public const byte FreeMemory = 0;

			// Token: 0x04003789 RID: 14217
			public const byte EnterNewWorld = 1;

			// Token: 0x0400378A RID: 14218
			public const byte LoadWorld = 2;

			// Token: 0x0400378B RID: 14219
			public const byte SaveWorld = 3;

			// Token: 0x0400378C RID: 14220
			public const byte LeaveWorld = 4;

			// Token: 0x0400378D RID: 14221
			public const byte GetArchivesInfo = 5;

			// Token: 0x0400378E RID: 14222
			public const byte DeleteArchive = 6;

			// Token: 0x0400378F RID: 14223
			public const byte SaveEnding = 7;

			// Token: 0x04003790 RID: 14224
			public const byte LoadEnding = 8;

			// Token: 0x04003791 RID: 14225
			public const byte GetEndingArchiveInfo = 9;
		}

		// Token: 0x02000D3C RID: 3388
		public static class LifeRecordMethodIds
		{
			// Token: 0x04003792 RID: 14226
			public const byte Add = 0;

			// Token: 0x04003793 RID: 14227
			public const byte Get = 1;

			// Token: 0x04003794 RID: 14228
			public const byte GetByDate = 2;

			// Token: 0x04003795 RID: 14229
			public const byte GetLast = 3;

			// Token: 0x04003796 RID: 14230
			public const byte Search = 4;

			// Token: 0x04003797 RID: 14231
			public const byte Remove = 5;

			// Token: 0x04003798 RID: 14232
			public const byte GenerateDead = 6;

			// Token: 0x04003799 RID: 14233
			public const byte GetDead = 7;

			// Token: 0x0400379A RID: 14234
			public const byte RemoveDead = 8;

			// Token: 0x0400379B RID: 14235
			public const byte GetAllByChar = 11;
		}
	}
}
