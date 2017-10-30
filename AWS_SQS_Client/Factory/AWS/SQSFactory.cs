using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Amazon.SQS;
using Amazon.SQS.Model;
using AWS_SQS_Client.Repository.AWS;

namespace AWS_SQS_Client.Factory.AWS
{
    //This is a Factory class for this project which orchestrates operations against the AWS SQS
    public class SQSFactory
    {
        //This repository object is used to carry out operations against the AWS SQS queue
        private readonly SQSRepository _sqsRepo = null;

        public SQSFactory(string accessKeyId, string secretAccessKey, Amazon.RegionEndpoint endPoint, string queueUrl)
        {
            //This SQS client could be a global object within this class if I were to use multiple repositories
            //but as there is no use for it in this demo I have instantiated and contained it within the Factory constructor
            AmazonSQSClient sqsClient = new AmazonSQSClient
                (
                    accessKeyId,
                    secretAccessKey,
                    endPoint
                );

            _sqsRepo = new SQSRepository(sqsClient, queueUrl);
        }

        /// <summary>
        /// Create, read and delete data in the AWS SQS.
        /// </summary>
        /// <param name="messages">List of objects serialized into JSON</param>
        /// <returns>List of JSON read from AWS SQS</returns>
        public List<string> AddReadAndRemoveMessages(List<string> messages)
        {
            List<string> retval = null;

            //Send data to SQS
            foreach (string msg in messages)
            {
                if (_sqsRepo.SendMessage(msg) == false)
                {
                    ThowSQSException("Message Sending");
                }
            }

            //Read data from queues into a list to confirm that 
            //we did manage to send and read said data
            List<Message> returnedMessages = _sqsRepo.GetMessages();

            if (returnedMessages == null)
            {
                ThowSQSException("Reading Messages");
            }
            else
            {
                //Delete messages one by one from queue
                foreach (Message msg in returnedMessages)
                {
                    if (_sqsRepo.DeleteMessage(msg.ReceiptHandle) == false)
                    {
                        ThowSQSException("Reading Messages");
                    }
                }
            }

            retval = (from msg in returnedMessages
                      select msg.Body).ToList();

            return retval;
        }

        private void ThowSQSException(string queueOperation)
        {
            //In case of error, freak out. Poorly handled error mechanism but sufficent for the purposes of this demonstration
            throw new Exception(string.Format("Error carrying out {0} operations on the AWS SQS", queueOperation));
        }

    }
}
