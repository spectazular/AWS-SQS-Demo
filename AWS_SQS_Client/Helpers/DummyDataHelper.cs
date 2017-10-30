using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AWS_SQS_Client.Model;

using Newtonsoft.Json;

namespace AWS_SQS_Client.Helpers
{
    public class DummyDataHelper
    {
        public List<string> GetdummyPersonData()
        {
            List<string> retval = null;

            retval = (from per in CreateDummyData()
                select JsonConvert.SerializeObject(per)).ToList();

            return retval;
        }


        private List<PersonModel> CreateDummyData()
        {
            List<PersonModel> retval = new List<PersonModel>();

            retval.Add(new PersonModel(Guid.NewGuid(), "Bob", "Markey", "bob.marley@mail.com", "021 123 456"));
            retval.Add(new PersonModel(Guid.NewGuid(), "Peter", "Parker", "pp@mail.com", "022 123 456"));
            retval.Add(new PersonModel(Guid.NewGuid(), "Bruce", "Wayne", "nw@mail.com", "023 123 456"));
            retval.Add(new PersonModel(Guid.NewGuid(), "Clark", "Kent", "ck@mail.com", "024 123 456"));

            return retval;
        }
    }
}
