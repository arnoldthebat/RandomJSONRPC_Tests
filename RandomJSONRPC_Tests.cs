using Xunit;
using org.random.JSONRPC;
using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using Microsoft.Extensions.Configuration;
using System.Text;

namespace RandomJSONRPC_Tests
{
    public class RandomJSONRPC_Tests
    {
        private static IConfigurationRoot Configuration { get => builder.Build(); }

        private static IConfigurationBuilder builder = new ConfigurationBuilder()
                .SetBasePath(Directory.GetCurrentDirectory())
                .AddJsonFile("appsettings.json");
  
        private org.random.JSONRPC.RandomJSONRPC testRandomJSONRPC = new org.random.JSONRPC.RandomJSONRPC(Configuration["API_KEY"]);

        private RandomJSONRPC TestRandomJSONRPC { get => testRandomJSONRPC; set => testRandomJSONRPC = value; }

        // Results specific setup
        int[] intResults = null;
        string[] strResults = null;
        double[] doubleResults = null;
        Guid[] guidResults = null;
        int[] intBase = new int[] { 2, 8, 10, 16 }; 

        [Fact]
        private void generateIntegers()
        {     
            var n =  new Random().Next(1, 100);
            var min = new Random().Next(1, 1000000000);
            var max = new Random().Next(min, 1000000000);
            
            intResults = TestRandomJSONRPC.GenerateIntegers(n, min, max);
            Assert.Equal(n, intResults.Length);
            Assert.NotEmpty(intResults);

            intResults = TestRandomJSONRPC.GenerateIntegers(n, min, max, true);
            Assert.Equal(n, intResults.Length);
            Assert.NotEmpty(intResults);

            strResults = TestRandomJSONRPC.GenerateIntegers(n, min, max, true, intBase[new Random().Next(0, intBase.Length - 1)]);
            Assert.Equal(n, strResults.Length);
            Assert.NotEmpty(strResults);

            intResults = TestRandomJSONRPC.GenerateSignedIntegers(n, min, max);
            Assert.True(TestRandomJSONRPC.VerifySignature());
            Assert.Equal(n, intResults.Length);
            Assert.NotEmpty(intResults);

            intResults = TestRandomJSONRPC.GenerateSignedIntegers(n, min, max, true);
            Assert.True(TestRandomJSONRPC.VerifySignature());
            Assert.Equal(n, intResults.Length);
            Assert.NotEmpty(intResults);

            strResults = TestRandomJSONRPC.GenerateSignedIntegers(n, min, max, true, intBase[new Random().Next(0, intBase.Length - 1)]);
            Assert.True(TestRandomJSONRPC.VerifySignature());
            Assert.Equal(n, strResults.Length);
            Assert.NotEmpty(strResults);
        }  

        [Fact]
        private void generateDecimalFractions()
        {
            var n =  new Random().Next(1, 100);

            doubleResults = TestRandomJSONRPC.GenerateDecimalFractions(n, new Random().Next(1, 20));
            Assert.Equal(n, doubleResults.Length);
            Assert.NotEmpty(doubleResults);

            doubleResults = TestRandomJSONRPC.GenerateDecimalFractions(n, new Random().Next(1, 20), true);
            Assert.Equal(n, doubleResults.Length);
            Assert.NotEmpty(doubleResults);

            doubleResults = TestRandomJSONRPC.GenerateSignedDecimalFractions(n, new Random().Next(1, 20));
            Assert.True(TestRandomJSONRPC.VerifySignature());
            Assert.Equal(n, doubleResults.Length);
            Assert.NotEmpty(doubleResults);

            doubleResults = TestRandomJSONRPC.GenerateSignedDecimalFractions(n, new Random().Next(1, 20), true);
            Assert.True(TestRandomJSONRPC.VerifySignature());
            Assert.Equal(n, doubleResults.Length);
            Assert.NotEmpty(doubleResults);
        }

        [Fact]
        private void generateGaussians()
        {
            var n =  new Random().Next(1, 100);
            int max = Convert.ToInt32(1E+6);

            doubleResults = TestRandomJSONRPC.GenerateGaussians(n, new Random().Next(1, max), new Random().Next(1, max), new Random().Next(2, 20));
            Assert.Equal(n, doubleResults.Length);
            Assert.NotEmpty(doubleResults);

            doubleResults = TestRandomJSONRPC.GenerateSignedGaussians(n, new Random().Next(1, max), new Random().Next(1, max), new Random().Next(2, 20));
            Assert.True(TestRandomJSONRPC.VerifySignature());
            Assert.Equal(n, doubleResults.Length);
            Assert.NotEmpty(doubleResults);
        }

        [Fact]
        private void generateStrings()
        {
            var n =  new Random().Next(1, 100);

            strResults = TestRandomJSONRPC.GenerateStrings(n, new Random().Next(1, 20), getStrings(new Random().Next(1, 80)));
            Assert.Equal(n, strResults.Length);
            Assert.NotEmpty(strResults);

            strResults = TestRandomJSONRPC.GenerateStrings(n, new Random().Next(1, 20), getStrings(new Random().Next(1, 80)), true);
            Assert.Equal(n, strResults.Length);
            Assert.NotEmpty(strResults);

            strResults = TestRandomJSONRPC.GenerateSignedStrings(n, new Random().Next(1, 20), getStrings(new Random().Next(1, 80)));
            Assert.True(TestRandomJSONRPC.VerifySignature());
            Assert.Equal(n, strResults.Length);
            Assert.NotEmpty(strResults);

            strResults = TestRandomJSONRPC.GenerateSignedStrings(n, new Random().Next(1, 20), getStrings(new Random().Next(1, 80)), true);
            Assert.True(TestRandomJSONRPC.VerifySignature());
            Assert.Equal(n, strResults.Length);
            Assert.NotEmpty(strResults);
        }
        
        [Fact]
        private void generateUUIDs()
        {
            var n =  new Random().Next(1, 100);

            guidResults = TestRandomJSONRPC.GenerateUUIDs(n);
            Assert.Equal(n, guidResults.Length);
            Assert.NotEmpty(guidResults);      

            guidResults = TestRandomJSONRPC.GenerateSignedUUIDs(n);
            Assert.True(TestRandomJSONRPC.VerifySignature());
            Assert.Equal(n, guidResults.Length);
            Assert.NotEmpty(guidResults);     
        }

        private string getStrings(int length)
        {
            StringBuilder strReturn = new StringBuilder();
            char[] strArray = @"abcdefghijklmnopqrstuvwxyzABCDEFGHIJKLMNOPQRSTUVWXYZ1234567890!""£$%^&*()-=_+[]{};'#:@~,./<>?`¬".ToCharArray();

            for(int i = 0; i < length; i++)
            {
                strReturn.Append(Convert.ToString(strArray[new Random().Next(0, strArray.Length-1)]));
            }
        
            return strReturn.ToString();
        }
    }
}
