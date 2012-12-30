#include "stdafx.h"
#include "CppUnitTest.h"
#include "TestCommon.h"

using namespace Microsoft::VisualStudio::CppUnitTestFramework;

namespace SDbgExt2Tests2
{
	TEST_CLASS(ServerHeapTests)
	{
	public:
#ifndef _WIN64
		ADD_TEST_INIT(L"..\\dumps\\x86\\iis_small.dmp")
#else
		ADD_TEST_INIT(L"..\\..\\dumps\\x64\\iis_small_2.dmp")
#endif

		TEST_METHOD(ClrGcHeapList_Server)
		{
			auto proc = p->GetProcess();

			ClrGcHeapData gcData = {};
			proc->GetGCHeapData(&gcData);

			Assert::IsTrue(gcData.ServerMode == 1);
			Assert::AreEqual((UINT)8, gcData.HeapCount);

			std::vector<CLRDATA_ADDRESS> heaps(gcData.HeapCount);
			auto hr = proc->GetGCHeapList(gcData.HeapCount, heaps.data(), 0);			

			ASSERT_SOK(hr);		
		}

		TEST_METHOD(ClrGcHeapData_Server)
		{
			auto proc = p->GetProcess();

			ClrGcHeapData gcData = {};
			proc->GetGCHeapData(&gcData);

			std::vector<CLRDATA_ADDRESS> heaps(gcData.HeapCount);
			auto hr = proc->GetGCHeapList(gcData.HeapCount, heaps.data(), 0);	

			ClrGcHeapStaticData gchData = {};
			hr = proc->GetGCHeapDetails(heaps[0], &gchData);

			ASSERT_SOK(hr);
		}

		TEST_METHOD(ClrGcHeap_EnumerateObjects_Server)
		{
			int n = 0;

			auto cb = [](CLRDATA_ADDRESS object, ClrObjectData objData, PVOID state)->BOOL {
				(*((int*)state))++;
				return TRUE;
			};
			
			p->EnumHeapObjects(cb, &n);

			Assert::AreEqual(BITNESS_CONDITIONAL(50475, 52100), n);
		}

	};
}