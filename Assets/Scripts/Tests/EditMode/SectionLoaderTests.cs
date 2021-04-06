using FarmSim.Grid;
using NUnit.Framework;
using UnityEngine;

namespace Tests
{
    public class SectionLoaderTests
    {
        [Test]
        public void SectionDimensionsTest()
        {
            AssertGridDims(0);
            AssertGridDims(1);
            AssertGridDims(2);
            AssertGridDims(3);
            AssertGridDims(4);
        }

        private void AssertGridDims(int sectionNum)
        {
            SectionLoader sectionLoader = new SectionLoader(Vector2.zero, sectionNum, null);
            var grid = sectionLoader.InitGrid();

            Assert.AreEqual(grid.GetLength(0), SectionLoader.SECTION_SIZE_Y);
            Assert.AreEqual(grid.GetLength(1), SectionLoader.SECTION_SIZE_X);
        }
    }
}
