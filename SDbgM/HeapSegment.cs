﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SDbgCore;

namespace SDbgM
{
    public struct HeapSegment
    {
        private ulong _segAddr;
        private ClrGcHeapSegmentData _data;

        internal HeapSegment(ulong segAddr, ClrGcHeapSegmentData data)
        {
            _segAddr = segAddr;
            _data = data;
        }

        public ulong Segment { get { return _segAddr; } }
        public ClrGcHeapSegmentData Data { get { return _data; } }
    }
}