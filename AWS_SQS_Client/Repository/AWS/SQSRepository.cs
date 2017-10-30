using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using Amazon.SQS;
using Amazon.SQS.Model;

namespace AWS_SQS_Client.Repository.AWS
{
    //Repository object carrying out Create, Read and Delete operations against the AWS SQS
    public class SQSRepository
    {
        //Client object from the imported Nuget package used to interact with the AWS SQS
        private AmazonSQSClient _sqsClient = null;

        //URL of the SQS queue we want to use
        private readonly string _queueUrl = string.Empty;

        public SQSRepository(AmazonSQSClient sqsClient, string queueUrl)
        {
            _sqsClient = sqsClient;
            _queueUrl = queueUrl;
        }

        /// <summary>
        /// Send a single message to the SQS queue
        /// </summary>
        /// <param name="message">Serialized object</param>
        /// <returns>boolean indicating success or failure</returns>
        public bool SendMessage(string message)
        {
            bool retval = false;

            SendMessageRequest sendMessageRequest = new SendMessageRequest(_queueUrl, message);
            SendMessageResponse sendMessageResponse = _sqsClient.SendMessage(sendMessageRequest);

            if (sendMessageResponse.HttpStatusCode == HttpStatusCode.OK)
            {
                retval = true;
            }

            return retval;
        }

        /// <summary>
        /// Delete a single message from the SQS queue
        /// </summary>
        /// <param name="messageId">Return handle id of a message read from the SQS queue</param>
        /// <returns>boolean indicating success or failure</returns>
        public bool DeleteMessage(string messageId)
        {
            bool retval = false;

            DeleteMessageRequest deleteMessageRequest = new DeleteMessageRequest(_queueUrl, messageId);
            DeleteMessageResponse deleteMessageResponse = _sqsClient.DeleteMessage(deleteMessageRequest);

            if (deleteMessageResponse.HttpStatusCode == HttpStatusCode.OK)
            {
                retval = true;
            }

            return retval;
        }

        /// <summary>
        /// Get all messages available in the queue i.e. visible messages only
        /// </summary>
        /// <returns>List of AWS SQS message objects that were available in the queue</returns>
        public List<Message> GetMessages()
        {
            List<Message> retval = null;

            //NOTE: I can put in smart logic to pull mesages in matches of 10 as there is a config parameter allowing for "batch" 
            //pulling with a max threshold of 10 i.e. recieveMessageRequest.Maxnumberofmessages = 10
            ReceiveMessageRequest receiveMessageRequest = new ReceiveMessageRequest(_queueUrl);

            for (int i = 0; i <= NumberOfMessagesInQueue(); i++)
            {
                ReceiveMessageResponse receiveMessageResponse = _sqsClient.ReceiveMessage(receiveMessageRequest);

                if (receiveMessageResponse.HttpStatusCode == HttpStatusCode.OK)
                {
                    if (retval == null)
                    {
                        retval = new List<Message>();
                    }

                    retval.Add(receiveMessageResponse.Messages.FirstOrDefault());
                }
            }

            return retval;
        }

        private int NumberOfMessagesInQueue()
        {
            int retval = 0;

            GetQueueAttributesRequest attReq = new GetQueueAttributesRequest();
            attReq.QueueUrl = _queueUrl;
            attReq.AttributeNames.Add("ApproximateNumberOfMessages");

            GetQueueAttributesResponse response = _sqsClient.GetQueueAttributes(attReq);

            retval = response.ApproximateNumberOfMessages;

            return retval;
        }
    }
}
