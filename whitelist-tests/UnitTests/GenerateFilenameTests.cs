using System;
using System.Collections.Generic;
using whitelist.models;
using Xunit;

namespace whitelist_tests.UnitTests
{
    public class GenerateFilenameTests
    {
        private const string Filename = "ServiceTags_Public_";
        
        [Fact]
        public void Create_Filename_From_DaysAfter1()
        {
            // arrange
            
            DateTime date = new DateTime(2020, 07, 07);
            var generateFilename = new GenerateFilename(date);
            
            // act
            var result = generateFilename.Create();
            
            // assert
            Assert.Equal($"{Filename}20200706", result);
        }
        
        [Fact]
        public void Create_Filename_From_DaysAfter2()
        {
            // arrange
            DateTime date = new DateTime(2020, 07, 08);
            var generateFilename = new GenerateFilename(date);
            
            // act
            var result = generateFilename.Create();
            
            // assert
            Assert.Equal($"{Filename}20200706", result);
        }
        
        [Fact]
        public void Create_Filename_From_DaysAfter3()
        {
            // arrange
            DateTime date = new DateTime(2020, 07, 09);
            var generateFilename = new GenerateFilename(date);
            
            // act
            var result = generateFilename.Create();
            
            // assert
            Assert.Equal($"{Filename}20200706", result);
        }
        
        [Fact]
        public void Create_Filename_From_DaysAfter4()
        {
            // arrange
            DateTime date = new DateTime(2020, 07, 10);
            var generateFilename = new GenerateFilename(date);
            
            // act
            var result = generateFilename.Create();
            
            // assert
            Assert.Equal($"{Filename}20200706", result);
        }
        
        [Fact]
        public void Create_Filename_From_DaysAfter5()
        {
            // arrange
            DateTime date = new DateTime(2020, 07, 11);
            var generateFilename = new GenerateFilename(date);
            
            // act
            var result = generateFilename.Create();
            
            // assert
            Assert.Equal($"{Filename}20200706", result);
        }
        
        [Fact]
        public void Create_Filename_From_DaysAfter6()
        {
            // arrange
            DateTime date = new DateTime(2020, 07, 12);
            var generateFilename = new GenerateFilename(date);
            
            // act
            var result = generateFilename.Create();
            
            // assert
            Assert.Equal($"{Filename}20200706", result);
        }
        
        [Fact]
        public void Create_Filename_From_DaysAfter7()
        {
            // arrange
            DateTime date = new DateTime(2020, 07, 13);
            var generateFilename = new GenerateFilename(date);
            
            // act
            var result = generateFilename.Create();
            
            // assert
            Assert.Equal($"{Filename}20200713", result);
        }
        
        [Fact]
        public void Create_Filename_From_DaysAfter8()
        {
            // arrange
            DateTime date = new DateTime(2020, 07, 14);
            var generateFilename = new GenerateFilename(date);
            
            // act
            var result = generateFilename.Create();
            
            // assert
            Assert.Equal($"{Filename}20200713", result);
        }
        
        public static List<object[]> GetData()
        {
            return new List<object[]>
            {
                new object[] { new DateTime(2020, 06, 01), "ServiceTags_Public_20200601"},
                new object[] { new DateTime(2021, 10, 11), "ServiceTags_Public_20211011"},
                new object[] { new DateTime(2030, 01, 01), "ServiceTags_Public_20291231"}
            };
        }
        
        [Theory]
        [MemberData(nameof(GetData))]
        public void Create_File_Name(DateTime date, string expected)
        {
            // arrange
            var generateFileName = new GenerateFilename(date);
            
            // act
            var result = generateFileName.Create();

            // assert
            Assert.Equal(expected, result);
        }
    }
}