using NUnit.Framework;

namespace Tests
{
    public class HeapTests
    {
        public class MaxHeapInt : IHeapItem<MaxHeapInt>
        {
            public int HeapIndex { get; set; }

            public int num = 0;

            public MaxHeapInt(int num)
            {
                this.num = num;
            }

            public int CompareTo(MaxHeapInt other)
            {
                return num.CompareTo(other.num);
            }
        }

        public class MinHeapInt : IHeapItem<MinHeapInt>
        {
            public int HeapIndex { get; set; }

            public int num = 0;

            public MinHeapInt(int num)
            {
                this.num = num;
            }

            public int CompareTo(MinHeapInt other)
            {
                return -num.CompareTo(other.num);
            }
        }


        [Test]
        public void HeapSortingIntTest()
        {
            Heap<MaxHeapInt> maxHeap = new Heap<MaxHeapInt>(10);
            Heap<MinHeapInt> minHeap = new Heap<MinHeapInt>(10);

            maxHeap.Add(new MaxHeapInt(5));
            maxHeap.Add(new MaxHeapInt(-10));
            maxHeap.Add(new MaxHeapInt(15));
            maxHeap.Add(new MaxHeapInt(-11));
            maxHeap.Add(new MaxHeapInt(-1));
            maxHeap.Add(new MaxHeapInt(1));
            maxHeap.Add(new MaxHeapInt(1));
            maxHeap.Add(new MaxHeapInt(8));
            maxHeap.Add(new MaxHeapInt(91));
            maxHeap.Add(new MaxHeapInt(-42));


            minHeap.Add(new MinHeapInt(5));
            minHeap.Add(new MinHeapInt(-10));
            minHeap.Add(new MinHeapInt(15));
            minHeap.Add(new MinHeapInt(-11));
            minHeap.Add(new MinHeapInt(-1));
            minHeap.Add(new MinHeapInt(1));
            minHeap.Add(new MinHeapInt(1));
            minHeap.Add(new MinHeapInt(8));
            minHeap.Add(new MinHeapInt(91));
            minHeap.Add(new MinHeapInt(-42));

            int lastItem = int.MinValue;

            while (minHeap.Count > 0)
            {
                MinHeapInt removed = minHeap.RemoveFirst();
                Assert.LessOrEqual(lastItem, removed.num);
                lastItem = removed.num;
            }

            lastItem = int.MaxValue;

            while (maxHeap.Count > 0)
            {
                MaxHeapInt removed = maxHeap.RemoveFirst();
                Assert.GreaterOrEqual(lastItem, removed.num);
                lastItem = removed.num;
            }
        }

        [Test]
        public void HeapAddTest()
        {
            Heap<MaxHeapInt> heap = new Heap<MaxHeapInt>(10);
            MaxHeapInt item = new MaxHeapInt(5);
            heap.Add(new MaxHeapInt(5));
            heap.Add(new MaxHeapInt(-10));
            heap.Add(new MaxHeapInt(15));
            heap.Add(new MaxHeapInt(-11));
            heap.Add(new MaxHeapInt(-1));
            heap.Add(new MaxHeapInt(1));
            heap.Add(item);

            Assert.IsTrue(heap.Contains(item));
        }

        [Test]
        public void RemoveFirstTest()
        {
            Heap<MaxHeapInt> maxHeap = new Heap<MaxHeapInt>(10);
            Heap<MinHeapInt> minHeap = new Heap<MinHeapInt>(10);

            MaxHeapInt maxItem = new MaxHeapInt(20);
            MinHeapInt minItem = new MinHeapInt(-42);

            maxHeap.Add(new MaxHeapInt(5));
            maxHeap.Add(new MaxHeapInt(-10));
            maxHeap.Add(new MaxHeapInt(15));
            maxHeap.Add(new MaxHeapInt(-11));
            maxHeap.Add(new MaxHeapInt(-1));
            maxHeap.Add(new MaxHeapInt(1));
            maxHeap.Add(maxItem);

            minHeap.Add(new MinHeapInt(1));
            minHeap.Add(new MinHeapInt(-1));
            minHeap.Add(new MinHeapInt(-8));
            minHeap.Add(new MinHeapInt(91));
            minHeap.Add(minItem);

            Assert.AreEqual(maxHeap.RemoveFirst(), maxItem);
            Assert.AreEqual(minHeap.RemoveFirst(), minItem);
        }

        [Test]
        public void ContainsTest()
        {
            Heap<MaxHeapInt> heap = new Heap<MaxHeapInt>(10);
            var item = new MaxHeapInt(5);
            var item_2 = new MaxHeapInt(-10);
            var item_3 = new MaxHeapInt(15);
            var item_4 = new MaxHeapInt(-11);

            heap.Add(item);
            heap.Add(item_2);
            heap.Add(item_3);
            heap.Add(item_4);

            Assert.IsTrue(heap.Contains(item));
            Assert.IsTrue(heap.Contains(item_2));
            Assert.IsTrue(heap.Contains(item_3));
            Assert.IsTrue(heap.Contains(item_4));
        }
    }
}
