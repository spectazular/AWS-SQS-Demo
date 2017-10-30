using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Amazon;
using Amazon.SQS;
using Amazon.SQS.Model;
using AWS_SQS_Client.Factory.AWS;
using AWS_SQS_Client.Helpers;
using AWS_SQS_Client.Model;

namespace AWS_SQS_Client
{
    public class Program
    {
        public static void Main(string[] args)
        {
            List<string> originalPeople = new DummyDataHelper().GetdummyPersonData();

            Console.WriteLine("Original data:");

            foreach (var per in originalPeople)
            {
                Console.WriteLine(per);
            }

            SQSFactory factory = new SQSFactory
            (
                "XXXXXXXXXXXXXXXXXXXX",
                "YYYYYYYYYYYYYYYYYYYYYYYYYYYYYYYYYYYYYYYY",
                Amazon.RegionEndpoint.APSoutheast2,
                @"https://sqs.ap-southeast-2.amazonaws.com/000000000000/{queue name}"
            );

            List<string> queuePeople = factory.AddReadAndRemoveMessages(originalPeople);

            Console.WriteLine("Queue data");

            foreach (var per in queuePeople)
            {
                Console.WriteLine(per);
            }

            Console.ReadLine();
        }

    }
}
