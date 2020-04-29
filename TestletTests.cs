using NUnit.Framework;
using StraszAssessment;
using System.Collections.Generic;
using System.Linq;

namespace StraszAssessmentTests
{
    public class TestletTests
    {      
        [TestFixture]
        public class RandomizeTests
        {
            private Testlet sut;

            [SetUp]
            public void Setup()
            {
                var items = new List<Item>() { 
                    new Item { ItemId = "0", ItemType = ItemTypeEnum.Operational }, 
                    new Item { ItemId = "1", ItemType = ItemTypeEnum.Operational }, 
                    new Item { ItemId = "2", ItemType = ItemTypeEnum.Pretest },
                    new Item { ItemId = "3", ItemType = ItemTypeEnum.Pretest }, 
                    new Item { ItemId = "4", ItemType = ItemTypeEnum.Pretest }, 
                    new Item { ItemId = "5", ItemType = ItemTypeEnum.Pretest }, 
                    new Item { ItemId = "6", ItemType = ItemTypeEnum.Operational }, 
                    new Item { ItemId = "7", ItemType = ItemTypeEnum.Operational }, 
                    new Item { ItemId = "8", ItemType = ItemTypeEnum.Operational }, 
                    new Item { ItemId = "9", ItemType = ItemTypeEnum.Operational }                
                };
                sut = new Testlet(string.Empty, items);
            }

            [Test]
            public void RandomizeReturnsTenUniqueItems()
            {
                var result = sut.Randomize();
                var itemIds = result.Select(o => o.ItemId).Distinct();
                Assert.AreEqual(10, itemIds.Count());                
            }
          
            [Test]
            public void FirstTwoItemsArePretest()
            {
               var result = sut.Randomize();
               Assert.IsTrue(result.Take(2).All(o => o.ItemType == ItemTypeEnum.Pretest)); 
            }
         
            [Test]
            public void RemainingEightItemsHaveSixOperationalItems()
            {
                var result = sut.Randomize();
                Assert.IsTrue(result.Skip(2).Count(o => o.ItemType == ItemTypeEnum.Operational) == 6);
            }

            [Test]
            public void RemainingEightItemsHaveTwoPretestItems()
            {
                var result = sut.Randomize();
                Assert.IsTrue(result.Skip(2).Count(o => o.ItemType == ItemTypeEnum.Pretest) == 2);
            }
        }

        [TestFixture]
        public class RandomizationTests
        {
            public class TestletRerverse : Testlet.ITestletRandomizer
            {
                public List<Item> RandomizeItemList(IEnumerable<Item> items)
                {
                    return items.OrderBy(o => o.ItemId).Reverse().ToList();
                }
            }

            private Testlet sut;
            private List<Item> result;

            [SetUp]
            public void Setup()
            {
                var items = new List<Item>() {
                    new Item { ItemId = "0", ItemType = ItemTypeEnum.Operational },
                    new Item { ItemId = "1", ItemType = ItemTypeEnum.Operational },
                    new Item { ItemId = "2", ItemType = ItemTypeEnum.Pretest },
                    new Item { ItemId = "3", ItemType = ItemTypeEnum.Pretest },
                    new Item { ItemId = "4", ItemType = ItemTypeEnum.Pretest },
                    new Item { ItemId = "5", ItemType = ItemTypeEnum.Pretest },
                    new Item { ItemId = "6", ItemType = ItemTypeEnum.Operational },
                    new Item { ItemId = "7", ItemType = ItemTypeEnum.Operational },
                    new Item { ItemId = "8", ItemType = ItemTypeEnum.Operational },
                    new Item { ItemId = "9", ItemType = ItemTypeEnum.Operational }
                };
                sut = new Testlet(string.Empty, items, new TestletRerverse());
                result = sut.Randomize();
            }

            [Test]
            public void FirstItemsAreFiveAndFour()
            {
                Assert.AreEqual("5", result[0].ItemId);
                Assert.AreEqual("4", result[1].ItemId);
            }

            [Test]
            public void RemainingItemsAreInDescendingOrderWithoutFiveAndFour()
            {
                Assert.AreEqual("9", result[2].ItemId);
                Assert.AreEqual("8", result[3].ItemId);
                Assert.AreEqual("7", result[4].ItemId);
                Assert.AreEqual("6", result[5].ItemId);
                Assert.AreEqual("3", result[6].ItemId);
                Assert.AreEqual("2", result[7].ItemId);
                Assert.AreEqual("1", result[8].ItemId);
                Assert.AreEqual("0", result[9].ItemId);
            }
        }
    }
}