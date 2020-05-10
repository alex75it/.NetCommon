using MongoDB.Driver;

namespace Alex75.Common.MongoDB
{
    public class DatabaseSettings
    {
        public WriteConcern WriteConcern { get; set; }
        public ReadConcern ReadConcern { get; set; }
        public ReadPreference ReadPreference { get; set; }
        public bool RetryWrites { get; set; }

        public DatabaseSettings(MongoClientSettings clientSettings)
        {
            WriteConcern = clientSettings.WriteConcern;
            ReadConcern = clientSettings.ReadConcern;
            ReadPreference = clientSettings.ReadPreference;
            RetryWrites = clientSettings.RetryWrites;
        }


        public override string ToString()
            => $"\nReadConcern: {ReadConcern.Level} "
            + $"\n WriteConcern: {{ "
            + $"\n\t W:{WriteConcern.W} \n\t Journal:{WriteConcern.Journal} \n\t IsAcknowledged:{WriteConcern.IsAcknowledged} \n\t FSync:{WriteConcern.FSync} \n\t IsServerDefault:{WriteConcern:IsServerDefault} "
            + $"\n }} "
            + $"\n ReadPreference:{ReadPreference.ReadPreferenceMode} "
            + $"\n RetryWrites:{RetryWrites}"
            ;
    }
}
